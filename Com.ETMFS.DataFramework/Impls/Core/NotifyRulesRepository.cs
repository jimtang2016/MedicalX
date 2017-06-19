using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Interfaces.Core;
using Com.ETMFS.DataFramework.View;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Impls.Core
{
    public partial class NotifyRulesRepository : Repository<NotificationRules, ETMFContext>, INotifyRulesRepository
    {

        public NotifyRulesRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }
        public List<NotifyReminderView> GetNotifyReminderList()
        {
          var list=  DataContext.NotifyReminderView.ToList();
          return list;
        }
       public PageResult<NotificationRules> GetNotifyRules(PageResult<NotificationRules> pagein, int? studyId, int? countryId, int? siteId,int? tmfid)
        {
            var query = from item in Dbset.ToList()
                        where
                            ( studyId == item.StudyId )
                            && (countryId.HasValue && countryId == item.StudyCountryId || !countryId.HasValue)
                            && (siteId.HasValue && siteId == item.StudySiteId || !siteId.HasValue)
                               && (tmfid.HasValue && tmfid == item.StudyTMFId || !tmfid.HasValue)
                               &&item.Active.Value
                        select item;
            pagein.Total = query.Count();
            pagein.ResultRows = query.Skip(pagein.SkipCount).Take(pagein.PageSize).ToList();
            return pagein;
        }
    }
}
