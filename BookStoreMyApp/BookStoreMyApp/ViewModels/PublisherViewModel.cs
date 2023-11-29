using BookStoreMyApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BookStoreMyApp.ViewModels
{
    public class PublisherViewModel
    {
        public int? PubId { get; set; }
        public string? PublisherName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }


    }
}
