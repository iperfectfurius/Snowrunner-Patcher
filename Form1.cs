using EasyConfig;
namespace Snowrunner_Parcher
{
    public partial class Form1 : Form
    {
        private Config cf = new();
        private string Token;
        private bool IsIniConfigLoaded = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckConfig();

            CheckForUpdates();
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
                Close();
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
            //label1.Text = pepi;
        }
    }
}