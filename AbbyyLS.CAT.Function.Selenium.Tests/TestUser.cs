using System.Runtime.Serialization;

namespace AbbyyLS.CAT.Function.Selenium.Tests
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
