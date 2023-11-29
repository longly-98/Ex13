using Ex13.services;
using System;

namespace Ex13
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome to the Employee Management App!");
			bool exit = false;
			EmployeeService employeeService = new EmployeeService();
			while (!exit)
			{
				Console.WriteLine("\n=====================================\n");
				Console.WriteLine("Please choose your action by type the following number:");
				Console.WriteLine("0 - Add Employee\n1 - Edit Employee Profile\n2 - Delete Employee\n3 - Search\n4 - Clear\n5 - Exit");

				int inputTyped = 0;
				try
				{
					inputTyped = Convert.ToInt32(Console.ReadLine());
				}
				catch (FormatException)
				{
					Console.WriteLine("That's not a valid syntax. Please retype!");
					continue;
				}

				switch (inputTyped)
				{
					case 0:
						Console.WriteLine("Adding Employee");
						employeeService.AddEmployee();
						break;

					case 1:
						Console.WriteLine("Editing Employee");
						employeeService.EditEmployee();
						break;

					case 2:
						Console.WriteLine("Deleting Employee");
						employeeService.DeleteEmployee();
						break;

					case 3:
						Console.WriteLine("Searching Employee");
						employeeService.SearchEmployee();
						break;

					case 4:
						Console.Clear();
						break;

					case 5:
						Console.WriteLine("Goodbye!");
						exit = true;
						break;
				}
			}
		}
	}
}
