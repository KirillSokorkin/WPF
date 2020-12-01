using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab2.ZapisModel;

namespace Lab2
{
    class Paging
    {
        public int PageNumber { get; set; }
        DataTable PagedList = new DataTable();

		private DataTable PagedTable (ArrayList list,bool rez)
		{
            if (rez)
            {
                Type columnType = typeof(Zapis);
                DataTable table = new DataTable();

                foreach (var Column in columnType.GetProperties())
                {
                    table.Columns.Add(Column.Name, Column.PropertyType);
                }
                List<Zapis> list1 = new List<Zapis>();
                foreach (Zapis item in list)
                {
                    list1.Add(item);
                }
                foreach (object item in list1)
                {
                    DataRow ReturnTableRow = table.NewRow();
                    foreach (var Column in columnType.GetProperties())
                    {
                        ReturnTableRow[Column.Name] = Column.GetValue(item);
                    }
                    table.Rows.Add(ReturnTableRow);
                }
                return table;
            }
            else
            {
                Type columnType = typeof(ZapisMin);
                DataTable table = new DataTable();

                foreach (var Column in columnType.GetProperties())
                {
                    table.Columns.Add(Column.Name, Column.PropertyType);
                }
                List<ZapisMin> list1 = new List<ZapisMin>();
                foreach (ZapisMin item in list)
                {
                    list1.Add(item);
                }
                foreach (object item in list1)
                {
                    DataRow ReturnTableRow = table.NewRow();
                    foreach (var Column in columnType.GetProperties())
                    {
                        ReturnTableRow[Column.Name] = Column.GetValue(item);
                    }
                    table.Rows.Add(ReturnTableRow);
                }
                return table;
            }
			
		}
		public DataTable SetPaging(ArrayList list, int ZapisPerPage,bool rez)
		{
            if (rez)
            {
                int PageGroup = PageNumber * ZapisPerPage;

                List<Zapis> PagedList = new List<Zapis>();

                List<Zapis> list1 = new List<Zapis>();
                foreach (Zapis item in list)
                {
                    list1.Add(item);
                }

                PagedList = list1.Skip(PageGroup).Take(ZapisPerPage).ToList();

                ArrayList PagedList1 = new ArrayList();
                foreach (Zapis item in PagedList)
                {
                    PagedList1.Add(item);
                }

                DataTable FinalPaging = PagedTable(PagedList1,rez);

                return FinalPaging;
            }
            else
            {
                int PageGroup = PageNumber * ZapisPerPage;

                List<ZapisMin> PagedList = new List<ZapisMin>();

                List<ZapisMin> list1 = new List<ZapisMin>();
                foreach (ZapisMin item in list)
                {
                    list1.Add(item);
                }

                PagedList = list1.Skip(PageGroup).Take(ZapisPerPage).ToList(); //This is where the Magic Happens. If you have a Specific sort or want to return ONLY a specific set of columns, add it to this LINQ Query.

                ArrayList PagedList1 = new ArrayList();
                foreach (ZapisMin item in PagedList)
                {
                    PagedList1.Add(item);
                }

                DataTable FinalPaging = PagedTable(PagedList1,rez);

                return FinalPaging;
            }
			
		}
        public DataTable Next(ArrayList list, int RecordsPerPage, bool rez)
        {
            PageNumber++;
            if (PageNumber >= list.Count / RecordsPerPage)
            {
                PageNumber = list.Count / RecordsPerPage;
            }
            PagedList = SetPaging(list, RecordsPerPage,rez);
            return PagedList;
        }
        public DataTable Previous(ArrayList list, int RecordsPerPage, bool rez)
        {
            PageNumber--;
            if (PageNumber <= 0)
            {
                PageNumber = 0;
            }
            PagedList = SetPaging(list, RecordsPerPage,rez);
            return PagedList;
        }
        public DataTable First(ArrayList list, int RecordsPerPage, bool rez)
        {
            PageNumber = 0;
            PagedList = SetPaging(list, RecordsPerPage,rez);
            return PagedList;
        }
        public DataTable Last(ArrayList list, int RecordsPerPage, bool rez)
        {
            PageNumber = list.Count / RecordsPerPage;
            PagedList = SetPaging(list, RecordsPerPage,rez);
            return PagedList;
        }
        
    }

}
