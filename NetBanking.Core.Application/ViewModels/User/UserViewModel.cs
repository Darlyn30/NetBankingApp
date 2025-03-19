using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string IdentificationNumber { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}
