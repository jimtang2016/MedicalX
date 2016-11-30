using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Com.ETMFS.Service.Common
{
   public class XMLHelper
    {
 
       public static T GetXMLEntity<T>(string path) 
       {
           XmlSerializer xmlser = new XmlSerializer(typeof(T));
           T t=default(T);
           if (File.Exists(path))
           {
               using (FileStream stream = new FileStream(path, FileMode.Open))
               {
                   t = (T)xmlser.Deserialize(stream);
               }

           }
           return t;
         }


       public static void SaveXMLEntity<T>(string path,T t)
       {
           XmlSerializer xmlser = new XmlSerializer(typeof(T));
           using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
           {
               xmlser.Serialize(stream, t);
           }
       }
    }
}
