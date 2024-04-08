using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using JunProject.utilitys;

namespace JunProject
{
    public class MySQLManager
    {
        /* DataBase */

        private MySqlConnection? _conn = default;

        public void Connection(string connectionString)
        {
            try
            {
                _conn = new MySqlConnection(connectionString);
                _conn.Open();
                //MessageBox.Show("연결되었습니다");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"connection 실패 \n {ex.Message}");

            }
        }

        public void Select(ObservableCollection<infomation> Infomations)
        {
            DataTable dt = new DataTable();
            string query = "select * from info;";
            using (MySqlCommand cmd = _conn.CreateCommand())
            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
            {
                cmd.CommandText = query;
                da.Fill(dt);
            }
            Infomations.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                string birthday = ((DateTime)dr["birthday"]).ToString("yyyy-MM-dd");
                Infomations.Add(new infomation() { Id = (int)dr["id"], Name = (string)dr["name"], Age = (int)dr["age"], Birthday = birthday, Salary = (int)dr["salary"] });
            }
        }

        public void Insert(infomation_st Temp)
        {
            if (Temp!=null)
            {
                try
                {
                    string query = "insert into info(name, age, birthday, salary) values" + "(" + "?name, ?age, ?birthday, ?salary" + ");";
                    using (MySqlCommand cmd = _conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("?name", Temp.Name);
                        cmd.Parameters.AddWithValue("?age", Temp.Age);
                        cmd.Parameters.AddWithValue("?birthday", Temp.Birthday);
                        cmd.Parameters.AddWithValue("?salary", Temp.Salary);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("insert 성공");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"insert 실패 \n {ex.Message}");

                }
            }
        }

        public void Delete(infomation_st Temp)
        {
            if (Temp != null)
            {
                try
                {
                    string query = "delete from info where" + " " + "id=?id" + ";";
                    using (MySqlCommand cmd = _conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("?id", Temp.Id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("delete 성공");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"insert 실패 \n {ex.Message}");

                }
            }
        }

        public void Update(infomation_st Temp)
        {
            if (Temp != null)
            {
                try
                {
                    string query = "update info set name=?name, age=?age, birthday=?birthday, salary=?salary where id=?id;";
                    using (MySqlCommand cmd = _conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("?name", Temp.Name);
                        cmd.Parameters.AddWithValue("?age", Temp.Age);
                        cmd.Parameters.AddWithValue("?birthday", Temp.Birthday);
                        cmd.Parameters.AddWithValue("?salary", Temp.Salary);
                        cmd.Parameters.AddWithValue("?Id", Temp.Id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("update 성공");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"update 실패 \n {ex.Message}");

                }
            }
        }

        public void Insert_Regist(Member Temp)
        {
            try
            {
                string query = "insert into logintable(loginId, loginPw) values (?loginId, ?loginPw);";
                using (MySqlCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("?loginId", Temp.Id);
                    cmd.Parameters.AddWithValue("?loginPw", Temp.Pw);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("회원가입 완료");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"insert 실패 \n {ex.Message}");

            }
            Close_Connection();
        }

        public int Check_info(string id, string pw)
        {
            string query = "SELECT COUNT(*) FROM logintable WHERE loginId = ?loginId AND loginPw = ?loginPw;";
            try
            {
                using (MySqlCommand cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("?loginId", id);
                    cmd.Parameters.AddWithValue("?loginPw", pw);
                    cmd.ExecuteNonQuery();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 1)
                    {
                        MessageBox.Show("로그인 성공");
                        return 1;
                    }
                    else
                    {
                        MessageBox.Show("로그인 실패"); return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"insert 실패 \n {ex.Message}");
                return 0;

            }
        }

        public void Close_Connection()
        {
            _conn.Close();
        }


        ////////////////////////Todo
        public void Select_Todo(ObservableCollection<Todo> Todos)
        {
            DataTable dt = new DataTable();
            string query = "select * from ToDoList;";
            using (MySqlCommand cmd = _conn.CreateCommand())
            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
            {
                cmd.CommandText = query;
                da.Fill(dt);
            }
            Todos.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                //string date = ((DateTime)dr["date"]).ToString();
                //string done = ((bool)dr["done"]).ToString();
                Todos.Add(new Todo() { Id = (int)dr["id"], Title = (string)dr["title"], Date = dr["date"].ToString(), Etc = (string)dr["etc"], Done= dr["done"].ToString() });
            }
        }

        public void Insert_Todo(Todo Temp)
        {
            if (Temp == null)
            {
                // Temp가 null이면 추가 작업을 건너뜁니다.
                return;
            }
            if (Temp != null)
            {
                try
                {
                    string query = "insert into todolist(title, date, etc, done) values (?title, ?date, ?etc, ?done);";
                    using (MySqlCommand cmd = _conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("?title", Temp.Title);
                        cmd.Parameters.AddWithValue("?date", Temp.Date);
                        cmd.Parameters.AddWithValue("?etc", Temp.Etc);
                        cmd.Parameters.AddWithValue("?done", Temp.Done);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("insert 성공");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"insert 실패 \n {ex.Message}");
                }
            }
        }

        public void Delete_todo(Todo Temp)
        {
            if (Temp != null)
            {
                try
                {
                    string query = "delete from todolist where" + " " + "id=?id" + ";";
                    using (MySqlCommand cmd = _conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("?id", Temp.Id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("delete 성공");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"insert 실패 \n {ex.Message}");

                }
            }
        }

        public void Update_Todo(Todo Temp)
        {
            if (Temp != null)
            {
                try
                {
                    string query = "update todolist set title=?title, date=?date, etc=?etc, done=?done where id=?id;";
                    using (MySqlCommand cmd = _conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("?title", Temp.Title);
                        cmd.Parameters.AddWithValue("?date", Temp.Date);
                        cmd.Parameters.AddWithValue("?etc", Temp.Etc);
                        cmd.Parameters.AddWithValue("?done", Temp.Done);
                        cmd.Parameters.AddWithValue("?Id", Temp.Id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("update 성공");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"update 실패 \n {ex.Message}");
                }
            }
        }
    }
}
