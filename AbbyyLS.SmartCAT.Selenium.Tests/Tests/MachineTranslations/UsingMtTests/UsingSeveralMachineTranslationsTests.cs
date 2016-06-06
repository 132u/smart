using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.MachineTranslations.UsingMTTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	class UsingSeveralMachineTranslationsTests<TWebDriverProvider> : BaseMTTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_workspacePage.GoToProjectsPage();
		}

		[Test, Description("S-7272"), ShortCheckList]
		public void UseSeveralMachineTranslationsTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName, new [] {PathProvider.DocumentFile});

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			var documentName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);

			_projectSettingsPage
				.HoverDocumentRow(documentName)
				.ClickDocumentSettings(documentName);

			_documentSettingsDialog
				.SelectMachineTranslation(MachineTranslationType.SmartCATTranslator)
				.SelectMachineTranslation(MachineTranslationType.MicrosoftTranslator)
				.ClickSaveButtonExpectingProjectSettingsPage();

			_projectSettingsPage
				.WaitUntilDocumentProcessed();

			_projectSettingsHelper
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), ThreadUser.FullName, _projectUniqueName);

			_projectSettingsPage
				.OpenDocumentInEditorWithTaskSelect(PathProvider.DocumentFile);
			
			_selectTaskDialog
				.ClickTranslateButton()
				.ClickContinueButton();

			int firstRowNumber = _editorPage.CatTypeRowNumber(CatType.MT);
			int secondRowNumber = _editorPage.CatTypeRowNumber(CatType.MT, firstRowNumber);

			Assert.IsTrue(secondRowNumber > firstRowNumber,
				"Произошла ошибка:\nне отображаются несколько переводов МТ");
		}
	}
}
