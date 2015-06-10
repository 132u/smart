using System.Runtime.Serialization;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	[DataContract(Name = "CatServer")]
	class CatServerConfig
	{
		[DataMember(Name = "Url")]
		public string Url { get; set; }

		[DataMember(Name = "Workspace")]
		public string Workspace { get; set; }

		/// <summary>
		/// На стенде включен https
		/// </summary>
		[DataMember(Name = "IsHttpsEnabled")]
		public bool IsHttpsEnabled { get; set; }

		[DataMember(Name = "Standalone")]
		public bool Standalone { get; set; }
	}
}