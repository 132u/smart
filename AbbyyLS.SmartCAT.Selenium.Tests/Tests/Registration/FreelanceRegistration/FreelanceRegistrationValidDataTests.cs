using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Ignore("PRX-15132")]
	[Parallelizable(ParallelScope.Fixtures)]
	class FreelanceRegistrationValidDataTests<TWebDriverProvider> : FreelanceRegistrationBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public FreelanceRegistrationValidDataTests()
		{
			StartPage = StartPage.FreelanceRegistration;
		}

		[Test, Ignore("Тест заигнорен, так как регистрация изменилась,необходимо подтверждение email по ссылке из письма PRX-14166")]
		public void RegistrationNewFreelancer()
		{
			_freelanceRegistrationFirstPage
				.FillFreelanceRegistrationFirstStep(
					email: _email,
					password: _password,
					confirmPassword: _password)
				.ClickContinueButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsNewProjectDocumentUploadPageOpened(),
				"Произошла ошибка:\nСтраница поректов не открылась.");

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_email),
				"Произошла ошибка:\nНеверное имя фрилансера в верхней панели.");
		}

		[Test, Ignore("Тест заигнорен, так как регистрация изменилась,необходимо подтверждение email по ссылке из письма PRX-14166")]
		public void LoginAfterRegistration()
		{
			_freelanceRegistrationFirstPage
				.FillFreelanceRegistrationFirstStep(
					email: _email,
					password: _password,
					confirmPassword: _password)
				.ClickContinueButton();
			
			_workspacePage.SignOutExpectingAlert();

			_signInPage
				.SubmitForm(_email, _password)
				.SelectAccount(LoginHelper.PersonalAccountName);
			
			Assert.IsTrue(_newProjectDocumentUploadPage.IsNewProjectDocumentUploadPageOpened(),
				"Произошла ошибка:\nСтраница поректов не открылась.");

			Assert.IsTrue(_workspacePage.IsUserNameMatchExpected(_email),
				"Произошла ошибка:\nНеверное имя фрилансера в верхней панели.");
		}
	}
}
