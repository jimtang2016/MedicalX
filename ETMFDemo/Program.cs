using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

using RepositoryT.EntityFramework;
using Com.ETMFS.Service.Common;
using System.Linq.Expressions;
using System.Dynamic;
namespace ETMFDemo
{
    class hcplevel : DynamicObject
    {
        public hcplevel()
        {
            PrintC();
        }
        public int Id { set; get; }
        public string HCPLevel { set; get; }
        Dictionary<string, object> Properties = new Dictionary<string, object>();
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!Properties.Keys.Contains(binder.Name))
            {
                //在此可以做一些小动作
                //if (binder.Name == "Col")
                //　　Properties.Add(binder.Name + (Properties.Count), value.ToString());
                //else
                //　　Properties.Add(binder.Name, value.ToString());


                Properties.Add(binder.Name, value.ToString());
            }
            return true;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {

            return Properties.TryGetValue(binder.Name, out result);
        }
        public virtual void PrintC(){
        
        }
    }

    class B:hcplevel
    {
        public B()
        {
            Id = 1;
        }
        public override void PrintC()
        {
            Console.WriteLine("hcpLevel Id is :{0}", this.Id);
        }
    }
    class Program
    {
        static String location;
        static DateTime time;

       
        static void Main(string[] args)
        {
            dynamic level = new hcplevel();
            level.Col1 = "this is test";
            Console.WriteLine(level.Col1);
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
            //hcplevel b = new B();
            //b.PrintC();
           // var tak = FirstasynOutPut();
           // Console.WriteLine("this is test");
           // foreach (var i in tak.Result)
           // {
           //     Console.WriteLine(i);
           // }
           // List<hcplevel> list = new List<hcplevel>();
           // list.Add(new hcplevel() { Id = 1 });
           // list.Add(new hcplevel() { Id = 2 });
           // list.Add(new hcplevel() { Id = 3 });
           // var result = Parallel.ForEach(list, hcplevel =>
           // {
           //     hcplevel.Id += 1;
           //     Console.WriteLine("Curent obj: " + hcplevel.Id);
           // });
           //while (!result.IsCompleted)
           //{
           //    Task.Delay(3000);
            
           //}
           
           //list.ForEach(obj =>
           //{
           //    Console.WriteLine("schy obj: " + obj.Id);
           //});
            // List<int> quary = new List<int>();
            // var dd = from item in quary
            //          select item;
            // dd.AsQueryable<int>();
            // Expression<Func<int, int, bool>> express = (ex1, ex2) => ex1 > ex2;
            // var dds = express.Compile();
            // quary.Add(1);
            // quary.Add(4);
            // quary.Add(5);
            // quary.Add(6);
            // quary.Add(7);
            //var temp= quary.Where(i=>dds(i,5)).ToList();

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
            int i=1;
            var refvalues = new hcplevel() { Id = 2 };
            testvaluetype(i);
            Console.WriteLine("value type i:{0}",i);
            testreftype(refvalues);
            Console.WriteLine("ref type i:{0}", refvalues.Id);
           return completedtasks;
        }

        public static void testvaluetype(int i) {
          i=i+1;
          Console.WriteLine(i);
    }
        public static void testreftype( hcplevel i)
        {
            i.Id = i.Id + 1;
            Console.WriteLine(i.Id);
        }
    }
}
