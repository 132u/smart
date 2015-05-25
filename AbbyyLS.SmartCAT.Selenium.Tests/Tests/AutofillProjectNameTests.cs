using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DriversAndSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[TestFixture]
	[PriorityMajor]
	[Standalone]
	class AutofillProjectNameTests<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			QuitDriverAfterTest = true;
			_createProjectHelper = new CreateProjectHelper();
		}

		[Test]
		public void AutofillProjectName()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.AddFileFromWizard(_filePath)
				.AssertProjectNameMatch(_fileName);
		}

		[Test]
		public void AutofillProjectNameAddTwoFiles()
		{
			 _createProjectHelper
				.ClickCreateProjectButton()
				.AddFileFromWizard(_filePath)
				.AssertProjectNameMatch(_fileName)
				.AddFileFromWizard(_filePath)
				.AssertProjectNameMatch(_fileName);
		}

		[Test]
		public void AutofillProjectNameDeleteFile()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.AddFileFromWizard(_filePath)
				.AssertProjectNameMatch(_fileName)
				.DeleteFileFromWizard(_filePath)
				.AssertProjectNameMatch(_fileName);
		}

		private CreateProjectHelper _createProjectHelper;
		private string _fileName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);
		private string _filePath = PathProvider.DocumentFile;
	}
}
