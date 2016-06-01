using System;
using System.IO;
using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;


namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Projects]
	public class UploadDocumentToProjectTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7142"), ShortCheckList]
		public void UploadDocumentToExistingProjectOneTargetLanguageTest()
		{
			var fileName = Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile);

			_createProjectHelper.CreateNewProject(_projectUniqueName, glossaryName: _projectUniqueName);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickDocumentUploadButton();

			_documentUploadGeneralInformationDialog.UploadDocument(new[] { PathProvider.EditorTxtFile });

			_documentUploadGeneralInformationDialog.ClickNextButton();

			Assert.IsTrue(_settingResourceStep.IsTMChecked(_projectUniqueName),
				"Произошла ошибка:\n на странице настроек должна быть подключена проектная память преводов.");

			Assert.IsTrue(_settingResourceStep.IsGlossaryChecked(_projectUniqueName),
				"Произошла ошибка:\n на странице настроек должен быть подключен проектный глоссарий.");

			_settingResourceStep.ClickFinish<ProjectSettingsPage>();
			
			_projectSettingsPage.WaitUntilDocumentProcessed();

			Assert.IsTrue(_projectSettingsPage.IsDocumentExist(PathProvider.EditorTxtFile),
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
			var targetLanguages = new[] {Language.French, Language.German, Language.Russian};

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, 
				glossaryName: _projectUniqueName, 
				targetLanguages: targetLanguages);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.ClickDocumentUploadButton();
				
			_documentUploadGeneralInformationDialog.UploadDocument(new[] { PathProvider.EditorTxtFile });

			Assert.IsTrue(_documentUploadGeneralInformationDialog.IsLanguagesExist(targetLanguages),
				"Произошла ошибка:\n не все таргет языки отмечены.");

			_documentUploadGeneralInformationDialog.ClickDeleteTargetLanguageButton(targetLanguages[0]);

			_documentUploadGeneralInformationDialog.ClickNextButton();

			Assert.IsTrue(_settingResourceStep.IsTMChecked(_projectUniqueName),
				"Произошла ошибка:\n на странице настроек должна быть подключена проектная память преводов.");

			Assert.IsTrue(_settingResourceStep.IsGlossaryChecked(_projectUniqueName),
				"Произошла ошибка:\n на странице настроек должен быть подключен проектный глоссарий.");

			_settingResourceStep.ClickFinish<ProjectSettingsPage>();

			_projectSettingsPage.WaitUntilDocumentProcessed();

			Assert.IsTrue(_projectSettingsPage.IsDocumentExist(PathProvider.EditorTxtFile, targetLanguages.Skip(1).ToArray()),
				"Произошла ошибка:\n появились джобы не для всех языков.");

			Assert.IsFalse(_projectSettingsPage.IsDocumentExist(PathProvider.EditorTxtFile, targetLanguages.Take(1).ToArray()),
				"Произошла ошибка:\n для языка {0} не должно быть джобов.", targetLanguages[0]);
		}
	}
}
