using Microsoft.EntityFrameworkCore;
using Test_API.Models;

namespace Test_API.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<Customer> customers { get; set; }
    }
}
