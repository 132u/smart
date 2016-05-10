using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum PreTranslateRulles
	{
		[Description("Translation Memory")]
		TM = 1,
		[Description("Machine Translation")]
		MT = 2,
		[Description("Source (Numbers Only)")]
		SRC = 3
	}
}