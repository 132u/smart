using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Coursera]
	class InvalidUserNameTests<TWebDriverProvider> : UserProfileBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(" ")]
		[TestCase("")]
		public void FillInvalidNameTest(string name)
		{
			_courseraHomePage.ClickProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.FillName(name: name);

			Assert.IsTrue(_editProfileDialog.IsNameErrorDisplayed(),
				"Произошла ошибка:\nНе появилась ошибка 'Please fill in the \"First name\" field'.");

			Assert.IsTrue(_editProfileDialog.IsNameRedBorderDisplayed(),
				"Произошла ошибка:\nНе появилась красная рамка у поля имени.");

			Assert.IsTrue(_editProfileDialog.IsSaveButtonInactive(),
				"Произошла ошибка:\nКнопка сохранения активна.");
		}

		[TestCase(" ")]
		[TestCase("")]
		public void FillInvalidSurnameTest(string surname)
		{
			_courseraHomePage.ClickProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.FillSurname(surname: surname);

			Assert.IsTrue(_editProfileDialog.IsSurnameErrorDisplayed(),
				"Произошла ошибка:\nНе появилась ошибка 'Please fill in the \"First name\" field'.");

			Assert.IsTrue(_editProfileDialog.IsSurnameRedBorderDisplayed(),
				"Произошла ошибка:\nНе появилась красная рамка у поля имени.");

			Assert.IsTrue(_editProfileDialog.IsSaveButtonInactive(),
				"Произошла ошибка:\nКнопка сохранения активна.");
		}
	}
}
