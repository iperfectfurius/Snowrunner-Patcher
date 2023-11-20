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

        public Patcher(string modPath,string backupFolder)
        {
            ModPath = modPath;
            BackupPath = backupFolder;
        }

        public bool CreateBackup()
        {
            if (ModPath == null || ModPath == "") return false;

            if (!File.Exists(ModPath)) return false;
            if (!Directory.Exists(ModPath)) CreateBackupDirectory();
            CreateBackupPakMod();

            return true;
        }
        private bool CreateBackupDirectory()
        {
            Directory.CreateDirectory(BackupPath);
            return Directory.Exists(BackupPath);
        }
        private void CreateBackupPakMod()
        {
            string name = string.Join("_", DateTime.Now.ToString().Split(Path.GetInvalidFileNameChars()));
            File.Copy(ModPath, BackupPath + $"\\{name}.bck");
        }
        public async Task<bool> PatchMod(string modDownload,string token = "",Method method = Method.Simple)
        {
            //if (method == Method.Simple)
            string tempDownloadedFile = BackupPath + $"\\{TEMP_NAME}";
            RestClient RestClient = new(modDownload);
            RestRequest request = new RestRequest();
            request.AddHeader("Authorization", $"token {token}");

            var restResponse = await RestClient.DownloadDataAsync(request);
            
            File.WriteAllBytes(tempDownloadedFile, restResponse);
            
            File.Delete(ModPath);
            File.Copy(tempDownloadedFile, ModPath);
            File.Delete(tempDownloadedFile);
            return true;
        }
        
    }
}
