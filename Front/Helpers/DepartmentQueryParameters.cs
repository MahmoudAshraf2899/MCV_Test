namespace Front.Helpers
{
    public class DepartmentQueryParameters : QueryParameters
    { 
        public string Name { get; set; }

        public int? MinNumberOfEmployee { get; set; }
        public int? MaxNumberOfEmployee { get; set; }



        
    }
}
