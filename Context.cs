using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking2_EF
{
    public class Context : DbContext
    {
        public DbSet<Office> Offices { get; set; }
        public DbSet<Product> Products { get; set; }
        public string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = AssetTracking2_EF_DavidNilsson; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(cs);
        }
    }
}
