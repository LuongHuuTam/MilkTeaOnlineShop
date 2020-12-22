namespace milkTea.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders_Detail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders_Detail()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        public int OrderDetailId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? ReceiveDate { get; set; }

        [Column(TypeName = "ntext")]
        public string Note { get; set; }

        public int? TotalAmount { get; set; }

        public float? TotalMoney { get; set; }

        public int? ShipId { get; set; }

        [Required]
        [StringLength(200)]
        public string Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        public virtual User_Accounts User_Accounts { get; set; }

        public virtual Ship_Method Ship_Method { get; set; }
    }
}
