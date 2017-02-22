using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.View;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Interfaces.Core
{
    public interface IStudyRepository : IRepository<Study>
    {
        PageResult<Study> GetStudyList(PageResult<Study> pagein);

        PageResult<TrialReginalView> GetTrialReginals(int id, PageResult<TrialReginalView> pagein);
 

        PageResult<StudyMember> GetMemberListById(int id, PageResult<StudyMember> pagein);

        List<TrialRegional> GetTrialCountries(int id);

        PageResult<TMFTemplate> GetTMFRefernceListById(int id, PageResult<TMFTemplate> pagein,int userId);

        PageResult<Site> GetStudySitesById(int id, PageResult<Site> pagein);

        List<Site> GetStudySitesById(int id);

        List<Study> GetStudyList();
        List<Study> GetUserStudyList(int userId);

        List<StudyTemplate> GetSutdyTemplates(Study studylist,TMFFilter condition);
    }
}
