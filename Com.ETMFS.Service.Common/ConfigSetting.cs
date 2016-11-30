using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.XmlConfiguration;
using System.Xml.Schema;

namespace Com.ETMFS.Service.Common
{
   public enum HostType{
        FileSystem=1,
        ShareFolder=2,
        SharePoint=3
    }
    [XmlRoot]
   public class ConfigSetting
    {
        [XmlElement]
        public int HostType { get; set; }
        [XmlElement]
        public bool IsShareFolder { get; set; }
        [XmlElement]
        public string UserId { get; set; }
        [XmlElement]
        public string Password { get; set; }
        [XmlElement]
        public string Domain { get; set; }
        [XmlElement]
        public string PathURI { get; set; }
        [XmlElement]
        public string RootFolder { get; set; }
    }
}
