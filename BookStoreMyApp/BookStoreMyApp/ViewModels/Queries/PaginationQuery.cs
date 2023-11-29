namespace BookStoreMyApp.ViewModels.Queries
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Text { get; set; }
        public PaginationQuery()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.Text = "";
        }
        public PaginationQuery(int pageNumber, int pageSize, string text)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize < 7 ? 10 : pageSize;
            this.Text = text;
        }

    }
}
