namespace milkTea.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products_Detail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products_Detail()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
        }

        [Key]
        public int ProductId { get; set; }

        [StringLength(200)]
        public string Seller { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Imgage_url { get; set; }

        [StringLength(500)]
        public string Desciption { get; set; }

        public int? CatId { get; set; }

        [StringLength(1)]
        public string Size { get; set; }

        public float Price { get; set; }

        public int Quantity { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        public virtual User_Accounts User_Accounts { get; set; }

        public virtual Size Size1 { get; set; }
    }
}
