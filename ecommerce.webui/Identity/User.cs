using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ecommerce.webui.Identity
{
    public class User: IdentityUser //IdentityUser içinden ekstra kolonlar gelecek
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}