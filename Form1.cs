using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace MapLister
{
    public partial class Form1 : Form
    {
        string[] folders;
        string defaultPath = Environment.ExpandEnvironmentVariables("%APPDATA%");
        string roamingFolder = "\\Roaming";
        string fileName = String.Format(@"{0}\songlist.txt", Application.StartupPath);
        Random randomSong = new Random();
        string beatmapID;
        string beatmapLink;
        string baseLink = "https://osu.ppy.sh/beatmapsets/";

        public Form1()
        {
            InitializeComponent();
        }

        private void GenerateList(object sender, EventArgs e)
        {
            string formattedDefault = defaultPath.Remove(defaultPath.Length - roamingFolder.Length);
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.Description = "Select the osu 'Songs' directory.";
            FBD.RootFolder = Environment.SpecialFolder.Desktop;
            FBD.SelectedPath = @formattedDefault;

            if (FBD.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();
                folders = Directory.GetDirectories(FBD.SelectedPath);

                foreach(string folder in folders)
                {
                    string formattedFolder = folder.Remove(0, folder.LastIndexOf('\\')+1);
                    listBox1.Items.Add(formattedFolder);
                }

                randomSongBox.Visible = true;
                writeToFileButton.Visible = true;
                randomSongButton.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void WriteToFile(object sender, EventArgs e)
        {
            using (var textFile = new StreamWriter("songlist.txt", false))
            {
                foreach (var item in listBox1.Items)
                {
                    textFile.WriteLine(item);
                }
            }
        }

        private void ChooseRandomSong(object sender, EventArgs e)
        {
            int index = randomSong.Next(folders.Count());
            string formattedFolder = folders[index].Remove(0, folders[index].LastIndexOf('\\') + 1);
            randomSongBox.Text = formattedFolder;

            if (beatmapPageButton.Visible == false)
            {
                beatmapPageButton.Visible = true;
            }
        }

        private void GoToTwitchPage(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("www.twitch.tv/verathis");
        }

        private void GoToSongPage(object sender, EventArgs e)
        {
            beatmapID = randomSongBox.Text;
            beatmapLink = baseLink + beatmapID.Remove(beatmapID.IndexOf(" "));
            Process.Start(beatmapLink);            
        }
    }
}