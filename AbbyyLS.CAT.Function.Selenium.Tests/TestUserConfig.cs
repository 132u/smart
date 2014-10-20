using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	[DataContract(Name = "TestUsers")]
	class TestUserConfig
	{
		[DataMember(Name = "TestUser")]
		public List<TestUser> Users { get; set; }

	}
}
