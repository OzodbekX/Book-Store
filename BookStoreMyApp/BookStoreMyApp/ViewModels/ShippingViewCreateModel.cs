using BookStoreMyApp.Models;

namespace BookStoreMyApp.ViewModels
{
    public class ShippingViewCreateModel
    {
        public string Address { get; set; }
        public int PostalCode { get; set; }
        public string SaleData { get; set; }
        public string PayTerms { get; set; }
        public int SpecialDiscount { get; set; } = 0;
        public int UserId { get; set; }

    }
}
