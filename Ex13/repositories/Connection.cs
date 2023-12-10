using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex13.repositories
{
	internal class Connection
	{
		public static SqlConnection GetConnection()
		{
			SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
			return connection;
		}
	}
}
