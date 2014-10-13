﻿using System;

using System.Runtime;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AbbyyLs.Coursera.Function.Selenium.Tests
{
	[DataContract (Name = "CourseraTargetServer")]
	class TargetServerConfig
	{
		[DataMember(Name = "Url")]
		public string Url { get; set; }

	}
}