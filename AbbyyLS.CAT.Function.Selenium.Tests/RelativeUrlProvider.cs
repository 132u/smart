
namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	static class RelativeUrlProvider
	{

		public static string Workspace  
		{ 
			get
			{ 
				return "/workspace";
			}
		}

		public static string SingIn
		{
			get
			{
				return "/sign-in";
			}
		}

		public static string LogIn
		{
			get
			{
				return "/login";
			}
		}

		public static string CorpReg
		{
			get
			{
				return "/corp-reg";
			}
		}

		public static string FreelanceReg
		{
			get
			{
				return "/freelance-reg";
			}
		}

		public static string Clients
		{
			get
			{
				return "/Clients/Index";
			}
		}

		public static string Domains
		{
			get
			{
				return "/Domains/Index";
			}
		}

		public static string Glossaries
		{
			get
			{
				return "/Enterprise/Glossaries";
			}
		}

		public static string TranslationMemories
		{
			get
			{
				return "/TranslationMemories/Index";
			}
		}

		public static string LicensePackages
		{
			get
			{
				return "/Billing/LicensePackages/";
			}
		}
	}
}
