using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum MachineTranslationType
	{
		[Description("SmartCAT Translator")]
		SmartCATTranslator,

		[Description("Google")]
		Google,

		[Description("Microsoft Translator")]
		MicrosoftTranslator,
	}
}
