using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GeekProProgress.Classes;
using GeekProProgress.UI.Pages;

namespace GeekProProgress.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : Window
    {
        public BaseWindow()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;
            MainFrame.Navigate(new ResultsPage());
        }

        private void rbResults_Checked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ResultsPage());
        }

        private void rbAnalytics_Checked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Analytics());
        }

        private void rbAuthUsers_Checked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AuthUsersPage());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            Close();
        }
    }
}
