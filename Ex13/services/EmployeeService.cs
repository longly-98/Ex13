using Ex13.entities;
using Ex13.validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ex13.services
{
	internal class EmployeeService
	{
		Dictionary<string, Employee> employees;

		public struct EmployeeCommonInfo
		{
			public string Id;
			public string Fullname;
			public string Birthday;
			public string Phone;
			public string Email;
			public List<Certificate> Ecertificates;
		}

		public EmployeeService()
		{
			employees = new Dictionary<string, Employee>();
			//seed data
			/*			employees = new Dictionary<string, Employee>() {
							{"1", new Experience("1", "A", DateOnly.Parse("2000-2-2"), "2", "2", EmpType.Experience, 2, "a", new List<Certificate>()) },
							{"2", new Fresher("2", "B", DateOnly.Parse("2000-2-2"), "2", "2", EmpType.Fresher, DateOnly.Parse("2000-2-2"),"a", "a", new List<Certificate>()) },
							{"3", new Intern("3", "C", DateOnly.Parse("2000-2-2"), "2", "2", EmpType.Intern, "a", 1, "a", new List<Certificate>(){new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")),new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")) }) },
							{"3r", new Intern("3r", "C", DateOnly.Parse("2000-2-2"), "2", "2", EmpType.Intern, "a", 1, "a", new List<Certificate>(){new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")),new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")) }) },
							{"32", new Intern("32", "C", DateOnly.Parse("2000-2-2"), "2", "2", EmpType.Intern, "a", 1, "a", new List<Certificate>(){new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")),new Certificate("123", "a","a", DateOnly.Parse("2020-3-3")) }) }
						};*/
		}

		public void AddEmployee()
		{
			bool returning = false;
			while (!returning)
			{
				Console.WriteLine("What type of employee do you want to add? Type in 3 to cancel.");
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
						AddExperience();
						break;
					case 1:
						AddFresher();
						break;
					case 2:
						AddIntern();
						break;
					case 3:
						returning = true;
						break;
					default:
						Console.WriteLine("That's not a valid type. Please retype!");
						break;
				}
			}
		}

		private void AddEmployeeCommon(ref EmployeeCommonInfo e, bool edit)
		{
			string id = "";
			if (!edit)
			{
				Console.WriteLine("Id:");
				id = Console.ReadLine();
			}
			Console.WriteLine("Fullname:");
			string fullname = Console.ReadLine();
			Console.WriteLine("Birthday:");
			string birthday = Console.ReadLine();
			Console.WriteLine("Phone:");
			string phone = Console.ReadLine();
			Console.WriteLine("Email:");
			string email = Console.ReadLine();

			e.Id = id;
			e.Fullname = fullname;
			e.Birthday = birthday;
			e.Phone = phone;
			e.Email = email;
		}

		private void AddEmployeeCert(ref EmployeeCommonInfo e, bool edit)
		{
			Console.WriteLine("Employee's Certificates Count:");
			int certCount = 0;
			string input = Console.ReadLine();
			if (!string.IsNullOrEmpty(input))
			{
				certCount = Convert.ToInt32(input);
			}
			else if (edit && string.IsNullOrEmpty(input))
			{
				return;
			}
			List<Certificate> certificates = new List<Certificate>();
			for (int i = 0; i < certCount; i++)
			{
				Console.WriteLine("Certificate ID:");
				string certId = Console.ReadLine();
				Console.WriteLine("Certificate Name:");
				string certName = Console.ReadLine();
				Console.WriteLine("Certificate Rank:");
				string certRank = Console.ReadLine();
				Console.WriteLine("Certificate Date:");
				string certDate = Console.ReadLine();

				// Validate
				certificates.Add(new Certificate(certId, certName, certRank, DateOnly.Parse(certDate)));
			}
			e.Ecertificates = certificates;
		}

		private EmployeeCommonInfo InputFresher(out string graduationDate, out string graduationRank, out string education, bool edit)
		{
			EmployeeCommonInfo emp = new EmployeeCommonInfo();
			AddEmployeeCommon(ref emp, edit);
			Console.WriteLine("Graduation Date:");
			graduationDate = Console.ReadLine();
			Console.WriteLine("Graduation Rank:");
			graduationRank = Console.ReadLine();
			Console.WriteLine("Education:");
			education = Console.ReadLine();
			AddEmployeeCert(ref emp, edit);
			return emp;
		}

		private EmployeeCommonInfo InputExperience(out string expInYear, out string proSkill, bool edit)
		{
			EmployeeCommonInfo emp = new EmployeeCommonInfo();
			AddEmployeeCommon(ref emp, edit);
			Console.WriteLine("Experience in year:");
			expInYear = Console.ReadLine();
			Console.WriteLine("Professional Skill:");
			proSkill = Console.ReadLine();
			AddEmployeeCert(ref emp, edit);
			return emp;
		}

		private EmployeeCommonInfo InputIntern(out string majors, out string semester, out string university, bool edit)
		{
			EmployeeCommonInfo emp = new EmployeeCommonInfo();
			AddEmployeeCommon(ref emp, edit);
			Console.WriteLine("Majors:");
			majors = Console.ReadLine();
			Console.WriteLine("Semester:");
			semester = Console.ReadLine();
			Console.WriteLine("University:");
			university = Console.ReadLine();
			AddEmployeeCert(ref emp, edit);
			return emp;
		}

		private void AddExperience()
		{
		adding:
			EmployeeCommonInfo emp = InputExperience(out string expInYear, out string proSkill, false);

			try
			{
				EmployeeValidator.ValidateInput(emp.Id);
				EmployeeValidator.ValidateFullname(emp.Fullname);
				EmployeeValidator.ValidateBirthday(emp.Birthday);
				EmployeeValidator.ValidatePhone(emp.Phone);
				EmployeeValidator.ValidateEmail(emp.Email);
				employees.Add(emp.Id, new Experience(emp.Id, emp.Fullname, DateOnly.Parse(emp.Birthday), emp.Phone, emp.Email, EmpType.Experience, Convert.ToInt32(expInYear), proSkill, emp.Ecertificates));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto adding;
			}
		}

		private void AddFresher()
		{
		adding:
			EmployeeCommonInfo emp = InputFresher(out string graduationDate, out string graduationRank, out string education, false);

			try
			{
				EmployeeValidator.ValidateInput(emp.Id);
				EmployeeValidator.ValidateFullname(emp.Fullname);
				EmployeeValidator.ValidateBirthday(emp.Birthday);
				EmployeeValidator.ValidatePhone(emp.Phone);
				EmployeeValidator.ValidateEmail(emp.Email);
				employees.Add(emp.Id, new Fresher(emp.Id, emp.Fullname, DateOnly.Parse(emp.Birthday), emp.Phone, emp.Email, EmpType.Fresher, DateOnly.Parse(graduationDate), graduationRank, education, emp.Ecertificates));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto adding;
			}
		}

		private void AddIntern()
		{
		adding:
			EmployeeCommonInfo emp = InputIntern(out string majors, out string semester, out string university, false);

			try
			{
				EmployeeValidator.ValidateInput(emp.Id);
				EmployeeValidator.ValidateFullname(emp.Fullname);
				EmployeeValidator.ValidateBirthday(emp.Birthday);
				EmployeeValidator.ValidatePhone(emp.Phone);
				EmployeeValidator.ValidateEmail(emp.Email);
				employees.Add(emp.Id, new Intern(emp.Id, emp.Fullname, DateOnly.Parse(emp.Birthday), emp.Phone, emp.Email, EmpType.Intern, majors, Convert.ToInt32(semester), university, emp.Ecertificates));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto adding;
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
			Console.WriteLine("Please enter new profile, leave empty if field doesn't need to be updated");
			switch (employee.EmployeeType)
			{
				case EmpType.Experience:
					EditExperience(employee as Experience);
					break;
				case EmpType.Fresher:
					EditFresher(employee as Fresher);
					break;
				case EmpType.Intern:
					EditIntern(employee as Intern);
					break;
			}
		}

		private void EditExperience(Experience oldEmp)
		{
		adding:
			EmployeeCommonInfo emp = InputExperience(out string expInYear, out string proSkill, true);
			if (string.IsNullOrEmpty(emp.Fullname)) { emp.Fullname = oldEmp.FullName; }
			if (string.IsNullOrEmpty(emp.Birthday)) { emp.Birthday = oldEmp.Birthday.ToString("d"); }
			if (string.IsNullOrEmpty(emp.Phone)) { emp.Phone = oldEmp.Phone; }
			if (string.IsNullOrEmpty(emp.Email)) { emp.Email = oldEmp.Email; }
			if (emp.Ecertificates == null) { emp.Ecertificates = oldEmp.Certificates; }
			if (string.IsNullOrEmpty(expInYear)) { expInYear = oldEmp.ExpInYear.ToString("d"); }
			if (string.IsNullOrEmpty(proSkill)) { proSkill = oldEmp.ProSkill; }
			try
			{
				EmployeeValidator.ValidateFullname(emp.Fullname);
				EmployeeValidator.ValidateBirthday(emp.Birthday);
				EmployeeValidator.ValidatePhone(emp.Phone);
				EmployeeValidator.ValidateEmail(emp.Email);
				employees[oldEmp.Id] = new Experience(oldEmp.Id, emp.Fullname, DateOnly.Parse(emp.Birthday), emp.Phone, emp.Email, EmpType.Experience, Convert.ToInt32(expInYear), proSkill, emp.Ecertificates);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto adding;
			}
		}

		private void EditFresher(Fresher oldEmp)
		{
		adding:
			EmployeeCommonInfo emp = InputFresher(out string graduationDate, out string graduationRank, out string education, true);
			if (string.IsNullOrEmpty(emp.Fullname)) { emp.Fullname = oldEmp.FullName; }
			if (string.IsNullOrEmpty(emp.Birthday)) { emp.Birthday = oldEmp.Birthday.ToString("d"); }
			if (string.IsNullOrEmpty(emp.Phone)) { emp.Phone = oldEmp.Phone; }
			if (string.IsNullOrEmpty(emp.Email)) { emp.Email = oldEmp.Email; }
			if (emp.Ecertificates == null) { emp.Ecertificates = oldEmp.Certificates; }
			if (string.IsNullOrEmpty(graduationDate)) { graduationDate = oldEmp.GraduationDate.ToString("d"); }
			if (string.IsNullOrEmpty(graduationRank)) { graduationRank = oldEmp.GraduationRank; }
			if (string.IsNullOrEmpty(education)) { education = oldEmp.Education; }
			try
			{
				EmployeeValidator.ValidateFullname(emp.Fullname);
				EmployeeValidator.ValidateBirthday(emp.Birthday);
				EmployeeValidator.ValidatePhone(emp.Phone);
				EmployeeValidator.ValidateEmail(emp.Email);
				employees[oldEmp.Id] = new Fresher(oldEmp.Id, emp.Fullname, DateOnly.Parse(emp.Birthday), emp.Phone, emp.Email, EmpType.Fresher, DateOnly.Parse(graduationDate), graduationRank, education, emp.Ecertificates);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto adding;
			}
		}

		private void EditIntern(Intern oldEmp)
		{
		adding:
			EmployeeCommonInfo emp = InputIntern(out string majors, out string semester, out string university, true);
			if (string.IsNullOrEmpty(emp.Fullname)) { emp.Fullname = oldEmp.FullName; }
			if (string.IsNullOrEmpty(emp.Birthday)) { emp.Birthday = oldEmp.Birthday.ToString("d"); }
			if (string.IsNullOrEmpty(emp.Phone)) { emp.Phone = oldEmp.Phone; }
			if (string.IsNullOrEmpty(emp.Email)) { emp.Email = oldEmp.Email; }
			if (emp.Ecertificates == null) { emp.Ecertificates = oldEmp.Certificates; }
			if (string.IsNullOrEmpty(majors)) { majors = oldEmp.Majors; }
			if (string.IsNullOrEmpty(semester)) { semester = oldEmp.Semester.ToString(); }
			if (string.IsNullOrEmpty(university)) { university = oldEmp.University; }
			try
			{
				EmployeeValidator.ValidateFullname(emp.Fullname);
				EmployeeValidator.ValidateBirthday(emp.Birthday);
				EmployeeValidator.ValidatePhone(emp.Phone);
				EmployeeValidator.ValidateEmail(emp.Email);
				employees[oldEmp.Id] = new Intern(oldEmp.Id, emp.Fullname, DateOnly.Parse(emp.Birthday), emp.Phone, emp.Email, EmpType.Intern, majors, Convert.ToInt32(semester), university, emp.Ecertificates);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto adding;
			}
		}

		public void DeleteEmployee()
		{
			Employee employee = SearchAll();
			employee.ShowInfo();
			employees.Remove(employee.Id);
		}

		public void SearchEmployee()
		{
			bool returning = false;
			while (!returning)
			{
				Console.WriteLine("What type of employee do you want to search? Type in 4 to cancel.");
				Console.WriteLine("0 - All\n1 - Experience\n2 - Fresher\n3 - Intern\n");
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
						searchList = SearchByType(EmpType.Experience);
						break;
					case 2:
						searchList = SearchByType(EmpType.Fresher);
						break;
					case 3:
						searchList = SearchByType(EmpType.Intern);
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
			Console.Write("Employee's ID: ");
			string id = Console.ReadLine();
			try
			{
				EmployeeValidator.ValidateInput(id);
			}
			catch
			{
				return null;
			}

			if (employees.ContainsKey(id))
			{
				return employees[id];
			}
			else
			{
				return null;
			}
		}

		private Dictionary<string, Employee> SearchByType(EmpType type)
		{
			var employeeRes = employees.Where(e => e.Value.EmployeeType.Equals(type)).ToDictionary(e => e.Key, e => e.Value);
			return employeeRes;
		}
	}
}