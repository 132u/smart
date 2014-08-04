using NUnit.Framework;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    /// <summary>
    /// Группа тестов для проверки авторизации
    /// </summary>
	class AuthorizationTest : BaseTest
    {
        /// <summary>
        /// Конструктор тестов
        /// </summary>
        /// <param name="url">Адрес</param>
        /// <param name="workspaceUrl">Адрес workspace</param>
        /// <param name="browserName">Название браузера</param>
		public AuthorizationTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        /// <summary>
        /// метод тестирования авторизации пользователя в системе
        /// </summary>
        [Test]
        public void AuthorizationMethodTest()
        {
            Authorization();
        }

    }
}
