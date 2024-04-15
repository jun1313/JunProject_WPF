using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Google.Protobuf.WellKnownTypes;
using JunProject.utilitys;
using JunProject.Weather;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.BC;


namespace JunProject.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        GPTManager gptManager;
        public MainViewModel()
        {
            _todos = new ObservableCollection<Todo>();
            _todo_st=new Todo();
            _selectedInfo = new Todo();
            Select();
            _temp =new Todo();
            Weather = new BusanWether();
            
            Player = new MusicPlayer();
            Music_st = new Music();
            //manager.Select_MusicPlayer(Player.Musics);
            //Player.Select_Folder();
            Player.Set_Playlist(Player.Musics);
            //MusicDoubleCommand = new RelayCommand<object>(Music_Double);
            Playing = "Music Player";
            Music_Temp = new Music();
            gptManager = new GPTManager();
            _chat=new ObservableCollection<Chatting>();
            GetWeatherAsync();
        }
        private string _playing { get; set; }
        public string Playing { get { return _playing; } set { _playing = value; OnPropertyChanged(nameof(Playing)); } }

        private ObservableCollection<Todo> _todos;
        public ObservableCollection<Todo> Todos {
            get 
            { 
                return _todos; 
            } 
            set 
            { 
                _todos = value;
                OnPropertyChanged(nameof(Todos));
            } 
        }

        private Todo _temp;
        public Todo Temp
        {
            get { return _temp; }
            set { _temp = value; OnPropertyChanged(nameof(Temp)); ; }
        }

        private Todo _todo_st;
        public Todo Todo_st
        {
            get { return _todo_st; }
            set { _todo_st = value; OnPropertyChanged(nameof(Todo_st));}
        }

        private DateTime _date_temp;
        public DateTime Date_temp { get { return _date_temp; } set { _date_temp = value; OnPropertyChanged(nameof(Date_temp)); Todo_st.Date = Date_temp.ToString("yyyy-MM-dd"); } }

        private Todo _selectedInfo;
        public Todo SelectedInfo
        {
            get { return _selectedInfo; }
            set
            {
                _selectedInfo = value;
                OnPropertyChanged(nameof(SelectedInfo));
                if (SelectedInfo != null)
                {
                    Temp = new Todo
                    {
                        Id = SelectedInfo.Id,
                        Title = SelectedInfo.Title,
                        Date = SelectedInfo.Date,
                        Etc = SelectedInfo.Etc,
                        Done = SelectedInfo.Done
                    };
                }
            }
        }

        private BusanWether _weather;
        public BusanWether Weather
        {
            get => _weather; set { _weather = value; OnPropertyChanged(nameof(Weather)); }
        }

        public async Task GetWeatherAsync()
        {
            await Weather.UpdateWeatherAsync();
            OnPropertyChanged(nameof(Weather));
        }

        private MusicPlayer _player;
        public MusicPlayer Player
        {
            get { return _player; }
            set { _player=value; OnPropertyChanged(nameof(Player)); }
        }

        public Music Music_Temp {  get; set; }

        private Music _music_st;
        public Music Music_st 
        {
            get { return _music_st; }
            set { _music_st = value; OnPropertyChanged(nameof(Music_st));
                if (Music_st != null)
                {
                    Music_Temp = new Music
                    {
                        Id = Music_st.Id,
                        Title = Music_st.Title,
                        Artist = Music_st.Artist,
                        Duration = Music_st.Duration,
                        Uri = Music_st.Uri
                    };
                }
            }
        }

        private Music _music_Playing;
        public Music Music_Playing
        {
            get { return _music_Playing; }
            set { _music_Playing = value; }
        }
        

        MySQLManager manager = new MySQLManager();

        string connectionString = "UID=root;PWD=1234;Server=127.0.0.1;Port=3306;Database=CRUD";

        public void Connection(object _)
        {
            manager.Connection(connectionString);
            manager.Select_Todo(Todos);
        }
        public void Select()
        {
            manager.Connection(connectionString);
            manager.Select_Todo(Todos);
        }

        public void Insert(object _)
        {
            //manager.Insert(Temp);
            manager.Select_Todo(Todos);
            //Set_Clear();
        }

        public void Delete(object _)
        {
            //manager.Delete(Temp);
            manager.Select_Todo(Todos);
            //Set_Clear();
        }

        public void Update(object _)
        {
            //manager.Update(Temp);
            manager.Select_Todo(Todos);
            //Set_Clear();
        }

        public void Insert_Todo(object _)
        {
            manager.Connection(connectionString);
            manager.Insert_Todo(Todo_st);
            manager.Select_Todo(Todos);
            manager.Close_Connection();
            Todo_st.Title = null;
            Todo_st.Etc = null;
        }

        public void Delete_Todo(object _)
        {
            manager.Connection(connectionString);
            manager.Delete_todo(SelectedInfo);
            manager.Select_Todo(Todos);
            manager.Close_Connection();
        }
        public void Delete_Todo_In(object _)
        {
            manager.Connection(connectionString);
            manager.Delete_todo(Temp);
            manager.Select_Todo(Todos);
            manager.Close_Connection();
        }
        public void Update_Todo(object _)
        {
            manager.Connection(connectionString);
            manager.Update_Todo(Temp);
            manager.Select_Todo(Todos);
            manager.Close_Connection();
        }

        //음악 플레이어
        public ICommand PathCommand => new RelayCommand<object>(Set_Path);
        public ICommand BackCommand => new RelayCommand<object>(Back_Music);
        public ICommand StopCommand => new RelayCommand<object>(Stop_Music);
        public ICommand NextCommand => new RelayCommand<object>(Next_Music);
        public ICommand MusicDoubleCommand => new RelayCommand<object>(Music_Double);
        public int playingId;

        public void Back_Music(object _)
        {
            if (playingId > 0)
            {
                Player.mediaPlayer.Stop();
                Player.mediaPlayer.Open(new Uri(Player.Musics[playingId - 1].Uri));
                Player.mediaPlayer.Play();
                Playing = "재생중 : " + Player.Musics[playingId - 1].Title;
                playingId = (int)Player.Musics[playingId - 1].Id;
                Player.playinginfo = 1;
            }
            else MessageBox.Show("첫 노래입니다");
        }

        public void Stop_Music(object _)
        {
            if (Player.playinginfo == 1)
            {
                Player.mediaPlayer.Pause();
                Player.playinginfo = 0;
                Playing = Playing.Replace("재생중", "일시정지");
            }
            else
            {
                Player.mediaPlayer.Play();
                Player.playinginfo = 1;
                Playing = Playing.Replace("일시정지", "재생중");
            }
        }

        public void Next_Music(object _)
        {
            if (playingId < Player.Musics.Count-1)
            {
                Player.mediaPlayer.Stop();
                Player.mediaPlayer.Open(new Uri(Player.Musics[playingId + 1].Uri));
                Player.mediaPlayer.Play();
                Playing = "재생중 : " + Player.Musics[playingId + 1].Title;
                playingId = (int)Player.Musics[playingId + 1].Id;
                Player.playinginfo = 1;
            }
            else MessageBox.Show("마지막 노래입니다");
        }

        public void Set_Path(object _)
        {
            Player.Select_Folder();
            Player.Set_Playlist(Player.Musics);
        }

        public void Music_Double(object _)
        {
            Player.mediaPlayer.Stop();
            string uri = Music_st.Uri;
            Player.mediaPlayer.Open(new Uri(uri));
            Player.mediaPlayer.Volume = 20;
            Player.mediaPlayer.Play();
            Playing = "재생중 : "+Music_st.Title;
            playingId = (int)Music_st.Id;
            Player.playinginfo = 1;
            


        }

        

        private string _inputGPT;
        public string InputGPT
        {
            get { return _inputGPT; }
            set { _inputGPT = value; OnPropertyChanged(nameof(InputGPT)); }
        }

        private string _resultGPT;
        public string ResultGPT
        {
            get { return _resultGPT; }
            set { _resultGPT = value; OnPropertyChanged(nameof(ResultGPT)); } 
        }

        private ObservableCollection<Chatting> _chat;
        public ObservableCollection<Chatting> Chat
        {
            get => _chat; set { _chat = value; OnPropertyChanged(nameof(Chat)); }
        }

        public async void Test_GPT(object _)
        {
            
            string? prompt = InputGPT;
            if (prompt == null) return;
            if (string.IsNullOrWhiteSpace(prompt))
            {
                MessageBox.Show("Please enter a prompt.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var task1 = Task<string>.Run(() => gptManager.GenerateText(prompt));
            Chat.Add(new Chatting { UserText = prompt, GptText=await task1 });
            InputGPT = null;
        }
        
        public ICommand ConnectCommand => new RelayCommand<object>(Connection);
        public ICommand InsertCommand => new RelayCommand<object>(Insert);
        public ICommand UpdateCommand => new RelayCommand<object>(Update);
        public ICommand DeleteCommand => new RelayCommand<object>(Delete);

        public ICommand AddCommand => new RelayCommand<object>(Insert_Todo);
        public ICommand DelCommand => new RelayCommand<object>(Delete_Todo);
        public ICommand DelInCommand => new RelayCommand<object>(Delete_Todo_In);
        public ICommand UpCommand => new RelayCommand<object>(Update_Todo);

        

        public ICommand Input_GPT => new RelayCommand<object>(Test_GPT);
    }
}
