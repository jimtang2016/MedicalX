using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Permission;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Interfaces.Core
{
  public  interface IOptionListRepository:   IRepository<OptionList>
    {

        List<OptionList> GetListByParentId(int parentId);
    }
}
