using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using static Lab2.ZapisModel;
using Excel = Microsoft.Office.Interop.Excel;


namespace Lab2
{
    public partial class MainWindow : Window
    {
        private int zapiseyOnPage;
        static Paging paging = new Paging();
        static ZapisModel zapismodel = new ZapisModel();
        static bool rez = true;
        ArrayList myList;
        static string LocalBase = @"C:\Users\Кирилл\Desktop\thrlist.xlsx";
        static string SaveFile = @"C:\Users\Кирилл\Desktop\1.xlsx";
        public MainWindow()
        {
            InitializeComponent();
            if (!System.IO.File.Exists(LocalBase))
            {
                string text = "Локальная БД не найдена и была загруженна из Космической Сети Интернет";
                label.Content = text;
                zapismodel.Download();
            }
            else
            {
                string text = "Работа на локальной БД";
                label.Content = text;
            }
            myList = zapismodel.GetData(rez);
            paging.PageNumber = 1;
            int[] RecordsToShow = { 10, 20, 30, 50, 100 };
            foreach (int RecordGroup in RecordsToShow)
            {
                NumberOfRecords.Items.Add(RecordGroup);
            }

            NumberOfRecords.SelectedItem = 10;

            zapiseyOnPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
            DataTable firstTable = paging.SetPaging(myList, zapiseyOnPage, rez);

            dataGrid.ItemsSource = firstTable.DefaultView;
        }
        public string PageNumberDisplay()
        {
            int PagedNumber = zapiseyOnPage * (paging.PageNumber + 1);
            if (PagedNumber > myList.Count)
            {
                PagedNumber = myList.Count;
            }
            return "Showing " + PagedNumber + " of " + myList.Count;
        }
        private void NumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zapiseyOnPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
            dataGrid.ItemsSource = paging.First(myList, zapiseyOnPage, rez).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void Rez_Click(object sender, RoutedEventArgs e)
        {
            string text = "Режим изменён";
            rez = !rez;

            label.Content = text;
            myList = zapismodel.GetData(rez);
            paging.PageNumber = 1;

            zapiseyOnPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
            DataTable firstTable = paging.SetPaging(myList, zapiseyOnPage, rez);

            dataGrid.ItemsSource = firstTable.DefaultView;

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string text;
            zapismodel.Download();

            ArrayList oldList = myList;
            List<string> raznost = new List<string>();
            myList = zapismodel.GetData(rez);
            paging.PageNumber = 1;

            zapiseyOnPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
            DataTable firstTable = paging.SetPaging(myList, zapiseyOnPage, rez);

            dataGrid.ItemsSource = firstTable.DefaultView;
            for (int i = 0; i < oldList.Count; i++)
            {
                if (oldList[i].ToString() != myList[i].ToString())
                {
                    string[] oldStr = oldList[i].ToString().Split('|');
                    string[] myStr = myList[i].ToString().Split('|');
                    string raz = $"Изменение в УБИ.{i}: ";
                    for (int j = 0; j < oldStr.Length; j++)
                    {
                        if (oldStr[j] != myStr[j])
                        {
                            if (j == 0)
                            {
                                raz += "Номер " + oldStr[j] + "--->" + myStr[j] + "___";
                            }
                            else if (j == 1)
                            {
                                raz += "Наименование " + oldStr[j] + "--->" + myStr[j] + "___";
                            }
                            else if (j == 2)
                            {
                                raz += "Описание " + oldStr[j] + "--->" + myStr[j] + "___";
                            }
                            else if (j == 3)
                            {
                                raz += "Источник " + oldStr[j] + "--->" + myStr[j] + "___";
                            }
                            else if (j == 4)
                            {
                                raz += "Цель " + oldStr[j] + "--->" + myStr[j] + "___";
                            }
                            else if (j == 5)
                            {
                                raz += "Нарушение конфид. " + oldStr[j] + "--->" + myStr[j] + "___";
                            }
                            else if (j == 6)
                            {
                                raz += "Нарушение целостн. " + oldStr[j] + "--->" + myStr[j] + "___";
                            }
                            else if (j == 7)
                            {
                                raz += "Нарушение доступ. " + oldStr[j] + "--->" + myStr[j] + "___";
                            }

                        }
                    }
                    raznost.Add(raz);
                }
            }

            text = $"Обновленно: {raznost.Count} элементов";
            foreach (var item in raznost)
            {
                text += "\n" + item;
            }
            label.Content = text;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workBook = excelApp.Workbooks.Add();
            Excel.Worksheet workSheet = workBook.ActiveSheet;

            if (rez)
            {
                int i = 2;
                workSheet.Cells[1, 1] = "Идентификатор УБИ";
                workSheet.Cells[1, 2] = "Наименование УБИ";
                workSheet.Cells[1, 3] = "Описание";
                workSheet.Cells[1, 4] = "Источник угрозы (характеристика и потенциал нарушителя)";
                workSheet.Cells[1, 5] = "Объект воздействия";
                workSheet.Cells[1, 6] = "Нарушение конфиденциальности";
                workSheet.Cells[1, 7] = "Нарушение целостности";
                workSheet.Cells[1, 8] = "Нарушение доступности";
                foreach (Zapis item in myList)
                {
                    string[] str = item.ToString().Split('|');
                    workSheet.Cells[i, 1] = str[0];
                    workSheet.Cells[i, 2] = str[1];
                    workSheet.Cells[i, 3] = str[2];
                    workSheet.Cells[i, 4] = str[3];
                    workSheet.Cells[i, 5] = str[4];
                    workSheet.Cells[i, 6] = str[5];
                    workSheet.Cells[i, 7] = str[6];
                    workSheet.Cells[i, 8] = str[7];
                    i++;
                }

            }
            else
            {
                int i = 2;
                workSheet.Cells[1, 1] = "Идентификатор УБИ";
                workSheet.Cells[1, 2] = "Наименование УБИ";
                foreach (ZapisMin item in myList)
                {
                    string[] str = item.ToString().Split('|');
                    workSheet.Cells[i, 1] = str[0];
                    workSheet.Cells[i, 2] = str[1];
                    i++;
                }
            }

            workBook.Close(true, SaveFile);
        }

        private void Backwards_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = paging.Previous(myList, zapiseyOnPage, rez).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void First_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = paging.First(myList, zapiseyOnPage, rez).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Last_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = paging.Last(myList, zapiseyOnPage, rez).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = paging.Next(myList, zapiseyOnPage, rez).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
    }

}
