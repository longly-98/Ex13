using System;
using System.Collections.Generic;

namespace Ex13.entities
{
	public class Fresher : Employee
	{
		public DateOnly GraduationDate { get; set; }
		public string GraduationRank { get; set; }
		public string Education { get; set; }

		public Fresher() { }

		public Fresher(string id, string name, DateOnly birthday, string phone,
			string email, EmpType type, DateOnly graduationDate, string graduationRank, string education, List<Certificate> certificates)
			: base(id, name, birthday, phone, email, type, certificates)
		{
			GraduationDate = graduationDate;
			GraduationRank = graduationRank;
			Education = education;
		}

		public override void ShowInfo()
		{
			Console.WriteLine($"Fresher:\n- Id: {Id}\n- Fullname: {FullName}\n" +
												$"- Birthday: {Birthday.ToString("d")}\n- Phone: {Phone}\n- Email: {Email}\n" +
												$"- Graduation Date: {GraduationDate.ToString("d")}\n- Graduation Rank: {GraduationRank}\n- Education: {Education}" +
												$"- Certificates: \n{ListCertificates()}\n");
		}
	}
}
