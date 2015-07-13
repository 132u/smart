using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum Period
	{
		[Description("2")]
		OneMonth = 1,

		[Description("3")]
		ThreeMonth = 3,

		[Description("4")]
		SixMonth = 6,

		[Description("5")]
		TwelveMonth = 12,
	};
}
