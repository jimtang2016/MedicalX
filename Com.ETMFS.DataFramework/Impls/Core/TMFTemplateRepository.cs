using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Interfaces.Core;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Impls.Core
{
   public class TMFTemplateRepository:Repository<TMFTemplate,ETMFContext> ,ITMFTemplateRepository
    {
       public TMFTemplateRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }
       public TMFTemplate GetByUniqueID(string p)
       {
           return Dbset.FirstOrDefault(f => f.UniqueID == p);
       }
       public PageResult<TMFTemplate> GetTMTList(int id, PageResult<TMFTemplate> pagein)
       {
           var query = Dbset.Where(f => f.Active.HasValue && f.Active.Value&&!f.StudyTemplate.Any(fg=>fg.Study.Id==id&&fg.Active.HasValue&&fg.Active.Value)).OrderBy(f => f.Id);
           var page = pagein;
           page.Total = query.Count();
           page.ResultRows = query.Skip(page.SkipCount).Take(pagein.PageSize).ToList();
           return page;
       }

       public PageResult<TMFTemplate> GetTMTList(PageResult<TMFTemplate> pagein)
       {
           var query = Dbset.Where(f => f.Active.HasValue && f.Active.Value).OrderBy(f=>f.Id);
           var page = pagein;
           page.Total = query.Count();
           page.ResultRows = query.Skip(page.SkipCount).Take(pagein.PageSize).ToList();
           return page;
       }
    }
}
