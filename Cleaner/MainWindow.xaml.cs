using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Cleaner
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private bool cancel;
        //private string dateUpdate;
        private long totalSize;
        private string lastScan;
        private string date;
        private string time;

        public bool Cancel
        {
            get => cancel;
            set => cancel = value;
        }

        public long TotalSize
        {
            get => totalSize;
            set => totalSize = value;
        }

        public string LastScan1
        {
            get => lastScan;
            set => lastScan = value;
        }

        public string Date
        {
            get => date;
            set => date = value;
        }

        public string Time
        {
            get => time;
            set => time = value;
        }

        public MainWindow()
        {
            InitializeComponent();
            this.date = DateTime.Now.ToString("dd-MM-yyyy");
            this.time = DateTime.Now.ToString("HH-mm");
            PcName.Content += $"{Environment.MachineName}";
            UserName.Content += $"{Environment.UserName}";
            PcOs.Content += $"{OSname()}";
        }

        public string OSname()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }

        //Event Btn For Scan 
        private async void scan_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Start scan");
            BtnClean.IsEnabled = false;
            BtnHistory.IsEnabled = false;
            BtnUpdate.IsEnabled = false;
            BigTitle.Content = "Scan in progress...";
            Scan.Visibility = Visibility.Hidden;
            Statisic.Visibility = Visibility.Hidden;
            Progress.Visibility = Visibility.Visible;
            Infos.Visibility = Visibility.Visible;
            // Clear.ClearTempData(temp);

            await Task.Run(PBarSleep);

            //When ProgressBar reached value = 100 =>
            if (Pbar.Value >= 100)
            {
                MessageBox.Show("Scan Complete");
                Console.WriteLine(Pbar.Value);
                BtnClean.IsEnabled = true;
                BtnHistory.IsEnabled = true;
                BtnUpdate.IsEnabled = true;
                BigTitle.Content = "Scan Completed";
                Statisic.Visibility = Visibility.Visible;
                Infos.Visibility = Visibility.Hidden;
                Progress.Visibility = Visibility.Hidden;
                SizeToClean.Content += totalSize.ToPrettySize();
                PcName.Visibility = Visibility.Visible;
                UserName.Visibility = Visibility.Visible;
                PcOs.Visibility = Visibility.Visible;
                this.lastScan = DateTime.Now.ToString("dd-MM-yyyy HH-mm");
                LastScan.Content += this.lastScan;
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
                {
                    if (!cancel)
                    {
                        Thread.Sleep(10);
                        Dispatcher.Invoke(() =>
                        {
                            if (cancel == false && length > 0)
                            {
                                Result.Text += $" {i}- {files[i - 1].FullName} {Environment.NewLine}";
                                Pbar.Value = CalcPer(i, length);
                                TextProg.Text = Math.Round(Pbar.Value, 1) + "%";
                                Result.ScrollToEnd();
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
                this.date = DateTime.Now.ToString("dd-MM-yyyy");
                this.time = DateTime.Now.ToString("HH-mm");
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
            Result.Visibility = Visibility.Hidden;
        }

        private void ShowInfo(object sender, RoutedEventArgs e)
        {
            Result.Visibility = Visibility.Visible;
        }

        //Event Click For Btn Clean
        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to Clean files?", "Confirmation", MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes)
            {
                Clear.ClearTempData();
                HistoryHundler();
                LastClean.Content += DateTime.Now.ToString("dd-MM HH-mm");

            }
        }

        //Hundle the History Of Cleaning
        private void HistoryHundler()
        {
            Task.Run(() =>
            {
                var d = Directory.CreateDirectory(@".\log\" + this.date);

                var file = File.Create($"{d.FullName}/{this.time}.txt");
                var directory = Path.GetFullPath(file.Name);
                //var fileTowrite = Directory.GetFiles(file.ToString());
                try
                {
                    var text = $"Last scan was {date} at {time} Total Deleted was : {totalSize.ToPrettySize()} \n";
                    MessageBox.Show(directory);

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(directory))
                        {
                            writer.Write(text);
                        }
                    }
                    catch (Exception exp)
                    {
                        Console.Write(exp.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        //Event Btn to Show History
        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(@".\log");
            Process.Start(@".\log\");
        }

        //Event Btn For Update
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var webClient = new WebClient();
            try
            {
                if (!webClient.DownloadString("https://pastebin.com/dl/FXi6TAuS").Contains("1.0.0"))
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
            BtnClean.IsEnabled = true;
            BtnHistory.IsEnabled = true;
            BtnUpdate.IsEnabled = true;
            Statisic.Visibility = Visibility.Visible;
            Infos.Visibility = Visibility.Hidden;
            Progress.Visibility = Visibility.Hidden;
            SizeToClean.Content += totalSize.ToPrettySize();
            LastScan.Content += DateTime.Now.ToString("dd-MM-yyyy HH-mm");
            PcName.Visibility = Visibility.Visible;
            UserName.Visibility = Visibility.Visible;
            PcOs.Visibility = Visibility.Visible;

        }

        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/adamkhairi/Clean");
        }
    }
}