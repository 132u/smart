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

		[DataMember(Name = "UserName")]
		public string UserName { get; set; }

		[DataMember(Name = "Login2")]
		public string Login2 { get; set; }

		[DataMember(Name = "Password2")]
		public string Password2 { get; set; }

		[DataMember(Name = "UserName2")]
		public string UserName2 { get; set; }

	}
}
