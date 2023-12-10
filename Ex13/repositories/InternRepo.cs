using Ex13.entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex13.repositories
{
	internal class InternRepo
	{
		CommonRepo commonRepo;

		public InternRepo()
		{
			commonRepo = new CommonRepo();
		}

		public void AddIntern(Intern emp)
		{
			using (var conn = Connection.GetConnection())
			{
				string queryEmp = "insert into " +
					"Employee (id, fullname, birthday, phone, email, employee_type) " +
					$"values ('{emp.Id}', '{emp.FullName}', '{emp.Birthday}', '{emp.Phone}', '{emp.Email}', '{(int)EmpType.Intern}')";
				string queryIntern = "insert into Intern (employee_id, majors, semester, university) " +
					$"values ('{emp.Id}', '{emp.Majors}', '{emp.Semester}', '{emp.University}')";

				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();
				try
				{
					SqlCommand cmd1 = new SqlCommand(queryEmp, conn, transaction);
					SqlCommand cmd2 = new SqlCommand(queryIntern, conn, transaction);

					cmd1.ExecuteNonQuery();
					cmd2.ExecuteNonQuery();

					commonRepo.AddCertificates(emp.Id, emp.Certificates, conn, transaction);

					transaction.Commit();
					Console.WriteLine("Intern saved to database");
				}
				catch (Exception EX)
				{
					transaction.Rollback();
					Console.WriteLine("Error! Rollback");
				}

			}
		}

		public void EditIntern(Intern emp)
		{
			using (var conn = Connection.GetConnection())
			{
				string queryEmp = $"UPDATE Employee SET " +
					$"fullname = '{emp.FullName}',  birthday = '{emp.Birthday}', phone = '{emp.Phone}', email = '{emp.Email}'" +
					$"WHERE id = '{emp.Id}'";
				string queryIntern = $"UPDATE Intern SET " +
					$"majors = '{emp.Majors}', semester = '{emp.Semester}', university = '{emp.University}'" +
					$"WHERE employee_id = '{emp.Id}'";

				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();
				try
				{
					SqlCommand cmd1 = new SqlCommand(queryEmp, conn, transaction);
					SqlCommand cmd2 = new SqlCommand(queryIntern, conn, transaction);

					cmd1.ExecuteNonQuery();
					cmd2.ExecuteNonQuery();

					commonRepo.DeleteCertificates(emp.Id, conn, transaction);
					commonRepo.AddCertificates(emp.Id, emp.Certificates, conn, transaction);

					transaction.Commit();
					Console.WriteLine("Intern saved to database");
				}
				catch (Exception EX)
				{
					transaction.Rollback();
					Console.WriteLine("Error! Rollback");
				}
			}
		}

		public void DeleteIntern(string id)
		{
			using (var conn = Connection.GetConnection())
			{
				string query1 = $"DELETE FROM intern WHERE employee_id = {id}";
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
