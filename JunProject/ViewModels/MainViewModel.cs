using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        public MainViewModel()
        {
            _todos = new ObservableCollection<Todo>();
            _todo_st=new Todo();
            _selectedInfo = new Todo();
            _temp=new Todo();

            Weather = new BusanWether();
            GetWeatherAsync();
        }

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
            set { _temp = value; OnPropertyChanged(nameof(Temp));}
        }

        private Todo _todo_st;
        public Todo Todo_st
        {
            get { return _todo_st; }
            set { _todo_st = value; OnPropertyChanged(nameof(Todo_st)); }
        }

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


        MySQLManager manager = new MySQLManager();

        string connectionString = "UID=root;PWD=1234;Server=127.0.0.1;Port=3306;Database=CRUD";

        public void Connection(object _)
        {
            manager.Connection(connectionString);
            manager.Select_Todo(Todos);
        }
        public void Select()
        {
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

        public ICommand ConnectCommand => new RelayCommand<object>(Connection);
        public ICommand InsertCommand => new RelayCommand<object>(Insert);
        public ICommand UpdateCommand => new RelayCommand<object>(Update);
        public ICommand DeleteCommand => new RelayCommand<object>(Delete);

        public ICommand AddCommand => new RelayCommand<object>(Insert_Todo);
        public ICommand DelCommand => new RelayCommand<object>(Delete_Todo);
        public ICommand DelInCommand => new RelayCommand<object>(Delete_Todo_In);
        public ICommand UpCommand => new RelayCommand<object>(Update_Todo);

    }
}
