using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.Service.Core.ViewModel
{
    public partial class MileStoneViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PlanStartDate { get; set; }

        public string PlanEndDate { get; set; }

        public string ActualStartDate { get; set; }

        public string ActualEndDate { get; set; }

        public int? StudySiteId { get; set; }

        public int? StudyCountryId { get; set; }
    }
}
