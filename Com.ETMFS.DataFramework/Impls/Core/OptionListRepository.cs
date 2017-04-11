using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.DataFramework.Interfaces.Core;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Impls.Core
{
   public class OptionListRepository:Repository<OptionList,ETMFContext> ,IOptionListRepository
    {
       public OptionListRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }
     public  List<OptionList> GetListByParentId(int parentId)
       {
           return Dbset.Where(f => f.Id == parentId || f.ParentId == parentId).ToList();
       }
    }
}
