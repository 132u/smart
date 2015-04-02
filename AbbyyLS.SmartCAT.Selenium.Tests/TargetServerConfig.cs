using System.Runtime.Serialization;

namespace AbbyyLS.SmartCAT.Selenium.Tests
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
