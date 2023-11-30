using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using GeekProProgress.Classes;

namespace GeekProProgress.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthUsers.xaml
    /// </summary>
    public partial class AuthUsersPage : Page
    {
        BindingList<UsersHelper> UsersList = new BindingList<UsersHelper>();
        public AuthUsersPage()
        {
            InitializeComponent();
            DGView();
        }
        private void DGView()
        {
            using(GeekProEntities db = new GeekProEntities())
            {
                var authUsers = db.AuthUsers;
                foreach (AuthUsers user in authUsers)
                {
                    UsersHelper usersHelper = new UsersHelper();
                    usersHelper.id = user.id.ToString();
                    usersHelper.DateTime = user.authDate.ToString();
                    usersHelper.postTitle = user.Users.postTitle;
                    UsersList.Add(usersHelper);
                }
                dgAuthUsers.ItemsSource = UsersList;
            }
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            GeekProEntities db = new GeekProEntities();
            var all = from u in db.AuthUsers select u;
            db.AuthUsers.RemoveRange(all);
            db.SaveChanges();
            Manager.InformationMessage("История входов очищена");
            UsersList.Clear();
        }
    }
}
