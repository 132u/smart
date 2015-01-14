using System.Runtime.Serialization;

namespace AbbyyLS.CAT.Function.Selenium.CategoriesList
{
	[DataContract(Name = "UserInfo")]
	class UserInfoConfig
	{
		[DataMember(Name = "Login")]
		public string Login { get; set; }

		[DataMember(Name = "Password")]
		public string Password { get; set; }

		[DataMember(Name = "Site")]
		public string Site { get; set; }
	}
}


