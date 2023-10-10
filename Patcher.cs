using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowrunner_Patcher
{
    internal class Patcher
    {
        public enum Method : byte
        {
            Simple,
            Advanced
        }
        private const string TEMP_NAME = "initial.pak";
        private readonly string ModPath;
        private readonly string BackupPath;
        public Method PatchingMethod;
        private ToolStripProgressBar progressBar;

        public Patcher(string modPath, string backupFolder, Method patchingMethod = Method.Simple)
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
        public async Task<bool> PatchMod(ToolStripProgressBar progress, string token = "", bool createBackup = true)
        {
            //if (method == Method.Simple)
            progressBar = progress;
            if (createBackup) CreateBackup();
            string tempDownloadedFile = await DownloadModFromSource(token);

            return PatchingMethod == Method.Simple ? NormalPatch(tempDownloadedFile) : AdvancedPatch(tempDownloadedFile);
        }

        private async Task<string> DownloadModFromSource(string token)
        {


            string tempDownloadedFile = BackupPath + $"\\{TEMP_NAME}";
            RestClient RestClient = new(MOD_DOWNLOAD_URL);
            RestRequest request = new RestRequest();
            request.AddHeader("Authorization", $"token {token}");

            var restResponse = await RestClient.DownloadDataAsync(request);
            progressBar.Value = 10;

            File.WriteAllBytes(tempDownloadedFile, restResponse);
            RestClient.Dispose();

            return tempDownloadedFile;
        }
        private bool NormalPatch(string tempDownloadedFile)
        {
            File.Delete(ModPath);
            File.Copy(tempDownloadedFile, ModPath);
            File.Delete(tempDownloadedFile);

            return true;
        }

        private bool AdvancedPatch(string tempDownloadedFile)
        {
            string tempUnzipNewPak = $"{BackupPath}\\tempNewVersion", tempUnzipCurrentPakInstalled = $"{BackupPath}\\tempCurrentInstalled";

            ZipFile.ExtractToDirectory(tempDownloadedFile, tempUnzipNewPak);
            progressBar.Value = 35;

            ZipFile.ExtractToDirectory(ModPath, tempUnzipCurrentPakInstalled);
            progressBar.Value = 50;

            PatchOlderVersionFiles(tempUnzipNewPak, tempUnzipCurrentPakInstalled);

            File.Delete(ModPath);
            ZipFile.CreateFromDirectory(tempUnzipNewPak,ModPath);

            Directory.Delete(tempUnzipNewPak, true);
            Directory.Delete(tempUnzipCurrentPakInstalled, true);

            //throw new NotImplementedException();
            return true;
        }
        private void PatchOlderVersionFiles(string newVersionPath, string olderVersionPath)
        {
            string olderParentFolder = String.Join("\\", olderVersionPath.Split("\\")[..^1]) + "\\" +olderVersionPath.Split("\\")[^1];

            foreach (string subdir in Directory.GetDirectories(newVersionPath))
            {
                string olderSubParentFolder = $"{olderParentFolder}\\{subdir.Split("\\")[^1]}";//This will allow to create mod folders if exists

                if (!Directory.Exists(olderSubParentFolder))
                    Directory.CreateDirectory(olderSubParentFolder);

                PatchOlderVersionFiles(subdir, olderSubParentFolder);
            }

            DirectoryInfo dir = new DirectoryInfo(newVersionPath);
            FileInfo[] files = dir.GetFiles().Where(e =>
            {

                return e.LastWriteTime > DateTime.Parse("01/01/1981",System.Globalization.CultureInfo.InvariantCulture);
            }).ToArray();

            if (files.Length == 0) return;
           
            foreach (FileInfo file in files){
                file.CopyTo($"{olderParentFolder}\\{file.Name}",true);
            }
        }
        public bool ReplaceLastBackup(string LastBackUp)
        {
            CreateBackup("Replaced_ModPak_");
            File.Delete(ModPath);
            File.Copy(LastBackUp, ModPath);
            return true;
        }
        private void CreateVersionModFile(string path)
        {
            if (File.Exists(path + "\\Version.txt")) File.Delete(path + "\\Version.txt");
        }
    }
}
