namespace EmployeesApp
{
    public class EmployeeResult
    {
        public EmployeeResult(int firstEmpId, int secondEmpId, int projectId, int daysWorked)
        {
            FirstEmpId = firstEmpId;
            SecondEmpId = secondEmpId;
            ProjectId = projectId;
            DaysWorked = daysWorked;
        }

        public int FirstEmpId { get; }

        public int SecondEmpId { get; }

        public int ProjectId { get; }

        public int DaysWorked { get; }
    }
}