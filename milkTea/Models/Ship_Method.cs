namespace milkTea.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ship_Method
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ship_Method()
        {
            Orders_Detail = new HashSet<Orders_Detail>();
        }

        [Key]
        public int ShipId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public float Fee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders_Detail> Orders_Detail { get; set; }
    }
}
