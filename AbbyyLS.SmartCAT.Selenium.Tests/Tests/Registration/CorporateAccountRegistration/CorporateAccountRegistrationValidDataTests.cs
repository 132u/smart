using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Registration]
	public class CorporateAccountRegistrationValidDataTests<TWebDriverProvider> :
		RegistrationBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7100")]
		public void CorporateAccountAndUserRegistrationTest()
		{
			_registrationPage
				.FillCorporateAccountRegistrationForm(
					password: _password,
					firstAndLastName: _firstAndLastName,
					phone: _phoneNumber,
					companyName: _companyName)
				.ClickConfirmButton();

			Assert.AreEqual(_workspacePage.GetUserName(), _firstAndLastName,
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(_companyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
		}

		[Test, Description("S-7102")]
		public void CorporateAccountAndUserRegistrationWithSimpleDataTest()
		{
			var password = "123456";
			var firstAndLastName = "Иван Борькин";
			var phoneNumber = "+7987876875865";
			var companyName = "ООО \"Рога и копыта\"";

			_registrationPage
				.FillCorporateAccountRegistrationForm(
					password: password,
					firstAndLastName: firstAndLastName,
					phone: phoneNumber,
					companyName: companyName)
				.ClickConfirmButton();

			Assert.AreEqual(_workspacePage.GetUserName(), firstAndLastName,
				"Произошла ошибка:\n Имя пользователя в черной плашке не совпадает с ожидаемым именем");

			Assert.IsTrue(_workspacePage.IsAccountNameMatchExpected(companyName),
				"Произошла ошибка:\n название аккаунта в черной плашке не совпадает с ожидаемым именем");
		}

		[Test, Description("S-7104")]
		public void ConfirmNewCompanyUserEmailTest()
		{
			var projectName = "Project-" + Guid.NewGuid();
			var email = Guid.NewGuid() + "@mailforspam.com";

			_registrationPage
				.GetPage(email)
				.FillCorporateAccountRegistrationForm(
					password: _password,
					firstAndLastName: _firstAndLastName,
					phone: _phoneNumber,
					companyName: _companyName)
				.ClickConfirmButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(projectName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

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

		[Test, Description("S-10527")]
		public void ConfirmNewCompanyUserEmailAgainTest()
		{
			var projectName = "Project-" + Guid.NewGuid();
			var email = Guid.NewGuid() + "@mailforspam.com";

			_registrationPage
				.GetPage(email)
				.FillCorporateAccountRegistrationForm(
					password: _password,
					firstAndLastName: _firstAndLastName,
					phone: _phoneNumber,
					companyName: _companyName)
				.ClickConfirmButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(projectName)
				.ClickNextButton();

			_newProjectWorkflowPage.ClickCreateProjectButton();

			_workspacePage.ClickSendAgainButton();

			_emailConfirmationInformationDialog.ClickCloseInformationButton();

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
	}
}

