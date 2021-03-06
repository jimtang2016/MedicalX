﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.Service.Core.ViewModel
{
 public  class DocumentViewModel
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public string Uploader { get; set; }
        public string Operation { get; set; }
        public int? UploaderId { get; set; }
        public string VersionId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string ModifiBy { get; set; }
     
        public DateTime? Modified { get; set; }
        public int StudyId { get; set; }
        public string StudyNum { get; set; }
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public string SiteName { get; set; }
        public int SiteId { get; set; }
        public string ReviewerName { get; set; }
        public int ReviewId { get; set; }
        public string ZoneNo { get; set; }
        public string ZoneName { get; set; }
        public string SectionName { get; set; }
        public string SectionNo { get; set; }
        public string ArtifactName { get; set; }
        public string ArtifactNo { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
     
        public int StudyTemplateId { get; set; }
        public string DocumentLevel { get; set; }
        public string ProtocolNumber { get; set; }
        public string Language { get; set; }
        public bool? IsCountryShared { get; set; }
        public bool? IsSiteShared { get; set; }
        public string SharedCountryIds { get; set; }
        public string SharedCountryNames { get; set; }
        public string SharedSiteIds { get; set; }
        public string SharedSiteNames { get; set; }
        public DateTime? DocumentDate { get; set; }

        public bool? HasIssue { get; set; }
        public string IssueLoges { get; set; }
        public string IssueLogIds { get; set; }
        public int? TMFId { get; set; }
     
    }
}
