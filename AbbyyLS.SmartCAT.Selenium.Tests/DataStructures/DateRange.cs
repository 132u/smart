﻿using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum DateRange
	{
		[Description("7 days")]
		Week,

		[Description("month")]
		Month,

		[Description("today")]
		Today,

		[Description("Anytime")]
		Anytime
	};
}