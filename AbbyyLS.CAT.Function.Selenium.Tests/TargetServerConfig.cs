using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	[DataContract(Name = "CatTargetServer")]
	class TargetServerConfig
	{
		[DataMember(Name = "Url")]
		public string Url { get; set; }

		[DataMember(Name = "Workspace")]
		public string Workspace { get; set; }

	}
}