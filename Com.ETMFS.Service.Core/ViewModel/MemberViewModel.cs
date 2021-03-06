﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.Service.Core.ViewModel
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public int StudyId { get; set; }
        public int StudyMemId { get; set; }
        public int MemberId { get; set; }
        public string  MemberName { get; set; }
        public string CountryName { get; set; }
        public string SiteName { get; set; }
        public string Role { get; set; }
        public string RoleLevel { get; set; }
        public int? SiteId { get; set; }
        public int? CountryCode { get; set; }
        public string OperatorId { get; set; }
        public bool? Active { get; set; }
        public int? UserId { get; set; }  
    }
}
