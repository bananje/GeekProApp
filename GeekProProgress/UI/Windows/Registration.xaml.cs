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
using GeekProProgress.Classes;

namespace GeekProProgress.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        private void DBInsert(string key, string[] FIO, string login, string password,string postTitle)
        {
            using (GeekProEntities db = new GeekProEntities())
            {
                try
                {
                    db.Users.Add(new Users
                    {
                        login = login,
                        password = password,
                        teacherKey = key,
                        lName = FIO[0],
                        fName = FIO[1],
                        patronymic = FIO[2],
                        postTitle = postTitle
                    });                    
                    db.SaveChanges();
                    if (postTitle == "Студент")
                    {
                        db.Students.Add(new Students
                        {
                            id_user = db.Users.OrderByDescending(x => x.id).FirstOrDefault().id,
                            surname = FIO[0],
                            name = FIO[1],
                            Patronymic = FIO[2],
                        });
                    }
                    db.SaveChanges();
                    tbFIO.Clear();
                    tbkey.Clear();
                    tbLogin.Clear();
                    tbPassword.Clear();
                    Manager.InformationMessage("Успешная регистрация");

                }
                catch
                {
                    Manager.ErrorMessage("ФИО вводится через пробел");
                }
            }

        }
        private void cbInputKey_Click(object sender, RoutedEventArgs e)
        {
            if (cbInputKey.IsChecked == true)
                tbkey.Visibility = Visibility.Visible;
            else
                tbkey.Visibility = Visibility.Hidden;
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            bool validation = Validator.ValidationPassword(tbFIO.Text,tbLogin.Text,tbPassword.Text);
            if (validation == false)
                return;

            using (GeekProEntities db = new GeekProEntities())
            {
                string[] FIO = tbFIO.Text.Split(' ');
                string postTitle = "";
                bool result = db.Users.Any(log => log.login == tbLogin.Text);

                if (result == true)
                {
                    Manager.ErrorMessage("Введённый логин уже зарегистрирован");
                    return;
                }

                if (cbInputKey.IsChecked == true)
                {
                    if (tbkey.Text != Manager.key)
                    {
                        Manager.ErrorMessage("Введён неверный ключ");
                        tbkey.Text = "";
                        return;
                    }
                    else
                    {
                        postTitle = "Преподаватель";
                        DBInsert(tbkey.Text, FIO, tbLogin.Text, tbPassword.Text,postTitle);
                    }
                }
                else
                {
                    postTitle = "Студент";
                    DBInsert(null, FIO, tbLogin.Text, tbPassword.Text,postTitle);                   
                }
            }

        }

        private void lbBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Authorization auth = new Authorization();
            this.Close();
            auth.Show();
        }

    }
}
