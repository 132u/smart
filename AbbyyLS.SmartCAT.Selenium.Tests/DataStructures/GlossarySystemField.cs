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
		Example,

		[Description("Source")]
		Source,

		[Description("Context")]
		Context,

		[Description("Context source")]
		ContextSource,

		[Description("Status")]
		Status,

		[Description("Label")]
		Label,

		[Description("Gender")]
		Gender,

		[Description("Number")]
		Number,

		[Description("Part of speech")]
		PartOfSpeech
	}
}
