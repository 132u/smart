using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AbbyyLS.Coursera.Function.Selenium.Tests
{
	[DataContract(Name = "TestUsers")]
	class TestUserConfig
	{
		[DataMember(Name = "CourseraUser")]
		public List<TestUser> CourseraUsers { get; set; }

	}
}
