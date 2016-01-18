using System.Runtime.Serialization;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Configs
{
	[DataContract(Name = "CourseraTargetServer")]
	class CourseraServerConfig
	{
		[DataMember(Name = "Url")]
		public string Url { get; set; }
	}
}
