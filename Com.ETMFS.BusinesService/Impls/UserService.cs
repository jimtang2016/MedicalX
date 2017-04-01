using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.BusinesService.Interfaces;
using Com.ETMFS.BusinesService.ViewModel;
using Com.ETMFS.BusinesService.ViewModel.Permission;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.DataFramework.Interfaces.Permission;
using Com.ETMFS.Service.Common;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.BusinesService.Impls
{
    public class UserService:IUserService
    {
       IUsersRepository _userRepo;
       IUnitOfWork _unitWork;

       public UserService(IUsersRepository userRepo,IUnitOfWork unitwork)
       {
           _userRepo = userRepo;
           _unitWork = unitwork;
       }

       public List<UserViewModel> GetDocumentUserList(TMFFilter tmf)
       {
           var memlevel = Constant.RoleLevel_Trial;
           if (tmf.Country != null)
           {
               memlevel = Constant.RoleLevel_Country;
           }
           if (tmf.Site >0)
           {
               memlevel = Constant.RoleLevel_Site;
           }

           var users = _userRepo.GetAll().Where(f => f.UserGroups.Any(user => user.Active.Value && user.StudyMember.Any(stm=> stm.Study.Id== tmf.Study
                 && (tmf.Site == null || tmf.Site != null && tmf.Site == stm.SiteId) && stm.RoleLevel == memlevel &&
                 (tmf.Country == null || tmf.Country != null && tmf.Country == stm.CountryCode)))).ToList();
          return Common<Users, UserViewModel>.ConvertToViewModel(users);
       }

       #region IUserService Members

       public UserViewModel LogIn(string userId, string password,string ipAddress)
       {
         var user=  _userRepo.GetUserByName(userId);
         UserViewModel userv = new UserViewModel();
         if (user != null&&user.Password.Equals(password))
         { 
                 userv.UserName = user.UserName;
                 userv.Id = user.Id;
                userv.Groups.AddRange( this.AddGroups(userv.Groups, user.UserGroups.ToList()));
                 _userRepo.SaveLoginHistory(userv.Id, ipAddress);
                 _unitWork.Commit();
         }
         return userv;
       }
       private List<UserGroupViewModel> AddGroups(List<UserGroupViewModel> tgroup, List<UserGroups> list)
       {
           var groups = tgroup;
           list.ForEach(g =>
           {
               var group = new UserGroupViewModel()
               {
                   Id = g.Id,
                   GroupName = g.GroupName,
                   Description = g.Description
               };
               groups.Add(group);
           });
           return groups;
       }
       public bool LogOff(UserViewModel user)
       {
         return  _userRepo.SaveLoginHistory(user.Id, user.IpAddress);
       }

       #endregion

       #region IUserService Members


       public PageResult<UserViewModel> GetUserList(int currentPage, int pageSize, string searchConditions)
       {
           PageResult<Users> page = new PageResult<Users>()
           {
               CurrentPage = currentPage,
               PageSize = pageSize
           };
            var rpage=_userRepo.GetUserList(page);
           
           PageResult<UserViewModel> tpage = new PageResult<UserViewModel>()
           {
               CurrentPage = currentPage,
               PageSize = pageSize,
               Total = rpage.Total,
               ResultRows = Common<Users, UserViewModel>.ConvertToViewModel(rpage.ResultRows)
           };

           return tpage;
       }
       public List<UserViewModel> GetUserList()
       {
           var rpage = _userRepo.GetActiveUserList().Where(u => !u.UserGroups.Any(f => f.Id == Constant.Group_Administrators)).ToList();
           return Common<Users, UserViewModel>.ConvertToViewModel(rpage);
       }
       #endregion

       #region IUserService Members


       public void SaveUser(UserViewModel user)
       {
           if (user.Id > 0)
           {
               _userRepo.Update(ConvertViewtoEntity(user));
           }
           else
           {
               _userRepo.Add(ConvertViewtoEntity(user));
           }
           _unitWork.Commit();
       }

       #endregion

       #region IUserService Members


       public void RemoveUsers(List<UserViewModel> useres, string curuser)
       {
           useres.ForEach(f => {
               f.ModifiBy = curuser;
                var item=ConvertViewtoEntity(f);
               item.Active=false;
               _userRepo.Update(item);
           });
           
           _unitWork.Commit();
       }

    
       #endregion

      Users ConvertViewtoEntity(UserViewModel userv)
      {

          Users user = new Users()
          {
              Id = userv.Id,
              UserName = userv.UserName,
              IsMainContact = true,
              Password = userv.Password,
              Country = userv.Country,
              Email=userv.Email
          };
          if (userv.Id == 0)
          {
              user.CreateBy = userv.CreateBy;
              user.Created = DateTime.Now;
          }
          user.Active = true;
          user.Modified = DateTime.Now;
          user.ModifiBy = userv.ModifiBy;
          return user;
      }

      #region IUserService Members


      public PageResult<UserGroupViewModel> GetGroupsByUserId(int page, int rows, int id)
      {
          var user = _userRepo.GetById(id);
          var skip = (page - 1) > 0 ?  (page - 1)*rows :0;
          var pageresult = new PageResult<UserGroupViewModel>()
          {
              CurrentPage = page,
              PageSize = rows,
              ResultRows = new List<UserGroupViewModel>()
          };
          if (user != null)
          {
              pageresult.Total = user.UserGroups.Count;
              user.UserGroups.Skip(skip).Take(rows).ToList().ForEach(f =>
              {
                  pageresult.ResultRows.Add(new UserGroupViewModel()
                  {
                      GroupName = f.GroupName,
                      Description = f.Description,
                      Id = f.Id
                  });
              });

          }
       
          return pageresult;
      }

      #endregion
    }
}
