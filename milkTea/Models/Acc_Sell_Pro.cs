namespace milkTea.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Acc_Sell_Pro
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(200)]
        public string Username { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        public int? Quantity { get; set; }

        public virtual Product Product { get; set; }

        public virtual User_Accounts User_Accounts { get; set; }
    }
}
