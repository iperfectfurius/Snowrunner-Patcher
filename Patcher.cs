using Newtonsoft.Json.Linq;
using RestSharp;
using Snowrunner_Parcher.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public enum CurrentState : byte
        {
            Finished
        }
        private const string TEMP_NAME = "initial.pak";
        private readonly string ModPath;
        private readonly string BackupPath;
        public Method PatchingMethod;
        private IProgress<ProgressInfo> Progress;

        public Patcher(string modPath, string backupFolder, ref IProgress<ProgressInfo> progress, Method patchingMethod = Method.Simple)
        {
            ModPath = modPath;
            BackupPath = backupFolder;
            PatchingMethod = patchingMethod;
            Progress = progress;

            AddLineLog($"[Patcher Loaded info] ModPath : {modPath}, BackupFolder : {backupFolder}, Patching Method : {patchingMethod}");
        }
        public bool CreateBackup()
        {
            if (ModPath == null || ModPath == "") return false;

            if (!File.Exists(ModPath)) return false;
            if (!Directory.Exists(BackupPath)) CreateBackupDirectory();
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
            string currentDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            File.Copy(ModPath, BackupPath + $"\\{currentDate}{(name != "" ? $"_{name}": "")}.pak");
            AddLineLog("[Created Backup] :" + BackupPath + $"\\{currentDate}{(name != "" ? $"_{name}" : "")}.pak");
        }
        public async Task<bool> PatchMod(string token = "", bool createBackup = true)
        {
            AddLineLog($"[Patch Mod Initiated] Create Backup: {createBackup}");

            ProgressInfo pi = new ProgressInfo();
            pi.Info = "Creating Backup...";
            Progress.Report(pi);

            if (createBackup) CreateBackup();

            pi.Info = "Downloading ModPak";
            Progress.Report(pi);

            string tempDownloadedFile = await DownloadModFromSource(token);

            pi.Info = "Installing ModPak...";
            Progress.Report(pi);

            
            return PatchingMethod == Method.Simple ? NormalPatch(tempDownloadedFile) : AdvancedPatch(tempDownloadedFile);
        }

        private async Task<string> DownloadModFromSource(string token)
        {
            string tempDownloadedFile = BackupPath + $"\\{TEMP_NAME}";
            RestClient RestClient = new(MOD_DOWNLOAD_URL);
            RestRequest request = new RestRequest();
            request.AddHeader("Authorization", $"token {token}");

            AddLineLog($"[ModPak] ModPak Download Initiated: {MOD_DOWNLOAD_URL}");
            var restResponse = await RestClient.DownloadDataAsync(request);

            File.WriteAllBytes(tempDownloadedFile, restResponse);
            AddLineLog($"[ModPak] ModPak Downloaded: {tempDownloadedFile}");

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
            File.Delete(tempDownloadedFile);
            return true;
        }
        private void PatchOlderVersionFiles(string newVersionPath)
        {
            string tempPathToExtract = $"{BackupPath}\\Temp\\";
            List<string> tempFilesReplaced = new();
            ProgressInfo pi = new ProgressInfo();

            if (!Directory.Exists(tempPathToExtract)) Directory.CreateDirectory(tempPathToExtract);

            using (ZipArchive currentPatch = ZipFile.Open(ModPath, ZipArchiveMode.Update))
            {
                using (ZipArchive newVersionPatch = ZipFile.Open(newVersionPath, ZipArchiveMode.Read))
                {
                    int numberOfFiles = newVersionPatch.Entries.Count;
                    int currentItem = 0;

                    foreach (ZipArchiveEntry entry in newVersionPatch.Entries)
                    {
                        //checks for modded files
                        if (entry.LastWriteTime > DateTime.Parse("01/01/1981", System.Globalization.CultureInfo.InvariantCulture))
                        {
                            entry.ExtractToFile(tempPathToExtract + entry.Name);

                            if (currentPatch.GetEntry(entry.FullName) == null)
                                tempFilesReplaced.Add($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] [WARNING] {entry.FullName} : The file does not exist");
                            else
                                currentPatch.GetEntry(entry.FullName)?.Delete();

                            currentPatch.CreateEntryFromFile(tempPathToExtract + entry.Name, entry.FullName);
                            tempFilesReplaced.Add($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] [File Replaced] {entry.FullName} : {entry.Length} bytes");
                        }
                        currentItem++;
                        pi.Info = $"{currentItem}/{numberOfFiles}";
                        
                        if (currentItem %(numberOfFiles/100) == 0)
                            Progress.Report(pi);
                    }
                }
            }
            pi.Info = "Deleting Temporal Files...";
            Progress.Report(pi);

            Directory.Delete(tempPathToExtract, true);

            pi.Info = CurrentState.Finished.ToString();
            Progress.Report(pi);

            AddToLog(tempFilesReplaced.ToArray());
        }
        public bool ReplaceLastBackup(string LastBackUp)
        {
            CreateBackup("Replaced_ModPak");
            File.Delete(ModPath);
            File.Copy(LastBackUp, ModPath);

            AddLineLog($"[Replaced Backup] {LastBackUp}");
            return true;
        }
    }
}
