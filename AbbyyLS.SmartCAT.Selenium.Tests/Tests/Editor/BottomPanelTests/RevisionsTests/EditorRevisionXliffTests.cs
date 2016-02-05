using System.IO;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
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
			var _projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(_projectUniqueName, filePath: PathProvider.EditorXliffFile);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.EditorXliffFile), ThreadUser.NickName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorXliffFile));

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
