using JunProject.utilitys;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JunProject.ViewModels
{
    public class testPageViewModel:ViewModelBase
    {
        public testPageViewModel()
        {
            _infomations = new ObservableCollection<infomation>();

        }

        private ObservableCollection<infomation> _infomations;
        public ObservableCollection<infomation> Infomations
        {
            get { return _infomations; }
            set
            {
                _infomations = value;
                OnPropertyChanged(nameof(Infomations));

            }
        }

        private infomation_st _temp;
        public infomation_st Temp
        {
            get { return _temp; }
            set { _temp = value; OnPropertyChanged(nameof(Temp)); }
        }



        private infomation _selectedInfo;
        public infomation SelectedInfo
        {
            get { return _selectedInfo; }
            set
            {
                _selectedInfo = value;
                OnPropertyChanged(nameof(SelectedInfo));
                if (SelectedInfo != null)
                {
                    Temp = new infomation_st
                    {
                        Id = SelectedInfo.Id.ToString(),
                        Name = SelectedInfo.Name,
                        Age = SelectedInfo.Age.ToString(),
                        Birthday = SelectedInfo.Birthday,
                        Salary = SelectedInfo.Salary.ToString()
                    };
                }
            }
        }

        private string? _searchInput;
        public string? SearchInput
        {
            get { return _searchInput; }
            set
            {
                if (_searchInput != value) // 값이 변경되었는지 확인
                {
                    _searchInput = value;
                    OnPropertyChanged(nameof(SearchInput));
                    Search(); // 값이 변경되면 검색 실행
                    if (_searchInput == null)
                    {
                        manager.Select(Infomations);
                    }
                }
            }
        }

        private string? _searchSelected;
        public string? SearchSelected
        {
            get { return _searchSelected; }
            set { _searchSelected = value; OnPropertyChanged(nameof(SearchSelected)); manager.Select(Infomations); Search(); }

        }
        public void Search()
        {
            if (SearchInput != null && SearchSelected != null)
            {
                manager.Select(Infomations);

                if (SearchSelected.EndsWith("이름"))
                {
                    var result = from info in Infomations
                                 where info.Name.Contains(SearchInput)
                                 select info;
                    Infomations = new ObservableCollection<infomation>(result);
                }
                else if (SearchSelected.EndsWith("나이"))
                {
                    var result = from info in Infomations
                                 where info.Age.ToString().Contains(SearchInput)
                                 select info;
                    Infomations = new ObservableCollection<infomation>(result);
                }
                else if (SearchSelected.EndsWith("생년월일"))
                {
                    var result = from info in Infomations
                                 where info.Birthday.Contains(SearchInput)
                                 select info;
                    Infomations = new ObservableCollection<infomation>(result);
                }
                else if (SearchSelected.EndsWith("연봉"))
                {
                    var result = from info in Infomations
                                 where info.Salary.ToString().Contains(SearchInput)
                                 select info;
                    Infomations = new ObservableCollection<infomation>(result);
                }

            }

        }
        public void Search(object _)
        {
            Search();
        }

        /* DataBase */

        MySQLManager manager = new MySQLManager();

        string connectionString = "UID=root;PWD=1234;Server=127.0.0.1;Port=3306;Database=CRUD";

        public void Connection(object _)
        {
            manager.Connection(connectionString);
            manager.Select(Infomations);
        }

        public void Select()
        {
            manager.Select(Infomations);
        }

        public void Insert(object _)
        {
            manager.Insert(Temp);
            manager.Select(Infomations);
            Set_Clear();
        }

        public void Delete(object _)
        {
            manager.Delete(Temp);
            manager.Select(Infomations);
            Set_Clear();
        }

        public void Update(object _)
        {
            manager.Update(Temp);
            manager.Select(Infomations);
            Set_Clear();
        }

        public void Set_Clear()
        {
            Temp = new infomation_st
            {
                Name = null,
                Age = null,
                Birthday = null,
                Salary = null,
            };
        }

        public ICommand ConnectCommand => new RelayCommand<object>(Connection);
        public ICommand InsertCommand => new RelayCommand<object>(Insert);
        public ICommand UpdateCommand => new RelayCommand<object>(Update);
        public ICommand DeleteCommand => new RelayCommand<object>(Delete);
        public ICommand ChangedCommand => new RelayCommand<object>(Search);
    }
}
