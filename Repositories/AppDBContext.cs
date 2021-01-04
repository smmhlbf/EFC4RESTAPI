using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFC4RESTAPI.Models;
using EFC4RESTAPI.Models.Super;
using EFC4RESTAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace EFC4RESTAPI.Repositories
{
    public class AppDBContext : DbContext
    {
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Config> Configs { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { Database.EnsureCreated(); }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Permission>().HasIndex(p => p.Score);
            modelBuilder.Entity<Config>().HasIndex(c => new { c.ParentId, c.Name }).IsUnique().HasDatabaseName("UIX_Configs_Pid_Name");
        }
    }
}