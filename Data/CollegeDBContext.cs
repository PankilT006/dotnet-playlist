using Microsoft.EntityFrameworkCore;

namespace WebApi.Data;

public class CollegeDBContext : DbContext
{
    public CollegeDBContext(DbContextOptions<CollegeDBContext> options)
        : base(options)
    {
    }

    public DbSet<Students> Students { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //student table 1
        // config in future whenever new tables comes apply there configuration here in this method
       modelBuilder.ApplyConfiguration(new Config.StudentConfig());
    }
}
