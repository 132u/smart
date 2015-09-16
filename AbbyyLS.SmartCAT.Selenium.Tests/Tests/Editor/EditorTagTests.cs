using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.ExplicitAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Standalone]
	class EditorTagTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper
				.CreateNewProject(projectUniqueName, filePath: PathProvider.DocumentFile)
				.GoToProjectSettingsPage(projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), ConfigurationManager.NickName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile))
				.SelectTask()
				.CloseTutorialIfExist();
		}

		[Test]
		public void TagButtonTest()
		{
			_editorHelper.InsertTag();
		}

		[Test]
		public void TagHotkeyTest()
		{
			_editorHelper
				.AddTextToSegment()
				.InsertTagByHotKey();
		}

		private readonly EditorHelper _editorHelper = new EditorHelper();
		private readonly  CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
	}
}
