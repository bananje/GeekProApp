using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;

namespace GeekProProgress.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для ResultsPage.xaml
    /// </summary>
    public partial class ResultsPage : System.Windows.Controls.Page
    {
        BindingList<JournalHelper> JournalList = new BindingList<JournalHelper>();
        string Report = null;

        public ResultsPage()
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
                var journals = db.Journal;
                foreach (Journal journal in journals)
                {
                    JournalHelper journalHelper = new JournalHelper();
                    journalHelper.FIO = journal.Students.surname + " " + journal.Students.name + " " + journal.Students.Patronymic;
                    journalHelper.grade = journal.numGrade.ToString();
                    journalHelper.workTitle = journal.title;
                    journalHelper.ID = journal.id;
                    journalHelper.teacherId = journal.id_user.ToString();
                    journalHelper.date = journal.date.ToString();
                    JournalList.Add(journalHelper);

                    Report += Environment.NewLine
                        + "ФИО (ученик) " + journalHelper.FIO + "\n"
                        + "Оценка: " + journal.numGrade.ToString() + "\n"
                        + "Наимнование работы: " + journal.title + "\n"
                        + "Дата получения оценки: " + journal.date + "\n"
                        + "Преподаватель (ID): " + journal.id_user;

                }
            }
            dgStudents.ItemsSource = JournalList;
        }
        private void Reload()
        {
            GeekProEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            cmbStudents.ItemsSource = GeekProEntities.GetContext().Students.ToList();
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            JournalList.Clear();
            DGView();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (dgStudents.Items == null)
            {
                Manager.ErrorMessage("Обьект печати не содержит данные");
                return;
            }

            if (printDialog.ShowDialog() == true)
                printDialog.PrintVisual(dgStudents, "Печать");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, Report);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            using (GeekProEntities db = new GeekProEntities())
            {
                if(dgStudents.SelectedIndex == -1)
                {
                    Manager.ErrorMessage("Выберите строку для удаления");
                    return;
                }
                for (int i = 0; i < JournalList.Count; i++)
                {
                    if (i == dgStudents.SelectedIndex)
                    {
                        int id = JournalList[i].ID;
                        var l = db.Journal.FirstOrDefault(k => k.id == id);
                        db.Journal.Remove(l);
                        db.SaveChanges();
                    }
                }
            }
            JournalList.Remove(JournalList[dgStudents.SelectedIndex]);
        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < dgStudents.Columns.Count; j++)
            {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = dgStudents.Columns[j].Header;
            }
            for (int i = 0; i < dgStudents.Columns.Count; i++)
            {
                for (int j = 0; j < dgStudents.Items.Count; j++)
                {
                    TextBlock b = dgStudents.Columns[i].GetCellContent(dgStudents.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Visibility = Visibility.Hidden;
            spLeft.Visibility = Visibility.Hidden;
            spRight.Visibility = Visibility.Hidden;
            spLower.Visibility = Visibility.Hidden;
            dgStudents.Visibility = Visibility.Hidden;
            btnReload.Visibility = Visibility.Hidden;
            MainFrame.Navigate(new StudentsPage());
        }     

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(cmbStudents.Text == "")
            {
                Manager.ErrorMessage("Выберите фамилию ученика");
                return;
            }
            JournalList.Clear();
            using (GeekProEntities db = new GeekProEntities())
            {
                var journals = db.Journal;
                foreach (Journal journal in journals)
                {
                    if (cmbStudents.Text == journal.Students.surname)
                    {
                        JournalHelper journalHelper = new JournalHelper
                        {
                            FIO = journal.Students.surname + " " + journal.Students.name + " " + journal.Students.Patronymic,
                            grade = journal.numGrade.ToString(),
                            workTitle = journal.title,
                            date = journal.date.ToString(),
                            teacherId = journal.id_user.ToString()
                        };
                        JournalList.Add(journalHelper);
                    }
                }
            }
            dgStudents.ItemsSource = JournalList;
        }

    }
}

