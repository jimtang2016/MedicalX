using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Core;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Interfaces.Core
{
    public interface ITMFTemplateRepository : IRepository<TMFTemplate>
    {
        PageResult<TMFTemplate> GetTMTList(PageResult<TMFTemplate> pagein);

        TMFTemplate GetByUniqueID(string p);

        PageResult<TMFTemplate> GetTMTList(int id, PageResult<TMFTemplate> pagein);
    }
}
