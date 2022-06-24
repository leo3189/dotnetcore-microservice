namespace CatalogApi.Context
{
    public class CatalogContext : DbContext
    {
        public DbSet<CatalogItem> CatalogItems { get; set; }

        public DbSet<CatalogType> CatalogTypes { get; set; }

        public DbSet<CatalogGroup> CatalogGroups { get; set; }

        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }

    }
}
