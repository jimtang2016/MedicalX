using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.Service.Core.ViewModel;

namespace Com.ETMFS.Service.Core.Interfaces
{
   public interface IStudyService
    {
       PageResult<StudyViewModel> GetStudyList(int page, int rows);
       void SaveStudyList(StudyViewModel studyv, string operationId);
       void DeleteStudyList(List<StudyViewModel> studylist, string operationId);

       PageResult<TrialReginalViewModel> GetTrialRegionals(int id,int page,int rows);

       void SaveTrialRegional(TrialReginalViewModel trialReg, string op);
       void RemoveRegionals(List<TrialReginalViewModel> trialRegs, string op);

       PageResult<MemberViewModel> GetStudyMembers(int id, int page, int rows);

       List<TrialReginalViewModel> GetTrialRegionals(int id);
       List<OptionList> GetOptionListByParentId(int parentId);

       void SaveStudyMembers(MemberViewModel mem,string op);

       void RemoveStudyMembers(List<MemberViewModel> memList, string p);

       PageResult<TMFRefernceViewModel> GetTrialTempaltes(int id, int page, int rows, int userId);

       void SaveTemplates(int id, List<TMFRefernceViewModel> list, string p, bool isdel, List<int> countrys);

       PageResult<SiteViewModel> GetStudySites(int id, int page, int rows);

       void SaveStudySite(SiteViewModel site,string op);

       List<SiteViewModel> GetStudySites(int id);

       void MappingFolders(Common.ConfigSetting config);
    }
}
