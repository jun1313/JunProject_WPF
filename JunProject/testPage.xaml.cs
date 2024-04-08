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
    /// testPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class testPage : Page
    {
        public testPage()
        {
            InitializeComponent();
            DataContext = new testPageViewModel();
        }

        private void Enter_Event(object sender, KeyEventArgs e)
        {
            e.Handled = true; // 엔터 키 이벤트 처리 완료
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }

        }
    }
}
