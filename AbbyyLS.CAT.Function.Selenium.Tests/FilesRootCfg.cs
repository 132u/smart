using System.Runtime.Serialization;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	[DataContract(Name="TestingFiles")]
	public class FilesRootCfg
	{
		[DataMember(Name = "RootDirectory")]
		public string Root { get; set; }

		[DataMember(Name = "ConfigDirectory")]
		public string RootToConfig { get; set; }
	}
}
