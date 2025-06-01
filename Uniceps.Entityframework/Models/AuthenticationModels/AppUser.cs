using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.AuthenticationModels
{
    public class AppUser : IdentityUser
    {
       public UserType UserType { get; set; }
    }
    public enum UserType
    {
        Normal=0,
        Business=1
    }
}
