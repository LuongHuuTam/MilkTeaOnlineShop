namespace milkTea.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Amount { get; set; }

        public byte Status { get; set; }

        public int Orders_Detail { get; set; }

        public virtual Orders_Detail Orders_Detail1 { get; set; }

        public virtual Products_Detail Products_Detail { get; set; }
    }
}
