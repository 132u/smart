using System.Runtime.Serialization;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	[DataContract(Name="TestingFiles")]
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
