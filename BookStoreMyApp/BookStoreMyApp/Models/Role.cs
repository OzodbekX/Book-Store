using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStoreMyApp.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }
        [Key]
        public short RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; }
    }
}
