namespace milkTea.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cart
    {
        public int CartID { get; set; }

        [StringLength(200)]
        public string Username { get; set; }

        public int ProductId { get; set; }

        public int Amount { get; set; }

        public virtual Products_Detail Products_Detail { get; set; }

        public virtual User_Accounts User_Accounts { get; set; }
    }
}
