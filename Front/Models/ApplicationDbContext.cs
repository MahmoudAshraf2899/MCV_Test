using Microsoft.EntityFrameworkCore;

namespace Front.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
        public  DbSet<Employee> Employees { get; set; } 
        public  DbSet<Department> Departments { get; set; } 
    }
}
