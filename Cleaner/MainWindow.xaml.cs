using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Cleaner
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool cancel;
        private string dateUpdate;
        private long totalSize;

        public MainWindow()
        {
            InitializeComponent();
        }


        //Event Btn For Scan 
        private async void scan_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Start scan");
            btnClean.IsEnabled = false;
            btnHistory.IsEnabled = false;
            btnUpdate.IsEnabled = false;

            bigTitle.Content = "Scan in progress...";

            scan.Visibility = Visibility.Hidden;
            statisic.Visibility = Visibility.Hidden;
            progress.Visibility = Visibility.Visible;
            infos.Visibility = Visibility.Visible;
            // Clear.ClearTempData(temp);

            await Task.Run(PBarSleep);

            //When ProgressBar reached value = 100 =>
            if (pbar.Value >= 100)
            {
                MessageBox.Show("Scan Complete");

                Console.WriteLine(pbar.Value);
                btnClean.IsEnabled = true;
                btnHistory.IsEnabled = true;
                btnUpdate.IsEnabled = true;

                bigTitle.Content = "Scan Completed";
                statisic.Visibility = Visibility.Visible;
                infos.Visibility = Visibility.Hidden;
                progress.Visibility = Visibility.Hidden;

                lastUpdate.Content += "2 Days ago";
                sizeToClean.Content += totalSize.ToPrettySize();
                lastScan.Content += dateUpdate;
                pcName.Visibility = Visibility.Visible;
                userName.Visibility = Visibility.Visible;
                pcOs.Visibility = Visibility.Visible;

                pcName.Content += $" {Environment.MachineName}.";
                userName.Content += $" {Environment.UserName}.";
                pcOs.Content += $" {Environment.OSVersion}";
            }
        }


        //Temp File and ProgressBar  
        private void PBarSleep()
        {
            try
            {
                var temp = Path.GetTempPath();
                var dir = new DirectoryInfo(temp);
                var files = dir.GetFiles("*.*", SearchOption.AllDirectories);

                var length = files.Length;

                //var length = 150;

                for (var i = 1; i <= length; i++)
                    if (!cancel)
                    {
                        Thread.Sleep(10);
                        Dispatcher.Invoke(() =>
                        {
                            if (cancel == false && length > 0)
                            {
                                result.Text += $" {i}- {files[i - 1].FullName} {Environment.NewLine}";
                                //pbar.Value += 100 / length;
                                pbar.Value = CalcPer(i, length);
                                textProg.Text = Math.Round(pbar.Value, 1) + "%";
                                result.ScrollToEnd();
                                Console.WriteLine(files[i - 1].Length.ToString());
                                totalSize += Convert.ToInt64(files[i - 1].Length);
                            }
                            else
                            {
                                totalSize = 0;
                            }
                        });
                    }
                    else
                    {
                        Console.WriteLine("Exit");

                        break;
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //Get The Percent Progress Value
        private double CalcPer(double newNum, double originalNum)
        {
            var increase = originalNum - newNum;
            var incPer = increase / originalNum * 100;
            return 100 - incPer;
        }


        //Event Click Btn For Website
        private void webSite_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/adamkhairi/Cleaner");
        }


        //Vs Btn
        private void HideInfo(object sender, RoutedEventArgs e)
        {
            result.Visibility = Visibility.Hidden;
        }

        private void ShowInfo(object sender, RoutedEventArgs e)
        {
            result.Visibility = Visibility.Visible;
        }


        //Event Click For Btn Clean
        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to Clean files?", "Confirmation", MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes)
            {
                Clear.ClearTempData();
                HistoryHundler();
            }
        }


        //Hundle the History Of Cleaning
        private void HistoryHundler()
        {
            Task.Run(() =>
            {
                var date = DateTime.Now.ToString("dd-MM-yyyy");
                var time = DateTime.Now.ToString("HH-mm");
                dateUpdate = $"{date} | {time}";

                var dir = Directory.CreateDirectory(@".\log\" + date);
                var file = File.Create($"./log/{date}/{time}.txt");
                var directory = Path.GetFullPath(file.Name);

                try
                {
                    File.OpenWrite(directory);
                    var text = $"Last scan was {date} at {time} Total Deleted was : {totalSize.ToPrettySize()} \n";
                    MessageBox.Show(directory);
                    File.GetAccessControl(directory);
                    File.WriteAllText(directory, text);

                    //Clear.ClearTempData();
                }
                catch (Exception ex)
                {
                    //File.OpenWrite(directory);
                    // File.GetAccessControl(directory);
                    //File.WriteAllText(directory, ex.Message);
                    Console.WriteLine(ex.Message);
                }
            });
        }


        //Event Btn to Show History
        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            //Vs.ToggleV(analyse);
            Process.Start(@"./log/");
        }


        //Event Btn For Update
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var webClient = new WebClient();
            try
            {
                if (!webClient.DownloadString("http://weirdof.com/adam.txt").Contains("1.0.0"))
                    if (MessageBox.Show("Looks like there is an update! Do you want to download it?", "Cleaner",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        using (var client = new WebClient())
                        {
                            Process.Start("./cUpdater.exe");
                            Close();
                        }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            cancel = true;
        }
    }
}