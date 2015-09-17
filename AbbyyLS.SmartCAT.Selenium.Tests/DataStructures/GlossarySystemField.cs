using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum GlossarySystemField
	{
		[Description("Multimedia")]
		Multimedia,

		[Description("Topic")]
		Topic,

		[Description("Image")]
		Image,

		[Description("Definition source")]
		InterpretationSource,

		[Description("Definition")]
		Interpretation,

		[Description("Example")]
		Example
	}
}
