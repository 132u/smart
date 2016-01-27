using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Coursera
{
	class UserProfileBaseTests<TWebDriverProvider> : CourseraBaseTests<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_editProfileDialog = new EditProfileDialog(Driver);
			_newUserName = "name" + Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-"));
			_newUserSurname = "surname" + Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-"));
			_newFullName = _newUserName + " " + _newUserSurname;
			_translationText = "Test" + Guid.NewGuid();
			_filePath = PathProvider.ImageFile;

			_loginHelper.LogInCoursera(CourseraCrowdsourceUser.Login, CourseraCrowdsourceUser.Password);
		}

		protected EditProfileDialog _editProfileDialog;

		protected string _newUserName;
		protected string _newUserSurname;
		protected string _newFullName;
		protected string _translationText;
		protected string _filePath;
	}
}
