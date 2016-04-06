using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	class DeleteProjectTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void DeleteProjectNoFileTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", _projectUniqueName);
		}

		[Test, Description("S-13753")]
		public void DeleteProjectWithFileTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { PathProvider.DocumentFile, PathProvider.DocumentFile2 });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", _projectUniqueName);
		}

		[Test, Description("S-13750")]
		public void DeleteDocumentFromProject()
		{
			var document = PathProvider.DocumentFile;

			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsHelper
				.UploadDocument(new[] { document })
				.DeleteDocument(Path.GetFileNameWithoutExtension(document));

			Assert.IsTrue(
				_projectsPage.IsDocumentRemovedFromProject(_projectUniqueName, document),
				"Произошла ошибка: документ {0} присутствует в проекте {1}", document, _projectUniqueName);
		}

		[Test]
		public void DeleteFileFromWizard()
		{
			var document = PathProvider.DocumentFile;

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { document })
				.DeleteDocument(Path.GetFileName(document));

			Assert.IsTrue(_newProjectDocumentUploadPage.IsFileDeleted(document),
				"Произошла ошибка:\n файл {0} не удалился.", document);
		}
	}
}
