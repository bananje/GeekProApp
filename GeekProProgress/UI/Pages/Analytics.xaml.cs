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
    /// Логика взаимодействия для Analytics.xaml
    /// </summary>
    public partial class Analytics : Page
    {
        BindingList<JournalHelper> JournalList = new BindingList<JournalHelper>();
        List<int> GradesCount = new List<int>();
        double grade2Counter, grade3Counter, grade4Counter, grade5Counter, sum, AVG;
        string FIO = "";


        string Report = "";
        public Analytics()
        {
            InitializeComponent();
            DGView();
            cmbStudents.ItemsSource = GeekProEntities.GetContext().Students.ToList();
        }
        private void DGView()
        {
            using (GeekProEntities db = new GeekProEntities())
            {
                var journals = db.Journal;
                foreach (Journal journal in journals)
                {
                    JournalHelper journalHelper = new JournalHelper();
                    journalHelper.FIO = journal.Students.surname + " " + journal.Students.name + " " + journal.Students.Patronymic;
                    journalHelper.grade = journal.numGrade.ToString();
                    journalHelper.workTitle = journal.title;
                    journalHelper.date = journal.date.ToString();
                    JournalList.Add(journalHelper);                    
                }
            }
            dgReport.ItemsSource = JournalList;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(dpDateStart.SelectedDate == null || dpDateEnd.SelectedDate == null)
            {
                Manager.ErrorMessage("Выберите даты");
                return;
            }
            JournalList.Clear();

            using(GeekProEntities db = new GeekProEntities())
            {
                foreach (Journal journal in db.Journal.OrderBy(s => s.numGrade))
                {
                    if (dpDateStart.SelectedDate.Value.Date <= journal.date && dpDateEnd.SelectedDate.Value.Date >= journal.date && cmbStudents.Text.Contains(journal.Students.surname))
                    {
                        if (journal.numGrade == 2)
                            grade2Counter++;
                        if (journal.numGrade == 3)
                            grade3Counter++;
                        if (journal.numGrade == 4)
                            grade4Counter++;
                        if (journal.numGrade == 5)
                            grade5Counter++;

                        GradesCount.Add(journal.numGrade);
                        sum += journal.numGrade;
                        FIO = journal.Students.surname + " " + journal.Students.name + " " + journal.Students.Patronymic;

                        JournalHelper journalHelper = new JournalHelper
                        {
                            FIO = journal.Students.surname + " " + journal.Students.name + " " + journal.Students.Patronymic,
                            grade = journal.numGrade.ToString(),
                            workTitle = journal.title,
                            date = journal.date.ToString()
                        }; 
                        JournalList.Add(journalHelper);                      
                    }
                }
                AVG = sum / GradesCount.Count;
                for (int i = 0; i < GradesCount.Count; i++)
                {
                    Report = Environment.NewLine
                    + "Cтудент: " + FIO + "\n"
                    + "Даты: от " + dpDateStart.Text + " до" + dpDateEnd.Text + "\n"
                    + "Общее кол-во оценок: " + i + 1 + "\n"
                    + "Оценок '2': " + grade2Counter + " Оценок '3': " + grade3Counter + " Оценок '4': " + grade4Counter + " Оценок '5': " + grade5Counter + "\n"
                    + "Средний балл за указанный период: " + AVG;
                    tbReport.Text = Report;
                }
            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            JournalList.Clear();
            DGView();
        }
    }
}
