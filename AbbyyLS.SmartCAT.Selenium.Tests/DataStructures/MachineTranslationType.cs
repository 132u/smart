using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum MachineTranslationType
	{
		[Description("Default MT")]
		DefaultMT,

		[Description("Google")]
		Google,

		[Description("Microsoft Bing")]
		MicrosoftBing,
	}
}
