namespace Com.ETMFS.DataFramework.Entities.Permission
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("T_LoginHistory")]
    public partial class LoginHistory
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LoginTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LogOffTime { get; set; }

        public decimal? Duration { get; set; }

        [StringLength(255)]
        public string LoginIpAddress { get; set; }

        public virtual Users Users { get; set; }
    }
}
