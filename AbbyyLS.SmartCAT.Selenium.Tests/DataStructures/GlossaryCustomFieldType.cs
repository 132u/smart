using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum GlossaryCustomFieldType
	{
		[Description("Text")]
		Text,

		[Description("Date")]
		Date,

		[Description("Audio and video")]
		Media,

		[Description("Image")]
		Image,

		[Description("Number")]
		Number,

		[Description("Yes/No")]
		YesNo,

		[Description("Multi-selection list")]
		 Multiselect,

		[Description("List")]
		List
	}
}
