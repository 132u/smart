using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	[Parallelizable(ParallelScope.Fixtures)]
	class UserProfileTests<TWebDriverProvider> : UserProfileBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void ChangeAboutMeTest()
		{
			string aboutMeInformation = "About Me Info " + Guid.NewGuid();

			_header.GoToUserProfile();

			_profilePage.ClickEditProfileButton();

			_editProfileDialog.EditProfile(aboutMe: aboutMeInformation);

			Assert.AreEqual(aboutMeInformation, _profilePage.GetAboutMeInformation(),
				"Произошла ошибка:\nНеверная информация о пользователе.");
		}

		[Test]
		public void OpenProfileFromHomePage()
		{
			_courseraHomePage.ClickProfile();

			Assert.IsTrue(_profilePage.IsUserProfilePageOpened(),
				"Произошла ошибка:\nНе открылась страница пользователя.");
		}

		[Test]
		public void OpenProfileFromHeaderMenuPage()
		{
			_header.GoToUserProfile();

			Assert.IsTrue(_profilePage.IsUserProfilePageOpened(),
				"Произошла ошибка:\nНе открылась страница пользователя.");
		}

		[Test]
		public void OpenProfileFromAnotherUserPage()
		{
			_courseraHomePage.ClickProfile();
			_header.GoToLeaderboardPage();

			var userName = _leaderboardPage.GetUserNameInList();
			_leaderboardPage.ClickUserName(userName);

			Assert.AreEqual(userName, _profilePage.GetNickname(),
				"Произошла ошибка:\nНеверное имя пользователя.");

			_header.GoToUserProfile();

			Assert.AreEqual(CourseraCrowdsourceUser.NickName, _profilePage.GetNickname(),
				"Произошла ошибка:\nНеверное имя пользователя.");
		}

		[Test]
		public void OpenProfileFromLeaderboard()
		{
			_courseraHomePage.ClickSelectCourse();

			_coursesPage.GoToLeaderboardPage();

			_leaderboardPage.ClickUserName(CourseraCrowdsourceUser.NickName);

			Assert.IsTrue(_profilePage.IsUserProfilePageOpened(),
				"Произошла ошибка:\nНе открылась страница пользователя.");
		}
	}
}
