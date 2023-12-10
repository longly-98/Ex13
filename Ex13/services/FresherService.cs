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
	internal class FresherService
	{
		CommonService commonService;
		FresherRepo fresherRepo;
		CommonRepo commonRepo;

		public FresherService() { }

		public FresherService(CommonService commonService)
		{
			this.commonService = commonService;
			this.fresherRepo = new FresherRepo();
			this.commonRepo = new CommonRepo();
		}

		internal EmployeeCommonInfo InputFresher(ref DateOnly graduationDate, ref string graduationRank, ref string education, bool edit, Employee? oldEmp)
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

		inputGradDate:
			try
			{
				Console.Write("\nGraduation Date:");
				string i_graduationDate = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(i_graduationDate))
				{
					goto inputRank;
				}
				graduationDate = ValidateDate(i_graduationDate);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputGradDate;
			}

		inputRank:
			try
			{
				Console.Write("\nGraduation Rank:");
				string i_graduationRank = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(i_graduationRank))
				{
					goto inputEdu;
				}
				ValidateEmpty(i_graduationRank);
				graduationRank = i_graduationRank;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputRank;
			}

		inputEdu:
			try
			{
				Console.Write("\nEducation:");
				string i_education = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(i_education))
				{
					goto addCert;
				}
				ValidateEmpty(i_education);
				education = i_education;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputEdu;
			}

		addCert:
			commonService.AddEmployeeCert(ref emp, edit);

			return emp;
		}

		public void AddFresher(ref Dictionary<string, Employee> employees)
		{
			DateOnly graduationDate = new DateOnly();
			string graduationRank = "";
			string education = "";
			EmployeeCommonInfo emp = InputFresher(ref graduationDate, ref graduationRank, ref education, false, null);

			try
			{
				Fresher employee = new Fresher(emp.Id, emp.Fullname, emp.Birthday, emp.Phone, emp.Email, EmpType.Fresher, graduationDate, graduationRank, education, emp.Ecertificates);
				fresherRepo.AddFresher(employee);
				employees.Add(emp.Id, employee);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("Fail to add\n\n");
			}
		}


		public void EditFresher(Fresher oldEmp, ref Dictionary<string, Employee> employees)
		{
			DateOnly graduationDate = oldEmp.GraduationDate;
			string graduationRank = oldEmp.GraduationRank;
			string education = oldEmp.Education;

			EmployeeCommonInfo emp = InputFresher(ref graduationDate, ref graduationRank, ref education, true, oldEmp);

			try
			{
				Fresher employee = new Fresher(oldEmp.Id, emp.Fullname, emp.Birthday, emp.Phone, emp.Email, EmpType.Fresher, graduationDate, graduationRank, education, emp.Ecertificates);
				fresherRepo.EditFresher(employee);
				employees[oldEmp.Id] = employee;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("Fail to edit\n\n");
			}
		}
	}
}
