using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	enum DocumentStatus
	{
		[Description("In progress")]
		InProgress,
		[Description("Completed")]
		Completed,
		[Description("Created")]
		Created
	}
}
