using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.ExplicitAttributes
{
	class UserDataExplicitAttribute : ExplicitAttribute
	{
		public UserDataExplicitAttribute()
			: base("Тест для локального прогона, так как требует ввод данных юзеров в конфиг")
		{

		}
	}
}
