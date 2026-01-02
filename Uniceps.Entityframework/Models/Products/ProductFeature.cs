using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.Products
{
    public class ProductFeature
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = "";
        public string DescriptionAr { get; set; } = "";
        public string Description { get; set; } = string.Empty;
        public int ProductId { get; set; } 
    }
}
