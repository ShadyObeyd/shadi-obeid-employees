using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesApp
{
    public class Program
    {
        public static void Main()
        {
            List<Employee> employeeList = GetEmployees();

            if (employeeList.Count == 0)
            {
                Console.WriteLine("No data to show.");
                Environment.Exit(0);
            }

            List<EmployeeResult> results = new List<EmployeeResult>();

            for (int i = 0; i < employeeList.Count; i++)
            {
                Employee currentEmployee = employeeList[i];

                List<Employee> colleagues = employeeList
                    .Where(e => e.ProjectId == currentEmployee.ProjectId && e.Id != currentEmployee.Id)
                    .ToList();

                if (colleagues.Count == 0)
                {
                    continue;
                }

                for (int j = 0; j < colleagues.Count; j++)
                {
                    Employee colleague = colleagues[j];

                    if (currentEmployee.DateTo.HasValue)
                    {
                        if (currentEmployee.DateTo.Value < colleague.DateFrom)
                        {
                            continue;
                        }
                    }

                    if (colleague.DateTo.HasValue)
                    {
                        if (colleague.DateTo.Value < currentEmployee.DateFrom)
                        {
                            continue;
                        }
                    }

                    int currentEmpDaysWorked = (SetTodayForDate(currentEmployee.DateTo) - currentEmployee.DateFrom).Days;
                    int colleagueDaysWorked = (SetTodayForDate(colleague.DateTo) - colleague.DateFrom).Days;

                    if (!results.Any(r => r.SecondEmpId == currentEmployee.Id && r.FirstEmpId == colleague.Id && r.ProjectId == currentEmployee.ProjectId))
                    {
                        EmployeeResult result = new EmployeeResult(currentEmployee.Id, colleague.Id, colleague.ProjectId, currentEmpDaysWorked + colleagueDaysWorked);
                        results.Add(result);
                    }
                }
            }

            PrintResult(results);
        }

        public static void PrintResult(List<EmployeeResult> results)
        {
            if (results.Count == 0)
            {
                Console.WriteLine("No data to show.");
                Environment.Exit(0);
            }

            var groupedResults = results
                .GroupBy(r => new { r.FirstEmpId, r.SecondEmpId })
                .Select(r => new
                {
                    FirstEmpId = r.Key.FirstEmpId,
                    SecondEmpId = r.Key.SecondEmpId,
                    TotalDays = r.Sum(e => e.DaysWorked)
                })
                .OrderByDescending(r => r.TotalDays)
                .FirstOrDefault();

            Console.WriteLine($"{groupedResults.FirstEmpId}, {groupedResults.SecondEmpId}, {groupedResults.TotalDays}");
        }

        public static List<Employee> GetEmployees()
        {
            // TODO: Add reading data from text file.
            List<Employee> employeeList = new List<Employee>();

            employeeList.Add(new Employee { Id = 143, ProjectId = 12, DateFrom = new DateTime(2013, 11, 01), DateTo = new DateTime(2014, 01, 05) });

            employeeList.Add(new Employee { Id = 218, ProjectId = 10, DateFrom = new DateTime(2012, 05, 16), DateTo = null });
            employeeList.Add(new Employee { Id = 143, ProjectId = 10, DateFrom = new DateTime(2009, 01, 01), DateTo = new DateTime(2011, 04, 27) });

            employeeList.Add(new Employee { Id = 115, ProjectId = 8, DateFrom = new DateTime(2010, 09, 02), DateTo = new DateTime(2016, 12, 22) });
            employeeList.Add(new Employee { Id = 365, ProjectId = 8, DateFrom = new DateTime(2008, 10, 05), DateTo = null });
            employeeList.Add(new Employee { Id = 284, ProjectId = 8, DateFrom = new DateTime(2011, 07, 28), DateTo = new DateTime(2012, 12, 21) });

            employeeList.Add(new Employee { Id = 211, ProjectId = 33, DateFrom = new DateTime(2000, 01, 10), DateTo = null });
            employeeList.Add(new Employee { Id = 245, ProjectId = 33, DateFrom = new DateTime(2005, 04, 03), DateTo = null });


            employeeList.Add(new Employee { Id = 115, ProjectId = 25, DateFrom = new DateTime(1991, 01, 27), DateTo = new DateTime(2002, 07, 31) });
            employeeList.Add(new Employee { Id = 365, ProjectId = 25, DateFrom = new DateTime(1995, 12, 01), DateTo = new DateTime(2006, 03, 03) });

            return employeeList;
        }

        public static DateTime SetTodayForDate(DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value;
            }

            return DateTime.Today;
        }
    }
}