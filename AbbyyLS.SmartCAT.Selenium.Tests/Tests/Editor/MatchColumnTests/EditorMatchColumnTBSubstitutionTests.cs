﻿using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorMatchColumnTBSubstitutionTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_editorHelper = new EditorHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			var _glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_createProjectHelper.CreateNewProject(
				projectName: projectUniqueName,
				filePath: PathProvider.TxtFileForMatchTest,
				glossaryName: _glossaryUniqueName);

			_createProjectHelper
				.GoToProjectSettingsPage(projectUniqueName)
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.TxtFileForMatchTest), ThreadUser.NickName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.TxtFileForMatchTest));

			_editorHelper
				.SelectTask()
				.CloseTutorialIfExist();
		}

		[Test]
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

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsPage _projectSettingsPage;
		private EditorHelper _editorHelper;
	}
}
