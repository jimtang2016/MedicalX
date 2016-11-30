using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.DataFramework
{
   public class PageResult<T> 
        
       where T:class
    {
       List<T> _rows = new List<T>();
       public List<T> ResultRows { get { return _rows; } set { _rows = value; } }
     public int Total { get; set; }
     public int PageSize { get; set; }
     public int CurrentPage { get; set; }
     public int TotalPage { get { return Total / PageSize + 1; } }
     public int SkipCount { get {

         if (CurrentPage == 1)
         {
             return 0;
         }
         else
         {
             return (CurrentPage - 1) * PageSize;
         }
     
     } }
    
    }
}
