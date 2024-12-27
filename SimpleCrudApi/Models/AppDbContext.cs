using Microsoft.EntityFrameworkCore;
using System.Net;

namespace SimpleCrudApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<MemberType> MemberTypes { get; set; }
    }
}
