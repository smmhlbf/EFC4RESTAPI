using Microsoft.EntityFrameworkCore;

namespace EFC4RESTAPI.Repositories
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
    }
}