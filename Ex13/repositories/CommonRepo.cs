using Ex13.entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Ex13.repositories
{
	internal class CommonRepo
	{
		public CommonRepo() { }

		private List<Certificate> GetCertificates(SqlConnection conn, string? id)
		{
			string queryCert;
			if (!string.IsNullOrEmpty(id))
			{
				queryCert = "SELECT *  FROM Certificate c JOIN Employee em ON c.employee_id = em.id " +
										$"WHERE c.employee_id = '{id}'";
			}
			else
			{
				queryCert = "SELECT *  FROM Certificate c JOIN Employee em ON c.employee_id = em.id ";
			}

			SqlCommand cmd = new SqlCommand(queryCert, conn);

			List<Certificate> certificates = new List<Certificate>();
			SqlDataReader sdr = cmd.ExecuteReader();

			while (sdr.Read())
			{
				certificates.Add(
					new Certificate(
						(string)sdr["id"],
						(string)sdr["name"],
						(string)sdr["rank"],
						DateOnly.FromDateTime((DateTime)sdr["certificate_date"]),
						(string)sdr["employee_id"]
					)
				);
			}

			sdr.Close();

			return certificates;
		}

		public Dictionary<string, Employee> Search(string? id, EmpType? emp_type)
		{
			Dictionary<string, Employee> employees = new Dictionary<string, Employee>();
			using (var conn = Connection.GetConnection())
			{
				string query = "SELECT em.id, em.fullname, em.birthday, em.phone, em.email, em.employee_type, " +
													"ex.exp, ex.pro_skill, " +
													"fr.graduation_date, fr.graduation_rank, fr.education, " +
													"i.majors, i.semester, i.university " +
												"FROM Employee em " +
												"LEFT JOIN Experience ex ON ex.employee_id = em.id " +
												"LEFT JOIN Fresher fr ON fr.employee_id = em.id " +
												"LEFT JOIN Intern i ON i.employee_id = em.id ";

				if (emp_type != null)
				{
					query += $"WHERE em.employee_type = {(int)emp_type}";
				}
				else if (!string.IsNullOrEmpty(id))
				{
					query += $"WHERE em.id = '{id}'";
				}

				SqlCommand cmd1 = new SqlCommand(query, conn);

				conn.Open();

				List<Certificate> certificates = GetCertificates(conn, id);

				SqlDataReader sdr = cmd1.ExecuteReader();
				while (sdr.Read())
				{

					switch (sdr["employee_type"])
					{
						case 0:
							employees.Add(
								(string)sdr["id"],
								new Experience(
									(string)sdr["id"],
									(string)sdr["fullname"],
									DateOnly.FromDateTime((DateTime)sdr["birthday"]),
									(string)sdr["phone"],
									(string)sdr["email"],
									EmpType.Experience,
									(int)sdr["exp"],
									(string)sdr["pro_skill"],
									certificates.Where(c => c.EmployeeId == (string)sdr["id"]).ToList()
								)
							);
							break;
						case 1:
							employees.Add(
								(string)sdr["id"],
								new Fresher(
									(string)sdr["id"],
									(string)sdr["fullname"],
									DateOnly.FromDateTime((DateTime)sdr["birthday"]),
									(string)sdr["phone"],
									(string)sdr["email"],
									EmpType.Fresher,
									DateOnly.FromDateTime((DateTime)sdr["graduation_date"]),
									(string)sdr["graduation_rank"],
									(string)sdr["education"],
									certificates.Where(c => c.EmployeeId == (string)sdr["id"]).ToList()
								)
							);
							break;
						case 2:
							employees.Add(
								(string)sdr["id"],
								new Intern(
									(string)sdr["id"],
									(string)sdr["fullname"],
									DateOnly.FromDateTime((DateTime)sdr["birthday"]),
									(string)sdr["phone"],
									(string)sdr["email"],
									EmpType.Fresher,
									(string)sdr["majors"],
									(int)sdr["semester"],
									(string)sdr["university"],
									certificates.Where(c => c.EmployeeId == (string)sdr["id"]).ToList()
								)
							);
							break;
					}
				}

				return employees;
			}
		}

		public void AddCertificates(string id, List<Certificate> certificates, SqlConnection conn, SqlTransaction transaction)
		{
			if (certificates.Count == 0)
			{
				return;
			}

			StringBuilder queryEmp = new StringBuilder("insert into " +
				"Certificate (id, name, rank, certificate_date, employee_id) ");

			for (int i = 0; i < certificates.Count; i++)
			{
				Certificate cert = certificates[i];
				queryEmp.Append($"values ('{cert.ID}', '{cert.Name}', '{cert.Rank}', '{cert.Date}', '{id}')");
				if (i < certificates.Count - 1)
				{
					queryEmp.Append(",");
				}
			}

			SqlCommand cmd1 = new SqlCommand(queryEmp.ToString(), conn, transaction);

			var rowAdded = cmd1.ExecuteNonQuery();

			if (rowAdded > 0)
			{
				Console.WriteLine("Certificates saved to database");
			}
			else
			{
				transaction.Rollback();
				Console.WriteLine("Certificates not saved to database");
			}
		}

		public void DeleteCertificates(string id, SqlConnection conn, SqlTransaction transaction)
		{
			string query = $"DELETE FROM Certificate WHERE employee_id = {id}";

			SqlCommand cmd1 = new SqlCommand(query, conn, transaction);

			var rowAdded = cmd1.ExecuteNonQuery();

			if (rowAdded == 0)
			{
				transaction.Rollback();
				Console.WriteLine("Error, rollback");
			}
		}

		public void DeleteEmployee(string id)
		{
			using (var conn = Connection.GetConnection())
			{
				string query = $"DELETE FROM employee WHERE id = '{id}'";

				conn.Open();
				SqlCommand cmd = new SqlCommand(query, conn);

				cmd.ExecuteNonQuery();
			}
		}
	}
}
