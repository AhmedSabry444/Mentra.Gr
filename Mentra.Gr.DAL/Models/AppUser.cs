using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentra.Gr.DAL.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public int Age { get; set; }
    }
}
