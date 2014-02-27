using NUnit.Framework;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    class AuthorizationTest : BaseTest
    {
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
