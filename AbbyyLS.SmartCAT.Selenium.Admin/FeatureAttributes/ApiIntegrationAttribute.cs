using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Admin
{
	/// <summary>
	/// Тесты, используемые для создания данных для тестов на интеграционное апи ката
	/// </summary>
	class ApiIntegrationAttribute : CategoryAttribute
	{
		public ApiIntegrationAttribute() : base("ApiIntegration")
		{
		}
	}
}
