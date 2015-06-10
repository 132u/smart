using System.Runtime.Serialization;

namespace AbbyyLS.SmartCAT.Selenium.Tests
{
	[DataContract(Name = "CatServer")]
	class CatServerConfig
	{
		[DataMember(Name = "Url")]
		public string Url { get; set; }

		/// <summary>
		/// Абсолютный путь к workspace
		/// </summary>
		/// <remarks>>
		/// Если путь не указан или указана пустая строка,
		/// адрес формируется как https://servername/workspace или http://servername/workspace,
		/// учитывая включен на стенде https или нет
		/// </remarks>
		[DataMember(Name = "Workspace")]
		public string Workspace { get; set; }

		/// <summary>
		/// На стенде включен https
		/// </summary>
		[DataMember(Name = "IsHttpsEnabled")]
		public bool IsHttpsEnabled { get; set; }

		/// <summary>
		/// Тесты запускаются на ОР
		/// </summary>
		[DataMember(Name = "Standalone")]
		public bool Standalone { get; set; }
	}
}
