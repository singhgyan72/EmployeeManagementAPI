using CodeMaze.Repository.Configuration;
using EmployeeManagementAPI.Entities.Models;
using EmployeeManagementAPI.Repository.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Repository
{
    //public class RepositoryContext : DbContext
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

            //TODO: Uncomment this line to seed the roles [currenlty its throwing exception while update-database]
            //modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
