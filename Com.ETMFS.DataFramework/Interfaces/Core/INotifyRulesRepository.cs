using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.View;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Interfaces.Core
{
    public interface INotifyRulesRepository : IRepository<NotificationRules>
    {
        PageResult<NotificationRules> GetNotifyRules(PageResult<NotificationRules> pagein,int? studyId, int? countryId, int? siteId,int? tmfId);

        List<NotifyReminderView> GetNotifyReminderList();
    }
}
