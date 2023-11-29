using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMyApp.Models
{
    public partial class User
    {
        public User()
        {
            RefreshTokens = new HashSet<RefreshToken>();
            UserResults = new HashSet<UserResult>();
        }
        [Key]
        public int UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        //public string Source { get; set; }
        public string FirstName { get; set; }
        public string? AvailableAddresses { get; set; } = null;
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        [ForeignKey("Role")]
        public short RoleId { get; set; }
        //[ForeignKey("Pub")]
        //public int PubId { get; set; }
        //public DateTime? HirkeDate { get; set; }
        //public virtual Publisher Pub { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<UserResult> UserResults { get; set; } = null!;

    }
}
