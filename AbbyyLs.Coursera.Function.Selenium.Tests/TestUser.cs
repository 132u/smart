﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AbbyyLS.Coursera.Function.Selenium.Tests
{
	[DataContract(Name = "TestUser")]
	public class TestUser
	{
		[DataMember(Name = "Login")]
		public string Login { get; set; }

		[DataMember(Name = "Password")]
		public string Password { get; set; }

		[DataMember(Name = "Activated", IsRequired = false)]
		public bool Activated { get; set; }
	}
}
