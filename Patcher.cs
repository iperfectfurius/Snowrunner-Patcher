using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowrunner_Patcher
{
    internal class Patcher
    {
        public enum Method:byte
        {
            Simple,
            Advanced
        }
        private const string TEMP_NAME = "initial.pak";
        private readonly string ModPath;
        private readonly string BackupPath;
        public Method PatchingMethod;

        public Patcher(string modPath,string backupFolder, Method patchingMethod = Method.Simple)
        {
            ModPath = modPath;

            BackupPath = backupFolder;
            PatchingMethod = patchingMethod;
        }

        public bool CreateBackup()
        {
            if (ModPath == null || ModPath == "") return false;

            if (!File.Exists(ModPath)) return false;
            if (!Directory.Exists(ModPath)) CreateBackupDirectory();
            CreateBackupPakMod();

            return true;
        }
        public bool CreateBackup(string name)
        {
            if (ModPath == null || ModPath == "") return false;

            if (!File.Exists(ModPath)) return false;
            if (!Directory.Exists(BackupPath)) CreateBackupDirectory();

            CreateBackupPakMod(name);

            return true;
        }
        private bool CreateBackupDirectory()
        {
            Directory.CreateDirectory(BackupPath);
            return Directory.Exists(BackupPath);
        }
        private void CreateBackupPakMod(string name = "")
        {
            name += string.Join("_", DateTime.Now.ToString().Split(Path.GetInvalidFileNameChars()));
            File.Copy(ModPath, BackupPath + $"\\{name}.pak");
        }
        public async Task<bool> PatchMod(ToolStripProgressBar progress, string token = "",Method method = Method.Simple)
        {
            //if (method == Method.Simple)
            string tempDownloadedFile = await DownloadModFromSource(progress, token);

            progress.Value = 50;

            return method == Method.Simple ? await NormalPatch(tempDownloadedFile) : await AdvancedPatch(tempDownloadedFile);
        }

        private async Task<string> DownloadModFromSource(ToolStripProgressBar progress,string token)
        {
            progress.Value = 1;

            string tempDownloadedFile = BackupPath + $"\\{TEMP_NAME}";
            RestClient RestClient = new(MOD_DOWNLOAD_URL);
            RestRequest request = new RestRequest();
            request.AddHeader("Authorization", $"token {token}");

            var restResponse = await RestClient.DownloadDataAsync(request);
            progress.Value = 10;
            File.WriteAllBytes(tempDownloadedFile, restResponse);
            RestClient.Dispose();

            return tempDownloadedFile;
        }
        private async Task<bool> NormalPatch(string tempDownloadedFile)
        {
            File.Delete(ModPath);
            File.Copy(tempDownloadedFile, ModPath);
            File.Delete(tempDownloadedFile);

            return true;
        }

        private async Task<bool> AdvancedPatch(string tempDownloadedFile)
        {
            throw new NotImplementedException();
        }
        public bool ReplaceLastBackup(string LastBackUp)
        {
            CreateBackup("Replaced_ModPak_");
            File.Delete(ModPath);
            File.Copy(LastBackUp, ModPath);
            return true;
        }
    }
}
