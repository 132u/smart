using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Configs
{
	[DataContract(Name = "TestUsers")]
	class TestUsersConfig
	{
		[DataMember(Name = "TestUser")]
		public List<TestUser> Users { get; set; }

		[DataMember(Name = "TestCompany")]
		public List<TestUser> Companies { get; set; }

		[DataMember(Name = "CourseraUser")]
		public List<TestUser> CourseraUsers { get; set; }

		[DataMember(Name = "AolUser")]
		public List<TestUser> AolUsers { get; set; }

		[DataMember(Name = "SocialNetworksUser")]
		public List<TestUser> SocialNetworksUsers { get; set; }

		[DataMember(Name = "RightsTestUser")]
		public List<TestUser> RightsTestUsers { get; set; }
	}

}
