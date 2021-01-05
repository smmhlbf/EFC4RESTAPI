using System.Collections.Generic;
using EFC4RESTAPI.Models;
using EFC4RESTAPI.Models.Super;
using Microsoft.EntityFrameworkCore;

namespace EFC4RESTAPI.Repositories
{
    public class AppDBContext : DbContext
    {
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Config> Configs { get; set; }
        private Dictionary<string, DbSet<T>> _tables<T>() where T : ISuper
        {
            var dict = new Dictionary<string, DbSet<T>>();
            dict[typeof(Permission).Name] = Permissions as DbSet<T>;
            dict[typeof(Config).Name] = Configs as DbSet<T>;
            return dict;
        }
        public DbSet<T> Sets<T>() where T : ISuper => _tables<T>()[typeof(T).Name];
        
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { Database.EnsureCreated(); }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Permission>().HasIndex(p => p.Score);
            modelBuilder.Entity<Config>().HasIndex(c => new { c.ParentId, c.Name }).IsUnique().HasDatabaseName("UIX_Configs_Pid_Name");
        }
    }
}