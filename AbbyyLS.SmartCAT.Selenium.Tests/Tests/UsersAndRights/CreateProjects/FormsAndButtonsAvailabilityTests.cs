﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class FormsAndButtonsAvailabilityTests<TWebDriverProvider> : CreateProjectWithRightBaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CheckConnectorButtonNotExist()
		{
			Assert.IsFalse(_projectsPage.IsSignInToConnectorButtonExist(),
				"Произошла ошибка:\n кнопка 'Sign in to Connector' не должна быть видна.");
		}

		[Test]
		public void QaCheckButtonExist()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new []{ PathProvider.DocumentFile });

			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsQualityAssuranceCheckButtonDisplayed(_projectUniqueName),
				"Произошла ошибка:\n кнопка 'QA Check' у проекта '{0}' отсутствует", _projectUniqueName);
		}

		[Test]
		public void CheckSettingsFormExist()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { PathProvider.DocumentFile });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			Assert.IsTrue(_settingsDialog.IsSettingsDialogOpened(),
				"Произошла ошибка: не открылся диалог настроек проекта");
		}

		[Test]
		public void CheckStatisticsFormExist()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { PathProvider.DocumentFile });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectStatisticsButtonExpectingBuildStatisticsPage(_projectUniqueName);

			Assert.IsTrue(_buildStatisticsPage.IsBuildStatisticsPageOpened(),
				"Произошла ошибка: не открылась страница построения статистики");
		}

		[Test]
		public void CheckLinkInProjectNotExist()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			Assert.IsFalse(_projectsPage.IsProjectLinkExist(_projectUniqueName),
				"Произошла ошибка:\n не должно быть ссылки на проект {0}", _projectUniqueName);
		}
	}
}
