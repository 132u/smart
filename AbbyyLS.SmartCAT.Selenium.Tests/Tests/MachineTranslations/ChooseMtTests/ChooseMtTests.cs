using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.MachineTranslations.ChooseMTTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ChooseMTTests<TWebDriverProvider> : BaseMTTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_document = PathProvider.DocumentFile;

			_workspacePage.GoToProjectsPage();

			_createProjectHelper.CreateNewProject(_projectUniqueName, new[] { _document });
		}

		[Test, Description("S-7270"), ShortCheckList]
		public void ChooseMTInDocumentInfoInWorkspaceTest()
		{
			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentSettings(_projectUniqueName, _document);

			Assert.IsFalse(_documentSettingsDialog.IsMachineTranslationSelected(MachineTranslationType.Google),
				"Произошла ошибка:\n для документа уже включен МТ Google");

			_documentSettingsDialog.SelectMachineTranslation(MachineTranslationType.SmartCATTranslator);

			Assert.IsTrue(_documentSettingsDialog.IsMachineTranslationSelected(MachineTranslationType.SmartCATTranslator), 
				"Произошла ошибка:\nдля документа не подключился MT SmartCAT Translator");
		}

		[Test, Description("S-29238"), ShortCheckList]
		public void ChooseMTInDocumentInfoInProjectPageTest()
		{
			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			var documentName = Path.GetFileNameWithoutExtension(_document);

			_projectSettingsPage
				.HoverDocumentRow(documentName)
				.ClickDocumentSettings(documentName);

			Assert.IsFalse(_documentSettingsDialog.IsMachineTranslationSelected(MachineTranslationType.SmartCATTranslator),
				"Произошла ошибка:\n для документа уже включен МТ SmartCATTranslator");

			_documentSettingsDialog.SelectMachineTranslation(MachineTranslationType.SmartCATTranslator);

			Assert.IsTrue(_documentSettingsDialog.IsMachineTranslationSelected(MachineTranslationType.SmartCATTranslator),
				"Произошла ошибка:\n для документа не подключился MT SmartCAT Translator");
		}

		private string _document;
	}
}
