using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Configs
{
	[DataContract(Name = "ThreadUsers")]
	class ThreadUserConfig
	{
		[DataMember(Name = "ThreadUser")]
		public List<TestUser> Users { get; set; }
	}
}
