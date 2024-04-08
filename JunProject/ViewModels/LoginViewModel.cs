using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JunProject.utilitys;
namespace JunProject.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        MySQLManager manager = new MySQLManager();

        public LoginViewModel()
        {
            _member1 = new Member();
            _member2 = new Member();
        }
        //Login
        private Member _member1;
        public Member member1
        {
            get { return _member1; }
            set { _member1 = value; OnPropertyChanged(nameof(member1)); }
        }
        //Regist
        private Member _member2;
        public Member member2
        {
            get { return _member2; }
            set { _member2 = value; OnPropertyChanged(nameof(member2)); }
        }

        string connectionString = "UID=root;PWD=1234;Server=127.0.0.1;Port=3306;Database=CRUD";

        public void Regist(object _)
        {
            manager.Connection(connectionString);
            manager.Insert_Regist(member2);
            manager.Close_Connection();
        }

        public void Login(object _)
        {
            manager.Connection(connectionString);
            //확인로직
            manager.Close_Connection();



        }

        public ICommand LoginCommand => new RelayCommand<object>(Login);
        public ICommand RegistCommand => new RelayCommand<object>(Regist);

    }
}
