using System.Runtime.Serialization;

namespace AbbyyLS.SmartCAT.Selenium.Tests
{
	[DataContract(Name = "UserInfo")]
	class UserInfoConfig
	{
		[DataMember(Name = "Login")]
		public string Login { get; set; }

		[DataMember(Name = "Password")]
		public string Password { get; set; }

		[DataMember(Name = "NickName")]
		public string NickName { get; set; }

		[DataMember(Name = "Login2")]
		public string Login2 { get; set; }

		[DataMember(Name = "Password2")]
		public string Password2 { get; set; }

		[DataMember(Name = "NickName2")]
		public string NickName2 { get; set; }

		[DataMember(Name = "TestRightsLogin")]
		public string TestRightsLogin { get; set; }

		[DataMember(Name = "TestRightsPassword")]
		public string TestRightsPassword { get; set; }

		[DataMember(Name = "TestRightsNickName")]
		public string TestRightsNickName { get; set; }

	}
}
