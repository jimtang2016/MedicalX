using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Com.ETMFS.DataFramework.Interfaces;
 
using Com.ETMFS.DataFramework.Impls.Permission;
using RepositoryT.Infrastructure;
using Com.ETMFS.DataFramework.Impls;
using RepositoryT.Infrastructure;
using Com.ETMFS.DataFramework.Entities;
using RepositoryT.EntityFramework;
namespace ETMFDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建容器  
            IUnityContainer container = new UnityContainer();
            //注册映射  

            container.RegisterType<IDataContextFactory<ETMFContext>, DefaultDataContextFactory<ETMFContext>>(); 
            container.RegisterType<ICompanyRepository, CompanyRepository>();
           var item= container.Resolve<ICompanyRepository>();
           item.GetAll();
        }
    }
}
