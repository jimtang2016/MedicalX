namespace Com.ETMFS.DataFramework.Entities.Permission
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("T_OptionList")]
    public partial class  OptionList
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string CNText { get; set; }

        [StringLength(200)]
        public string ENText { get; set; }

        [StringLength(200)]
        public string OptionValue { get; set; }

        public int? ParentId { get; set; }
    }
}
