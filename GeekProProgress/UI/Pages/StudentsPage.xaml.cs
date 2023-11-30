using GeekProProgress.Classes;
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

namespace GeekProProgress.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для StudentsPage.xaml
    /// </summary>
    public partial class StudentsPage : Page
    {
        BindingList<JournalHelper> StudentsList = new BindingList<JournalHelper>();
        public StudentsPage()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;
            DGView();
            Reload();
        }

        private void DGView()
        {
            using(GeekProEntities db = new GeekProEntities())
            {
                var students = db.Students;
                foreach (Students student in students)
                {
                    JournalHelper journalHelper = new JournalHelper();
                    journalHelper.studentsName = student.name;
                    journalHelper.studentsSurname = student.surname;
                    journalHelper.studentsPatronymic = student.Patronymic;
                    journalHelper.StudentID = student.id;
                    StudentsList.Add(journalHelper);
                }
            }
            dgStudents.ItemsSource = StudentsList;
        }

        private void Reload()
        {
            cmbStudents.ItemsSource = GeekProEntities.GetContext().Students.ToList();
            cmbTeachers.ItemsSource = GeekProEntities.GetContext().Users.Where(post => post.postTitle == "Преподаватель").ToList();
            GeekProEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
        }
        private void btnAddEntry_Click(object sender, RoutedEventArgs e)
        {
            if(cmbStudents.SelectedItem == null || cmbTeachers.SelectedItem == null || cmbGrade.SelectedItem == null
               || tbWorkTitle.Text == "" || dpDate.SelectedDate.Value.Date == null)
            {
                Manager.ErrorMessage("Не все обязательные поля заполнены");
                return;
            }
            
            using (GeekProEntities db = new GeekProEntities())
            {
                Journal journal = new Journal
                {
                    id_student = db.Students.FirstOrDefault(id => id.surname.Contains(cmbStudents.Text)).id,
                    numGrade = int.Parse(cmbGrade.Text),
                    title = tbWorkTitle.Text,
                    date = dpDate.SelectedDate.Value.Date,
                    id_user = db.Users.FirstOrDefault(id => id.lName.Contains(cmbTeachers.Text)).id
                };
                try
                {
                    db.Journal.Add(journal);
                    db.SaveChanges();
                    Manager.InformationMessage("Данные успешно добавлены");
                    cmbStudents.Text = null;
                    cmbGrade.Text = null;
                    cmbTeachers.Text = null;

                }
                catch (Exception ex)
                {
                    Manager.ErrorMessage(ex.Message);
                }
            }
        }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            using (GeekProEntities db = new GeekProEntities())
            {
                if(dgStudents.SelectedIndex == -1)
                {
                    Manager.ErrorMessage("Выберите строку для удаления");
                    return;
                }
                for (int i = 0; i < StudentsList.Count; i++)
                {
                    if (i == dgStudents.SelectedIndex)
                    {
                        int id = StudentsList[i].StudentID;
                        var l = db.Students.Find(id);
                        var h = db.Users.Find(id);
                        try
                        {
                            db.Students.Remove(l);
                            db.Users.Remove(h);
                            db.SaveChanges();
                            Manager.InformationMessage("Данные успешно удалены");
                        }
                        catch
                        {
                            Manager.ErrorMessage("Возникла непредвиденная ошибка");
                        }
                    }
                }
            }
            Reload();
            StudentsList.Remove(StudentsList[dgStudents.SelectedIndex]);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
           MainFrame.Navigate(new ResultsPage());
           gpColumn2.Visibility = Visibility.Hidden;
           column1.Visibility = Visibility.Hidden;
           textBlock.Visibility = Visibility.Hidden;
           btnBack.Visibility = Visibility.Hidden;
        }
     
    }
}
