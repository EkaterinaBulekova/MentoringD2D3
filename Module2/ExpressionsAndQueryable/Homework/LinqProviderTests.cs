using System;
using System.Linq;
using Homework.Data.Entities;
using Homework.LinqProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Homework
{
	[TestClass]
	public class LinqProviderTests
	{
		private void CheckResult(IQueryable<Employee> employees, int expectedCount)
		{
			var employeeList = employees.ToList();
			foreach (var employee in employeeList)
			{
				Console.WriteLine($"{employee.Workstation}");
			}
			Assert.AreEqual(expectedCount, employeeList.Count);
		}

		[TestMethod]
		public void TestEqualRight()
		{
			var employees = new EntitySet<Employee>();
			this.CheckResult(employees.Where(e => e.Workstation == "EPRUIZHW0249"), 1);
		}

		[TestMethod]
		public void TestEqualLeft()
		{
			var employees = new EntitySet<Employee>();
			this.CheckResult(employees.Where(e => "EPRUIZHW0249" == e.Workstation), 1);
		}

		[TestMethod]
		public void TestStartsWith()
		{
			var employees = new EntitySet<Employee>();
			this.CheckResult(employees.Where(e => e.Workstation.StartsWith("EPRUIZH")), 2);
		}

		[TestMethod]
		public void TestEndsWith()
		{
			var employees = new EntitySet<Employee>();
			this.CheckResult(employees.Where(e => e.Workstation.EndsWith("23")), 2);
		}

		[TestMethod]
		public void TestContains()
		{
			var employees = new EntitySet<Employee>();
			this.CheckResult(employees.Where(e => e.Workstation.Contains("RYASD")), 1);
		}

		[TestMethod]
		public void TestAnd()
		{
			var employees = new EntitySet<Employee>();
			this.CheckResult(employees.Where(e => e.Workstation.Contains("RYASD") && e.Workstation.EndsWith("23")), 1);
		}
	}
}
