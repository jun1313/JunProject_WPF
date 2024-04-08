using JunProject.utilitys;
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

namespace JunProject
{
    /// <summary>
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void DoubleClick_Calandar(object sender, MouseButtonEventArgs e)
        {
            // 선택된 날짜를 가져옴
            if (sender is Calendar calendar)
            {
                DateTime selectedDate = calendar.SelectedDate ?? DateTime.Today;

                // 새 윈도우를 띄우고 선택된 날짜 값을 전달
                AddWindow window = new AddWindow(selectedDate);
                window.ShowDialog();

                // 선택된 날짜 값을 사용하여 뭔가를 처리할 수 있음
            }
        }

        private void DoubleClick_Data(object sender, MouseButtonEventArgs e)
        {
            UpdateWindow window = new UpdateWindow();
            window.DataContext = DataContext;
            window.ShowDialog();
        }
    }
}
