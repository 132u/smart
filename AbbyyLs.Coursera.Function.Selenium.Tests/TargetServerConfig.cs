using System;

using System.Runtime;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace AbbyyLS.Coursera.Function.Selenium.Tests
{
	[DataContract (Name = "CourseraTargetServer")]
	class TargetServerConfig
	{
		[DataMember(Name = "Url")]
		public string Url { get; set; }

	}
}
