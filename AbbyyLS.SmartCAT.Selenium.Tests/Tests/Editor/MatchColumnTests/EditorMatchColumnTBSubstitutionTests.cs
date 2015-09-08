using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Standalone]
	public class EditorMatchColumnTBSubstitutionTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			
			_createProjectHelper.CreateNewProject(
				projectName: projectUniqueName,
				filePath: PathProvider.TxtFileForMatchTest,
				createGlossary: true);

			_projectsHelper
				.OpenProjectInfo(projectUniqueName)
				.OpenDocumentInfoForProject(projectUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.TxtFileForMatchTest), NickName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.TxtFileForMatchTest))
				.SelectTask()
				.CloseTutorialIfExist();
		}

		[Test, Explicit("PRX-11805")]
		public void CheckMatchAfterGlossarySubstitution()
		{
			var sourceTerm = _editorHelper
								.ClickTargetSegment(1)
								.SourceText(1);

			_editorHelper
				.AddNewTerm(sourceTerm, "термин глоссария")
				.PasteTranslationFromCAT(catType: CatType.TB)
				.AssertMatchColumnCatTypeMatch(catType: CatType.TB);
		}

		private readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		private readonly ProjectsHelper _projectsHelper = new ProjectsHelper();
		private readonly EditorHelper _editorHelper = new EditorHelper();
	}
}
