using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace cUpdater
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartUpdate_Click(object sender, RoutedEventArgs e)
        {

            //try
            //{
            Task.Run(() =>
            {
                for (var i = 0; i <= 100; i++)
                {
                    Thread.Sleep(200);

                    Dispatcher.InvokeAsync(() =>
                    {
                        PBarUp.Value = i;
                    });
                }
            });
            Task.Run(() =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    Update();
                });
            });

            if (PBarUp.Value == 100)
            {
                Process.Start(@".\Cleaner.exe");
                Close();
            }
        }

        private void Update()
        {
            try
            {
                var client = new WebClient();
                //File.Delete(@".\Cleaner.exe");
                client.DownloadFile("https://github.com/adamkhairi/cc/raw/main/Cleaner.zip", @"Cleaner.zip");
                var zipPath = @".\Cleaner.zip";
                var extractPath = @".\";
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                File.Delete(@".\Cleaner.zip");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/adamkhairi/Clean");
        }
    }
}