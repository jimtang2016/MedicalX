using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Web;
 
namespace Com.ETMFS.Service.Common
{
   public  sealed partial class EmailHelper
    {
      static EmailHelper _helper=new EmailHelper();
      EmailConfig _config;
       private EmailHelper()
       {
            
       }
       public  void LoadConfig  (HttpServerUtilityBase Server)
        {
            lock (_helper)
            {
                if (EmailConfig == null)
                {
                    var path = Server.MapPath(ConfigList.EmailConfigXMLPath);
                    EmailConfig = XMLHelper.GetXMLEntity<EmailConfig>(path);
                }
            }
       }
       public static EmailHelper Current
       {
           get
           {
               return _helper;
           }
       }
       SmtpClient GetEmailClient()
       {
           SmtpClient client = new SmtpClient();
           client.Host = EmailConfig.Server;
           client.Port = EmailConfig.Port;
           client.UseDefaultCredentials = false;
           client.Credentials = new System.Net.NetworkCredential(EmailConfig.UserName, EmailConfig.Password);
           client.EnableSsl = EmailConfig.UseSSL;

           return client;
       }
       public   EmailConfig EmailConfig
       {
           get
           {
             return  _config;
           }
           set { _config = value;
           }
       }
       public bool SendEmail( string receivers, string copy, string body)
       {
           using (SmtpClient client = GetEmailClient())
           {

               MailMessage massage = new MailMessage()
               {
                   IsBodyHtml = true,
                   Subject = EmailConfig.Subject,
                   BodyEncoding = Encoding.UTF8,

               };

               var from = new MailAddress(EmailConfig.From, EmailConfig.UserName, Encoding.UTF8);
               massage.From = massage.Sender = from;
               var receiverarr = receivers.Split(';');
               receiverarr.ToList().ForEach(f => massage.To.Add(new MailAddress(f, f, Encoding.UTF8)));
               if (!string.IsNullOrEmpty(copy))
               {
                   var ccarr = copy.Split(';');
                   ccarr.ToList().ForEach(f => massage.CC.Add(new MailAddress(f, f, Encoding.UTF8)));
               }
               massage.Body = body;

               client.Send(massage);
           }
           return true;
       }
       
    }

   [Serializable]
   public sealed partial class EmailConfig
   {
       public string Server { get; set; }
       public string UserName { get; set; }
       public string Password { get; set; }
       public string IssueTemplate { get; set; }
       public string PermissionTemplate { get; set; }
       public string Subject { get; set; }
       public int Port { get; set; }
       public bool UseSSL { get; set; }
       public string From { get; set; }
   }
}
