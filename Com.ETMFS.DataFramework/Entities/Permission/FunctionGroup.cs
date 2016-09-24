namespace Com.ETMFS.DataFramework.Entities.Permission
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("T_Function_Group")]
    public partial class FunctionGroup
    {
        public int Id { get; set; }

        public int? FunctionId { get; set; }

        public int? GroupId { get; set; }

        [StringLength(50)]
        public string AccessRight { get; set; }

        public virtual Functions Functions { get; set; }

        public virtual UserGroups T_UserGroups { get; set; }
    }
}
