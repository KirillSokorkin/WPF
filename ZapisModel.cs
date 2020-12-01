using System;
using System.Collections;
using System.Net;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
namespace Lab2
{
    class ZapisModel
    {
        //Класс для создания Полных Записей
        public class Zapis
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Sourse { get; set; }
            public string Target { get; set; }
            public bool Privacy { get; set; }
            public bool Integrity { get; set; }
            public bool Availability { get; set; }

            public override string ToString()
            {
                return $"{Id}|{Name}|{Description}|{Sourse}|{Target}|{Privacy}|{Integrity}|{Availability}";
            }
        }

        //Класс для создания Кратких Записей
        public class ZapisMin
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                return $"{Id}|{Name}";
            }
        }
        //Метод загрузки БД
        public void Download()
        {
            WebClient client = new WebClient();
            client.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", @"C:\Users\Кирилл\Desktop\thrlist.xlsx");
        }
        //Возвращает Коллекцию записей из БД
        public ArrayList GetData(bool rez)
        {
            ArrayList listZapisey = new ArrayList();
            int count = 1;
            string FileName = @"C:\Users\Кирилл\Desktop\thrlist.xlsx";
            object rOnly = true;
            object SaveChanges = false;
            object MissingObj = System.Reflection.Missing.Value;

            Excel.Application app = new Excel.Application();
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Sheets sheets = null;
            workbooks = app.Workbooks;
            workbook = workbooks.Open(FileName, MissingObj, rOnly, MissingObj, MissingObj,
                                        MissingObj, MissingObj, MissingObj, MissingObj, MissingObj,
                                        MissingObj, MissingObj, MissingObj, MissingObj, MissingObj);

            // Получение всех страниц докуента
            sheets = workbook.Sheets;

            foreach (Excel.Worksheet worksheet in sheets)
            {
                Excel.Range UsedRange = worksheet.UsedRange;
                Excel.Range urRows = UsedRange.Rows;
                Excel.Range urColums = UsedRange.Columns;

                int RowsCount = urRows.Count;
                int ColumnsCount = urColums.Count;

                int id = 0;
                string name = null;
                string description = null;
                string sourse = null;
                string target = null;
                bool privacy = true;
                bool integrity = true;
                bool availability = true;

                if (rez)
                {
                    Zapis zapis;
                    for (int i = 3; i <= RowsCount; i++)
                    {
                        for (int j = 1; j <= ColumnsCount; j++)
                        {
                            Excel.Range CellRange = UsedRange.Cells[i, j];
                            // Получение текста ячейки
                            string CellText = (CellRange == null || CellRange.Value2 == null) ? null :
                                                (CellRange as Excel.Range).Value2.ToString();
                            if (count == 1)
                            {
                                id = Convert.ToInt32(CellText);
                                count++;
                            }
                            else if (count == 2)
                            {
                                name = CellText;
                                count++;
                            }
                            else if (count == 3)
                            {
                                description = CellText;
                                count++;
                            }
                            else if (count == 4)
                            {
                                sourse = CellText;
                                count++;
                            }
                            else if (count == 5)
                            {
                                target = CellText;
                                count++;
                            }
                            else if (count == 6)
                            {
                                if (CellText == "0")
                                {
                                    privacy = false;
                                }
                                else
                                {
                                    privacy = true;
                                }

                                count++;
                            }
                            else if (count == 7)
                            {
                                if (CellText == "0")
                                {
                                    integrity = false;
                                }
                                else
                                {
                                    integrity = true;
                                }

                                count++;
                            }
                            else if (count == 8)
                            {
                                if (CellText == "0")
                                {
                                    availability = false;
                                }
                                else
                                {
                                    availability = true;
                                }
                                count++;
                            }
                            else if (count == 9)
                            {
                                count++;
                            }
                            else if (count == 10)
                            {
                                zapis = new Zapis { Id = id, Name = name, Description = description, Sourse = sourse, Target = target, Privacy = privacy, Integrity = integrity, Availability = availability };
                                listZapisey.Add(zapis);
                                count = 1; ;
                                id = 0;
                                name = null;
                                description = null;
                                sourse = null;
                                target = null;
                                privacy = true;
                                integrity = true;
                                availability = true;
                            }
                        }
                    }
                }
                else
                {
                    ZapisMin zapis;
                    for (int i = 3; i <= RowsCount; i++)
                    {
                        for (int j = 1; j <= ColumnsCount; j++)
                        {
                            Excel.Range CellRange = UsedRange.Cells[i, j];
                            // Получение текста ячейки
                            string CellText = (CellRange == null || CellRange.Value2 == null) ? null :
                                                (CellRange as Excel.Range).Value2.ToString();
                            if (count == 1)
                            {
                                description = "УБИ." + Convert.ToInt32(CellText);
                                count++;
                            }
                            else if (count == 2)
                            {
                                name = CellText;
                                count++;
                            }
                            else if (count == 3)
                            {

                                count++;
                            }
                            else if (count == 4)
                            {

                                count++;
                            }
                            else if (count == 5)
                            {

                                count++;
                            }
                            else if (count == 6)
                            {


                                count++;
                            }
                            else if (count == 7)
                            {


                                count++;
                            }
                            else if (count == 8)
                            {

                                count++;
                            }
                            else if (count == 9)
                            {
                                count++;
                            }
                            else if (count == 10)
                            {
                                zapis = new ZapisMin { Id = description, Name = name };
                                listZapisey.Add(zapis);
                                count = 1; ;
                                id = 0;
                                name = null;
                                description = null;
                                sourse = null;
                                target = null;
                                privacy = true;
                                integrity = true;
                                availability = true;
                            }
                        }
                    }
                }
            }
            return listZapisey;
        }
    }
}
