using MediaToolkit.Model;
using MediaToolkit;
using Microsoft.Win32;
using VideoLibrary;

namespace VideoToMP3WF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // using Microsoft.Win32;
        string GetDownloadFolderPath()
        {
            return Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var downloadPath = GetDownloadFolderPath();
            var youtubeURL = youtubeLink.Text.Trim();
            StatusLabel.Text= "Downloading";
            SaveMP3(downloadPath, youtubeURL);
            StatusLabel.Text = "Download Complete";
        }

        private async Task SaveMP3(string SaveToFolder, string VideoURL)
        {
            string source = SaveToFolder;
            var youtube = YouTube.Default;
            var vid = youtube.GetVideo(VideoURL);
            string videopath = Path.Combine(source, vid.FullName);
            File.WriteAllBytes(videopath, vid.GetBytes());
            string MP3Name = vid.FullName.ToString();

            var inputFile = new MediaFile { Filename = Path.Combine(source, vid.FullName) };
            var outputFile = new MediaFile { Filename = Path.Combine(source, $"{MP3Name}.mp3") };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                engine.Convert(inputFile, outputFile);
            }
            File.Delete(Path.Combine(source, vid.FullName));
        }
    }
}