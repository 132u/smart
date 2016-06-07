using NUnit.Framework;

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
		public void QaCheckButtonExist()
		{
			var document = PathProvider.DocumentFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new []{ document });

			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsQualityAssuranceCheckButtonDisplayed(_projectUniqueName),
				"Произошла ошибка:\n кнопка 'QA Check' у проекта '{0}' отсутствует", _projectUniqueName);
		}

		[Test]
		public void CheckSettingsFormExist()
		{
			var document = PathProvider.DocumentFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { document });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			Assert.IsTrue(_settingsDialog.IsSettingsDialogOpened(),
				"Произошла ошибка: не открылся диалог настроек проекта");
		}

		[Test]
		public void CheckStatisticsFormExist()
		{
			var document = PathProvider.DocumentFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { document });

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

			_projectsPage.HoverProjectRow(_projectUniqueName);

			Assert.IsFalse(_projectsPage.IsProjectLinkExist(_projectUniqueName),
				"Произошла ошибка:\n не должно быть ссылки на проект {0}", _projectUniqueName);
		}
	}
}
