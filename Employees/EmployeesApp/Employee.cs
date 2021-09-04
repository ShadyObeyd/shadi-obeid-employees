using System;

namespace EmployeesApp
{
    public class Employee
    {
        public Employee(int id, int projectId, DateTime dateFrom, DateTime? dateTo)
        {
            Id = id;
            ProjectId = projectId;
            DateFrom = dateFrom;
            DateTo = dateTo ?? DateTime.Today;
        }

        public int Id { get; }

        public int ProjectId { get; }

        public DateTime DateFrom { get; }

        public DateTime DateTo { get; }
    }
}