using Microsoft.EntityFrameworkCore;

namespace MCV_Test.Models
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
