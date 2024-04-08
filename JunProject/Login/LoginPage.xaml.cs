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
    /// LoginPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        private void To_MainWindow(object sender, RoutedEventArgs e)
        {
            //비밀번호 암호화
            MySQLManager manager = new MySQLManager();
            string connectionString = "UID=root;PWD=1234;Server=127.0.0.1;Port=3306;Database=CRUD";
            manager.Connection(connectionString);
            int num = manager.Check_info(login_Id.Text, ((LoginViewModel)this.DataContext).member1.Pw);
            if (num == 1)
            {
                manager.Close_Connection();
                var uri = new Uri("MainWindow.xaml", UriKind.Relative);
                var newWindow = Application.LoadComponent(uri) as Window;
                newWindow.Show();
                Window currentWindow = Window.GetWindow(this);
                currentWindow.Close();
            }
        }

        private void To_Regist(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Login/RegistPage.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        private void APasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((LoginViewModel)this.DataContext).member1.Pw = ((PasswordBox)sender).Password;
            }
        }
    }
}
