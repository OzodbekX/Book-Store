using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMyApp.Models
{
    public partial class Sale
    {
        [Key]
        public int SaleId { get; set; }
        [ForeignKey("Store")]
        public string StoreId { get; set; } = null!;
        public string OrderNum { get; set; } = null!;
        public int SpecialDiscount { get; set; } = 0!;
        public DateTime OrderDate { get; set; }
        public short Quantity { get; set; }
        public string PayTerms { get; set; } = null!;
        [ForeignKey("Book")]
        public int BookId { get; set; } 
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Shipping")]
        public int ShippingId { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual Shipping Shipping { get; set; } = null!;
        public virtual Store Store { get; set; } = null!;
    }
}
