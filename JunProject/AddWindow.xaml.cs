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
using System.Windows.Shapes;

namespace JunProject
{
    /// <summary>
    /// AddWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow(DateTime selectedDate)
        {
            InitializeComponent();
            Loaded += (sender, e) =>
            {
                ;
                //새로 만들지말고
                DataContext = new MainViewModel();
                ((MainViewModel)DataContext).Todo_st.Date = selectedDate.ToString("yyyy-MM-dd");
                ((MainViewModel)DataContext).Todo_st.Done = "0";
            };
        }

    }
}