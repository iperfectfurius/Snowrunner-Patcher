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
            PatchOlderVersionFiles(tempDownloadedFile);
            progressBar.Value = 100;
            return true;
        }
        private void PatchOlderVersionFiles(string newVersionPath)
        {
            string tempPathToExtract = $"{BackupPath}\\Temp\\";
            if (!Directory.Exists(tempPathToExtract)) Directory.CreateDirectory(tempPathToExtract);

            using (ZipArchive currentPatch = ZipFile.Open(ModPath, ZipArchiveMode.Update))
            {
                using (ZipArchive newVersionPatch = ZipFile.Open(newVersionPath, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in newVersionPatch.Entries)
                    {
                        if (entry.LastWriteTime > DateTime.Parse("01/01/1981", System.Globalization.CultureInfo.InvariantCulture))
                        {
                            entry.ExtractToFile(tempPathToExtract + entry.Name);
                            currentPatch.GetEntry(entry.FullName).Delete();
                            currentPatch.CreateEntryFromFile(tempPathToExtract + entry.Name, entry.FullName);
                        }
                    }
                }
            }

            Directory.Delete(tempPathToExtract, true);

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
