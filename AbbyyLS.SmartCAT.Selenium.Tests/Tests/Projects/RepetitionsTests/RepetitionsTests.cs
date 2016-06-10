using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Projects]
	class RepetitionsTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpRepetitionsTests()
		{
			_documentName = PathProvider.EditorCrossFileRepetitionsFirstFile;
		}

		[Test, Description("S-7265"), ShortCheckList]
		public void RepetitionArrowDisplayedTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { _documentName });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _documentName);

			Assert.IsTrue(_editorPage.IsRepetitionArrowDisplayed(1), 
				"Произошла ошибка:\n не показывается стрелка, говорящая, что в сегменте №1 есть повторы.");
		}

		[Test, Description("S-7266"), ShortCheckList]
		public void DeleteRepetitionTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { _documentName },
				tasks: new[] { WorkflowTask.Translation, WorkflowTask.Editing });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenAssignDialog(_projectUniqueName, _documentName);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 1)
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 2)
				.ClickSaveButton()
				.ClickBackToProjectButton();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_documentName);

			_selectTaskDialog
				.ClickTranslateButton()
				.ClickContinueButton();

			_editorPage.ClickRepetitionArrow();

			Assert.IsTrue(_editorPage.IsRepetitionExcluded(1),
				"Произошла ошибка: первый сегмент должен быть исключён из повторов");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_documentName);
			
			_selectTaskDialog
				.ClickEditingButton()
				.ClickContinueButton();

			Assert.IsTrue(_editorPage.IsRepetitionExcluded(1),
				"Произошла ошибка: первый сегмент должен быть исключён из повторов");
		}

		[Test, Description("S-7267"), ShortCheckList]
		public void DeleteRepetitionMultiLanguagesTest()
		{
			var targetLanguages = new[] { Language.Russian, Language.German };

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { _documentName },
				targetLanguages: targetLanguages);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, _documentName, language: targetLanguages[0]);

			_editorPage.ClickRepetitionArrow();

			Assert.IsTrue(_editorPage.IsRepetitionExcluded(1),
				"Произошла ошибка: первый сегмент должен быть исключён из повторов");

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(_documentName, language: targetLanguages[1]);

			Assert.IsTrue(_editorPage.IsRepetitionExcluded(1),
				"Произошла ошибка: первый сегмент должен быть исключён из повторов");
		}
	}
}
