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
            var filesInDir = Directory.GetFiles(dir.FullName,"*.*", SearchOption.AllDirectories);
            var directories = dir.GetDirectories("*.*", SearchOption.AllDirectories);
            
            try
            {

                foreach (var file in filesInDir)
                {
                    try
                    {
                        //Console.WriteLine(file.FullName);
                        //Console.WriteLine(file.DirectoryName);
                        //Directory.Delete(directories.);
                        //if (file.DirectoryName != null) Directory.Delete(file.DirectoryName.ToString());
                        
                        //File.Delete(file.Name);
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                        Console.WriteLine(ex.Message);
                    }

                }

                foreach (var direc in directories)
                {
                    try
                    {
                        Directory.Delete(direc.FullName);
                        //direc.Delete(true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }


        }
    }
}