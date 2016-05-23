using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	class EditorRevisionTBCatTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupTest()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_settingsDialog = new ProjectSettingsDialog(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_editorPage = new EditorPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_signInPage = new SignInPage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_pretranslationDialog = new PretranslationDialog(Driver);
			_glossaryHelper = new GlossariesHelper(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_workspacePage = new WorkspacePage(Driver);

			var _glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { PathProvider.EditorTxtFile });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(
				Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile), ThreadUser.NickName, _projectUniqueName);

			_workspacePage.GoToGlossariesPage();

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			var dictionary = new Dictionary<string, string>
			{ 
				{"first", "первый"},
				{"sentence", "предложение"}
			};

			_glossaryPage.CreateTerms(dictionary);

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage
				.SelectGlossaryByName(_glossaryUniqueName)
				.OpenDocumentInEditorWithTaskSelect(Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile));

			_selectTaskDialog.SelectTask();
		}

		[Test, Ignore("PRX-16581")]
		public void TBRevisionTest()
		{
			_editorPage.PasteTranslationFromCAT(catType: CatType.TB);

			Assert.AreEqual(RevisionType.InsertTb.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка:\nНеверный тип ревизии.");
		}

		[Test, Ignore("PRX-16581")]
		public void TBRevisionByHotKeyTest()
		{
			_editorPage.PasteTranslationFromCATByHotkey(catType: CatType.TB);

			Assert.AreEqual(RevisionType.InsertTb.Description(), _editorPage.GetRevisionType(),
				"Произошла ошибка:\nНеверный тип ревизии.");
		}

		protected CreateProjectHelper _createProjectHelper;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected ProjectSettingsPage _projectSettingsPage;
		protected ProjectSettingsDialog _settingsDialog;
		protected SignInPage _signInPage;
		protected ProjectsPage _projectsPage;
		protected EditorPage _editorPage;
		protected SelectTaskDialog _selectTaskDialog;
		protected string _projectUniqueName;
		private GlossaryPage _glossaryPage ;
		private GlossariesHelper _glossaryHelper ;
		protected PretranslationDialog _pretranslationDialog;
		protected WorkspacePage _workspacePage;
	}
}
