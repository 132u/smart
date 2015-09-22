using System.Runtime.Serialization;

namespace AbbyyLS.SmartCAT.Selenium.Tests
{
	[DataContract(Name = "TestUser")]
	public class TestUser
	{
		[DataMember(Name = "Login")]
		public string Login { get; set; }

		[DataMember(Name = "Password")]
		public string Password { get; set; }

		[DataMember(Name = "NickName", IsRequired = false)]
		public string NickName { get; set; }

		[DataMember(Name = "Name", IsRequired = false)]
		public string Name { get; set; }

		[DataMember(Name = "Surname", IsRequired = false)]
		public string Surname { get; set; }

		[DataMember(Name = "Activated", IsRequired = false)]
		public bool Activated { get; set; }

		[DataMember(Name = "StandaloneUrl", IsRequired = false)]
		public string StandaloneUrl { get; set; }
	}
}
