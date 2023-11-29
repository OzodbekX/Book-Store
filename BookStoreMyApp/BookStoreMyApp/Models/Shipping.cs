using System.ComponentModel.DataAnnotations;

namespace BookStoreMyApp.Models
{
    public class Shipping
    {
        public Shipping()
        {
            Sales= new HashSet<Sale>();
        }
        [Key]
        public int ShippingId { get; set; }
        public int PostalCode { get; set; }
        public string Address { get; set;}
        public ShippingState ShippingState { get; set;}
        public virtual ICollection<Sale> Sales { get; set; }

    }
}
