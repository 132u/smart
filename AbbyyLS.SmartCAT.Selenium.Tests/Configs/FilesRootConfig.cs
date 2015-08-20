using System.Runtime.Serialization;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Configs
{
	[DataContract(Name = "TestingFiles")]
	public class FilesRootCfg
	{
		[DataMember(Name = "FilesDirectory")]
		public string FilesDirectory { get; set; }

		[DataMember(Name = "ConfigDirectory")]
		public string ConfigDirectory { get; set; }

		[DataMember(Name = "ResultDirectory")]
		public string ResultDirectory { get; set; }
	}
}
