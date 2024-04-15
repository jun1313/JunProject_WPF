using Google.Protobuf.WellKnownTypes;
using JunProject.utilitys;
using JunProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
        public MediaPlayer mediaPlayer;
        int num = 0;
        private readonly string synologyServerUrl;
        public MainPage()
        {

            InitializeComponent();
            DataContext = new MainViewModel();
            mediaPlayer = new MediaPlayer();
            synologyServerUrl = "http://your-synology-server-ip/music/test.mp3";
            
        }

        private void DoubleClick_Calandar(object sender, MouseButtonEventArgs e)
        {
            // 선택된 날짜를 가져옴
            if (sender is Calendar calendar)
            {
                DateTime selectedDate = calendar.SelectedDate ?? DateTime.Today;

                // 새 윈도우를 띄우고 선택된 날짜 값을 전달
                AddWindow window = new AddWindow(selectedDate);
                window.DataContext = DataContext;
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
        /*
        private void Music_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            mediaPlayer.Stop();
            if (sender is ListView listView && listView.SelectedItem is Music temp)
            {
                string uri = temp.Uri;
                mediaPlayer.Open(new Uri("C:\\Users\\최준영\\source\\repos\\JunProject\\JunProject\\"+uri));
                mediaPlayer.Volume = 20;
                mediaPlayer.Play();
            }
        }
        
        */

        private void Play_And_Stop(object sender, MouseButtonEventArgs e)
        {
            if (num == 0)
            {
                mediaPlayer.Pause();
                num++;
            }
            else
            {
                mediaPlayer.Play();
                num = 0;
            }
        }

        private async void test(object sender, RoutedEventArgs e)
        {
            MusicPlayer dd=new MusicPlayer();
            dd.Select_Folder();
            //dd.Set_Playlist();
            // MP3 파일이 있는 폴더 경로
            string folderPath = @"C:\Users\최준영\source\repos\JunProject\JunProject\music";

            // 폴더 내의 모든 MP3 파일을 검색
            string[] mp3Files = Directory.GetFiles(folderPath, "*.mp3");
            MessageBox.Show(mp3Files[0]);
            //mediaPlayer.Open(new Uri(folderPath+mp3Files));
            mediaPlayer.Open(new Uri(mp3Files[0]));
            mediaPlayer.Play();
            

        }

        private void ListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange > 0)
            {
                ((ScrollViewer)sender).ScrollToEnd();
            }
        }

        private void inputWidth_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
