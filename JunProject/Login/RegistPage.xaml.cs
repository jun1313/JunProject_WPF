using JunProject.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JunProject.Login
{
    /// <summary>
    /// RegistPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RegistPage : Page
    {
        public RegistPage()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        private void To_Login(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Login/LoginPage.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        private void APasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((LoginViewModel)this.DataContext).member2.Pw = ((PasswordBox)sender).Password;
            }
        }
    }
}
