using System;
using System.Collections.Generic;

namespace Ex13.entities
{
	public class Experience : Employee
	{
		public int ExpInYear { get; set; }
		public string ProSkill { get; set; }

		public Experience() { }

		public Experience(string id, string name, DateOnly birthday, string phone,
			string email, EmpType type, int expInYear, string proSkill, List<Certificate> certificates)
			: base(id, name, birthday, phone, email, type, certificates)
		{
			ExpInYear = expInYear;
			ProSkill = proSkill;
		}

		public override void ShowInfo()
		{
			Console.WriteLine($"Experience:\n- Id: {Id}\n- Fullname: {FullName}\n" +
												$"- Birthday: {Birthday.ToString("d")}\n- Phone: {Phone}\n- Email: {Email}\n" +
												$"- Experience in year: {ExpInYear}\n- Professional Skill: {ProSkill}\n" +
												$"- Certificates: \n{ListCertificates()}\n");
		}
	}
}
