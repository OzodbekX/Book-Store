using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreMyApp.Models
{
    public partial class Book
    {
        public Book()
        {
            BookAuthors = new HashSet<BookAuthor>();
            Sales = new HashSet<Sale>();
        }
        [Key]
        public int BookId { get; set; }
        public float Rating { get; set; }
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        [ForeignKey("Pub")]
        public int? PubId { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int Vat { get; set; }
        public decimal? Advance { get; set; }
        public int? Royalty { get; set; }
        public int? YtdSales { get; set; }
        public string? Notes { get; set; }
        public DateTime PublishedDate { get; set;}
        public virtual Publisher Pub { get; set;} = null!;
        public virtual ICollection<Picture> Pictures { get; set;}
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }

    }
}
