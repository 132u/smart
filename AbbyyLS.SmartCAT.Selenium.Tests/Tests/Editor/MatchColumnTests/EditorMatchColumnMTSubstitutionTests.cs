﻿using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	class EditorMatchColumnMTSubstitutionTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_editorHelper = new EditorHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);

			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(
				projectName: projectUniqueName,
				filePath: PathProvider.TxtFileForMatchTest,
				createNewTm: true,
				tmxFilePath: PathProvider.TmxFileForMatchTest,
				useMachineTranslation: true);

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
		public void CheckMatchAfterMtSubstitution()
		{
			_editorHelper
				.PasteTranslationFromCAT(catType: CatType.MT)
				.AssertMatchColumnCatTypeMatch(catType: CatType.MT);
		}

		[Test]
		public void CheckMatchAfterBothSubstitutions()
		{
			_editorHelper
				.PasteTranslationFromCAT(catType: CatType.MT)
				.AssertMatchColumnCatTypeMatch(catType: CatType.MT)
				.AddTextToSegment(string.Empty)
				.PasteTranslationFromCAT(catType: CatType.TM)
				.AssertMatchColumnCatTypeMatch(catType: CatType.TM);

			var catRowNumber = _editorHelper.CatRowNumber(CatType.TM);

			_editorHelper.AssertCATPercentMatchTargetPercent(1, catRowNumber);
		}

		[Test]
		public void CheckMtMatchAfterAdd()
		{
			_editorHelper
				.AddTextToSegment()
				.PasteTranslationFromCAT(CatType.MT)
				.AssertMatchColumnCatTypeMatch(CatType.MT);
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectSettingsPage _projectSettingsPage;
		private EditorHelper _editorHelper;
	}
}
