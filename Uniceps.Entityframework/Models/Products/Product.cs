using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.Products
{
    [Index(nameof(Slug), IsUnique = true)]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string NameAr { get; set; } = "";
        public Platform Platform { get; set; }
        public string Description { get; set; } = "";
        public string DescriptionAr { get; set; } = "";
        public string? HeroImage { get; set; } = "";
        public int AppId { get; set; }
        public string Slug { get; set; } = "";

        public void GenerateSlug()
        {
            string baseSlug = Name.ToLower()
                                .Replace(" ", "_")
                                .Replace("/", "_")
                                .Trim('_');

            Slug = $"{baseSlug}-{Platform.ToString().ToLower()}-{AppId}";
        }
    }
    public enum Platform
    {
        Mobile = 1,
        Desktop = 2,
        Web = 3,
    }
}