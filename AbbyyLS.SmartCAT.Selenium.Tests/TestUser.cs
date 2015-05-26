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

		[DataMember(Name = "Activated", IsRequired = false)]
		public bool Activated { get; set; }
	}
}
