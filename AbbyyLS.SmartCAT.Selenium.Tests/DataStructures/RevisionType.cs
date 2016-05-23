using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum RevisionType
	{
		[Description("Manual input")]
		ManualInput,
		[Description("Confirmation")]
		Confirmed,
		[Description("MT insertion")]
		InsertMT,
		[Description("TM insertion")]
		InsertTM,
		[Description("Restored")]
		Restored,
		[Description("TB insertion")]
		InsertTb,
		[Description("Pretranslation")]
		Pretranslation,
		[Description("Source Insertion")]
		SourceInsertion
	}
}
