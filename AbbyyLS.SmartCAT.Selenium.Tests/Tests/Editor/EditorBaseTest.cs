using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class EditorBaseTest<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			EditorHelper = new EditorHelper(Driver);
			CreateProjectHelper = new CreateProjectHelper(Driver);
			ProjectSettingsPage = new ProjectSettingsPage(Driver);
			SettingsDialog = new SettingsDialog(Driver);
			ProjectSettingsHelper = new ProjectSettingsHelper(Driver);

			var projectUniqueName = CreateProjectHelper.GetProjectUniqueName();

			CreateProjectHelper
				.CreateNewProject(projectUniqueName, filePath: PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile), ThreadUser.NickName);

			ProjectSettingsPage.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			EditorHelper
				.SelectTask()
				.CloseTutorialIfExist();
		}

		protected EditorHelper EditorHelper;
		protected CreateProjectHelper CreateProjectHelper;
		protected ProjectSettingsHelper ProjectSettingsHelper;
		protected ProjectSettingsPage ProjectSettingsPage;
		protected SettingsDialog SettingsDialog;
	}
}
