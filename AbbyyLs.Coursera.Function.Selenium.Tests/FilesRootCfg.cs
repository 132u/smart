using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace AbbyyLS.Coursera.Function.Selenium.Tests
{
	[DataContract(Name = "TestingFiles")]
	public class FilesRootCfg
	{
		[DataMember(Name = "RootDirectory")]
		public string Root { get; set; }
	}
}
