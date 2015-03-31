using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	[DataContract(Name = "TestUsers")]
	class TestUserConfig
	{
		[DataMember(Name = "TestUser")]
		public List<TestUser> Users { get; set; }

		[DataMember(Name = "TestCompany")]
		public List<TestUser> Companies { get; set; }

		[DataMember(Name = "CourseraUser")]
		public List<TestUser> CourseraUsers { get; set; }

		[DataMember(Name = "AolUser")]
		public List<TestUser> AolUsers { get; set; }
	}

}
