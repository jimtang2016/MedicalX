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


        PageResult<TrialMemberView> GetMemberListById(int id, PageResult<TrialMemberView> pagein,int? countryId,int? siteId);

        List<TrialRegional> GetTrialCountries(int id);

        List<StudyListView> GetStudyListView(int p);

        PageResult<TMFTemplate> GetTMFRefernceListById(int id, PageResult<TMFTemplate> pagein, int userId);

        PageResult<Site> GetStudySitesById(int id, PageResult<Site> pagein);

        List<Site> GetStudySitesById(int id,int? p);

        List<Study> GetStudyList();
        List<Study> GetUserStudyList(int userId);

        List<StudyTemplate> GetSutdyTemplates(Study studylist, TMFFilter condition);

        List<StudyTemplate> GetTMFRefernceListById(int id, int p);
    }
}
