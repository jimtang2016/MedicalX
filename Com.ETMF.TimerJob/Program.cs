using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.BusinesService.Impls;
using Com.ETMFS.BusinesService.Interfaces;
using Com.ETMFS.DataFramework.Entities;
using Com.ETMFS.DataFramework.Impls.Core;
using Com.ETMFS.DataFramework.Impls.Permission;
using Com.ETMFS.DataFramework.Interfaces.Core;
using Com.ETMFS.DataFramework.Interfaces.Permission;
using Com.ETMFS.Service.Core.Impls;
using Com.ETMFS.Service.Core.Interfaces;
using Microsoft.Practices.Unity;
using RepositoryT.EntityFramework;
using RepositoryT.Infrastructure;

namespace Com.ETMF.TimerJob
{
    
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            InitService();
            var document = container.Resolve<IDocumentService>();
           var list= document.GetNotifyReminderList();
           
           //Com.ETMFS.Service.Common.EmailHelper.Current.SendEmail();
        }
       
        static UnityContainer container = new UnityContainer();
        static void InitService()
        {
            container.RegisterType<IDataContextFactory<ETMFContext>, DefaultDataContextFactory<ETMFContext>>();
            container.RegisterType<IUnitOfWork, UnitOfWork<ETMFContext>>();
            container.RegisterType<IOptionListRepository, OptionListRepository>();
            container.RegisterType<IUsersRepository, UsersRepository>();
            container.RegisterType<IUserGroupsRepository, UserGroupsRepository>();
            container.RegisterType<ITMFTemplateRepository, TMFTemplateRepository>();
            container.RegisterType<IDocumentRepository, DocumentRepository>();
            container.RegisterType<IDocumentService, DocumentService>();
            container.RegisterType<IStudyRepository, StudyRepository>();
            container.RegisterType<ICountryRepository, CountryRepository>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IGroupService, GroupService>();
            container.RegisterType<ICountryService, CountryService>();
            container.RegisterType<ITMFReferenceService, TMFReferenceService>();
            container.RegisterType<IStudyService, StudyService>();
            container.RegisterType<IIssuelogRepository, IssuelogRepository>();
            container.RegisterType<INotifyRulesRepository, NotifyRulesRepository>();
        }
    }
}
