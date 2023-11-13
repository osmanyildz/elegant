using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.webui.Identity;
using Microsoft.AspNetCore.Identity;

namespace ecommerce.webui.Models
{
    public class RoleModel
    {
        public string Name { get; set; }
    }
   public class RoleDetails{
        public IdentityRole Role { get; set; }
        public IEnumerable<User> Members { get; set; } //İlgili role üye olan kullanıcıların listesi
        public IEnumerable<User> NonMembers { get; set; } //İlgili role üye olmayan kullanıcıların listesi
    }
    public class RoleEditModel{
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}