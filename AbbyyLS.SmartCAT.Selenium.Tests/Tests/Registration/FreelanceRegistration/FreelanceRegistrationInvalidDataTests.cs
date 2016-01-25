﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Registration
{
	[Parallelizable(ParallelScope.Fixtures)]
	class FreelanceRegistrationInvalidDataTests<TWebDriverProvider> : FreelanceRegistrationBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public FreelanceRegistrationInvalidDataTests()
		{
			StartPage = StartPage.FreelanceRegistration;
		}

		[Test]
		public void MismatchConfirmPasswordRegistration()
		{
			_freelanceRegistrationFirstPage
				.FillFreelanceRegistrationFirstStep(
					email: _email,
					password: _password,
					confirmPassword: "wrong password")
				.ClickSignUpButtonExpectingError();

			Assert.IsTrue(_freelanceRegistrationFirstPage.IsPasswordMatchErrorMessageDisplayed(),
				"Произошла ошибка:\nНе отображается сообщение о неверном подтверждении пароля.");
		}

		[Test]
		public void UserAlreadyExistRegistration()
		{
			_freelanceRegistrationFirstPage
				.FillFreelanceRegistrationFirstStep(
					email: ThreadUser.Login,
					password: _password,
					confirmPassword: _password)
				.ClickSignUpButtonExpectingError();

			Assert.IsTrue(_freelanceRegistrationFirstPage.IsUserAlreadyExistErrorMessageDisplayed(),
				"Произошла ошибка:\nНе отображается сообщение том, что пользователь уже зарегистрирован в системе.");
		}

		[TestCase(" ")]
		[TestCase("")]
		public void EmptySpacePasswordRegistration(string password)
		{
			_freelanceRegistrationFirstPage
				.FillFreelanceRegistrationFirstStep(
					email: ThreadUser.Login,
					password: password,
					confirmPassword: _password);

			Assert.IsTrue(_freelanceRegistrationFirstPage.IsContinueButtonDisabled(),
				"Произошла ошибка:\nКнопка Continue активна.");

			Assert.IsTrue(_freelanceRegistrationFirstPage.IsInvalidPasswordErrorMessageDisplayed(),
				"Произошла ошибка:\nНе появилось сообщение 'The password must have at least 6 characters'.");
		}
		
		[TestCase("66!/.6@mailforspam.com ")]
		[TestCase("%%%/%%%")]
		[TestCase("‘ or ‘a’ = ‘a'; DROP TABLE user; SELECT * FROM blog WHERE code LIKE ‘a%’;")]
		[TestCase("<script>alert(“Hello, world!”)</alert>, <script>document.getElementByID(“…”).disabled=true</script>")]
		[TestCase("<form action=”http://live.hh.ru”><input type=”submit”></form>")]
		[TestCase("testsdsdsd.com")]
		[TestCase("asadsdsa,asddsa@asd.asd")]
		[TestCase("ывавыааыва@ывааываыв.com")]
		public void InvalidEmailRegistration(string email)
		{
			_freelanceRegistrationFirstPage
				.FillFreelanceRegistrationFirstStep(
					email: email,
					password: _password,
					confirmPassword: _password);

			Assert.IsTrue(_freelanceRegistrationFirstPage.IsContinueButtonDisabled(),
				"Произошла ошибка:\nКнопка Continue активна.");
		}

		[Test]
		public void NotExitEmailSignIn()
		{
			_freelanceRegistrationFirstPage.ClickExistAccountAbbyyOnlineLink();

			_freelanceRegistrationSignInPage
				.FillSignInForm(_email, _wrongPassword)
				.ClickSignInButtonExpectingError();

			Assert.IsTrue(_freelanceRegistrationSignInPage.IsUserNotExistErrorMessageDisplayed(),
				"Произошла ошибка:\nНе появилось валидационное сообщение о том, что пользователь не существует.");
		}
	}
}