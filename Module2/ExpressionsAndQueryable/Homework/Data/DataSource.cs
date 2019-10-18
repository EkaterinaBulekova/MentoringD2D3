using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Homework.Data.Entities;

namespace Homework.Data
{
	public static class DataSource
	{
		private static readonly Lazy<DataTable> EmployeesTable = new Lazy<DataTable>(() =>
		{
			var employeesTable = new DataTable("Employees");
			employeesTable.Columns.Add(nameof(Employee.Workstation), typeof(string));

			var newEmployeeRow = employeesTable.NewRow();
			newEmployeeRow[nameof(Employee.Workstation)] = "EPRUIZHW0249";
			employeesTable.Rows.Add(newEmployeeRow);

			newEmployeeRow = employeesTable.NewRow();
			newEmployeeRow[nameof(Employee.Workstation)] = "EPRURYASDW0023";
			employeesTable.Rows.Add(newEmployeeRow);

			newEmployeeRow = employeesTable.NewRow();
			newEmployeeRow[nameof(Employee.Workstation)] = "EPRUIZHW0123";
			employeesTable.Rows.Add(newEmployeeRow);

			return employeesTable;
		});

		public static IEnumerable GetEntities(Type entityType, string query)
		{
			if (entityType == typeof(Employee))
			{
				return GetEmployees(query);
			}

			throw new NotSupportedException($"Entity {entityType.Name} is not supported");
		}

		private static List<Employee> GetEmployees(string query)
		{
			try
			{
				return EmployeesTable.Value.Select(query)
					.ToList()
					.Select(r =>
					{
						var employee = new Employee();
						employee.Workstation = (string) r[nameof(employee.Workstation)];
						return employee;
					})
					.ToList();
			}
			catch
			{
				Console.WriteLine(query);
				throw;
			}
		}
	}
}
