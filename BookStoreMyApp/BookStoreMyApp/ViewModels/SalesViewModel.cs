namespace BookStoreMyApp.ViewModels
{
    public class SalesViewModel
    {

        public string StoreId { get; set; } = null!;
        public string OrderNum { get; set; } = null!;
        public string SpecialDiscount { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public short Quantity { get; set; }
        public string PayTerms { get; set; } = null!;
        public int BookId { get; set; }
        public int UserId { get; set; }
    }
}
