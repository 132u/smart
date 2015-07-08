using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class EditorBaseTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectUniqueName, PathProvider.EditorTxtFile)
				.GoToProjectSettingsPage(projectUniqueName)
				.ClickDocumentProgress(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.ClickAssignButtonInDocumentInfo()
				.SelectAssignmentType()
				.SelectAssignee(NickName)
				.CloseTaskAssignmentDialog()
				.ClickSaveButton()
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile))
				.SelectTask()
				.CloseTutorialIfExist();
		}

		public EditorHelper EditorHelper = new EditorHelper();

		private readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
	}
}
