using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStoreMyApp.Models
{
    public partial class Job
    {
        [Key]
        public short JobId { get; set; }
        public string JobDesc { get; set; } = null!;
    }
}
