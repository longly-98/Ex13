using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex13.validators
{
	internal class EmployeeException
	{
		public class BirthdayException : Exception
		{
			public BirthdayException(string msg) : base(msg) { }
		}

		public class FullnameException : Exception
		{
			public FullnameException(string msg) : base(msg) { }
		}

		public class PhoneException : Exception
		{
			public PhoneException(string msg) : base(msg) { }
		}

		public class EmailException : Exception
		{
			public EmailException(string msg) : base(msg) { }
		}
	}
}
