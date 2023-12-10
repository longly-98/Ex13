using Ex13.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex13.validators.EmployeeValidator;

namespace Ex13.services
{
	internal class CommonService
	{
		public CommonService() { }

		public struct EmployeeCommonInfo
		{
			public string Id;
			public string Fullname;
			public DateOnly Birthday;
			public string Phone;
			public string Email;
			public List<Certificate> Ecertificates;
		}

		public void AddEmployeeCommon(ref EmployeeCommonInfo em, bool edit)
		{
			string id, fullname, phone, email, birthday;

		inputId:
			try
			{
				if (edit)
				{
					goto inputName;
				}
				Console.Write("\nId:");
				id = Console.ReadLine();
				ValidateEmpty(id);
				em.Id = id;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputId;
			}

		inputName:
			try
			{
				Console.Write("\nFullname:");
				fullname = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(fullname))
				{
					goto inputBirthDay;
				}

				ValidateFullname(fullname);
				em.Fullname = fullname;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputName;
			}

		inputBirthDay:
			try
			{
				Console.Write("\nBirthday:");
				birthday = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(birthday))
				{
					goto inputPhone;
				}
				em.Birthday = ValidateBirthday(birthday);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputBirthDay;
			}

		inputPhone:
			try
			{
				Console.Write("\nPhone:");
				phone = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(phone))
				{
					goto inputEmail;
				}

				ValidatePhone(phone);
				em.Phone = phone;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputPhone;
			}

		inputEmail:
			try
			{
				Console.Write("\nEmail:");
				email = Console.ReadLine();
				if (edit && string.IsNullOrEmpty(email))
				{
					return;
				}

				ValidateEmail(email);
				em.Email = email;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto inputEmail;
			}
		}

		public void AddEmployeeCert(ref EmployeeCommonInfo em, bool edit)
		{
		addCert:
			Console.Write("\nEmployee's Certificates Count:");
			int certCount = 0;
			string input = Console.ReadLine();

			if (edit && string.IsNullOrEmpty(input))
			{
				return;
			}

			try
			{
				certCount = ValidateInt(input);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				goto addCert;
			}

			List<Certificate> certificates = new List<Certificate>();

			for (int i = 0; i < certCount; i++)
			{
				string certId, certName, certRank;
				DateOnly certDate;

				while (true)
				{
					try
					{
						Console.Write("\nCertificate ID:");
						certId = Console.ReadLine();
						ValidateEmpty(certId);
						break;
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						continue;
					}
				}

				while (true)
				{
					try
					{
						Console.Write("\nCertificate Name:");
						certName = Console.ReadLine();
						ValidateEmpty(certName);
						break;
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						continue;
					}
				}

				while (true)
				{
					try
					{
						Console.Write("\nCertificate Rank:");
						certRank = Console.ReadLine();
						ValidateEmpty(certRank);
						break;
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						continue;
					}
				}

				while (true)
				{
					try
					{
						Console.Write("\nCertificate Date:");
						certDate = ValidateDate(Console.ReadLine());
						break;
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						continue;
					}
				}

				// Validate
				certificates.Add(new Certificate(certId, certName, certRank, certDate));
			}

			em.Ecertificates = certificates;
		}
	}
}
