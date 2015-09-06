using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum GlossaryStructureLevel
	{
		[Description("Language")]
		Language,

		[Description("Term")]
		Term,

		[Description("General info")]
		GeneralInfo
	}
}
