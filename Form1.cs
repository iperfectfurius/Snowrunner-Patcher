using EasyConfig;
using IniParser.Model.Formatting;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net.Security;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Runtime.CompilerServices;
using Snowrunner_Parcher.Resources;

namespace Snowrunner_Patcher
{
    public partial class Form1 : Form
    {
        private string Token;
        private bool IsIniConfigLoaded = false;
        private static readonly string APP_VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private static readonly Dictionary<(string, string), string> DefaultConfig =
            new() { { ("App", "Version"), APP_VERSION }, { ("Game", "ModVersion"), "0" }, { ("Game", "PatchingMode"), Patcher.Method.Simple.ToString() } };
        private Config cf = new(defaultConfig: DefaultConfig);
        private string LastVersionInstalled, CurrentVersionInstalled;
        private Patcher patcher;
        private string ModVersionReleased;

        private string ModPakPathFolder => string.Join('\\', cf.ConfigData["Game"]["ModsPath"].Split('\\')[..^1]);
        private string ModPakPath => cf.ConfigData["Game"]["ModsPath"];
        private string ModPakName => string.Join('\\', cf.ConfigData["Game"]["ModsPath"].Split('\\')[^1]);
        private string BackupFolder => cf.DirectoryConfig + "\\Backups";
        private string PatchingMode => cf.ConfigData["Game"]["PatchingMode"];
        private IProgress<ProgressInfo> ProgressPatcher;

        delegate void ChangeText(string str);
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
            LastVersionInstalled = cf.ConfigData["Game"]["ModVersion"];
            ModVersionLabel.Text += LastVersionInstalled;
        }
        private void CheckConfig()
        {
            if (ModPakPath == null || cf.ConfigData["Game"]["ModsPath"] == "") IniConfig();

            changeModPathToolStripMenuItem.ToolTipText = ModPakPath;

            if (PatchingMode == null) cf.ConfigData["Game"]["PatchingMode"] = Patcher.Method.Simple.ToString();

            if ((Patcher.Method)Enum.Parse(typeof(Patcher.Method), PatchingMode) == Patcher.Method.Advanced)
                advancedPatchingToolStripMenuItem.Checked = true;

            CheckLastVersionInstalled();
        }

        private void CheckLastVersionInstalled()
        {
            //throw new NotImplementedException();
        }

        private void LoadPatcher()
        {
            Patcher.Method method = (Patcher.Method)Enum.Parse(typeof(Patcher.Method), PatchingMode);
            ProgressPatcher = new Progress<ProgressInfo>(ChangeName);//Reporter
            patcher = new(ModPakPath, BackupFolder,ref ProgressPatcher, method);
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
            if (Token != "Error") //Todo Handle error{}
            {
                forceInstallToolStripMenuItem.Enabled = true;
                await CheckModVersion();
            }

            else
            {
                forceInstallToolStripMenuItem.Enabled = false;
                forceInstallToolStripMenuItem.ToolTipText = "Error on check new version";
            }

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
            toolStripStatusInfo.Text = "New APP Version Released";
            toolStripStatusInfo.IsLink = true;

        }

        private async void OpenDownloadPage()
        {
            bool result = MessageBox.Show("New APP version released. Do you want to download?", "New Update Available", MessageBoxButtons.YesNo) == DialogResult.Yes;

            //TODO Remove
            const string TempToken = "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";//Development key this has no sense in the future

            RestClient RestClient = new(APP_REALEASED_VERSIONS_URL);
            RestRequest request = new RestRequest();
            request.AddHeader("Authorization", $"token {TempToken}");

            var restResponse = await RestClient.GetAsync(request);

            JObject lastRelease = (JObject)JArray.Parse(restResponse.Content)[0];

            if (result)
            {
                OpenNewRelease((string)lastRelease["html_url"]);
            }
            //toolStripStatusInfo.Tag = ;
            toolStripStatusInfo.Click += (e, ar) => OpenNewRelease((string)lastRelease["html_url"]);
        }

        private void OpenNewRelease(string wepPage)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = wepPage,
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

            if (ModVersionReleased != LastVersionInstalled || !File.Exists(ModPakPath)) ShowNewModVersion(restResponse.Content);

            ShowModVersionReleased();
            return true;
        }
        private void ShowNewModVersion(string version)
        {
            UpdateModButton.Enabled = true;
            UpdateModButton.Text = "Update to " + version;

            LastVersionLabel.ForeColor = Color.Green;
        }
        private void ShowModVersionReleased()
        {
            LastVersionLabel.Text += $" {ModVersionReleased}";
        }

        private void openConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cf.OpenConfig();
        }

        private void UpdateModButton_Click(object sender, EventArgs e)
        {
            toolStripStatusInfo.Visible = false;
            toolStripStatusLabelInfoPatch.Visible = true;          

            Task.Run(() => patcher.PatchMod(Token));
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
            Process.Start(new ProcessStartInfo("explorer.exe")
            {
                UseShellExecute = true,
                Arguments = "/select, \"" + ModPakPath + "\""
            });
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("explorer.exe")
            {
                UseShellExecute = true,
                Arguments = BackupFolder
            });
        }

        private void replaceBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Todo replace list of backups
        }

        private void lastBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] backupFiles = Directory.GetFiles(BackupFolder).OrderBy(file => new FileInfo(file).Name).ToArray();
            if (backupFiles.Length == 0)
            {
                MessageBox.Show("No Backups found", "Info", MessageBoxButtons.OK);
                return;
            }

            string fileToReplace = backupFiles[^1];
            if (MessageBox.Show($"Do you want to Replace your current ModPak for {fileToReplace.Split('\\')[^1]}?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            patcher.ReplaceLastBackup(fileToReplace);

        }

        private void advancedPatchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            advancedPatchingToolStripMenuItem.Checked = !advancedPatchingToolStripMenuItem.Checked;
            cf.ConfigData["Game"]["PatchingMode"] = advancedPatchingToolStripMenuItem.Checked ? Patcher.Method.Advanced.ToString() : Patcher.Method.Simple.ToString();
            patcher.PatchingMethod = advancedPatchingToolStripMenuItem.Checked ? Patcher.Method.Advanced : Patcher.Method.Simple;
        }

        private void deleteModPakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete the current ModPak installed?", "Delete ModPak", MessageBoxButtons.YesNo) == DialogResult.No) return;

            File.Delete(ModPakPath);
        }

        private void forceInstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateModButton_Click(sender, EventArgs.Empty);
        }

        private void createBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            patcher.CreateBackup();
        }
        private void ChangeName(ProgressInfo info)
        {

            toolStripStatusLabelInfoPatch.Text = info.Info;
            int s = info.Total;
        }
    }
}