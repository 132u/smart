using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class EditorBaseTest<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			CreateProjectHelper = new CreateProjectHelper(Driver);
			ProjectSettingsPage = new ProjectSettingsPage(Driver);
			SettingsDialog = new SettingsDialog(Driver);
			ProjectSettingsHelper = new ProjectSettingsHelper(Driver);
			EditorPage = new EditorPage(Driver);
			SelectTaskDialog = new SelectTaskDialog(Driver);

			var projectUniqueName = CreateProjectHelper.GetProjectUniqueName();

			CreateProjectHelper
				.CreateNewProject(projectUniqueName, filePath: PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(projectUniqueName)
				.AssignTasksOnDocument(PathProvider.EditorTxtFile, ThreadUser.NickName);

			ProjectSettingsPage.OpenDocumentInEditorWithTaskSelect(PathProvider.EditorTxtFile);

			SelectTaskDialog.SelectTask();

			EditorPage.CloseTutorialIfExist();
		}

		protected CreateProjectHelper CreateProjectHelper;
		protected ProjectSettingsHelper ProjectSettingsHelper;
		protected ProjectSettingsPage ProjectSettingsPage;
		protected SettingsDialog SettingsDialog;
		public EditorPage EditorPage;
		public SelectTaskDialog SelectTaskDialog;
	}
}
