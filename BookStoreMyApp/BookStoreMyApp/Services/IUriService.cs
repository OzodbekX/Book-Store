using BookStoreMyApp.ViewModels.Queries;

namespace BookStoreMyApp.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationQuery filter, string route);
    }
}
