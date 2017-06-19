using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.DataFramework.View;
using Com.ETMFS.Service.Core.ViewModel;

namespace Com.ETMFS.Service.Core.Interfaces
{
   public enum MileStoneType{
        Country,
        Site,
       Study
    }
   public interface IStudyService
    {

       PageResult<StudyViewModel> GetStudyList(int page, int rows);

       int SaveStudyList(StudyViewModel studyv, string operationId);

       void DeleteStudyList(List<StudyViewModel> studylist, string operationId);

       PageResult<TrialReginalViewModel> GetTrialRegionals(int id,int page,int rows);

       void SaveTrialRegional(TrialReginalViewModel trialReg, string op);

       void RemoveRegionals(List<TrialReginalViewModel> trialRegs, string op);

       PageResult<MemberViewModel> GetStudyMembers(int id, int page, int rows, int? countryId, int? siteId);

       List<TrialReginalViewModel> GetTrialRegionals(int id,int p);

       List<OptionList> GetOptionListByParentId(int parentId);

       List<TmfNote> GetStudyListView(int p);

       void SaveStudyMembers(MemberViewModel mem,string op);

       void RemoveStudyMembers(List<MemberViewModel> memList, string p);

       PageResult<TMFRefernceViewModel> GetTrialTempaltes(int id, int page, int rows, int userId);

       void SaveTemplates(int id, List<TMFRefernceOptionViewModel> list, string p, bool isdel, List<int> countrys);

       PageResult<SiteViewModel> GetStudySites(int id, int page, int rows);

       void SaveStudySite(SiteViewModel site,string op);

       List<SiteViewModel> GetStudySites(int id, int? countryId,int? p);

       void MappingFolders(Common.ConfigSetting config, int? studyId);

       List<StudyViewModel> GetUserStudyList( int p);

       PermissionViewModel GetPermission(TMFFilter filter,int p);

       List<TMFRefernceOptionViewModel> GetTrialTempaltes(TMFFilter condition);

         List<TrialReginalViewModel> GetTrialRegionals(int id);

         List<SiteViewModel> GetStudySites(int id);

         PageResult<TMFRefernceOptionViewModel> GetOutCountryTMFModels(int id, int countryId, int page, int rows);

         PageResult<MileStoneViewModel> GetMileStones(int studyId, int id, MileStoneType stoneType, int page, int rows);

         bool SaveMileStones(int studyId, MileStoneType stoneType, List<MileStoneViewModel> milestones);

         List<MileStoneViewModel> GetMileStones(int p1, int p2, MileStoneType type);
    }
}
