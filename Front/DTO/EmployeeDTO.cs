namespace Front.DTO
{
    public class EmployeeDTO
    {
         public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Title { get; set; }
        public DateTime HiringDate { get; set; }

        public int DepartmentId { get; set; }
    }
}
