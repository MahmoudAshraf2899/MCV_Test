using System.ComponentModel.DataAnnotations;

namespace Front.Models
{
    public class Employee
    {
        [Key]
        public int UniqueIdentifier { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Title { get; set; }
        public DateTime HiringDate { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
