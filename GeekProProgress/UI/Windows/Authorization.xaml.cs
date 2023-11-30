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
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }
        private void lbRegistration_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Registration reg = new Registration();
            Hide();
            reg.Show();
        }
        private void cbViewHidePassword_Click(object sender, RoutedEventArgs e)
        {
            if (cbViewHidePassword.IsChecked == true)
            {
                tbPassword.Text = pbPassword.Password;
                tbPassword.Visibility = Visibility.Visible;
                pbPassword.Visibility = Visibility.Hidden;
            }
            else
            {
                pbPassword.Password = tbPassword.Text;
                tbPassword.Visibility = Visibility.Hidden;
                pbPassword.Visibility = Visibility.Visible;
            }
        }
        private void btnAuthorization_Click(object sender, RoutedEventArgs e)
        {
            if (tbLogin.Text == "" || pbPassword.Password == "")
            {
                Manager.ErrorMessage("Не все обязательные поля заполнены");
                return;
            }

            using(GeekProEntities db = new GeekProEntities())
            {
                var users = db.Users.FirstOrDefault(log => log.login == tbLogin.Text);

                if (users.password.Contains(pbPassword.Password))
                {
                    if (users.teacherKey == null)
                    {
                        db.AuthUsers.Add(new AuthUsers
                        {
                            id_user = users.id,
                            authDate = DateTime.Now,
                        });
                        var userID = db.Students.FirstOrDefault(k => k.id_user == users.id);
                        int i = userID.id;
                        db.SaveChanges();
                        PartWindow partWindow = new PartWindow(i);
                        partWindow.Show();
                        Close();
                    }
                    else if (users.teacherKey != null)
                    {
                        db.AuthUsers.Add(new AuthUsers
                        {
                            id_user = users.id,
                            authDate = DateTime.Now,
                        });
                        db.SaveChanges();
                        BaseWindow baseWindow = new BaseWindow();
                        baseWindow.Show();
                        Close();
                    }
                }
                else 
                    Manager.ErrorMessage("Неверный логин или пароль");
            }          
        }
    }
}
