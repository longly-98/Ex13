using System;
using System.Collections.Generic;

namespace Ex13.entities
{
	public class Intern : Employee
	{
		public string Majors {  get; set; }
		public int Semester { get; set; }
		public string University { get; set; }

		public Intern() { }

		public Intern(string id, string name, DateOnly birthday, string phone,
			string email, EmpType type, string majors, int semester, string university, List<Certificate> certificates)
			: base(id, name, birthday, phone, email, type, certificates)
		{
			Majors = majors;
			Semester = semester;
			University = university;
		}

		public override void ShowInfo()
		{
			Console.WriteLine($"Intern:\n- Id: {Id}\n- Fullname: {FullName}\n" +
												$"- Birthday: {Birthday.ToString("d")}\n- Phone: {Phone}\n- Email: {Email}\n" +
												$"- Majors: {Majors}\n- Semester: {Semester}\n- University Name: {University}\n" +
												$"- Certificates: \n{ListCertificates()}\n");
		}
	}
}
