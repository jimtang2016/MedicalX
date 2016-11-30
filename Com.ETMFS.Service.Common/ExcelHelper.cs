using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;

namespace Com.ETMFS.Service.Common
{
   public class ExcelHelper
    {
       public static DataTable ConvertToDataTable(string filepath, int cols)
       {

           DataTable dt = new DataTable();
           using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
           {

               var hssfworkbook = new XSSFWorkbook(file);
               var sheet = hssfworkbook.GetSheetAt(0);
               System.Collections.IEnumerator rows = sheet.GetRowEnumerator();


               for (int j = 0; j < cols; j++)
               {
                   dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
               }

               while (rows.MoveNext())
               {
                   var row = (XSSFRow)rows.Current;
                   if (row.GetCell(0) != null && !string.IsNullOrEmpty(row.GetCell(0).ToString()))
                   {
                       DataRow dr = dt.NewRow();

                       for (int i = 0; i < cols; i++)
                       {
                           var cell = row.GetCell(i);


                           if (cell == null)
                           {
                               dr[i] = "";
                           }
                           else
                           {
                               dr[i] = cell.ToString();
                           }
                       }
                       dt.Rows.Add(dr);
                   }

               }

           }
           return dt;
       }

    
    }
}
