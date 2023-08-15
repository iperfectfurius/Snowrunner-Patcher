using EasyConfig;
using IniParser.Model.Formatting;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net.Security;
using System.Reflection;
using static Snowrunner_Patcher.Resources.ResourcesApp;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace Snowrunner_Patcher
{
    public partial class Form1 : Form
    {
        private string Token;
        private bool IsIniConfigLoaded = false;
        private static readonly string APP_VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private static readonly Dictionary<(string, string), string> DefaultConfig =
            new() { { ("App", "Version"), APP_VERSION }, { ("Game", "ModVersion"), "0" } };
        private Config cf = new(defaultConfig: DefaultConfig);
        private string ModVersionInstalled;
        private Patcher patcher;
        private string ModVersionReleased;

        public Form1()
        {
            InitializeComponent();
            IniForm();
            CheckConfig();
            LoadPatcher();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForUpdates();
        }
        private void IniForm()
        {
            VersionAppLabel.Text = APP_VERSION;
            ModVersionInstalled = cf.ConfigData["Game"]["ModVersion"];
            ModVersionLabel.Text += ModVersionInstalled;
        }
        private void CheckConfig()
        {
            if (cf.ConfigData["Game"]["ModsPath"] == null || cf.ConfigData["Game"]["ModsPath"] == "") IniConfig();

            changeModPathToolStripMenuItem.ToolTipText = cf.ConfigData["Game"]["ModsPath"];
        }
        private void LoadPatcher()
        {
            patcher = new(cf.ConfigData["Game"]["ModsPath"], cf.DirectoryConfig + "\\Backups");
        }
        private void IniConfig()
        {
            if (!ChangeModPath())
            {
                MessageBox.Show("In order to apply the patch, we need to configure the destination path for the pack mods folder.", "Attention", MessageBoxButtons.OK);
                Environment.Exit(1);
            }
            IsIniConfigLoaded = true;
        }

        private bool ChangeModPath()
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    cf.ConfigData["Game"]["ModsPath"] = openFileDialog.FileName;
                }
                else return false;
            }

            return true;
        }
        private async void CheckForUpdates()
        {
            await CheckAppVersion();
            Token = await GetToken.GetTokenFromRequest();
            await CheckModVersion();
        }

        private async Task<bool> CheckAppVersion()
        {
            const string TempToken = "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";//Development key this has no sense in the future
            string versionReleased = string.Empty;

            RestClient RestClient = new(APP_VERSION_URL);
            RestRequest request = new RestRequest();
            request.AddHeader("Authorization", $"token {TempToken}");

            var restResponse = await RestClient.GetAsync(request);

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(restResponse.Content);
            }
            catch (Exception ex)
            {
                toolStripStatusInfo.Text = "Can't Check app versions";
                toolStripStatusInfo.ForeColor = Color.Red;
                return false;
            }

            versionReleased = doc["Project"]["PropertyGroup"]["AssemblyVersion"].InnerText;

            bool SameVersion = versionReleased == APP_VERSION;

            if (!SameVersion)
            {
                ShowNewAPPVersion();

                OpenDownloadPage();
            }

            return true;
        }

        private void ShowNewAPPVersion()
        {
            toolStripStatusInfo.Text = "New Version Released";
            toolStripStatusInfo.ForeColor = Color.Green;
        }

        private async void OpenDownloadPage()
        {
            if (MessageBox.Show("New APP version released. Do you want to download?", "New Update Available", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            //TODO Remove
            const string TempToken = "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";//Development key this has no sense in the future

            RestClient RestClient = new(APP_REALEASED_VERSIONS_URL);
            RestRequest request = new RestRequest();
            request.AddHeader("Authorization", $"token {TempToken}");

            var restResponse = await RestClient.GetAsync(request);

            JObject lastRelease = (JObject)JArray.Parse(restResponse.Content)[0];

            Process.Start(new ProcessStartInfo
            {
                FileName = (string)lastRelease["html_url"],
                UseShellExecute = true
            });

        }
        private async Task<bool> CheckModVersion()
        {
            RestClient RestClient = new(MOD_VERSION_URL);
            RestRequest request = new RestRequest();
            request.AddHeader("Authorization", $"token {Token}");

            var restResponse = await RestClient.GetAsync(request);
            ModVersionReleased = restResponse.Content;

            if (ModVersionReleased != ModVersionInstalled || !File.Exists(cf.ConfigData["Game"]["ModsPath"])) ShowNewModVersion(restResponse.Content);

            ShowVersionReleased();
            return true;
        }
        private void ShowNewModVersion(string version)
        {
            UpdateModButton.Enabled = true;
            UpdateModButton.Text += " To " + version;

            LastVersionLabel.ForeColor = Color.Green;
        }
        private void ShowVersionReleased()
        {
            UpdateModButton.Text = "Up to Date";
            LastVersionLabel.Text += $" {ModVersionReleased}"; 
        }
        private void openConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cf.OpenConfig();
        }

        private async void UpdateModButton_Click(object sender, EventArgs e)
        {
            patcher.CreateBackup();
            if (await patcher.PatchMod(MOD_DOWNLOAD_URL, Token)) UpdateFormPatched();
        }
        private void UpdateFormPatched()
        {
            UpdateModButton.Enabled = false;
            UpdateModButton.Text = "Patch Applied!";
            ProgressBar.Value = 100;
            cf.ConfigData["Game"]["ModVersion"] = ModVersionReleased;
        }

        private void changeModPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeModPath();
        }

        private void openModDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Process.Start(new ProcessStartInfo(fullPathConfig)
            //{
            //    UseShellExecute = true
            //});
        }
    }
}