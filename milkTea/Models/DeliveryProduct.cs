namespace milkTea.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DeliveryProduct")]
    public partial class DeliveryProduct
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(200)]
        public string Seller { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(200)]
        public string Customer { get; set; }

        public bool? Status { get; set; }

        public virtual User_Accounts User_Accounts { get; set; }

        public virtual Products_detail Products_detail { get; set; }

        public virtual User_Accounts User_Accounts1 { get; set; }
    }
}
