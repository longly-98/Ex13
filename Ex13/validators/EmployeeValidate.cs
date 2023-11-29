using Ex13.entities;
using System;
using System.Data;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ex13.validators
{
	class BirthdayException : Exception
	{
		public BirthdayException(string msg) : base(msg) { }
	}

	class FullnameException : Exception
	{
		public FullnameException(string msg) : base(msg) { }
	}

	class PhoneException : Exception
	{
		public PhoneException(string msg) : base(msg) { }
	}

	class EmailException : Exception
	{
		public EmailException(string msg) : base(msg) { }
	}

	static class EmployeeValidator
	{
		public static void ValidateBirthday(string input)
		{
			if (input == null || input == "")
			{
				throw new BirthdayException("Invalid Birthday!");
			}
			try
			{
				DateOnly.Parse(input);
			}
			catch (FormatException)
			{
				throw new BirthdayException("Invalid Birthday!");
			}
		}
		public static void ValidateFullname(string input)
		{
			if (input == null || input == "")
			{
				throw new FullnameException("Invalid Name!");
			}
		}

		public static void ValidatePhone(string input)
		{
			if (input == null || input == "" || !input.All(char.IsDigit))
			{
				throw new PhoneException("Invalid Phone!");
			}
		}

		public static void ValidateEmail(string input)
		{
			string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
			Regex regex = new Regex(pattern);
			if (input == null || input == "" || regex.IsMatch(input))
			{
				throw new EmailException("Invalid Email!");
			}
		}

		public static void ValidateInput(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				throw new NoNullAllowedException();
			}
		}
	}
}
