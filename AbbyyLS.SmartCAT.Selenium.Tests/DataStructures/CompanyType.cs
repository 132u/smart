using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum CompanyType
	{
		[Description("Language service provider")]
		LanguageServiceProvider = 1,

		[Description("")]
		EmptyCompanyType = 2
	}
}
