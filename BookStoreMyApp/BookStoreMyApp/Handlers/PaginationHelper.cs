using BookStoreMyApp.Responses;
using BookStoreMyApp.Services;
using BookStoreMyApp.ViewModels.Queries;

namespace BookStoreMyApp.Handlers
{
    public class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationQuery validFilter, int totalRecords, IUriService uriService, string route)
        {
            var respose = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages/ validFilter.PageSize));
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                ? uriService.GetPageUri(new PaginationQuery(validFilter.PageNumber + 1, validFilter.PageSize,validFilter.Text), route)
                : null;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                ? uriService.GetPageUri(new PaginationQuery(validFilter.PageNumber - 1, validFilter.PageSize, validFilter.Text), route)
                : null;
            respose.FirstPage = uriService.GetPageUri(new PaginationQuery(1, validFilter.PageSize, validFilter.Text), route);
            respose.LastPage = uriService.GetPageUri(new PaginationQuery(roundedTotalPages, validFilter.PageSize, validFilter.Text), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
    }
}
