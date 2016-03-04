﻿using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	enum ProjectStatus
	{
		[Description("In Progress")]
		InProgress,
		[Description("Completed")]
		Completed,
		[Description("Created")]
		Created
	}
}