﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace cUpdater
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartUpdate_Click(object sender, RoutedEventArgs e)
        {


            // var client = new WebClient();

            //try
            //{
            Task.Run((() =>
            {

                Thread.Sleep(10);
                Dispatcher.Invoke((() =>
                {
                    for (int i = 0; i <= 100; i++)
                    {
                        pBarUp.Value = i;
                        //Update();
                    }

                }));
            }));
        }


        private void Update()
        {
            try
            {
                //Thread.Sleep(5000);
                //File.Delete(@".\Demo.exe");
                //client.DownloadFile("yourhostinglink", @"Demo.zip");
                //var zipPath = @".\Demo.zip";
                //var extractPath = @".\";
                //ZipFile.ExtractToDirectory(zipPath, extractPath);
                //File.Delete(@".\Demo.zip");
                //Process.Start(@".\Cleaner.exe");
                //Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //    Process.Start("./Cleaner.exe");
                //    Close();
            }
        }
    }
}