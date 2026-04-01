using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.DBContext
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=db20602.public.databaseasp.net; Database=db20602; User Id=db20602; Password=A#i54mP+k_8H; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
