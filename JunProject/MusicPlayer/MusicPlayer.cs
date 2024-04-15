using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TagLib;
namespace JunProject
{
    public class MusicPlayer
    {
        public MediaPlayer mediaPlayer;
        public string selectedFolderPath;
        public string[] mp3Files;
        public int playinginfo = 0;
        public string folderPath;
        public MusicPlayer() 
        {
            folderPath = "C:\\Users\\최준영\\Desktop\\Music";
            _musics = new ObservableCollection<Music>();
            mediaPlayer=new MediaPlayer();
            //Set_Playlist();


        }
        private ObservableCollection<Music> _musics;
        public ObservableCollection<Music> Musics {  get { return _musics; } set { _musics = value;  } }

        public void Select_Folder()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "폴더 선택",
                CheckFileExists = false, // 파일 선택 불가능
                CheckPathExists = true, // 경로가 유효한지 확인
                FileName = "Folder Selection",
                Filter = "모든 파일 (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true, // 다음 열릴 때 기본 폴더 복원
                ShowReadOnly = false,
                ReadOnlyChecked = true // 읽기 전용 체크
            };
            // 폴더 선택 다이얼로그 표시
            if (openFileDialog.ShowDialog() == true)
            {
                // 사용자가 선택한 폴더 경로를 얻어옴
                folderPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);

                // 선택한 폴더 경로를 처리
                //MessageBox.Show($"선택한 폴더: {folderPath}");
            }
        }
        public void Set_Playlist(ObservableCollection<Music> Temp)
        {
            Temp.Clear();
            //string folderPath = selectedFolderPath;
            mp3Files = Directory.GetFiles(folderPath, "*.mp3");
            
            //Temp.Clear();
            for (int i = 0; i<mp3Files.Length; i++)
            {
                try
                {
                    var file = TagLib.File.Create(mp3Files[i]);
                    string title = string.IsNullOrWhiteSpace(file.Tag.Title) ? "Unknown Title" : file.Tag.Title;
                    string artist = file.Tag.Performers.Length > 0 ? file.Tag.Performers[0] : "Unknown Artist";
                    Temp.Add(new Music
                    {
                        Id=Temp.Count,
                        Title = title,
                        Artist = artist,
                        Duration = file.Properties.Duration.ToString(@"hh\:mm\:ss"),
                        Uri = mp3Files[i]
                    });
                    /*
                    Console.WriteLine("Title: " + file.Tag.Title);
                    Console.WriteLine("Artist: " + file.Tag.Performers[0]);
                    Console.WriteLine("Album: " + file.Tag.Album);
                    Console.WriteLine("Genre: " + file.Tag.Genres[0]);
                    Console.WriteLine("Year: " + file.Tag.Year);
                    Console.WriteLine("Duration: " + file.Properties.Duration);
                    */
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류 발생: " + ex.Message);
                }
            }
        }


        

    }

    public class Music
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? Duration { get; set; }
        public string? Uri { get; set; }
    }

    


}
