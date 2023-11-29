using BookStoreMyApp.Models;

namespace BookStoreMyApp.ViewModels
{
    public class ShippingViewModel
    {
        public string Address { get; set; }
        public int PostalCode { get; set; }
        public ShippingState ShippingState { get; set; }
        public SalesViewModel SalesViewModel { get; set; }

    }
}
