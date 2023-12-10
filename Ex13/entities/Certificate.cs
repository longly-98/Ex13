using System;

namespace Ex13.entities
{
	public class Certificate
	{
		public string ID {  get; set; }
		public string Name { get; set; }
		public string Rank { get; set; }
		public DateOnly Date { get; set; }

		public string EmployeeId { get; set; }

		public Certificate() { }

		public Certificate(string iD, string name, string rank, DateOnly date, string employeeId)
		{
			ID = iD;
			Name = name;
			Rank = rank;
			Date = date;
			EmployeeId = employeeId;
		}

		public override string ToString()
		{
			return $"\t*ID: {ID}\n \tCertificate Name: {Name}\n \tRank: {Rank}\n \tDate:{Date.ToString("d")}\n";
		}
	}
}
