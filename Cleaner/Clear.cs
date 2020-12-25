using System;
using System.IO;
using System.Windows;

namespace Cleaner
{
    internal class Clear
    {
        public static void ClearTempData()
        {
            var tempF = Path.GetTempPath();

            //var tempt = @"‪C:\Windows\Temp";
            MessageBox.Show(tempF);
            var dir = new DirectoryInfo(tempF);
            var filesInDir = dir.GetFiles();
            var ss = 5000;
            ss.ToPrettySize();
            ss.ToPrettySize();

            foreach (var file in filesInDir)
                try
                {
                    Console.WriteLine(file.FullName);
                    Console.WriteLine(file.DirectoryName);

                    File.Delete(file.FullName);
                    Directory.Delete(file.DirectoryName ?? string.Empty);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    Console.WriteLine(ex.Message);
                }
        }
    }
}