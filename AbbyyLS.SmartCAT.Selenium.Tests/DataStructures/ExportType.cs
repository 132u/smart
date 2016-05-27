using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum ExportType
	{
		[Description("Source")]
		Original,
		[Description("Tmx")]
		TMX,
		[Description("Target")]
		Translation
	};
}