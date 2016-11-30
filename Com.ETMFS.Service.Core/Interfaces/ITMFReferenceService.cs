using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework;
using Com.ETMFS.Service.Core.ViewModel;

namespace Com.ETMFS.Service.Core.Interfaces
{
   public interface ITMFReferenceService
    {
       PageResult<TMFRefernceViewModel> GetTMFModelList(int page, int rows);

       void SaveTMFReference(TMFRefernceViewModel template, string operatorId);
       void DeleteTMFReference(List<TMFRefernceViewModel> templates, string operatorId);

       void SaveTMFReferences(System.Data.DataTable datatable,string op);

      byte[] GetAllTemplateStream();

      PageResult<TMFRefernceViewModel> GetTMFModelList(int id, int page, int rows);

     
    }
}
