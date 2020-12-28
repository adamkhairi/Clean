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

                    Dispatcher.Invoke(() =>
                    {
                        PBarUp.Value = i;
                    });
                }
            });
            Task.Run((() =>
            {
                Update();
            }));
            Thread.Sleep(50);

            //Update();

            if (PBarUp.Value == 100)
            {
                MessageBox.Show("Update Complete", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                Process.Start(@".\Cleaner.exe");
                Close();
            }
        }

        private void Update()
        {
            try
            {
                var client = new WebClient();
                var files = Directory.GetFiles("./", "*.*", SearchOption.AllDirectories);
                var bakup = Directory.CreateDirectory("./backup");

                foreach (var file in files)
                {
                    try
                    {
                        File.Copy(file, $"./backup/{file}", true);
                        //File.Delete(file);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                //File.Delete(@".\Cleaner.exe");
                client.DownloadFile("https://github.com/adamkhairi/cc/raw/main/Cleaner.zip", @"Cleaner.zip");
                var zipPath = @".\Cleaner.zip";
                Directory.CreateDirectory("./update");
                var extractPath = @".\update";
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                foreach (var file in Directory.GetFiles(extractPath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(file, "../", true);
                    File.Delete(file);
                }
                Directory.Delete(extractPath, true);
                File.Delete(@".\Cleaner.zip");
            }
            catch (Exception ex)
            {
                var files = Directory.GetFiles("./backup", "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    File.Copy(file, $"../{file}", true);
                    //File.Delete(file);
                }
                MessageBox.Show(ex.Message);
            }
        }

        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/adamkhairi/Clean");
        }
    }
}