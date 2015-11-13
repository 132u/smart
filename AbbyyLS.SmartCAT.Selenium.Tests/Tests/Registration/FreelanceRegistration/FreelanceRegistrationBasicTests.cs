using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration.FreelanceRegistration;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	class FreelanceRegistrationBasicTests<TWebDriverProvider> : FreelanceRegistrationBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public FreelanceRegistrationBasicTests()
		{
			StartPage = StartPage.FreelanceRegistration;
		}
		
		[Test]
		public void RegistrationNewFreelancer()
		{
			_freelanceRegistrationFirstPage
				.FillFreelanceRegistrationFirstStep(
					email: _email,
					password: _password,
					confirmPassword: _password)
				.ClickContinueButton();

			_freelanceRegistrationSecondPage
				.FillFreelanceRegistrationSecondStep(
					firstName: _firstName,
					lastName: _lastName)
				.ClickCreateAccountButton();

			Assert.IsTrue(_projectsPage.IsProjectsPageOpened(),
				"Произошла ошибка:\nСтраница поректов не открылась.");

			Assert.IsTrue(_workspacePage.IsNickNameMatch(_nickName),
				"Произошла ошибка:\nНеверное имя фрилансера в верхней панели.");
		}

		[Test]
		public void LoginAfterRegistration()
		{
			_freelanceRegistrationFirstPage
				.FillFreelanceRegistrationFirstStep(
					email: _email,
					password: _password,
					confirmPassword: _password)
				.ClickContinueButton();

			_freelanceRegistrationSecondPage
				.FillFreelanceRegistrationSecondStep(
					firstName: _firstName,
					lastName: _lastName)
				.ClickCreateAccountButton();

			_workspacePage
				.CloseHelpIfOpened()
				.ClickSignOut();

			_signInPage
				.SubmitForm(_email, _password)
				.SelectAccount(LoginHelper.PersonalAccountName);
			
			Assert.IsTrue(_projectsPage.IsProjectsPageOpened(),
				"Произошла ошибка:\nСтраница поректов не открылась.");

			Assert.IsTrue(_workspacePage.IsNickNameMatch(_nickName),
				"Произошла ошибка:\nНеверное имя фрилансера в верхней панели.");
		}
	}
}
