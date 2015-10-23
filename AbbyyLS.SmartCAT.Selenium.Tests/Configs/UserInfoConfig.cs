using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Configs
{
	[DataContract(Name = "UsersInfo")]
	class UserInfoConfig
	{
		[DataMember(Name = "UserInfo")]
		public List<TestUser> UsersInfo { get; set; }
	}
}
