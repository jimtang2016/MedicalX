using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.DataFramework.Interfaces.Core;
using Com.ETMFS.DataFramework.View;
using Com.ETMFS.Service.Common;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Impls.Core
{
    public class StudyRepository : Repository<Study, ETMFContext>,IStudyRepository
    {
        public StudyRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }
      public  PageResult<Study> GetStudyList(PageResult<Study> pagein)
        {
            var page = pagein;
            var query = Dbset.Where(fg => fg.Active.HasValue && fg.Active.Value).ToList();
            page.Total = query.Count;
            page.ResultRows = query.Skip(page.SkipCount).Take(page.PageSize).ToList();
            return page;
        }



   public   List<StudyTemplate> GetTMFRefernceListById(int id, int p)
      {
          var study = Dbset.Find(id);
          var query = from item in study.StudyTemplate.ToList()
                      let users = study.StudyMember.Where(u => u.UserGroup.Users.Any(us => us.Id == p) && u.Active.HasValue && u.Active.Value&&(u.Role==Constant.Role_Owner||u.Role==Constant.Role_Uploader)).ToList() 
                      where item.Active.HasValue && item.Active.Value
                            && users.Any(mem=>mem.RoleLevel==Constant.RoleLevel_Trial&&
                            item.TMFTemplate.IsTrialLevel==Constant.TMF_BelongFlag||
                           mem.RoleLevel == Constant.RoleLevel_Country &&
                            item.TMFTemplate.IsCountryLevel == Constant.TMF_BelongFlag
                            || mem.RoleLevel == Constant.RoleLevel_Site &&
                            item.TMFTemplate.IsSiteLevel == Constant.TMF_BelongFlag
                            )
                      select item;
          return query.ToList();
      }



   public List<StudyListView> GetStudyListView(int p)
   {
  
       var query = from study in  Dbset.ToList() join view in DataContext.StudyListView
                                                 on study.Id equals view.StudyId
                   where study.StudyMember.Any(st=>st.UserGroup.Users.Any(usr=>usr.Id==p))
                   select view;
       return query.ToList();
   }

      public PageResult<TMFTemplate> GetTMFRefernceListById(int id, PageResult<TMFTemplate> pagein,int userId)
      {
         var page = pagein;
         var query = GetTMFRefernceListById(id, userId);
         page.Total = query.Count;
         page.ResultRows = query.ToList().Skip(page.SkipCount)
             .Take(page.PageSize).Select(i=>i.TMFTemplate).ToList();
         return page;
      }
      public PageResult<Site> GetStudySitesById(int id, PageResult<Site> pagein)
      {
          var page = pagein;
          var study = GetById(id);
          if (study != null)
          {
             
              var temp = from site in study.StudySite.Where(f => f.Site.Active.Value).ToList()  
                         join reg in study.TrialRegional.Select(f => f.Country).Distinct().ToList()
                         on site.CountryId equals reg.Id
                         select ConvertSite(site,reg);
              page.Total = temp.Count();
              page.ResultRows = temp.ToList();
          }
          return page;
      }

      public List<Site> GetStudySitesById(int id,int? p)
      {
          var study = GetById(id);
      var temp=study.StudySite.Where(f => f.Site.Active.Value&&
       (!p.HasValue || p.HasValue && study.StudyMember.Any(u => u.UserGroup.Users.Any(us => us.Id == p) && u.Active.HasValue && u.Active.Value && (!u.SiteId.HasValue || u.SiteId.HasValue && f.SiteId == u.SiteId)
       ))
          ).ToList();
      temp.ForEach(s => { s.Site.CountryId = s.CountryId; s.Site.Status = s.Status; s.Site.StudySiteId = s.Id; });
      return temp.Select(s => s.Site).ToList();
      }
      Site ConvertSite(StudySite studysite,Country country)
      {
          var mem = studysite.Study.StudyMember.FirstOrDefault(u =>  u.RoleLevel == Constant.RoleLevel_Site
                             && u.Role == Constant.Role_Owner && u.SiteId == studysite.SiteId
                             && u.Active.HasValue && u.Active.Value);
         
        studysite.Site.CountryId = country.Id;
        studysite.Site.CountryName = country.CountryName;
        studysite.Site.Status = studysite.Status;
        studysite.Site.StudySiteId = studysite.Id;
        if (mem != null)
        {
            studysite.Site.OwnerId = studysite.OwnerId;
            studysite.Site.MemberId = mem.Id;
        }
        return studysite.Site;
      }
      public PageResult<TrialMemberView> GetMemberListById(int id, PageResult<TrialMemberView> pagein,int? countryId,int? siteId)
      {
          var page = pagein;
              var temp = from mem in DataContext.TrialMemberView.ToList()
                         where mem.StudyId == id && (countryId.HasValue && countryId.Value == mem.CountryCode || !countryId.HasValue)
                         && (siteId.HasValue && siteId.Value == mem.SiteId || !siteId.HasValue)
                         select  mem;
              var temlist=temp.Skip(page.SkipCount).Take(page.PageSize).ToList();
              page.Total = temp.Count();
              page.ResultRows = temlist;
          return page;
      }
     
      public List<TrialRegional> GetTrialCountries(int id)
      {
          var study = GetById(id);
          return study.TrialRegional.Where(reg=>reg.Active.HasValue&&reg.Active.Value).ToList();
      }
      public PageResult<TrialReginalView> GetTrialReginals(int id, PageResult<TrialReginalView> pagein)
      {

          var page = pagein;
          var study = GetById(id);
          if (study != null)
          {
             
              var templist = from reginal in study.TrialRegional.ToList() 
                             
                             where reginal.Active.Value  
                             select ConvertReginalView(reginal);

              page.Total = templist.Count();
              var temp=templist.Skip(page.SkipCount).Take(page.PageSize).ToList();
             
              page.ResultRows = temp;
          }
       
        
          return page;
      }
      public List<Study> GetStudyList()
      {
        return  Dbset.Where(f => f.Active.HasValue && f.Active.Value).ToList();
      }

      public List<Study> GetUserStudyList(int userId)
      {
          var list = from study in Dbset
                     where study.StudyMember.Any(f => f.UserGroup.Users.Any(user=>user.Id == userId) && f.Active.HasValue && f.Active.Value)
                     select study;
          return list.ToList();
      }
    

     TrialReginalView   ConvertReginalView(TrialRegional reginal){
         var user = reginal.Study.StudyMember.FirstOrDefault(u =>   u.RoleLevel == Constant.RoleLevel_Country && u.Role == Constant.Role_Owner && u.CountryCode == reginal.CountryId
                             && u.Active.HasValue && u.Active.Value);
         var vreginal = new TrialReginalView()
                           {
                               Id = reginal.Id,
                               OwnerId = reginal.OwnerId,
                               CountryName = reginal.Country.CountryName,
                               CountryId = reginal.CountryId,
                               Status = reginal.Status,
                               Active = reginal.Active
                           };
       
         return vreginal;
        }

     #region IStudyRepository Members


     public List<StudyTemplate> GetSutdyTemplates(Study studylist,TMFFilter condition)
     {
         condition.TMFLevel = Constant.RoleLevel_Trial;

         if (condition.Country.HasValue)
         {
             condition.TMFLevel = Constant.RoleLevel_Country;
         }
         if (condition.Site.HasValue)
         {
             condition.TMFLevel = Constant.RoleLevel_Site;
         }
         var query = from tmf in studylist.StudyTemplate
                     where tmf.Active.HasValue && tmf.Active.Value
                     && ((condition.TMFLevel == Constant.RoleLevel_Trial) ||
                  (condition.TMFLevel == Constant.RoleLevel_Country
                  && tmf.TMFTemplate.IsCountryLevel == Constant.TMF_BelongFlag
                && !tmf.TemplateOutcluding.Any(count => count.CountryCode == condition.Country && count.Active.HasValue && count.Active.Value))
                  || (condition.TMFLevel == Constant.RoleLevel_Site
                  && tmf.TMFTemplate.IsSiteLevel == Constant.TMF_BelongFlag
                  && !tmf.TemplateOutcluding.Any(count => count.CountryCode == condition.Country && count.Active.HasValue && count.Active.Value)
                  )
                 )
                     select tmf;
         return query.ToList();
     }

     #endregion
    }
}
