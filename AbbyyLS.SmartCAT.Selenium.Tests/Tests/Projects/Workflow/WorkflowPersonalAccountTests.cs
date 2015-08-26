using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects.Workflow
{
	class WorkflowPersonalAccountTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		public WorkflowPersonalAccountTests()
		{
			StartPage = StartPage.PersonalAccount;
		}

		[SetUp]
		public void WorkflowPersonalAccountTestsSetUp()
		{
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test]
		public void WorkflowStepNotExistInDialogProjectCreationTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				createNewTm: true,
				createGlossary: true,
				personalAccount: true);
		}

		[Test]
		public void WorkflowNotExistInProjectSettingsTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, personalAccount: true)
				.GoToProjectSettingsPage(_projectUniqueName)
				.OpenProjectSettings()
				.AssertWorkflowSettingsNotExist();
		}

		[Test]
		public void ChooseTaskDialogNotDisplayTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, personalAccount: true)
				.GoToProjectSettingsPage(_projectUniqueName)
				.UploadDocument(PathProvider.DocumentFile)
				.OpenDocument<EditorPage>(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}

		private readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		private string _projectUniqueName;
	}
}
