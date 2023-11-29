using System;

namespace Ex13.entities
{
	public class Certificate
	{
		private string ID {  get; set; }
		private string Name { get; set; }
		private string Rank { get; set; }
		private DateOnly Date { get; set; }

		public Certificate() { }

		public Certificate(string iD, string name, string rank, DateOnly date)
		{
			ID = iD;
			Name = name;
			Rank = rank;
			Date = date;
		}

		public override string ToString()
		{
			return $"\t*ID: {ID}\n \tCertificate Name: {Name}\n \tRank: {Rank}\n \tDate:{Date.ToString("d")}\n";
		}
	}
}
