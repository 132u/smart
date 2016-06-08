using System;
using System.IO;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Projects]
	public class UploadDocumentToProjectTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7142"), ShortCheckList]
		public void UploadDocumentToExistingProjectOneTargetLanguageTest()
		{
			var file = PathProvider.EditorTxtFile;
			var fileName = Path.GetFileNameWithoutExtension(file);
			
			_createProjectHelper.CreateNewProject(_projectUniqueName, glossaryName: _projectUniqueName);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickDocumentUploadButton();

			_addFilesStep.UploadDocument(new[] { file });

			_addFilesStep.ClickNextButton();

			Assert.IsTrue(_settingResourceStep.IsTMChecked(_projectUniqueName),
				"Произошла ошибка:\n на странице настроек должна быть подключена проектная память преводов.");

			Assert.IsTrue(_settingResourceStep.IsGlossaryChecked(_projectUniqueName),
				"Произошла ошибка:\n на странице настроек должен быть подключен проектный глоссарий.");

			_settingResourceStep.ClickFinish<ProjectSettingsPage>();

			_projectSettingsPage.WaitUntilDocumentProcessed();

			Assert.IsTrue(_projectSettingsPage.IsDocumentExist(file),
				"Произошла ошибка:\n документ {0} отсутствует в проекте.", fileName);

			_projectSettingsPage
				.HoverDocumentRow(fileName)
				.ClickDocumentSettings(fileName);

			Assert.IsTrue(_documentSettingsDialog.IsTMChecked(_projectUniqueName),
				"Произошла ошибка:\n в диалоге настроек должна быть подключена проектная память преводов.");

			Assert.IsTrue(_documentSettingsDialog.IsGlossaryChecked(_projectUniqueName),
				"Произошла ошибка:\n в диалоге настроек должен быть подключен проектный глоссарий.");
		}

		[Test, Description("S-16404"), ShortCheckList]
		public void UploadDocumentToExistingProjectMultiTargetLanguageTest()
		{
			var targetLanguages = new[] { Language.French, Language.German, Language.Russian };
			var file = PathProvider.EditorTxtFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				glossaryName: _projectUniqueName,
				targetLanguages: targetLanguages);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickDocumentUploadButton();

			_addFilesStep.UploadDocument(new[] { file });

			Assert.IsTrue(_addFilesStep.IsLanguagesExist(targetLanguages),
				"Произошла ошибка:\n не все таргет языки отмечены.");

			_addFilesStep.ClickDeleteTargetLanguageButton(targetLanguages[0]);

			_addFilesStep.ClickNextButton();

			Assert.IsTrue(_settingResourceStep.IsTMChecked(_projectUniqueName),
				"Произошла ошибка:\n на странице настроек должна быть подключена проектная память преводов.");

			Assert.IsTrue(_settingResourceStep.IsGlossaryChecked(_projectUniqueName),
				"Произошла ошибка:\n на странице настроек должен быть подключен проектный глоссарий.");

			_settingResourceStep.ClickFinish<ProjectSettingsPage>();

			_projectSettingsPage.WaitUntilDocumentProcessed();

			Assert.IsTrue(_projectSettingsPage.IsDocumentExist(file, targetLanguages.Skip(1).ToArray()),
				"Произошла ошибка:\n появились джобы не для всех языков.");

			Assert.IsFalse(_projectSettingsPage.IsDocumentExist(file, targetLanguages.Take(1).ToArray()),
				"Произошла ошибка:\n для языка {0} не должно быть джобов.", targetLanguages[0]);
		}

		[Test, Description("S-7164"), ShortCheckList]
		[TestCase(true)]
		//[TestCase(false)] Ignore("PRX-17326")]
		public void UploadSecondDocumentToUpdateFirstDocumentTest(bool isDocx)
		{
			var file = PathProvider.EditorTxtDublicateFile3;
			var dublicateFile = PathProvider.EditorTxt3DublicateFile;
			//TODO исправить null, когда тестировщики дадут файл PRX-17326
			var filePath = isDocx ? file : null;
			var dublicateFilePath = isDocx ? dublicateFile : null;
			var translation = "Translation";
			
			_createProjectHelper.CreateNewProject(_projectUniqueName, filesPaths: new[] { filePath });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(filePath, ThreadUser.FullName, _projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(filePath);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(rowNumber: 2)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(rowNumber: 3)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(rowNumber: 4)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton()
				.UploadDublicateDocument(new[] { dublicateFilePath });

			Assert.IsTrue(_dublicateFileErrorDialog.IsDublicateFileErrorDialogOpened(),
				"Произошла ошибка: не открылся диалог обновления документа.");

			_dublicateFileErrorDialog
				.ClickUpdateButton()
				.ClickOkButton();

			Assert.IsTrue(_addFilesStep.IsFileUploaded(dublicateFilePath),
				"Произошла ошибка: файл не загрузился.");

			_addFilesStep.ClickNextButton();

			_settingResourceStep.ClickFinish<ProjectsPage>();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(_projectUniqueName, dublicateFilePath);

			Assert.AreEqual(translation, _editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка: неверный текст в таргете №1.");

			Assert.AreEqual(translation, _editorPage.GetTargetText(rowNumber: 2),
				"Произошла ошибка: неверный текст в таргете №2.");

			Assert.AreEqual(translation, _editorPage.GetTargetText(rowNumber: 3),
				"Произошла ошибка: неверный текст в таргете №3.");

			Assert.AreEqual(String.Empty, _editorPage.GetTargetText(rowNumber: 4),
				"Произошла ошибка: неверный текст в таргете №4.");
		}

		[Test, Description("S-13746"), ShortCheckList, Ignore("PRX-17347")]
		public void UpdateDocumentToIdenticalDocumentTest()
		{
			var translation = "Translation";

			var filePath = PathProvider.EditorTxtFile2;
			var dublicateFilePath = PathProvider.EditorTxtDublicateFile2;

			_createProjectHelper.CreateNewProject(_projectUniqueName, filesPaths: new[] { filePath });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentUploadButton()
				.UploadDublicateDocument(new[] { dublicateFilePath });

			Assert.IsTrue(_dublicateFileErrorDialog.IsDublicateFileErrorDialogOpened(),
				"Произошла ошибка: не открылся диалог обновления документа.");

			_dublicateFileErrorDialog
				.ClickUpdateButton()
				.ClickOkButton();
			//TODO дописать ассерт после фикса PRX-17347
		}
	}
}
