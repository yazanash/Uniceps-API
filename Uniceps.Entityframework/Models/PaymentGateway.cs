using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models
{
    public class PaymentGateway
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TransferInfo { get; set; } = null!;
        public string? QrCodeUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
