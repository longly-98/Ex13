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
	internal class ExperienceService
	{
		CommonService commonService;
		ExperienceRepo experienceRepo;
		CommonRepo commonRepo;

		public ExperienceService() { }
		public ExperienceService(CommonService commonService)
		{
			this.commonService = commonService;
			this.experienceRepo = new ExperienceRepo();
			this.commonRepo = new CommonRepo();
		}

		public void AddExperience(ref Dictionary<string, Employee> employees)
		{
			int expInYear = 0;
			string proSkill = "";
			EmployeeCommonInfo emp = InputExperience(ref expInYear, ref proSkill, false, null);

			try
			{
				Experience employee = new Experience(emp.Id, emp.Fullname, emp.Birthday, emp.Phone, emp.Email, EmpType.Experience, expInYear, proSkill, emp.Ecertificates);
				employees.Add(emp.Id, employee);
				experienceRepo.AddExperience(employee);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("Fail to add\n\n");
			}
		}

		internal EmployeeCommonInfo InputExperience(ref int expInYear, ref string proSkill, bool edit, Employee? oldEmp)
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

		inputExp:
			try
			{
				Console.Write("\nExperience in year:");
				string i_expInYear = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(i_expInYear))
				{
					goto inputPro;
				}
				expInYear = ValidateInt(i_expInYear);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputExp;
			}

		inputPro:
			try
			{
				Console.Write("\nProfessional Skill:");
				string i_proSkill = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(i_proSkill))
				{
					goto addCert;
				}
				ValidateEmpty(i_proSkill);
				proSkill = i_proSkill;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputPro;
			}

		addCert:
			commonService.AddEmployeeCert(ref emp, edit);

			return emp;
		}

		public void EditExperience(Experience oldEmp, ref Dictionary<string, Employee> employees)
		{
			int expInYear = oldEmp.ExpInYear;
			string proSkill = oldEmp.ProSkill;

			EmployeeCommonInfo emp = InputExperience(ref expInYear, ref proSkill, true, oldEmp);

			try
			{
				Experience employee = new Experience(oldEmp.Id, emp.Fullname, emp.Birthday, emp.Phone, emp.Email, EmpType.Experience, expInYear, proSkill, emp.Ecertificates);
				experienceRepo.EditExperience(employee);
				employees[oldEmp.Id] = employee;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("Fail to edit\n\n");
			}

			employees = commonRepo.Search(null, null);
		}
	}
}
