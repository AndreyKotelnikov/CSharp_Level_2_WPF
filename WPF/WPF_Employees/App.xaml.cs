using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_Employees
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void FrameworkElement_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ((Control)sender).Background = new SolidColorBrush(Colors.Red);
        }

        private void FrameworkElement_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //((Control)sender).Background = null;
        }
    }
}
