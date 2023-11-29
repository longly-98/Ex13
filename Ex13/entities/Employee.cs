using System;
using System.Collections.Generic;

namespace Ex13.entities
{
	public enum EmpType
	{
		Experience,
		Fresher,
		Intern
	};
	public abstract class Employee
	{
		public string Id { get; set; }
		public string FullName { get; set; }
		public DateOnly Birthday { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public EmpType EmployeeType { get; set; }
		public static int count = 0;
		public List<Certificate> Certificates { get; set; }

		public Employee()
		{
			count++;
		}
		public Employee(string id, string name, DateOnly birthday, string phone, string email, EmpType type, List<Certificate> certificates)
		{
			Id = id;
			FullName = name;
			Birthday = birthday;
			Phone = phone;
			Email = email;
			EmployeeType = type;
			Certificates = certificates;
		}

		public abstract void ShowInfo();

		public string ListCertificates()
		{
			string certificates = "";
			foreach (Certificate certificate in Certificates)
			{
				certificates += certificate.ToString();
			}

			return certificates;
		}
	}
}
