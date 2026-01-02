using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.Products
{
    public class UserStep
    {
        public int Id { get; set; }
        public int StepNumber { get; set; } // ترتيب الخطوة
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = "";
        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = "";
        public string? ImageUrl { get; set; } // صورة توضيحية للخطوة
        public int ProductId { get; set; }
    }
}
