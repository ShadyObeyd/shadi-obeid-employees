using System;

namespace EmployeesApp
{
    public class Employee
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}