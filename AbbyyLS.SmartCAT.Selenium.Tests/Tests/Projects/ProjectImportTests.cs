using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Standalone]
	class ProjectImportTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Setup()
		{
			WorkspaceHelper.GoToProjectsPage();
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
		}

		[Test]
		public void ImportUnsupportedFileTest()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.UploadFile(PathProvider.AudioFile)
				.AssertErrorFormatDocument();
		}

		[Test]
		public void ImportSomeFilesTest()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.UploadFile(PathProvider.DocumentFile)
				.UploadFile(PathProvider.TtxFile)
				.AssertNoErrorFormatDocument();
		}

		[Test]
		public void ImportTtxFileTest()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.UploadFile(PathProvider.TtxFile)
				.AssertNoErrorFormatDocument();
		}

		[Test]
		public void ImportTxtFileTest()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.UploadFile(PathProvider.TxtFile)
				.AssertNoErrorFormatDocument();
		}

		[Test]
		public void ImportSrtFileTest()
		{
			_createProjectHelper
				.ClickCreateProjectButton()
				.UploadFile(PathProvider.SrtFile)
				.AssertNoErrorFormatDocument();
		}

		[Test]
		public void ImportDocumentAfterCreationTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName)
				.GoToProjectSettingsPage(_projectUniqueName)
				.UploadDocument(PathProvider.DocumentFile)
				.AssertDocumentExist(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile));
		}

		[Test]
		public void ImportDuplicateDocumentTest()
		{
			_createProjectHelper
				.CreateNewProject(_projectUniqueName, PathProvider.DocumentFile)
				.GoToProjectSettingsPage(_projectUniqueName)
				.ClickDocumentUploadButton()
				.UploadDocument(PathProvider.DocumentFile)
				.UploadDocument(PathProvider.DocumentFile)
				.AssertErrorDuplicateDocumentNameExist();
		}

		private string _projectUniqueName;
		private CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
	}
}
