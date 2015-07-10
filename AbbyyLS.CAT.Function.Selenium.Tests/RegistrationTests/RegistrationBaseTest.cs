using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.CommonDataStructures;
using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.RegistrationTests
{

	public class RegistrationBaseTest<TWebDriverSettings> : AdminTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[SetUp]
		public void SetUp()
		{
			if (Standalone)
			{
				Assert.Ignore("Тест игнорируется, так как это отделяемое решение");
			}
		}

		/// <summary>
		/// Метод регистрации нового фрилансера
		/// </summary>
		public void RegistrationNewUser(string email, string password)
		{
			Logger.Trace("Регистрация фрилансера");
			GoToRegistrationPage(RegistrationType.User);
			RegistrationPage.FillRegistrationDataInFirstStep(email, password, password);
			RegistrationPage.ClickSignUpButton();
			RegistrationPage.FillRegistrationDataInSecondStep(RegistrationPage.FirstName, RegistrationPage.LastName);
			RegistrationPage.ClickCreateAccountButton();
		}
	}
}
