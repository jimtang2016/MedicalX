using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

using RepositoryT.EntityFramework;
using Com.ETMFS.Service.Common;
namespace ETMFDemo
{
    class hcplevel
    {
        public int Id { set; get; }
        public string HCPLevel { set; get; }
    }
    class Program
    {
        static String location;
        static DateTime time;

       
        static void Main(string[] args)
        {
            //FileHelper helper = new FileHelper();
            
            //List<string> list=new List<string>();
            //list.Add("\\\\192.168.99.58\\Users\\test\\");
            //IdentityScope.Context.ConnectShareFolder("C:\\Roche\\AppConfig.xml");
            //try
            //{
            //    helper.MappingFileFolder(list);
            //}
            //catch (Exception ex)
            //{
            //    Console.Write(ex);
            //}
            var tak = FirstasynOutPut();
            Console.WriteLine("this is test");
            foreach (var i in tak.Result)
            {
                Console.WriteLine(i);
            }
             

             Console.Read();
        }

        static async Task<string[]> FirstasynOutPut()
        {
           
           var taks2= new Task<string>(()=>{
           Console.WriteLine("hello world");
           return "this is test too";
      }) ;
           var taks3 = new Task<string>(() =>
           {
               Console.WriteLine("hello world 1");
               return "this is test three";
           });
         
          taks2.Start(TaskScheduler.Default);
          taks3.Start(TaskScheduler.Default);
          Console.WriteLine("this is main thead");
         
           var completedtasks= await Task.WhenAll(taks2, taks3);
           return completedtasks;
        }
    }
}
