using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum ProjectStatus
	{
		[Description("In Progress")]
		InProgress,
		[Description("Completed")]
		Completed,
		[Description("Created")]
		Created,
		[Description("Cancelled")]
		Cancelled,
		[Description("Pretranslated")]
		Pretranslated,
		[Description("Manager Review")]
		ManagerReview
	}
}