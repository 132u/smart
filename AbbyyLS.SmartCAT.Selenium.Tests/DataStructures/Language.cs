using System.ComponentModel;


namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum Language
	{
		[Description("en")]
		English = 9,
		[Description("ru")]
		Russian = 25,
		[Description("de")]
		German = 7,
		[Description("fr")]
		French = 12,
		Japanese = 1041,
		[Description("lt")]
		Lithuanian = 1063,
		NoLanguage 
	}
}
