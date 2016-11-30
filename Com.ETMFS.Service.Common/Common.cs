using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.Service.Common
{
    public class Common<T, Target> where T:class where Target:class,new()
    {
        public static List<Target> ConvertToViewModel(  List<T> srclist)
      {
          List<Target> list = new List<Target>();
          var targetpors = (typeof(Target)).GetProperties();
          srclist.ForEach(row =>
          {
              Target des = new Target();
              foreach (var pro in targetpors)
              {
                  var despro = row.GetType().GetProperty(pro.Name);
                  if (despro != null)
                  {
                      var srcpr = despro.GetValue(row, null);
                      if (srcpr != null)
                          pro.SetValue(des, srcpr);
                  }
                 
              }
              list.Add(des);
          });
          return list;
      }

        
    }
}
