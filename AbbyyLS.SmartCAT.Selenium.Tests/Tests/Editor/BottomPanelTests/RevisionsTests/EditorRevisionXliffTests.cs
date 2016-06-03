using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	class EditorRevisionXliffTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void EditorRevisionXliffTestsSetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_editorPage = new EditorPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
		}

		[Test]
		public void RevisionsInXlfTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			var document = PathProvider.EditorXliffFile;
			var documentName = Path.GetFileNameWithoutExtension(document);

			_createProjectHelper.CreateNewProject(
				projectUniqueName, filesPaths: new[] { document });

			_projectsPage.OpenProjectSettingsPage(projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(documentName, ThreadUser.NickName, projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(documentName);

			_selectTaskDialog.SelectTask();

			Assert.AreEqual(RevisionType.Pretranslation.Description(), _editorPage.GetRevisionType(),
						"Произошла ошибка:\nНеверный тип ревизии.");
		}

		protected CreateProjectHelper _createProjectHelper;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected EditorPage _editorPage;
		protected ProjectSettingsPage _projectSettingsPage;
		protected SelectTaskDialog _selectTaskDialog;
		protected ProjectsPage _projectsPage;
	}
}
