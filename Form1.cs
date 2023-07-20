using EasyConfig;
using IniParser.Model.Formatting;
using Microsoft.VisualBasic;
using RestSharp;
using System.Reflection;

namespace Snowrunner_Parcher
{
    public partial class Form1 : Form
    {
        private string Token;
        private bool IsIniConfigLoaded = false;
        private static readonly string APP_VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private static readonly Dictionary<(string, string), string> DefaultConfig =
            new() { { ("App", "Version"), APP_VERSION }, { ("Game", "ModVersion"), "0" } };
        private Config cf = new(defaultConfig: DefaultConfig);
        private string ModVersion;

        public Form1()
        {
            InitializeComponent();
            IniForm();
            CheckConfig();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForUpdates();
        }
        private void IniForm()
        {
            VersionAppLabel.Text = APP_VERSION;
            ModVersion = cf.ConfigData["Game"]["ModVersion"];
            ModVersionLabel.Text += ModVersion;
        }
        private void CheckConfig()
        {
            if (cf.ConfigData["Game"]["ModsPath"] == null || cf.ConfigData["Game"]["ModsPath"] == "") IniConfig();

            changeModPathToolStripMenuItem.ToolTipText = cf.ConfigData["Game"]["ModsPath"];
        }

        private void IniConfig()
        {
            if (!ChangeModPath())
            {
                MessageBox.Show("In order to apply the patch, we need to configure the destination path for the pack mods folder.", "Atention", MessageBoxButtons.OK);
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
            Token = await GetToken.GetTokenFromRequest();
            await CheckModVersion();
        }

        private async Task<bool> CheckModVersion()
        {
            RestClient RestClient = new(@"https://raw.githubusercontent.com/iperfectfurius/Snowrunner-balance/main/Version.txt");
            RestRequest request = new RestRequest();
            request.AddHeader("Authorization", $"token {Token}");

            var restResponse = await RestClient.GetAsync(request);

            if (restResponse.Content != ModVersion) ShowNewVersion(restResponse.Content);
            return true;
        }

        private void ShowNewVersion(string version)
        {
            UpdateModButton.Enabled = true;
            LastVersionLabel.Text += version;
        }

        private void openConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cf.OpenConfig();
        }
    }
}