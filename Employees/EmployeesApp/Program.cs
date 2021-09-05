using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

                    if (currentEmployee.DateTo < colleague.DateFrom)
                    {
                        continue;
                    }

                    if (colleague.DateTo < currentEmployee.DateFrom)
                    {
                        continue;
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
            string[] fileLines = new string[0];

            try
            {
                fileLines = File.ReadAllLines("../../../Employees.txt");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
            
            
            if (fileLines.Length == 0)
            {
                Console.WriteLine("No data to show.");
                Environment.Exit(0);
            }

            List<Employee> employeeList = new List<Employee>();

            foreach (string line in fileLines)
            {
                string[] lineTokens = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()).ToArray();

                if (lineTokens.Length != 4)
                {
                    continue;
                }

                int id = int.Parse(lineTokens[0]);
                int projectId = int.Parse(lineTokens[1]);
                DateTime dateFrom = CreateDateFromString(lineTokens[2]);
                DateTime? dateTo = null;

                if (lineTokens[3].ToLower() != "null")
                {
                    dateTo = CreateDateFromString(lineTokens[3]);
                }

                Employee employee = new Employee(id, projectId, dateFrom, dateTo);

                employeeList.Add(employee);
            }

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

        public static DateTime CreateDateFromString(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return DateTime.Today;
            }

            string forwardSlashFormat = "MM/dd/yyyy";
            string dashFormat = "yyyy-MM-dd";
            string dotFormat = "dd.MM.yyyy";

            if (dateString.Contains('/'))
            {
                return DateTime.ParseExact(dateString, forwardSlashFormat, CultureInfo.InvariantCulture);
            }
            else if (dateString.Contains('-'))
            {
                return DateTime.ParseExact(dateString, dashFormat, CultureInfo.InvariantCulture);
            }
            else if (dateString.Contains('.'))
            {
                return DateTime.ParseExact(dateString, dotFormat, CultureInfo.InvariantCulture);
            }
            else
            {
                return DateTime.Parse(dateString);
            }
        }
    }
}