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

		[Description("InterpretationSource")]
		DefinitionSource,

		[Description("Interpretation")]
		Definition,

		[Description("Example")]
		Example
	}
}
