using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreMyApp.Models
{
    public partial class Author
    {
        public Author()
        {
            BookAuthors = new HashSet<BookAuthor>();
        }
        [Key]
        public int AuthorId { get; set;}
        public string LastName { get; set;} = null!;
        public string FirstName { get; set;} = null!;
        public string Phone { get; set;} = null!;
        public string? Address { get; set;}
        public string? City { get; set;}
        public string? State { get; set;}
        public string? Zip { get; set;}
        public string? EmailAddress { get; set;}
        public virtual ICollection<BookAuthor>? BookAuthors { get; set; }
    }
}
