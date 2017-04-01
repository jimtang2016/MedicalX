using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.DataFramework.Interfaces.Core;
using Com.ETMFS.DataFramework.Interfaces.Permission;
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
      IUserGroupsRepository _groupRepos;
      IUsersRepository _userRepos;
      ICountryRepository _countryRepos;
      ITMFTemplateRepository _tmfRepos;
      public StudyService(IStudyRepository studyRepos, IUnitOfWork unitwork, IOptionListRepository optionlist, IUserGroupsRepository groupRepos, IUsersRepository userRepos, ICountryRepository countryRepos,ITMFTemplateRepository tmfRepos)
      {
          _countryRepos = countryRepos;
          _studyRepos = studyRepos;
          _unitWork = unitwork;
          _optionlist = optionlist;
          _groupRepos = groupRepos;
          _userRepos = userRepos;
          _tmfRepos = tmfRepos;
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

      public List<SiteViewModel> GetStudySites(int id,int? countryId,int? p)
      {
          if (id > 0)
          {
              var list = _studyRepos.GetStudySitesById(id,p).Where(f => countryId.HasValue&&f.CountryId == countryId || !countryId.HasValue).ToList();
             
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
          if(!study.StudyMember.Any(fg=>fg.RoleLevel==Constant.RoleLevel_Site&&fg.SiteId==studysite.Id))
          {
               MemberViewModel mem = new MemberViewModel()
              {
                  RoleLevel = Constant.RoleLevel_Site,
                  CountryCode = site.CountryId,
                  SiteId = studysite.Id,
                  Active = site.Active,
                  OperatorId = op
              };
              AddOrSaveMember(study, mem);
          }

          _unitWork.Commit();
         
      }
      public void SaveTemplates(int id, List<TMFRefernceOptionViewModel> list, string p, bool isdel, List<int> countrys)
      {
          var study = _studyRepos.GetById(id);

          list.ForEach(f =>
          {
              StudyTemplate temp = null;
              if (f.StudyTemplateId > 0)
              {
                  temp = study.StudyTemplate.FirstOrDefault(fg => fg.Id == f.StudyTemplateId);
              }
              else
              {
               temp = study.StudyTemplate.FirstOrDefault(fg => fg.TemplateId == f.Id);
              }
            
              if (temp == null)
              {
                  temp = new StudyTemplate()
              {
                  TemplateId = f.Id,
                  Study = study,
                  CreateBy = p,
                  Created = DateTime.Now,
                  TMFTemplate = _tmfRepos.GetById(f.Id)
              };
              }
              temp.Modified = DateTime.Now;
              temp.ModifiBy = p;
              if (isdel && countrys.Count > 0)
              {
                  countrys.ForEach(fg =>
                  {
                      temp.TemplateOutcluding.Add(new TemplateOutcluding()
                      {
                          StudyTemplateId = temp.Id,
                          CountryCode = fg,
                          Active = true,
                          CreateBy = p,
                          Created = DateTime.Now,
                          ModifiBy = p,
                          Modified = DateTime.Now
                      });
                  });
              }
              else if (!isdel && countrys.Count > 0)
              {
                  countrys.ForEach(country =>
                  {
                      var slist = temp.TemplateOutcluding.Where(ot=>ot.Active.HasValue&&ot.Active.Value).ToList();
                      slist.ForEach(otc =>
                      {
                          otc.Active = false;
                          otc.Modified = DateTime.Now;
                          otc.CreateBy = p;
                      });
                  });
                 
              }
              else
              {
                  temp.Active = !isdel;
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
          var study = _studyRepos.GetById(mem.StudyId);

          var user = _userRepos.GetById(mem.MemberId);
          var group = study.StudyMember.FirstOrDefault(st => st.RoleLevel
              == mem.RoleLevel && mem.Role == st.Role
              && (mem.SiteId == null || mem.SiteId.HasValue && mem.SiteId == st.SiteId) && (mem.CountryCode == null || mem.CountryCode.HasValue && mem.CountryCode == st.CountryCode)
              );
          var oldgroup = study.StudyMember.FirstOrDefault(st => st.Id == mem.StudyMemId);
          if (group != null)
          {
              if (oldgroup != null)
              {
                  var guser = oldgroup.UserGroup.Users.FirstOrDefault(u => u.Id == mem.UserId);
                  if(guser!=null)
                  oldgroup.UserGroup.Users.Remove(guser);
              }
              group.UserGroup.Users.Add(user);
          }
          _unitWork.Commit();
      }
      public PageResult<MemberViewModel> GetStudyMembers(int id, int page, int rows,int? countryId,int? siteId)
      {
          PageResult<TrialMemberView> pagein = new PageResult<TrialMemberView>
          {
              CurrentPage = page,
              PageSize = rows
          };
          pagein = _studyRepos.GetMemberListById(id, pagein, countryId,  siteId);
          PageResult<MemberViewModel> pageout = new PageResult<MemberViewModel>
          {
              CurrentPage = page,
              PageSize = rows,
              Total = pagein.Total,
              ResultRows = Common<TrialMemberView, MemberViewModel>.ConvertToViewModel(pagein.ResultRows)
          };
          return pageout;
      }
     public List<TrialReginalViewModel> GetTrialRegionals(int id,int p)
      {
          return _studyRepos.GetTrialCountries(id).Where(f => f.Study.StudyMember.Any(fg => (fg.Role == Constant.Role_Owner || fg.Role == Constant.Role_Uploader)
              &&(!fg.CountryCode.HasValue 
              ||(fg.UserGroup.Users.Any(user=>user.Id==p)
              &&fg.Active.Value&&(!f.CountryId.HasValue||f.CountryId.HasValue&&f.CountryId==fg.CountryCode))))).Select(f => new TrialReginalViewModel()
          {
              CountryId=f.CountryId,
              CountryName=f.Country.CountryName,
              CountryCode=f.Country.CountryCode,
              Status=f.Status,
              Id=f.Id,
              Active=f.Active
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

      public int SaveStudyList(StudyViewModel studyv,string operationId)
      {
          var study = ConvertToEntity(studyv, operationId);
          if (study.Id <= 0)
          {
              _studyRepos.Add(study);
          }

          if(!study.StudyMember.Any(fg=>fg.RoleLevel==Constant.RoleLevel_Trial)){
               MemberViewModel memv = new MemberViewModel()
              {
                  RoleLevel =  Constant.RoleLevel_Trial,
                  Role = Constant.Role_Owner,
                  OperatorId = operationId
              };

              AddOrSaveMember(study, memv);
          }
          _unitWork.Commit();
          return study.Id;
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
            var meme=study.StudyMember.FirstOrDefault(f => f.Id == mem.StudyMemId);
            if (meme != null)
            {
              var user =_userRepos .GetById(mem.MemberId);
                meme.UserGroup.Users.Remove(user);
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
            trial.Status = trialReg.Status;
            trial.CountryId = trialReg.CountryId;
            trial.ModifiBy = op;
            trial.Modified = DateTime.Now;
            trial.Active = trialReg.Active;
            if (trial.Id<= 0)
            {
                study.TrialRegional.Add(trial);
            }
            _unitWork.Commit();
            if (!study.StudyMember.Any(fg => fg.RoleLevel == Constant.RoleLevel_Country && fg.CountryCode == trial.Id))
            {
                MemberViewModel memv = new MemberViewModel()
                {
                    CountryCode = trialReg.CountryId,
                    RoleLevel = Constant.RoleLevel_Country,
                    OperatorId = op,
                    Active = trialReg.Active,
                    CountryName = trialReg.CountryName
                };
                AddOrSaveMember(study, memv);
            }
               
        }
        _unitWork.Commit();
 
    }
      #endregion

    public List<TmfNote> GetStudyListView(int p)
    {
        var studyList = new List<TmfNote>();
        var studylist = _studyRepos.GetStudyListView(p);
        studylist.GroupBy(f => f.StudyId).ToList().ForEach(study =>
        {
            var stfilter=new TMFFilter();
            var tempst = study.First();
            stfilter.Study=tempst.StudyId;
            stfilter.StudyNum = tempst.StudyNum;
            var studytemp = new TmfNote()
            {
                id = study.Key.ToString(),
                text = tempst.StudyNum + "-" + tempst.Status,
                RoleLevel=Constant.RoleLevel_Trial,
                category = stfilter
            };
            var countrylist = new List<TmfNote>();
            study.GroupBy(fg => fg.CountryId).ToList().ForEach(country =>
            {
                var ctfilter = new TMFFilter();
                
                var temp = country.First();
                ctfilter.Study = temp.StudyId;
                ctfilter.StudyNum = temp.StudyNum;
                ctfilter.Country = temp.CountryId;
                ctfilter.CountryName = temp.CountryCode;
                var countrytemp = new TmfNote()
                {
                    id = country.Key.ToString(),
                    text = temp.CountryName + "-" + temp.CountryStatus
                    ,
                    RoleLevel = Constant.RoleLevel_Country,
                    category = ctfilter
                };
                var sitelist = new List<TmfNote>();
                country.ToList().ForEach(site =>
                {
                    if (site.SiteId != null)
                    {
                        var sitfilter = new TMFFilter();
                        sitfilter.Study = site.StudyId;
                        sitfilter.StudyNum = site.StudyNum;
                        sitfilter.Country = site.CountryId;
                        sitfilter.CountryName = site.CountryCode;
                        sitfilter.Site = site.SiteId;
                        sitfilter.SiteName = temp.SiteName;
                        var sitetemp = new TmfNote()
                        {
                            id = site.SiteId.ToString(),
                            text = site.SiteName + "-" + site.SiteStatus
                             ,
                            RoleLevel = Constant.RoleLevel_Site,
                            category = sitfilter
                        };
                        sitelist.Add(sitetemp);
                    }
                    
                });
                countrytemp.children.AddRange(sitelist);
                countrylist.Add(countrytemp);
            });
            studytemp.children.AddRange(countrylist);
            studyList.Add(studytemp);
        });
        return studyList;
    }

    public PageResult<TMFRefernceOptionViewModel> GetOutCountryTMFModels(int id, int countryId, int page, int rows)
    {
        var study = _studyRepos.GetById(id);
        var query = from sttemp in study.StudyTemplate where sttemp.TemplateOutcluding.Any(outc => outc.CountryCode == countryId && outc.Active.HasValue && outc.Active.Value) select ConvertTrialTemplate(sttemp);
        var temps=query.ToList();
        PageResult<TMFRefernceOptionViewModel> pageout = new PageResult<TMFRefernceOptionViewModel>()
        {
            CurrentPage = page,
            PageSize = rows,
            Total = temps.Count
           
        };
        pageout.ResultRows = temps.Skip(pageout.SkipCount).Take(pageout.PageSize).ToList();
        return pageout;
    }
    TMFRefernceOptionViewModel ConvertTrialTemplate(StudyTemplate f)
    {
        TMFRefernceOptionViewModel item = new TMFRefernceOptionViewModel()
        {
            ZoneNo = f.TMFTemplate.ZoneNo,
            ZoneName = f.TMFTemplate.ZoneName,
            SectionName = f.TMFTemplate.SectionName,
            SectionNo = f.TMFTemplate.SectionNo,
            StudyTemplateId = f.Id,
            ArtifactNo = f.TMFTemplate.ArtifactNo,
            IsCountryLevel = f.TMFTemplate.IsCountryLevel,
            IsSiteLevel = f.TMFTemplate.IsSiteLevel,
            IsTrialLevel = f.TMFTemplate.IsTrialLevel,
            Purpose = f.TMFTemplate.Purpose,
            ArtifactName = f.TMFTemplate.ArtifactName,
            Id = f.TMFTemplate.Id
        };
        return item;
    }

    public List<TMFRefernceOptionViewModel> GetTrialTempaltes(TMFFilter condition )
    {
         var query = from tmf in _studyRepos.GetSutdyTemplates(_studyRepos.GetById(condition.Study.Value), condition)
                    select tmf;
         var list = query.ToList();
        return list.Select(f =>ConvertTrialTemplate(f)).ToList();
    }
    public PermissionViewModel GetPermission(TMFFilter filter, int p)
    {
        PermissionViewModel permission = new PermissionViewModel();
        var study = _studyRepos.GetById(filter.Study.Value);
        IEnumerable<StudyMember> query = null;
        if (filter.Study.HasValue)
        {

            if (filter.Country != null && filter.Site==null)
            {
                query = study.StudyMember.Where(mem => (mem.CountryCode == filter.Country  &&mem.RoleLevel == Constant.RoleLevel_Country ||
              mem.RoleLevel == Constant.RoleLevel_Trial )&& mem.UserGroup.Users.Any(usr => usr.Id == p) && mem.Active.HasValue && mem.Active.Value
              );
            }
            else
                if (filter.Site != null)
                {

                    query = study.StudyMember.Where(mem => (
                        mem.SiteId == filter.Site
                 || mem.RoleLevel == Constant.RoleLevel_Trial 
                 || mem.RoleLevel == Constant.RoleLevel_Country && mem.CountryCode == filter.Country)
                 && mem.UserGroup.Users.Any(usr => usr.Id == p)
                  && mem.Active.HasValue && mem.Active.Value
                    );
                }
                else
                {
                    query = study.StudyMember.Where(mem => mem.UserGroup.Users.Any(usr => usr.Id == p)
               && mem.RoleLevel == Constant.RoleLevel_Trial
               && mem.Active.HasValue && mem.Active.Value
                 );
                }
            var mems = query.ToList();
            permission.IsOwner = mems.Any(mem => mem.Role == Constant.Role_Owner);
            permission.IsUploader = mems.Any(mem => mem.Role == Constant.Role_Uploader);
            permission.IsReviewer = mems.Any(mem => mem.Role == Constant.Role_Reviewer);
            permission.IsStudyOwner = mems.Any(f => f.Role == Constant.Role_Owner&&f.RoleLevel == Constant.RoleLevel_Trial && f.Role == Constant.Role_Owner);
            permission.IsCountryOwner = mems.Any(f => f.RoleLevel == Constant.RoleLevel_Country && f.Role == Constant.Role_Owner );
            permission.IsSiteOwner = mems.Any(f => f.RoleLevel == Constant.RoleLevel_Site && f.Role == Constant.Role_Owner);
            permission.IsStudyUploader = mems.Any(mem => mem.Role == Constant.Role_Uploader && mem.RoleLevel == Constant.RoleLevel_Trial);
            permission.IsCountryUploader = mems.Any(mem => mem.Role == Constant.Role_Uploader && mem.RoleLevel == Constant.RoleLevel_Country);
            permission.IsSiteUploader = mems.Any(mem => mem.Role == Constant.Role_Uploader && mem.RoleLevel == Constant.RoleLevel_Site);

            permission.IsStudyReviewer = mems.Any(mem => mem.Role == Constant.Role_Reviewer && mem.RoleLevel == Constant.RoleLevel_Trial);
            permission.IsCountryReviewer = mems.Any(mem => mem.Role == Constant.Role_Reviewer && mem.RoleLevel == Constant.RoleLevel_Country);
            permission.IsSiteReviewer = mems.Any(mem => mem.Role == Constant.Role_Reviewer && mem.RoleLevel == Constant.RoleLevel_Site);
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
        StudyMember ownerTeam = new StudyMember(){
            Role=Constant.Role_Owner,
            RoleLevel=memv.RoleLevel,
            ModifiBy=memv.OperatorId,
            Modified=DateTime.Now,
            CreateBy=memv.OperatorId,
            Created=DateTime.Now,
            Active=true
             
        };
        StudyMember uploaderTeam = new StudyMember()
        {
            Role = Constant.Role_Uploader,
            RoleLevel = memv.RoleLevel,
            ModifiBy = memv.OperatorId,
            Modified = DateTime.Now,
            CreateBy = memv.OperatorId,
            Created = DateTime.Now,
            Active = true
        };

        StudyMember reviewerTeam = new StudyMember()
        {
            Role=Constant.Role_Reviewer,
            RoleLevel=memv.RoleLevel,
            ModifiBy=memv.OperatorId,
            Modified=DateTime.Now,
            CreateBy=memv.OperatorId,
            Created=DateTime.Now,
            Active=true
             
        };

        if (memv.CountryCode != null && memv.CountryCode.HasValue)
        {
            var country = _countryRepos.GetById(memv.CountryCode.Value);
            reviewerTeam.CountryCode = uploaderTeam.CountryCode = ownerTeam.CountryCode = country.Id;
         memv.CountryName=   reviewerTeam.CountryName = uploaderTeam.CountryName = ownerTeam.CountryName = country.CountryName;
        }
        
        if (memv.RoleLevel == Constant.RoleLevel_Site)
        {
            var site = study.StudySite.FirstOrDefault(f => f.SiteId == memv.SiteId);
            reviewerTeam.SiteId = uploaderTeam.SiteId = ownerTeam.SiteId = memv.SiteId;
            memv.SiteName = reviewerTeam.SiteName = uploaderTeam.SiteName = ownerTeam.SiteName = site.Site.SiteName;
        }
       
        var description=string.Format( Constant.Group_Split_Flag, study.StudyNum,memv.CountryName,memv.SiteName);
        reviewerTeam.UserGroup = new UserGroups()
        {
            GroupName = Constant.Role_Reviewer,
            Description = description,
            Active = true,
            CreateBy=memv.OperatorId,
            Created=DateTime.Now,
            ModifiBy = memv.OperatorId,
            Modified = DateTime.Now
        };
        ownerTeam.UserGroup = new UserGroups()
        {
            GroupName = Constant.Role_Owner,
            Description = description,
         
            Active = true,
            CreateBy=memv.OperatorId,
            Created=DateTime.Now,
            ModifiBy = memv.OperatorId,
            Modified = DateTime.Now
        };

        uploaderTeam.UserGroup = new UserGroups()
        {
            GroupName = Constant.Role_Uploader,
            Description = description,
            Active = true,
            CreateBy=memv.OperatorId,
            Created=DateTime.Now,
            ModifiBy = memv.OperatorId,
            Modified = DateTime.Now
        };
        var user = _userRepos.GetUserByName(memv.OperatorId);
        uploaderTeam.UserGroup.Users.Add(user);
        reviewerTeam.UserGroup.Users.Add(user);
        ownerTeam.UserGroup.Users.Add(user);
        study.StudyMember.Add(uploaderTeam);
        study.StudyMember.Add(ownerTeam);
        study.StudyMember.Add(reviewerTeam);
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
    public void MappingFolders(Common.ConfigSetting config,int? studyId)
    {
        var isChecked = "X";
        List<string> paths = new List<string>();

        var studies = _studyRepos.GetStudyList().Where(f => studyId!=null&&f.Id == studyId || studyId == null).ToList();
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
                   var tempsits = study.StudyTemplate.Where(f => f.Active.HasValue && f.Active.Value && f.TMFTemplate.IsSiteLevel == isChecked  ).Select(fg => fg.TMFTemplate).ToList();
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


    #region IStudyService Members


    public List<TrialReginalViewModel> GetTrialRegionals(int id)
    {
        return _studyRepos.GetTrialCountries(id).Select(f => new TrialReginalViewModel()
             {
                 CountryId = f.CountryId,
                 CountryName = f.Country.CountryName,
                 CountryCode = f.Country.CountryCode,
                 Id=f.Id,
                 StudyId=f.StudyId,
                 Status=f.Status
             }).Distinct().ToList();
    }

    #endregion

    #region IStudyService Members


    public List<SiteViewModel> GetStudySites(int id )
    {
        if (id > 0)
        {
           
            var list = _studyRepos.GetStudySitesById(id,p:null).ToList();
            return Common<Site, SiteViewModel>.ConvertToViewModel(list);
        }
        else
        {
            return null;
        }
    }

    #endregion
    }
}
