using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.DataFramework.Entities.Core
{
  public  class TMFFilter
    {
      public string ZoneNo { get; set; }
      public string ZoneName { get; set; }
      public string SectionNo { get; set; }
      public string SectionName { get; set; }
      public int? Study { get; set; }
      public int? Country { get; set; }
      public int? Site { get; set; }
      public int? TMFId { get; set; }
      public int DocumentId { get; set; }
      public string SiteName { get; set; }
      public string CountryName { get; set; }
      public string StudyNum { get; set; }
      public string DocumentName { get; set; }
      public string DocumentType { get; set; }
      public string ArticleNo { get; set; }
      public string ArticleName { get; set; }
      public string VersionId { get; set; }
      public string Operation { get; set; }
      public string Comments { get; set; }
      public bool? Active { get; set; }
      public string TMFLevel { get; set; }
      public int? StudyTemplateId { get; set; }
      public IssueLogViewModel IssueLogViewModel { get; set; }
      public string DocumentLevel { get; set; }
      public string ProtocolNumber { get; set; }
      public string Language { get; set; }
      public bool? IsCountryShared { get; set; }
      public bool? IsSiteShared { get; set; }
      public string SharedCountryIds { get; set; }
      public string SharedCountryNames { get; set; }
      public string SharedSiteIds { get; set; }
      public string SharedSiteNames { get; set; }
      public string DocumentDate { get; set; }
      public string TMFType { get; set; }
      public string IssueLoges { get; set; }
      public bool? HasIssue { get; set; }
      public string IssueLogIds { get; set; }
    }

  public partial class IssueLogViewModel
  {
      public int Id { get; set; }
      public int ReviewerId { get; set; }
      public string ReviewName { get; set; }
      public string LogNum { get; set; }
      public string Reason  { get; set; }
      public int DocumentId { get; set; }
      public string Status { get; set; }
      public string ReviewDate { get; set; }
      public string Comments { get; set; }
      public bool? Active { get; set; }
      public DateTime? Created { get; set; }
      public string CreateBy { get; set; }
      public DateTime? Modified { get; set; }
      public string ModifiBy { get; set; }
      public string AssignedUsers{get;set;}
      public string AssignUserIds { get; set; }
      public string OthersUsers { get; set; }
      
      
  }


}
