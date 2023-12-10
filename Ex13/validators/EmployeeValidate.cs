using Ex13.entities;
using System;
using System.Data;
using System.Text.RegularExpressions;
using static Ex13.validators.EmployeeException;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ex13.validators
{
	public static class EmployeeValidator
	{
		public static DateOnly ValidateBirthday(string? input)
		{
			if (input == null || input == "")
			{
				throw new BirthdayException("Invalid Birthday!");
			}
			return ValidateDate(input);
		}
		public static void ValidateFullname(string? input)
		{
			if (input == null || input == "")
			{
				throw new FullnameException("Invalid Name!");
			}
		}

		public static void ValidatePhone(string? input)
		{
			if (input == null || input == "" || !input.All(char.IsDigit))
			{
				throw new PhoneException("Invalid Phone!");
			}
		}

		public static void ValidateEmail(string? input)
		{
			string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
			Regex regex = new Regex(pattern);
			if (input == null || input == "" || !regex.IsMatch(input))
			{
				throw new EmailException("Invalid Email!");
			}
		}

		public static void ValidateEmpty(string? input)
		{
			if (string.IsNullOrEmpty(input))
			{
				throw new NoNullAllowedException("Field can't be null");
			}
		}

		public static DateOnly ValidateDate(string? input)
		{
			if (!DateOnly.TryParse(input, out DateOnly date))
			{
				throw new FormatException("Wrong date format");
			}
			return date;
		}

		public static int ValidateInt(string? input)
		{
			ValidateEmpty(input);

			if (!int.TryParse(input, out int number))
			{
				throw new InvalidDataException("Invalid input number");
			}
			return number;
		}
	}
}
