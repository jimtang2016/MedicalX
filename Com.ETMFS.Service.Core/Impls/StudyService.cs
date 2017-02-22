using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.DataFramework.Interfaces.Core;
using Com.ETMFS.DataFramework.View;
using Com.ETMFS.Service.Common;
using Com.ETMFS.Service.Core.Interfaces;
using Com.ETMFS.Service.Core.ViewModel;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.Service.Core.Impls
{
  public  class StudyService:IStudyService
    {
      IStudyRepository _studyRepos;
      IOptionListRepository _optionlist;
      IUnitOfWork _unitWork;
      public StudyService(IStudyRepository studyRepos,IUnitOfWork unitwork ,IOptionListRepository optionlist)
      {
          _studyRepos = studyRepos;
          _unitWork = unitwork;
          _optionlist = optionlist;
      }

      #region IStudyService Members
      public List<OptionList> GetOptionListByParentId(int parentId)
      {
         return _optionlist.GetListByParentId(parentId);
      }

      public PageResult<SiteViewModel> GetStudySites(int id, int page, int rows)
      {
          PageResult<Site> pagein = new PageResult<Site>
          {
              CurrentPage = page,
              PageSize = rows
          };

          pagein = _studyRepos.GetStudySitesById(id, pagein);
          PageResult<SiteViewModel> pageout = new PageResult<SiteViewModel>
          {
              CurrentPage = page,
              PageSize = rows,
              Total = pagein.Total,
              ResultRows = Common<Site, SiteViewModel>.ConvertToViewModel(pagein.ResultRows)
          };


          return pageout;
      }

      public PageResult<TMFRefernceViewModel> GetTrialTempaltes(int id, int page, int rows,int userId)
      {
       
          PageResult<TMFTemplate> pagein = new PageResult<TMFTemplate>
          {
              CurrentPage = page,
              PageSize = rows
          };

          pagein = _studyRepos.GetTMFRefernceListById(id, pagein, userId);
          PageResult<TMFRefernceViewModel> pageout = new PageResult<TMFRefernceViewModel>
          {
              CurrentPage = page,
              PageSize = rows,
              Total = pagein.Total,
              ResultRows = Common<TMFTemplate, TMFRefernceViewModel>.ConvertToViewModel(pagein.ResultRows)
          };
          return pageout;

      }

      public List<SiteViewModel> GetStudySites(int id,int? countryId)
      {
          if (id > 0)
          {
              var list = _studyRepos.GetStudySitesById(id).Where(f => countryId.HasValue&&f.CountryId == countryId || !countryId.HasValue).ToList();
              return Common<Site, SiteViewModel>.ConvertToViewModel(list);
          }
          else
          {
              return null;
          }
      }
      public void SaveStudySite(SiteViewModel site, string op)
      {
          var study = _studyRepos.GetById(site.StudyId);
          var studysite= study.StudySite.FirstOrDefault(sts => sts.SiteId == site.Id);
          Site esite;
          if (studysite == null)
          {
              studysite = new StudySite();
              esite = new Site()
              {
                  CreateBy = op,
                  Created = DateTime.Now,
              };
          }
          else
          {
                esite = studysite.Site;
          }
          esite.SiteName = site.SiteName;
          esite.SiteNum = site.SiteNum;
          esite.Modified = DateTime.Now;
          esite.ModifiBy = op;
          esite.Active = site.Active;
          studysite.Status = site.Status;
          studysite.CountryId = site.CountryId;
          if (studysite.Id <= 0)
          {
              studysite.Site = esite;
              study.StudySite.Add(studysite);
          }
          _unitWork.Commit();
          if (site.OwnerId.HasValue)
          {
              MemberViewModel mem = new MemberViewModel()
              {
                  Id = site.MemberId,
                  Role = Constant.Role_Owner,
                  RoleLevel = Constant.RoleLevel_Site,
                  CountryCode = site.CountryId,
                  SiteId = esite.Id,
                  MemberId = site.OwnerId.Value,
                  Active=site.Active
              };
             
              AddOrSaveMember(study, mem);
              _unitWork.Commit();
          }
         
      }
      public void SaveTemplates(int id, List<TMFRefernceViewModel> list, string p, bool isdel,List<int> countrys)
      {
          var study = _studyRepos.GetById(id);
     
          list.ForEach(f =>
          {
              var temp = study.StudyTemplate.FirstOrDefault(fg => fg.TemplateId == f.Id);
              if (temp == null)
              {
                  temp = new StudyTemplate()
              {
                  TemplateId = f.Id,
                  Study = study,
                  CreateBy = p,
                  Created = DateTime.Now

              };
              }
              temp.Modified = DateTime.Now;
              temp.ModifiBy = p;
              temp.Active = !isdel;
            if (isdel&&countrys .Count>0)
                  {
                      countrys.ForEach(fg =>
                      {
                          temp.TemplateOutcluding.Add(new TemplateOutcluding()
                          {
                              StudyTemplateId = temp.Id,
                              CountryCode = fg
                          });
                      });
                  }
                    
            
              if (temp.Id == 0)
              {
                  study.StudyTemplate.Add(temp);
              }

          });
          _unitWork.Commit();
      }
      public void SaveStudyMembers(MemberViewModel mem, string op)
      {
         var study= _studyRepos.GetById(mem.StudyId);
         mem.OperatorId = op;
         AddOrSaveMember(study,mem);
          
         _unitWork.Commit();
      }
      public PageResult<MemberViewModel> GetStudyMembers(int id, int page, int rows)
      {
          PageResult<StudyMember> pagein = new PageResult<StudyMember>
          {
              CurrentPage = page,
              PageSize = rows
          };
          pagein = _studyRepos.GetMemberListById(id, pagein);
          PageResult<MemberViewModel> pageout = new PageResult<MemberViewModel>
          {
              CurrentPage = page,
              PageSize = rows,
              Total = pagein.Total,
              ResultRows = Common<StudyMember,MemberViewModel>.ConvertToViewModel(pagein.ResultRows)
          };
          return pageout;
      }
     public List<TrialReginalViewModel> GetTrialRegionals(int id,int p)
      {
          return _studyRepos.GetTrialCountries(id).Where(f=>f.OwnerId==p||f.Study.StudyMember.Any(fg=>fg.Role==Constant.Role_Owner&&fg.RoleLevel==Constant.RoleLevel_Trial
              &&fg.MemberId==p&&fg.Active.Value)).Select(f => new TrialReginalViewModel()
          {
              CountryId=f.CountryId,
              CountryName=f.Country.CountryName,
              CountryCode=f.Country.CountryCode
             
          }).Distinct().ToList();
      }
      public  PageResult< StudyViewModel> GetStudyList(int page, int rows)
      {
          PageResult<Study> pagein = new PageResult<Study>
          {
              CurrentPage = page,
              PageSize = rows
          };
          pagein = _studyRepos.GetStudyList(pagein);
          PageResult<StudyViewModel> pageout = new PageResult<StudyViewModel>()
          {
              Total = pagein.Total,
              PageSize = pagein.PageSize,
              CurrentPage = pagein.CurrentPage
          };
          pageout.ResultRows = Common.Common<Study, StudyViewModel>.ConvertToViewModel(pagein.ResultRows);

          return pageout;
      }
      public void SaveStudyList(StudyViewModel studyv,string operationId)
      {
          var study = ConvertToEntity(studyv, operationId);
          if (study.Id <= 0)
          {
              _studyRepos.Add(study);
          }
          _unitWork.Commit();
      }
      public void DeleteStudyList(List<StudyViewModel> studylist, string operationId)
      {
          studylist.ForEach(f =>
          {
              var study = ConvertToEntity(f, operationId);
          });
          _unitWork.Commit();
      }
      Study ConvertToEntity(StudyViewModel studyv, string operationId)
      {
          var study = _studyRepos.GetById(studyv.Id);
          if (study== null)
          {
              study = new Study();
              study.CreateBy = operationId;
              study.Created = DateTime.Now;
          }
          study.ShortTitle = studyv.ShortTitle;
          study.Status = studyv.Status;
          study.StudyNum = studyv.StudyNum;
          study.Active = studyv.Active;
           
          study.ModifiBy= operationId;
          study.Modified = DateTime.Now;
          return study;
      }
    public   PageResult<TrialReginalViewModel> GetTrialRegionals(int id, int page, int rows)
      {
          PageResult<TrialReginalView> pagein = new PageResult<TrialReginalView>
          {
              CurrentPage = page,
              PageSize = rows
          };
          pagein = _studyRepos.GetTrialReginals(  id,pagein);
          PageResult<TrialReginalViewModel> pageout = new PageResult<TrialReginalViewModel>()
          {
              Total = pagein.Total,
              PageSize = pagein.PageSize,
              CurrentPage = pagein.CurrentPage
          };
          pageout.ResultRows = Common.Common<TrialReginalView, TrialReginalViewModel>.ConvertToViewModel(pagein.ResultRows);

          return pageout;
      }
    public void RemoveStudyMembers(List<MemberViewModel> memList, string p)
    {
        var study = _studyRepos.GetById(memList[0].StudyId);
        memList.ForEach(mem =>
        {
            var meme=study.StudyMember.FirstOrDefault(f => f.Id == mem.Id);
            if (meme != null)
            {
                meme.Active = false;
                meme.Modified = DateTime.Now;
                meme.ModifiBy = p;
            }
        });
        _unitWork.Commit();
    }
    public void SaveTrialRegional(TrialReginalViewModel trialReg,string op)
    {
        var study = _studyRepos.GetById(trialReg.StudyId.Value);
        if (study != null)
        {
            var trial = study.TrialRegional.Where(f => f.Id == trialReg.Id).FirstOrDefault();
            if (trial== null)
            {
                trial = new TrialRegional()
                {
                    Created = DateTime.Now,
                    CreateBy = op
                };
            }
            trial.CountryId = trialReg.CountryId;
            trial.ModifiBy = op;
            trial.Modified = DateTime.Now;
            trial.Active = trialReg.Active;
            if (trial.Id == 0)
            {
                study.TrialRegional.Add(trial);
            }
            MemberViewModel memv = new MemberViewModel()
            {
                Id = trialReg.MemberId==null ? 0 : trialReg.MemberId.Value,
                MemberId = trialReg.OwnerId.Value,
                CountryCode = trialReg.CountryId,
                Role = Constant.Role_Owner,
                RoleLevel = Constant.RoleLevel_Country,
                OperatorId=op,
                Active = trialReg.Active
            };
            AddOrSaveMember(study, memv);
        }
        _unitWork.Commit();
    }
      #endregion

    public PermissionViewModel GetPermission(TMFFilter filter,int p)
    {
        PermissionViewModel permission = new PermissionViewModel();
        var study = _studyRepos.GetById(filter.Study.Value);
        IEnumerable<StudyMember> query = null;
        if (filter.Study.HasValue)
        {
            
            if (filter.Country != null)
            {
               
                query = study.StudyMember.Where(mem => mem.MemberId == p
              && (mem.CountryCode == filter.Country|| 
              mem.RoleLevel == Constant.RoleLevel_Trial
              && mem.Study.Id == filter.Study)
              && mem.Active.HasValue && mem.Active.Value
              );
            }
            else 
            if (filter.Site != null)
            {
            
                query = study.StudyMember.Where(mem => mem.MemberId == p
             && (mem.SiteId == filter.Site
             || mem.RoleLevel == Constant.RoleLevel_Trial && mem.Study.Id == filter.Study
             || mem.RoleLevel == Constant.RoleLevel_Country && mem.CountryCode==filter.Country)
              && mem.Active.HasValue && mem.Active.Value
                );
            }
            else
            {
                query = study.StudyMember.Where(mem => mem.MemberId == p
           && mem.RoleLevel == Constant.RoleLevel_Trial
           && mem.Study.Id == filter.Study
           && mem.Active.HasValue && mem.Active.Value
             );
            }
            var mems = query.ToList();
            permission.IsOwner = mems.Any(mem => mem.Role == Constant.Role_Owner);
            permission.IsUploader = mems.Any(mem => mem.Role == Constant.Role_Uploader);
            permission.IsReviewer = mems.Any(mem => mem.Role == Constant.Role_Reviewer);
        }

        return permission;
    }
    public void RemoveRegionals(List<TrialReginalViewModel> trialRegs, string op)
    {
        if(trialRegs.Count>0){
            var reg=trialRegs.FirstOrDefault();
            var study = _studyRepos.GetById(reg.StudyId.Value);
            trialRegs.ForEach(rg =>
            {
                var regs = study.TrialRegional.Where(f => f.Id == rg.Id).FirstOrDefault();
                regs.Active = false;
                regs.Modified = DateTime.Now;
                regs.ModifiBy = op;
                var mem = study.StudyMember.Where(f => f.Id == rg.MemberId).FirstOrDefault();
                if (mem != null)
                {
                    mem.Active = false;
                    mem.Modified = DateTime.Now;
                    mem.ModifiBy = op;
                }
              
            });
           
        _unitWork.Commit();
        }
      
    }

    void AddOrSaveMember(Study study, MemberViewModel memv)
    {
        StudyMember member = null;

        if (memv.Role == Constant.Role_Owner)
        {
            if (memv.RoleLevel == Constant.RoleLevel_Site)
            {
                var site = study.StudySite.FirstOrDefault(f => f.SiteId == memv.SiteId);
                site.OwnerId = memv.MemberId;
                site.Site.Modified = DateTime.Now;
                site.Site.ModifiBy = memv.OperatorId;
                member = study.StudyMember.Where(f => f.SiteId == memv.SiteId
                    && f.CountryCode == memv.CountryCode
                    && f.Active.HasValue
                    && f.RoleLevel == Constant.RoleLevel_Site
                    && f.Role == Constant.Role_Owner
                    && f.Active.Value).FirstOrDefault();
            }
            else if (memv.RoleLevel == Constant.RoleLevel_Country)
            {
                var country = study.TrialRegional.FirstOrDefault(f => f.CountryId == memv.CountryCode);
                country.OwnerId = memv.MemberId;
                country.Modified = DateTime.Now;
                country.ModifiBy = memv.OperatorId;
                member = study.StudyMember.Where(f => f.CountryCode == memv.CountryCode
                      && f.RoleLevel == Constant.RoleLevel_Country
                    && f.Role == Constant.Role_Owner
                   && f.Active.HasValue
                   && f.Active.Value).FirstOrDefault();
            }
            else if (memv.RoleLevel == Constant.RoleLevel_Trial)
            {
                member = study.StudyMember.Where(f => f.RoleLevel == Constant.RoleLevel_Trial
    && f.Role == Constant.Role_Owner
   && f.Active.HasValue
   && f.Active.Value).FirstOrDefault();
            }
        }
        else
        {
            member = study.StudyMember.Where(f => f.Id == memv.Id)
                   .FirstOrDefault();

        }


        if (member == null)
        {
            member = new StudyMember()
            {
                CreateBy = memv.OperatorId,
                Created = DateTime.Now
            };
        }

        member.MemberId = memv.MemberId;
        member.RoleLevel = memv.RoleLevel;
        member.Role = memv.Role;
        member.CountryCode = memv.CountryCode;
        member.Modified = DateTime.Now;
        member.ModifiBy = memv.OperatorId;
        member.SiteId = memv.SiteId;
        member.Active = memv.Active;

        if (member.Id == 0)
        {
            study.StudyMember.Add(member);
        }

    }
    string GetURI(string rootpath,bool isfilesystem,string newfolder)
    {
        var temp = string.Empty;
        if (isfilesystem)
        {
            temp = string.Format(@"{0}\{1}", rootpath, newfolder);
        }
        else
        {
            temp = string.Format("{0}/{1}", rootpath, newfolder);
        }
        return temp;
    }

    string GetURI(string rootpath, bool isfilesystem, string newfolder,string countrycode)
    {
        var temp = string.Empty;
        if (isfilesystem)
        {
            temp = string.Format(@"{0}\{2}\{1}", rootpath, newfolder, countrycode);
        }
        else
        {
            temp = string.Format("{0}/{2}/{1}", rootpath, newfolder, countrycode);
        }
        return temp;
    }
    public void MappingFolders(Common.ConfigSetting config)
    {
        var isChecked = "X";
        List<string> paths = new List<string>();
        var studies = _studyRepos.GetStudyList();
        var isfilesystem=config.HostType == (int)HostType.FileSystem || config.HostType == (int)HostType.ShareFolder;
        string rootpaths = string.Empty;
        if(!isfilesystem)
            rootpaths = GetURI(config.PathURI, isfilesystem, config.RootFolder);
        else
            rootpaths = GetURI(config.PathURI, isfilesystem, config.RootFolder);
         paths.Add(rootpaths);
        studies.ForEach(study =>
        {
            var sutdypath = GetURI(rootpaths, isfilesystem, study.StudyNum);
            paths.Add(sutdypath);
           List<Country> countries=study.TrialRegional.Where(fg => fg.Active.HasValue && fg.Active.HasValue).Select(fg=>fg.Country).ToList();
           countries.ForEach(coun =>
           {
               var temp = GetURI(sutdypath, isfilesystem, coun.CountryCode);
               paths.Add(temp);
               var tempc = study.StudyTemplate.Where(f => f.Active.HasValue && f.Active.Value && f.TMFTemplate.IsCountryLevel == isChecked).Select(fg => fg.TMFTemplate).ToList();
               paths.AddRange(GetPathsFromTemplates(temp, tempc, isfilesystem));
           });
           List<StudySite> sites = study.StudySite.Where(fg => fg.Site.Active.HasValue && fg.Site.Active.Value).ToList();
           sites.ForEach(coun =>
           {
               var country = countries.FirstOrDefault(f => f.Id == coun.CountryId);
               if (country != null)
               {

                   var tems = GetURI(sutdypath, isfilesystem, coun.Site.SiteName, country.CountryCode);
                   paths.Add(tems);
                   var tempsits = study.StudyTemplate.Where(f => f.Active.HasValue && f.Active.Value && f.TMFTemplate.IsSiteLevel == isChecked).Select(fg => fg.TMFTemplate).ToList();
                   paths.AddRange(GetPathsFromTemplates(tems, tempsits, isfilesystem));
               }
               
           });
           var temps = study.StudyTemplate.Where(f => f.Active.HasValue && f.Active.Value && f.TMFTemplate.IsTrialLevel == isChecked).Select(fg => fg.TMFTemplate).ToList();
           paths.AddRange(GetPathsFromTemplates(sutdypath, temps, isfilesystem));
        });
        Mappingfolder(paths, config);
       
    }

    void Mappingfolder(List<string> paths, Common.ConfigSetting config)
    {
        FileHelper helper = new FileHelper();
        if (config.HostType == (int)HostType.FileSystem 
            || config.HostType == (int)HostType.ShareFolder)
        {
            helper.MappingFileFolder(paths);
        }
        else if (config.HostType == (int)HostType.SharePoint)
        {
            helper.MappingSharePointFolder(paths, config);
        }
    }
    public List<StudyViewModel> GetUserStudyList(int p)
    {
      var list = _studyRepos.GetUserStudyList(p);
      return  Common<Study, StudyViewModel>.ConvertToViewModel(list.Distinct().ToList());
    }
    List<string> GetPathsFromTemplates(string rootpath, List<TMFTemplate> temps, bool isfilesystem)
    {
        List<string> paths =new  List<string>();
          temps.GroupBy(fg => fg.ZoneNo).ToList().ForEach(fg =>
           {
               var temp = GetURI(rootpath, isfilesystem, fg.Key);
               paths.Add(temp);
               fg.GroupBy(fs => fs.SectionNo).ToList().ForEach(fr =>
               {
                   var tems = GetURI(temp, isfilesystem, fr.Key);
                     paths.Add(tems);
                     fr.GroupBy(ft => ft.ArtifactNo).ToList().ForEach(ft =>
                     {
                         var temt = GetURI(tems, isfilesystem, ft.Key);
                         paths.Add(temt);
                         ft.ToList().ForEach(ftt =>
                         {
                             var temtt = GetURI(temt, isfilesystem, ftt.ArtifactNo);
                             paths.Add(temtt);
                         });
                     });
               });
           });
          return paths;
      }
  
    }
}
