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
using Snowrunner_Patcher.Resources;
using System.IO.Compression;
using System.Media;
using System.Runtime.InteropServices;


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
        private SoundPlayer asterisk = new SoundPlayer(Properties.Resources.Windows_Background);

        delegate void ChangeText(string str);
        public Form1()
        {
            InitializeComponent();
            CheckConfig();
            IniForm();
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
            CurrentVersionLabel.Text += CurrentVersionInstalled;

            AddLineLog($"[Program Initiated] v{APP_VERSION}");
        }
        private void CheckConfig()
        {
            Logger.logPath = cf.DirectoryConfig + "\\Logs";
            LoadLogFile();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(SaveLog);

            if (ModPakPath == null || cf.ConfigData["Game"]["ModsPath"] == "") IniConfig();

            changeModPathToolStripMenuItem.ToolTipText = ModPakPath;

            if (PatchingMode == null) cf.ConfigData["Game"]["PatchingMode"] = Patcher.Method.Simple.ToString();

            if ((Patcher.Method)Enum.Parse(typeof(Patcher.Method), PatchingMode) == Patcher.Method.Advanced)
                advancedPatchingToolStripMenuItem.Checked = true;

            if (cf.ConfigData["ModPak"]["UrlDownload"] != null && cf.ConfigData["ModPak"]["UrlDownload"] != DEFAULT_VALUE)
                MOD_DOWNLOAD_URL = cf.ConfigData["ModPak"]["UrlDownload"];
            else
                cf.ConfigData["ModPak"]["UrlDownload"] = DEFAULT_VALUE;

            if (cf.ConfigData["ModPak"]["UrlVersion"] != null && cf.ConfigData["ModPak"]["UrlVersion"] != DEFAULT_VALUE)
                MOD_VERSION_URL = cf.ConfigData["ModPak"]["UrlVersion"];
            else
            {
                cf.ConfigData["ModPak"]["UrlVersion"] = DEFAULT_VALUE;
                MOD_VERSION_URL = "";

            }             

            if (cf.ConfigData["ModPak"]["Token"] != null && cf.ConfigData["ModPak"]["Token"] != DEFAULT_VALUE)
                Token = cf.ConfigData["ModPak"]["Token"];
            else
                cf.ConfigData["ModPak"]["Token"] = DEFAULT_VALUE;

            cf.ConfigData["App"]["Version"] = APP_VERSION;


            CheckCurrentModVersionInstalled();


        }

        private bool CheckCurrentModVersionInstalled()
        {
            //This can happen when the game patches for a new version.
            ZipArchive installedModPak = ZipFile.Open(ModPakPath, ZipArchiveMode.Read);
            ZipArchiveEntry? versionPatch = installedModPak.GetEntry("Version.txt");

            if (versionPatch == null)
            {
                CurrentVersionInstalled = "Not Found in the modPak";
                installedModPak.Dispose();
                return false;
            }

            StreamReader read = new StreamReader(versionPatch.Open());
            CurrentVersionInstalled = read.ReadToEnd();
            installedModPak.Dispose();

            return true;
        }

        private void LoadPatcher()
        {
            Patcher.Method method = (Patcher.Method)Enum.Parse(typeof(Patcher.Method), PatchingMode);
            ProgressPatcher = new Progress<ProgressInfo>(UpdateReport);//Reporter
            patcher = new(ModPakPath, BackupFolder, ref ProgressPatcher, method);
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
                    AddLineLog($"[Config] : {ModPakPath}");
                }
                else return false;
            }

            return true;
        }
        private async void CheckForUpdates()
        {
            //TODO remove and apply the config
            if (!await CheckAppVersion())
            {
                forceInstallToolStripMenuItem.ToolTipText = "Error on check new version";
                AddLineLog($"[Error] Error Checking APP Version");
            }

            if (await CheckModVersion()) forceInstallToolStripMenuItem.Enabled = true;
        }
        private async Task<bool> CheckAppVersion()
        {
            
            string versionReleased = string.Empty;

            RestClient RestClient = new(APP_VERSION_URL);
            RestRequest request = new RestRequest();
            //TODO Remove on release
            //request.AddHeader("Authorization", $"token {TempToken}");
            RestResponse restResponse;

            XmlDocument doc = new XmlDocument();

            try
            {
                restResponse = await RestClient.GetAsync(request);

                if (restResponse == null || restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception($"Error on check version app. {restResponse?.StatusCode}");

                doc.LoadXml(restResponse.Content);
            }
            catch (Exception ex)
            {
                toolStripStatusInfo.Text = "Can't Check app versions";
                toolStripStatusInfo.ForeColor = Color.Red;
                AddLineLog(new string[] { $"[Error] {ex.Message}", $"[Call Stack] {new StackTrace()}" });
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

            //TODO Remove on release
            const string TempToken = "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";//Development key this has no sense in the future

            RestClient RestClient = new(APP_REALEASED_VERSIONS_URL);
            RestRequest request = new RestRequest();
            request.AddHeader("Authorization", $"token {TempToken}");
            RestResponse restResponse;

            try
            {
                restResponse = await RestClient.GetAsync(request);
                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception(restResponse.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                toolStripStatusInfo.Text = "Can't Check app versions";
                toolStripStatusInfo.ForeColor = Color.Red;
                toolStripStatusInfo.IsLink = false;
                AddLineLog(new string[] { $"[Error] {ex.Message}", $"[Call Stack] {new StackTrace()}" });
                return;
            }

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
            if (MOD_VERSION_URL == "")
            {
                ContinueWithoutCheckingModVersion();
                return true;
            }
           
            RestClient RestClient = new(MOD_VERSION_URL);
            RestRequest request = new RestRequest();

            if (Token != null && Token != "") request.AddHeader("Authorization", $"token {Token}");


            RestResponse restResponse;

            try
            {
                restResponse = await RestClient.GetAsync(request);
                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception(restResponse.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                AddLineLog(new string[] { $"[Error] {ex.Message}", $"[Call Stack] {new StackTrace()}" });
                return false;
            }

            ModVersionReleased = restResponse.Content;

            if (ModVersionReleased != LastVersionInstalled || LastVersionInstalled != CurrentVersionInstalled ||
                !File.Exists(ModPakPath)) ShowNewModVersion(restResponse.Content);

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
        private void ContinueWithoutCheckingModVersion()
        {
            ModVersionReleased = "Unknown";
            UpdateModButton.Enabled = true;
            UpdateModButton.Text = "Update";
            LastVersionLabel.Text += $" No link is provided in config.ini";
        }

        private void openConfigFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cf.OpenConfig();
        }

        private void UpdateModButton_Click(object sender, EventArgs e)
        {
            toolStripStatusInfo.Visible = false;
            toolStripStatusLabelInfoPatch.Visible = true;
            forceInstallToolStripMenuItem.Enabled = false;

            Task.Run(() => { if (patcher.PatchMod(Token).Result) AddLineLog($"[Patch Mod Finished] {ModVersionReleased}"); });
        }

        private void UpdateFormPatched()
        {
            asterisk.Play();
            UpdateModButton.Enabled = false;
            UpdateModButton.Text = "Patch Applied!";
            ProgressBar.Value = 100;
            cf.ConfigData["Game"]["ModVersion"] = ModVersionReleased;
            ModVersionLabel.Text = "Mod Version Installed: " + ModVersionReleased;
            CurrentVersionLabel.Text = "Mod Version Installed: " + ModVersionReleased;
            ModVersionLabel.ForeColor = Color.Green;
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
            AddLineLog($"[Deleted Modpak] {ModPakPath}");
        }

        private void forceInstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddLineLog("FORCE INSTALL INITIATED");
            UpdateModButton_Click(sender, EventArgs.Empty);
        }

        private void createBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            patcher.CreateBackup("ManualBackup");
        }
        private void UpdateReport(ProgressInfo info)
        {

            toolStripStatusLabelInfoPatch.Text = info.Info;

            if (info.Info == Patcher.CurrentState.Finished.ToString())
            {
                forceInstallToolStripMenuItem.Enabled = true;
                toolStripStatusLabelInfoPatch.Visible = false;
                toolStripStatusInfo.Visible = true;
                UpdateFormPatched();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            AddLineLog("[Program Terminated]");
        }

        private void openCurrentLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(OpenParam.LogFile);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveLog(null, null);
        }

        private void openFolderLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(OpenParam.LogFolder);
        }

        private async void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(BackupFolder);
            long dirSize = await Task.Run(() => dirInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly).Sum(file => file.Length));

            if (MessageBox.Show($"Do you want to delete all Backups? ({dirSize / 1024 / 1024}MB)", "Delete All Backups", MessageBoxButtons.YesNo) == DialogResult.No) return;

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }

            AddLineLog($"[Deleted Backups] Files deleted: {dirInfo.GetFiles().Length}, a total size of {dirSize} bytes");
        }
    }
}