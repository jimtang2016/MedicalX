namespace Com.ETMFS.DataFramework.Entities.Permission
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("T_Functions")]
    public partial class Functions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Functions()
        {
            FunctionGroup = new HashSet<FunctionGroup>();
        }

        public int Id { get; set; }

        [StringLength(200)]
        public string FunctionName { get; set; }

        [StringLength(200)]
        public string ENTitle { get; set; }

        [StringLength(200)]
        public string CNTitle { get; set; }

        public string UrlAddres { get; set; }

        public bool? Active { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }

        [StringLength(100)]
        public string ModifiBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FunctionGroup> FunctionGroup { get; set; }
    }
}
