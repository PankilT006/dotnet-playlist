using Microsoft.EntityFrameworkCore;

namespace WebApi.Data;

public class CollegeDBContext : DbContext
{
    public CollegeDBContext(DbContextOptions<CollegeDBContext> options)
        : base(options)
    {
    }

    public DbSet<Students> Students { get; set; } = null!;
}
