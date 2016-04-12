using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	class FreelancerRegistrationValidDataTests<TWebDriverProvider> :
		RegistrationBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public FreelancerRegistrationValidDataTests()
		{
			StartPage = StartPage.Registration;
		}

		[Test, Description("S-7105")]
		public void NewFreelancerRegistrationTest()
		{
			_registrationPage
				.FillFreelancerRegistrationForm(password: _password, firstAndLastName: _firstAndLastName)
				.ClickConfirmButton();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_firstAndLastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");
		}

		[Test, Description("S-7107")]
		public void NewFreelancerRegistrationWithSimpleDataTest()
		{
			var firstAndLastName = "Иван Иванов";
			var password = "123456";

			_registrationPage
				.FillFreelancerRegistrationForm(password: password, firstAndLastName: firstAndLastName)
				.ClickConfirmButton();

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(firstAndLastName),
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");
		}
	}
}
