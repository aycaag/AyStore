using Microsoft.EntityFrameworkCore;

public class AyStoreContext : DbContext
{
    public AyStoreContext()
    {

    }

    public AyStoreContext(DbContextOptions<AyStoreContext> options)
        : base(options)
    {

    }


    public virtual DbSet<CategoriesDMO> Category { get; set; }

}