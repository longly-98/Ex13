using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Ex13.entities;
using System.Data.SqlClient;

namespace Ex13.repositories
{
	internal class ExperienceRepo
	{
		CommonRepo commonRepo;

		public ExperienceRepo()
		{
			commonRepo = new CommonRepo();
		}

		public void AddExperience(Experience emp)
		{
			using (var conn = Connection.GetConnection())
			{
				string queryEmp = "insert into " +
					"Employee (id, fullname, birthday, phone, email, employee_type) " +
					$"values ('{emp.Id}', '{emp.FullName}', '{emp.Birthday}', '{emp.Phone}', '{emp.Email}', '{(int)EmpType.Experience}')";
				string queryExp = "insert into Experience (employee_id, exp, pro_skill) " +
					$"values ('{emp.Id}', '{emp.ExpInYear}', '{emp.ProSkill}')";

				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();
				try
				{
					SqlCommand cmd1 = new SqlCommand(queryEmp, conn, transaction);
					SqlCommand cmd2 = new SqlCommand(queryExp, conn, transaction);

					cmd1.ExecuteNonQuery();
					cmd2.ExecuteNonQuery();

					commonRepo.AddCertificates(emp.Id, emp.Certificates, conn, transaction);

					transaction.Commit();
					Console.WriteLine("Experience saved to database");
				}
				catch 
				{
					transaction.Rollback();
					Console.WriteLine("Error! Rollback");
				}
			}
		}

		public void EditExperience(Experience emp)
		{
			using (var conn = Connection.GetConnection())
			{
				string queryEmp = $"UPDATE Employee SET " +
					$"fullname = '{emp.FullName}',  birthday = '{emp.Birthday}', phone = '{emp.Phone}', email = '{emp.Email}'" +
					$"WHERE id = '{emp.Id}'";
				string queryExp = $"UPDATE Experience SET " +
					$"exp = '{emp.ExpInYear}',  pro_skill = '{emp.ProSkill}'" +
					$"WHERE employee_id = '{emp.Id}'";

				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();
				try
				{
					SqlCommand cmd1 = new SqlCommand(queryEmp, conn, transaction);
					SqlCommand cmd2 = new SqlCommand(queryExp, conn, transaction);

					cmd1.ExecuteNonQuery();
					cmd2.ExecuteNonQuery();

					commonRepo.DeleteCertificates(emp.Id, conn, transaction);
					commonRepo.AddCertificates(emp.Id, emp.Certificates, conn, transaction);

					transaction.Commit();
					Console.WriteLine("Experience saved to database");
				}
				catch
				{
					transaction.Rollback();
					Console.WriteLine("Error! Rollback");
				}

			}
		}
	}
}
