using Ex13.entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex13.repositories
{
	internal class FresherRepo
	{
		CommonRepo commonRepo;

		public FresherRepo() {
			commonRepo = new CommonRepo();
		}

		public void AddFresher(Fresher emp)
		{
			using (var conn = Connection.GetConnection())
			{
				string queryEmp = "insert into " +
					"Employee (id, fullname, birthday, phone, email, employee_type) " +
					$"values ('{emp.Id}', '{emp.FullName}', '{emp.Birthday}', '{emp.Phone}', '{emp.Email}', '{(int)EmpType.Fresher}')";
				string queryFre = "insert into Fresher (employee_id, graduation_date, graduation_rank, education) " +
					$"values ('{emp.Id}', '{emp.GraduationDate}', '{emp.GraduationRank}', '{emp.Education}')";

				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();
				try
				{
					SqlCommand cmd1 = new SqlCommand(queryEmp, conn, transaction);
					SqlCommand cmd2 = new SqlCommand(queryFre, conn, transaction);

					cmd1.ExecuteNonQuery();
					cmd2.ExecuteNonQuery();

					commonRepo.AddCertificates(emp.Id, emp.Certificates, conn, transaction);

					transaction.Commit();
					Console.WriteLine("Fresher saved to database");
				}
				catch (Exception EX)
				{
					transaction.Rollback();
					Console.WriteLine("Error! Rollback");
				}

			}
		}

		public void EditFresher(Fresher emp)
		{
			using (var conn = Connection.GetConnection())
			{
				string queryEmp = $"UPDATE Employee SET " +
					$"fullname = '{emp.FullName}',  birthday = '{emp.Birthday}', phone = '{emp.Phone}', email = '{emp.Email}'" +
					$"WHERE id = '{emp.Id}'";
				string queryFre = $"UPDATE Fresher SET " +
					$"graduation_date = '{emp.GraduationDate}', graduation_rank = '{emp.GraduationRank}', education = '{emp.Education}'" +
					$"WHERE employee_id = '{emp.Id}'";

				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();
				try
				{
					SqlCommand cmd1 = new SqlCommand(queryEmp, conn, transaction);
					SqlCommand cmd2 = new SqlCommand(queryFre, conn, transaction);

					cmd1.ExecuteNonQuery();
					cmd2.ExecuteNonQuery();

					commonRepo.DeleteCertificates(emp.Id, conn, transaction);
					commonRepo.AddCertificates(emp.Id, emp.Certificates, conn, transaction);

					transaction.Commit();
					Console.WriteLine("Fresher saved to database");
				}
				catch (Exception EX)
				{
					transaction.Rollback();
					Console.WriteLine("Error! Rollback");
				}

			}
		}

		public void DeleteFresher(string id)
		{
			using (var conn = Connection.GetConnection())
			{
				string query1 = $"DELETE FROM fresher WHERE employee_id = {id}";
				string query2 = $"DELETE FROM employee WHERE id = {id}";

				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				try
				{
					SqlCommand cmd1 = new SqlCommand(query1, conn, transaction);
					SqlCommand cmd2 = new SqlCommand(query2, conn, transaction);

					cmd1.ExecuteNonQuery();
					cmd2.ExecuteNonQuery();

					commonRepo.DeleteCertificates(id, conn, transaction);
				}
				catch
				{
					transaction.Rollback();
				}
			}
		}
	}
}
