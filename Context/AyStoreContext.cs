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
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Login> Login { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }

}