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
using System.Windows.Shapes;
using GeekProProgress.Classes;

namespace GeekProProgress.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для PartWindow.xaml
    /// </summary>
    public partial class PartWindow : Window
    {
        BindingList<JournalHelper> JournalList = new BindingList<JournalHelper>();
        int id = 0;
        public PartWindow(int id)
        {
            InitializeComponent();
            this.id = id;
            DGView();
        }
        private void DGView()
        {
            using (GeekProEntities db = new GeekProEntities())
            {
                var journals = db.Journal;
                foreach (Journal journal in journals)
                {
                    if(journal.Students.id == id)
                    {
                        JournalHelper journalHelper = new JournalHelper();
                        journalHelper.FIO = journal.Students.surname + " " + journal.Students.name + " " + journal.Students.Patronymic;
                        journalHelper.grade = journal.numGrade.ToString();
                        journalHelper.workTitle = journal.title;
                        journalHelper.ID = journal.id;
                        journalHelper.teacherId = journal.Users.lName + " " + journal.Users.fName + " " + journal.Users.patronymic;
                        journalHelper.date = journal.date.ToString();
                        JournalList.Add(journalHelper);
                    }                                   
                }
            }
            dgStudents.ItemsSource = JournalList;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (dpStart.SelectedDate == null || dpEnd.SelectedDate == null)
            {
                Manager.ErrorMessage("Выберите даты");
                return;
            }
            JournalList.Clear();
            using (GeekProEntities db = new GeekProEntities())
            {
                foreach (Journal journal in db.Journal.OrderBy(s => s.numGrade))
                {
                    if (dpStart.SelectedDate.Value.Date <= journal.date && dpEnd.SelectedDate.Value.Date >= journal.date)
                    {
                        JournalHelper journalHelper = new JournalHelper
                        {
                            teacherId = journal.Users.lName + " " + journal.Users.fName + " " + journal.Users.patronymic,
                            grade = journal.numGrade.ToString(),
                            workTitle = journal.title,
                            date = journal.date.ToString()
                        };
                        JournalList.Add(journalHelper);                       
                    }
                }
            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            JournalList.Clear();
            DGView();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            Close();
        }
    }
}
