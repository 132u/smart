using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class FormsAndButtonsAvailabilityProjectsPageTests<TWebDriverProvider> : FormsAndButtonsAvailabilityBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void GreenCreateProjectButtonTest()
		{
			Assert.IsFalse(_projectsPage.IsGreenCreateProjectButtonDisplayed(),
				"Произошла ошибка:\n Зеленая кнопка создания поекта отображается.");
		}

		[Test]
		public void OpenEditorFromProjectSettingsPageTest()
		{
			_projectsPage.OpenProjectInfo(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(PathProvider.DocumentFile);

			Assert.IsTrue(_editorPage.IsEditorPageOpened(),
				"Произошла ошибка:\n Редактор не открылся.");
		}

		[Test]
		public void DeleteFileButtonTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.SelectDocument(_projectUniqueName, PathProvider.DocumentFile);

			Assert.IsFalse(_projectsPage.IsDeleteFileButtonDisplayed(_projectUniqueName),
				"Произошла ошибка:\n кнопка удаления файла отображается.");
		}

		[Test]
		public void DeleteFileButtonWithoutSelectDocumentTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName);

			Assert.IsFalse(_projectsPage.IsDeleteFileButtonDisplayed(_projectUniqueName),
				"Произошла ошибка:\n кнопка удаления файла отображается.");
		}

		[Test]
		public void AddFilesButtonTest()
		{
			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsFalse(_projectsPage.IsAddFilesButtonDisplayed(),
				"Произошла ошибка:\n кнопка 'Add Files' отображается для проекта {0}.", _projectUniqueName);
		}

		[Test]
		public void QualityAssuranceCheckButtonTest()
		{
			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsQualityAssuranceCheckButtonDisplayed(_projectUniqueName),
				"Произошла ошибка:\n кнопка 'QA Check' у проекта '{0}' отсутствует", _projectUniqueName);

			_projectsPage.ClickQACheckButton(_projectUniqueName);

			Assert.IsTrue(_qualityAssuranceDialog.IsQualityAssuranceDialogOpened(),
				"Произошла ошибка:\n Не открылся диалог контроля качества.");
		}

		[Test]
		public void CreateProjectButtonTest()
		{
			Assert.IsFalse(_projectsPage.IsCreateProjectButtonDisplayed(),
				"Произошла ошибка:\n Кнопка создания поекта отображается.");
		}

		[Test]
		public void DocumentDeleteButtonTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.SelectDocument(_projectUniqueName, PathProvider.DocumentFile);

			Assert.IsFalse(_projectsPage.IsDeleteFileButtonDisplayed(_projectUniqueName),
				"Произошла ошибка:\n Кнопка удаления проекта присутсвует на странице проектов.");
		}

		[Test]
		public void ProjectDeleteButtonTest()
		{
			_projectsPage.ClickProjectCheckboxInList(_projectUniqueName);

			Assert.IsFalse(_projectsPage.IsDeleteProjectButtonDisplayed(),
				"Произошла ошибка:\n Кнопка удаления проекта присутсвует на странице проектов.");
		}

		[Test]
		public void DocumentUploadButtonTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName);

			Assert.IsFalse(_projectsPage.IsDocumentUploadButtonDisplayed(),
				"Произошла ошибка:\n Кнопка загрузки документа присутсвует на странице проектов.");
		}

		[Test]
		public void AssignTaskButtonInDocumentPanelTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName);
			
			Assert.IsFalse(_projectsPage.IsAssignTaskButtonInDocumentPanelDisabled(_projectUniqueName),
				"Произошла ошибка:\n Кнопка назначения неактивна.");
		}

		[Test]
		public void AssignTaskButtonInProjectPanelTest()
		{
			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsFalse(_projectsPage.IsAssignTaskButtonInProjectPanelDisabled(_projectUniqueName),
				"Произошла ошибка:\n Кнопка назначения активна.");

			_projectsPage.SelectDocument(_projectUniqueName, PathProvider.DocumentFile);

			Assert.IsFalse(_projectsPage.IsAssignTaskButtonInProjectPanelDisabled(_projectUniqueName),
				"Произошла ошибка:\n Кнопка назначения неактивна.");
		}

		[Test]
		public void StatisticsButtonTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectStatisticsButtonExpectingBuildStatisticsPage(_projectUniqueName);

			Assert.IsTrue(_statisticsPage.IsBuildStatisticsPageOpened(),
				"Произошла ошибка:\n Не открылась страница построения статистики.");
		}

		[Test]
		public void DocumentSettingsButtonTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.ClickDocumentSettings(_projectUniqueName);

			Assert.IsTrue(_documentSettingsDialog.IsDocumentSettingsDialogOpened(),
				"Произошла ошибка:\n Не открылся диалог настроек документа.");
		}
	}
}
