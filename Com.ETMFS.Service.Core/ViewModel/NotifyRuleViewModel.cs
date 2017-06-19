using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.Service.Common;

namespace Com.ETMFS.Service.Core.ViewModel
{
    public partial class NotifyRuleViewModel
    {
        public int Id { get; set; }
        public int StudyId { get; set; }
        public int? StudyCountryId { get; set; }
        public int? StudySiteId { get; set; }
        public int? AlertType { get; set; }
        public int? TriggerDay { get; set; }
        public int? StudyTMFId { get; set; }
        public int? MileStoneId { get; set; }
        
        public DateTime? TriggerOnDate { get; set; }
        public int? AlertRule { get; set; }
        public string RuleField { get; set; }
        public string TriggerOnDateText { get { 
            return TriggerOnDate.HasValue ? TriggerOnDate.Value.ToString(Constant.Date_formatV1) : string.Empty; }
            set { if (!string.IsNullOrEmpty(value))TriggerOnDate = DateTime.Parse(value); } }

    }
}
