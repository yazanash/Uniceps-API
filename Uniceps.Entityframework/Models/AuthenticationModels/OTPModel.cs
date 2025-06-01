using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.AuthenticationModels
{
    public class OTPModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public int Otp { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
