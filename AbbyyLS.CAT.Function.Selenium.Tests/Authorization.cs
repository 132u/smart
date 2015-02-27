using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки авторизации
	/// </summary>
	class AuthorizationTest : BaseTest
	{
		/// <summary>
		/// Конструктор тестов
		/// </summary>
		 
		 
		/// <param name="browserName">Название браузера</param>
		public AuthorizationTest(string browserName)
			: base(browserName)
		{
			
		}

		/// <summary>
		/// метод тестирования авторизации пользователя в системе
		/// </summary>
		[Test]
		public void AuthorizationMethodTest()
		{
			Authorization(Login, Password);
		}
	}
}
