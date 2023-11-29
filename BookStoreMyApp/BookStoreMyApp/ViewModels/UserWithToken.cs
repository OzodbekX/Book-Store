using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreMyApp.Models
{
    public class UserWithToken : User
    {

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; internal set; }

        public UserWithToken(User admins)
        {
            this.UserId = admins.UserId;
            this.EmailAddress = admins.EmailAddress;
            this.FirstName = admins.FirstName;
            this.MiddleName = admins.MiddleName;
            this.LastName = admins.LastName;
            this.Role= admins.Role;
        }
    }

}