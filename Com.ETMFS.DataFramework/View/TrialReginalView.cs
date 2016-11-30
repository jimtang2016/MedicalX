﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.DataFramework.View
{
   public class TrialReginalView
    {
        public int Id { set; get; }
        public int? CountryId { set; get; }
        public int? StudyId { set; get; }
        public int? OwnerId { set; get; }
        public string CountryName { set; get; }
        public string OwnerName { set; get; }
        public int? MemberId { set; get; }
        public bool? Active { set; get; }
    }
}
