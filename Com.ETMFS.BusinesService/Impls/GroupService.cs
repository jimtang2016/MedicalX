using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.BusinesService.Interfaces;
using Com.ETMFS.BusinesService.ViewModel.Permission;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.DataFramework.Interfaces.Permission;
using Com.ETMFS.Service.Common;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.BusinesService.Impls
{
    

    public class GroupService:IGroupService
    {
        IUserGroupsRepository _groupRepo;
        IUsersRepository _userRepo;
        IUnitOfWork _unitWork;
        public GroupService(IUserGroupsRepository groupRepo, IUnitOfWork unitWork,IUsersRepository userRepo)
        {
            _groupRepo = groupRepo;
            _unitWork = unitWork;
            _userRepo = userRepo;
        }

        #region IGroupService Members

        public PageResult<UserGroupViewModel> GetGroupList(int currentPage, int pageSize, string searchConditions)
        {
            PageResult<UserGroups> page = new PageResult<UserGroups>()
            {
                CurrentPage = currentPage,
                PageSize = pageSize
            };
            var rpage = _groupRepo.GetGroupList(page);
            PageResult<UserGroupViewModel> tpage = new PageResult<UserGroupViewModel>()
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Total = rpage.Total,
                ResultRows = Common<UserGroups, UserGroupViewModel>.ConvertToViewModel(rpage.ResultRows)
            };
            return tpage;
        }

        #endregion


 
        public void SaveGroup(UserGroupViewModel vgroups,string username)
        {
            var group = ConvertoEntiy(vgroups, username);

            if (group.Id > 0)
            {
                _groupRepo.Update(group);
            }
            else
            {
                _groupRepo.Add(group);
            }
            _unitWork.Commit();
        }
     

        UserGroups ConvertoEntiy(UserGroupViewModel vgroups, string username)
        {
            var group = _groupRepo.GetById(vgroups.Id);

            if (group == null)
            {
                group = new UserGroups()
                {

                    CreateBy = username,
                    Created = DateTime.Now,

                };
            }

            group.GroupName = vgroups.GroupName;
            group.Description = vgroups.Description;
            group.Active = true;
            group.Modified = DateTime.Now;
            group.ModifiBy = username;
            if (vgroups.GUsers != null)
            {
                vgroups.GUsers.ForEach(u =>
                {
                    var user = _userRepo.GetById(u.Id);
                    if (u.OPStatus == Constant.Add)
                    {
                        if (!group.Users.Contains(user))
                            group.Users.Add(user);
                    }
                    else if (u.OPStatus == Constant.Remove)
                    {
                        group.Users.Remove(user);
                    }

                });
            }

            return group;
        }

     

        public void RemoveGroups(List<UserGroupViewModel> vgroups,string username)
        {
            vgroups.ForEach(g =>
            {
                var group = _groupRepo.GetById(g.Id);
                group.Users.Clear();
                group.Active = false;
                group.ModifiBy = username;
                group.Modified = DateTime.Now;
                _groupRepo.Update(group);
            });
            _unitWork.Commit();
        }



        #region IGroupService Members


        public PageResult<UserViewModel> GetUserList(int page, int rows, int id)
        {
            var group = _groupRepo.GetById(id);
           
            PageResult<UserViewModel> tpage = new PageResult<UserViewModel>();
            tpage.CurrentPage = page;
            tpage.PageSize = rows;
            if (group != null)
            {
                var skip = 0;
                if (page > 1)
                {
                    skip = (page - 1) * rows;
                }
                tpage.Total = group.Users.Count;
                tpage.ResultRows = Common<Users, UserViewModel>.ConvertToViewModel(group.Users.ToList().Skip(skip).Take(rows).ToList());


            }
            return tpage;

        }

        #endregion
    }
}
