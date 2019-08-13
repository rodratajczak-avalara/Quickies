using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace DeleteFormDueDate
{
    public partial class TaxFormCatalogContext : DbContext
    {
        private static IConfiguration _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

        public TaxFormCatalogContext()
        {

        }

        public TaxFormCatalogContext(DbContextOptions<TaxFormCatalogContext> options)
        : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("TaxFormCatalog"));
        }
    }
}
