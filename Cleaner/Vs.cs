using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cleaner
{
    public static class Vs
    {

        public static object ToggleV(object sn)
        {
            try
            {
                Control item = sn as Label;
                item.Visibility = item.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return sn;
        }
    }
}
