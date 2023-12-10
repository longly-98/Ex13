using Ex13.entities;
using static Ex13.validators.EmployeeValidator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Ex13.services.CommonService;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Ex13.repositories;

namespace Ex13.services
{
	internal class EmployeeService
	{
		CommonService commonService;
		ExperienceService experienceService;
		FresherService fresherService;
		InternService internService;

		CommonRepo commonRepo;

		Dictionary<string, Employee> employees;

		public EmployeeService()
		{
			commonRepo = new CommonRepo();
			employees = commonRepo.Search(null, null);
			//seed data
			/*			employees = new Dictionary<string, Employee>() {
							{"1", new Experience("1", "A", DateOnly.Parse("2000-2-2"), "2", "2", EmpType.Experience, 2, "a", new List<Certificate>()) },
							{"2", new Fresher("2", "B", DateOnly.Parse("2000-2-2"), "2", "2", EmpType.Fresher, DateOnly.Parse("2000-2-2"),"a", "a", new List<Certificate>()) },
							{"3", new Intern("3", "C", DateOnly.Parse("2000-2-2"), "2", "2", EmpType.Intern, "a", 1, "a", new List<Certificate>(){new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")),new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")) }) },
							{"3r", new Intern("3r", "C", DateOnly.Parse("2000-2-2"), "2", "2", EmpType.Intern, "a", 1, "a", new List<Certificate>(){new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")),new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")) }) },
							{"32", new Intern("32", "C", DateOnly.Parse("2000-2-2"), "2", "2", EmpType.Intern, "a", 1, "a", new List<Certificate>(){new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")),new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")) }) }
						};*/
		}

		public EmployeeService(CommonService commonService, ExperienceService experienceService, FresherService fresherService, InternService internService)
		{
			commonRepo = new CommonRepo();
			employees = commonRepo.Search(null, null);
			this.commonService = commonService;
			this.experienceService = experienceService;
			this.fresherService = fresherService;
			this.internService = internService;
			this.employees = employees;
		}

		public void AddEmployee()
		{
			bool returning = false;
			while (!returning)
			{
				Console.WriteLine("\n\nWhat type of employee do you want to add? Type in 3 to cancel.");
				Console.WriteLine("0 - Experience\n1 - Fresher\n2 - Intern\n");

				int type = 0;
				try
				{
					type = Convert.ToInt32(Console.ReadLine());
				}
				catch (FormatException)
				{
					Console.WriteLine("That's not a valid syntax. Please retype!");
					continue;
				}

				switch (type)
				{
					case 0:
						experienceService.AddExperience(ref employees);
						break;
					case 1:
						fresherService.AddFresher(ref employees);
						break;
					case 2:
						internService.AddIntern(ref employees);
						break;
					case 3:
						returning = true;
						break;
					default:
						Console.WriteLine("That's not a valid type. Please retype!");
						break;
				}

				Console.WriteLine("Employee added");
			}
		}

		public void EditEmployee()
		{
			Employee employee = SearchAll();
			if (employee == null)
			{
				Console.WriteLine("Employee not found!");
				return;
			}
			Console.WriteLine("\nPlease enter new profile, leave empty if field doesn't need to be updated");
			switch (employee.EmployeeType)
			{
				case EmpType.Experience:
					experienceService.EditExperience(employee as Experience, ref employees);
					break;
				case EmpType.Fresher:
					fresherService.EditFresher(employee as Fresher, ref employees);
					break;
				case EmpType.Intern:
					internService.EditIntern(employee as Intern, ref employees);
					break;
			}

			Console.WriteLine("Employee edited");
		}

		public void DeleteEmployee()
		{
			Employee employee = SearchAll();
			employee.ShowInfo();
			commonRepo.DeleteEmployee(employee.Id);
			employees = commonRepo.Search(null, null);
			Console.WriteLine("Employee deleted");
		}

		public void SearchEmployee()
		{
			bool returning = false;
			while (!returning)
			{
				Console.WriteLine("\n\nWhat type of employee do you want to search? Type in 4 to cancel.");
				Console.WriteLine("0 - All\n1 - Experience\n2 - Fresher\n3 - Intern\n");

				int type = 0;
				if (!int.TryParse(Console.ReadLine(), out type))
				{
					Console.WriteLine("That's not a valid syntax. Please retype!");
					continue;
				}

				Dictionary<string, Employee> searchList = new Dictionary<string, Employee>();
				switch (type)
				{
					case 0:
						var employee = SearchAll();
						if (employee == null)
						{
							Console.WriteLine("Employee not found!");
						}
						else
						{
							employee.ShowInfo();
						}
						break;
					case 1:
						searchList = commonRepo.Search("", EmpType.Experience);
						break;
					case 2:
						searchList = commonRepo.Search("", EmpType.Fresher);
						break;
					case 3:
						searchList = commonRepo.Search("", EmpType.Intern);
						break;
					case 4:
						returning = true;
						break;
					default:
						Console.WriteLine("That's not a valid type. Please retype!");
						break;
				}

				if (searchList != null)
				{
					foreach (var item in searchList)
					{
						item.Value.ShowInfo();
					}
				}
			}
		}

		private Employee SearchAll()
		{
			Console.Write("\nEmployee's ID: ");
			string id = Console.ReadLine();
			try
			{
				ValidateEmpty(id);
			}
			catch
			{
				return null;
			}

			var emps = commonRepo.Search(id, null);
			if (emps.Count == 0)
			{
				return null;
			}

			Employee emp = emps.First().Value;

			return emp;
		}
	}
}