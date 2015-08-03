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
		[Description("TB insertion")]
		Restored,
		[Description("Restored")]
		InsertTb,
		[Description("Pretranslation")]
		Pretranslation
	}
}
