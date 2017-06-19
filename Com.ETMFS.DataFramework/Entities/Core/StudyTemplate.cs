using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace Com.ETMFS.DataFramework.Entities.Core
{
    [Table("T_StudyTemplate")]
    public partial class StudyTemplate
    {
        ICollection<StudyDocument> _studydoc = null;
        ICollection<TemplateOutcluding> _templateOutcluding = null;
        ICollection<NotificationRules> _notificationRules = null;
        public StudyTemplate()
        {
            _studydoc = new HashSet<StudyDocument>();
            _templateOutcluding = new HashSet<TemplateOutcluding>();
            ICollection<NotificationRules> _notificationRules = new List<NotificationRules>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StudyId { get; set; }
        public int TemplateId { get; set; }
        public bool? Active { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }

        [StringLength(100)]
        public string ModifiBy { get; set; }

        public virtual Study Study { get; set; }
        public virtual TMFTemplate TMFTemplate { get; set; }

        public virtual ICollection<StudyDocument> StudyDocument { get { return _studydoc; } }

        public virtual ICollection<TemplateOutcluding> TemplateOutcluding { get { return _templateOutcluding; } }

        public virtual ICollection<NotificationRules> NotificationRules { get { return _notificationRules; } }
    }
}
