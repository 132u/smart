using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Registration]
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

			Assert.AreEqual(_workspacePage.GetUserName(), _firstAndLastName,
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

			Assert.AreEqual(_workspacePage.GetUserName(), firstAndLastName,
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");
		}

		[Test, Description("S-7109")]
		public void ConfirmNewFreelancerEmailTest()
		{
			var projectName = "Project-" + Guid.NewGuid();
			var email = Guid.NewGuid() + "@mailforspam.com";

			_registrationPage
				.GetPage(email)
				.FillFreelancerRegistrationForm(password: _password, firstAndLastName: _firstAndLastName)
				.ClickConfirmButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] {PathProvider.DocumentFile})
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(projectName)
				.ClickCreateProjectButton();

			Assert.IsTrue(_workspacePage.IsLimitedAccessMessageDisplayed(),
				"Произошла ошибка: не появилось сообщение об ограниченном доступе.");

			Assert.IsTrue(_workspacePage.IsSendAgainButtonDisplayed(),
				"Произошла ошибка: не появилась кнопка повторной отправки письма с  инструкциями по активации.");

			_adminSignInPage
				.GetPage()
				.SignIn(ThreadUser.Login, ThreadUser.Password);

			_adminHelper.ConfirmEmail(email);

			Assert.IsFalse(_workspacePage.IsLimitedAccessMessageDisplayed(),
				"Произошла ошибка: не появилось сообщение об ограниченном доступе.");

			Assert.IsFalse(_workspacePage.IsSendAgainButtonDisplayed(),
				"Произошла ошибка: не появилась кнопка повторной отправки письма с  инструкциями по активации.");

			Assert.AreEqual(_workspacePage.GetUserName(), _firstAndLastName,
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");
		}

		[Test, Description("S-10528")]
		public void ConfirmNewFreelancerEmailAgainTest()
		{
			var projectName = "Project-" + Guid.NewGuid();
			var email = Guid.NewGuid() + "@mailforspam.com";

			_registrationPage
				.GetPage(email)
				.FillFreelancerRegistrationForm(password: _password, firstAndLastName: _firstAndLastName)
				.ClickConfirmButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(projectName)
				.ClickCreateProjectButton();

			_workspacePage.ClickSendAgainButton();

			_emailConfirmationInformationDialog.ClickCloseInformationButton();

			_adminSignInPage
				.GetPage()
				.SignIn(ThreadUser.Login, ThreadUser.Password);

			_adminHelper.ConfirmEmail(email, resend: true);

			Assert.IsFalse(_workspacePage.IsLimitedAccessMessageDisplayed(),
				"Произошла ошибка: не появилось сообщение об ограниченном доступе.");

			Assert.IsFalse(_workspacePage.IsSendAgainButtonDisplayed(),
				"Произошла ошибка: не появилась кнопка повторной отправки письма с  инструкциями по активации.");

			Assert.AreEqual(_workspacePage.GetUserName(), _firstAndLastName,
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");
		}
	}
}
