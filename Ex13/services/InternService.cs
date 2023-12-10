using Ex13.entities;
using Ex13.repositories;
using Ex13.validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex13.services.CommonService;
using static Ex13.validators.EmployeeValidator;

namespace Ex13.services
{
	internal class InternService
	{
		CommonService commonService;
		InternRepo internRepo;
		CommonRepo commonRepo;

		public InternService() { }

		public InternService(CommonService commonService)
		{
			this.commonService = commonService;
			this.internRepo = new InternRepo();
			this.commonRepo = new CommonRepo();
		}

		internal EmployeeCommonInfo InputIntern(ref string majors, ref int semester, ref string university, bool edit, Employee? oldEmp)
		{
			EmployeeCommonInfo emp = new EmployeeCommonInfo();
			if (edit)
			{
				emp.Id = oldEmp.Id;
				emp.Birthday = oldEmp.Birthday;
				emp.Phone = oldEmp.Phone;
				emp.Email = oldEmp.Email;
				emp.Ecertificates = oldEmp.Certificates;
			}

			commonService.AddEmployeeCommon(ref emp, edit);

		inputMajor:
			try
			{
				Console.Write("\nMajors:");
				string i_majors = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(i_majors))
				{
					goto inputSemester;
				}
				ValidateEmpty(i_majors);
				majors = i_majors;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputMajor;
			}

		inputSemester:
			try
			{
				Console.Write("\nSemester:");
				string i_semester = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(i_semester))
				{
					goto inputUni;
				}

				semester = ValidateInt(i_semester);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputSemester;
			}

		inputUni:
			try
			{
				Console.Write("\nUniversity:");
				string i_university = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(i_university))
				{
					goto addCert;
				}
				ValidateEmpty(i_university);
				university = i_university;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputUni;
			}

		addCert:
			commonService.AddEmployeeCert(ref emp, edit);

			return emp;
		}

		public void AddIntern(ref Dictionary<string, Employee> employees)
		{
			string majors = "";
			int semester = 0;
			string university = "";
			EmployeeCommonInfo emp = InputIntern(ref majors, ref semester, ref university, false, null);

			try
			{
				Intern employee = new Intern(emp.Id, emp.Fullname, emp.Birthday, emp.Phone, emp.Email, EmpType.Intern, majors, semester, university, emp.Ecertificates);
				internRepo.AddIntern(employee);
				employees.Add(emp.Id, employee);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("Fail to add\n\n");
			}
		}

		public void EditIntern(Intern oldEmp, ref Dictionary<string, Employee> employees)
		{
			string majors = oldEmp.Majors;
			int semester = oldEmp.Semester;
			string university = oldEmp.University;

			EmployeeCommonInfo emp = InputIntern(ref majors, ref semester, ref university, true, oldEmp);
			
			try
			{
				Intern employee = new Intern(oldEmp.Id, emp.Fullname, emp.Birthday, emp.Phone, emp.Email, EmpType.Intern, majors, semester, university, emp.Ecertificates);
				internRepo.EditIntern(employee);
				employees[oldEmp.Id] = employee;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
