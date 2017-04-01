using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.Service.Common
{
   public  sealed class Constant
    {
       public static  readonly  int Update=2;
       public static readonly int Add = 1;
       public static readonly int Remove = 3;
       public static readonly string Role_Owner = "Owner";
       public static readonly string Role_Uploader = "Uploader";
       public static readonly string Role_Reviewer = "Reviewer";
       public static readonly string RoleLevel_Country = "Country";
       public static readonly string RoleLevel_Site = "Site";
       public static readonly string RoleLevel_Trial = "Study";
       public static readonly string Date_format = "yyyy-MM-dd HH:mm:ss";
       public static readonly int Group_Administrators = 1;
       public static readonly int Group_Uploaders = 7;
       public static readonly int Group_Reviewers = 8;
       public static readonly string Group_Split_Flag="{0}|{1}|{2}";
       public static readonly string TMF_BelongFlag = "X";
       public static readonly string TMF_Issued = "Issued";
       public static readonly string TMF_Resolved = "Resolved";
       public static readonly string Document_WebSplitFlag = "/";
       public static readonly string Document_FileSplitFlag = "\\";
       public static readonly char Document_TypeSplitFlag = '.';
       public static readonly string Document_ContentType_Other = "other";
       public static readonly char Document_UserFlag = ',';
       public static readonly char Document_EmailFlag = ';';
       public static readonly string Document_LogPrefix = "Log-";
       public static Dictionary<string, string> ContentTypeLib = new Dictionary<string, string>();
       public static void InitContentLib()
       {
           ContentTypeLib.Add("doc", "application/ms-word");
           ContentTypeLib.Add("xls", "application/ms-excel");
           ContentTypeLib.Add("xlsx", "application/ms-excel");
           ContentTypeLib.Add("ppt", "application/x-ppt");
           ContentTypeLib.Add("jpg", "application/x-jpg");
           ContentTypeLib.Add("pdf", "application/pdf");
           ContentTypeLib.Add("jpeg", "image/jpeg");
           ContentTypeLib.Add("other", "application/octet-stream");
           
       }
    }
}
