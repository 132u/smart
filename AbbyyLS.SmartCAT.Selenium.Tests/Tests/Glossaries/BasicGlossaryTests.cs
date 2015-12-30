using System.Collections.Generic;
using System.IO;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	class BasicGlossaryTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_newGlossaryDialog = new NewGlossaryDialog(Driver);
			_glossaryPropertiesDialog = new GlossaryPropertiesDialog(Driver);
			_glossaryImportDialog = new GlossaryImportDialog(Driver);
			_glossarySuccessImportDialog = new GlossarySuccessImportDialog(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.GoToGlossariesPage();
		}

		[Test]
		public void CreateGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(_glossaryUniqueName),
				"Произошла ошибка:\n глоссарий отсутствует в списке");

		}

		[Test]
		public void CreateGlossaryWithoutNameTest()
		{
			_glossariesHelper.CreateGlossary("", errorExpected: true);

			Assert.IsTrue(_newGlossaryDialog.IsSpecifyGlossaryNameErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'Specify glossary name' не появилось.");
		}
		
		[Test]
		public void CreateGlossaryWithExistingNameTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(_glossaryUniqueName, errorExpected: true);

			Assert.IsTrue(_newGlossaryDialog.IsExistNameErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'A glossary with this name already exists' не появилось");
		}

		[Test]
		public void CheckLanguageNotExistTest()
		{
			_glossariesPage.ClickCreateGlossaryButton();

			_newGlossaryDialog
				.ExpandLanguageDropdown(dropdownNumber: 1)
				.SelectLanguage(Language.German)
				.ExpandLanguageDropdown(dropdownNumber: 2);

			Assert.IsFalse(_newGlossaryDialog.IsLanguageExistInDropdown(Language.German),
				"Произошла ошибка:\n язык присутствует в дропдауне");
		}

		[Test]
		public void DeleteLanguageCreateGlossaryTest()
		{
			_glossariesPage.ClickCreateGlossaryButton();

			var languagesCountBefore = _newGlossaryDialog.GetGlossaryLanguageCount();

			_newGlossaryDialog.ClickDeleteLanguageButton(languagesCountBefore);

			var languagesCountAfter = _newGlossaryDialog.GetGlossaryLanguageCount();

			Assert.AreNotEqual(languagesCountAfter, languagesCountBefore,
				"Произошла ошибка:\n количество языков не изменилось.");
		}

		[Ignore("PRX-10784"), Test]
		public void CreatedDateGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsDateModifiedMatchCurrentDate(_glossaryUniqueName),
				"Произошла ошибка:\n дата создания глоссария не совпадет с текущей датой");
		}

		[Test]
		public void ModifiedDateGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			var modifiedDateBefore = _glossariesPage.GlossaryDateModified(_glossaryUniqueName);

			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			// Sleep нужен, чтобы дата изменения изменилась (точность до минуты)
			Thread.Sleep(60000);

			_glossaryPage.CreateTerm();

			_workspacePage.GoToGlossariesPage();

			var modifiedDateAfter = _glossariesPage.GlossaryDateModified(_glossaryUniqueName);

			Assert.AreNotEqual(modifiedDateBefore, modifiedDateAfter,
				"Произошла ошибка:\n неверная дата изменния глоссария.");
		}

		[Test]
		public void ModifiedByAuthorTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			var modifiedBy = _glossariesPage.GetModifiedByAuthor(_glossaryUniqueName);

			Assert.AreEqual(modifiedBy, ThreadUser.NickName, 
				"Произошла ошибка:\n имя {0} не совпадает с {1}.", modifiedBy, ThreadUser.NickName);
		}

		[Test]
		public void DeleteGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog.ClickDeleteGlossaryButton();

			Assert.IsTrue(_glossaryPropertiesDialog.IsConfirmDeleteMessageDisplayed(),
				"Произошла ошибка: \nне появилось сообщение с подтверждением удаления глоссария");

			_glossaryPropertiesDialog.ClickConfirmDeleteGlossaryButton();

			Assert.IsTrue(_glossariesPage.IsGlossaryNotExist(_glossaryUniqueName),
				"Произошла ошибка:\n глоссарий присутствует в списке.");
		}

		[Test]
		public void DeleteLanguageExistTermTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.CreateTerm()
				.OpenGlossaryProperties();

			var languagesCountBefore = _glossaryPropertiesDialog.LanguagesCount();

			_glossaryPropertiesDialog.CancelDeleteLanguageInPropertiesDialog();

			var languagesCountAfter = _glossaryPropertiesDialog.LanguagesCount();

			Assert.AreEqual(languagesCountBefore, languagesCountAfter,
				"Произошла ошибка:\n количество языков {0} не совпадает с {1} в диалоге свойств глоссария.",
				languagesCountBefore, languagesCountAfter);
		}

		[Test]
		public void EditGlossaryStructureTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddNewSystemField(GlossarySystemField.Topic);

			_glossaryPage.ClickNewEntryButton();
				
			Assert.IsTrue(_glossaryPage.IsCreationModeActivated(),
				"Произошла ошибка:\n новый термин не открыт в расширенном режиме");
		}

		[Test]
		public void ImportGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.ClickImportButton();

			_glossaryImportDialog
				.ImportGlossary(PathProvider.ImportGlossaryFile)
				.ClickImportButtonInImportDialog();

			_glossarySuccessImportDialog.ClickCloseButton();

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}

		[Test]
		public void ImportGlossaryReplaceAllTermsTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.CreateTerm()
				.CreateTerm("termsecond", "termsecond")
				.ClickImportButton();

			_glossaryImportDialog
				.ClickReplaceTermsButton()
				.ImportGlossary(PathProvider.ImportGlossaryFile)
				.ClickImportButtonInImportDialog();

			_glossarySuccessImportDialog.ClickCloseButton();

			Assert.IsTrue(_glossaryPage.IsGlossaryContainsCorrectTermsCount(expectedTermsCount: 1),
				"Произошла ошибка:\n глоссарий содержит неверное количество терминов");
		}

		[Test]
		public void ExportGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.CreateTerm()
				.CreateTerm("secondTerm1", "secondTerm2")
				.ClickExportGlossary();

			Assert.IsTrue(_glossaryPage.IsGlossaryExportedSuccesfully(
				Path.Combine(PathProvider.ExportFiles, _glossaryUniqueName.Replace(":", "-") + ".xlsx")),
				"Произошла ошибка:\n файл не был скачан за отведенное время");

		}

		[Test]
		public void ChangeGlossaryNameTest()
		{
			var newGlossaryName = GlossariesHelper.UniqueGlossaryName();

			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.FillGlossaryName(newGlossaryName)
				.ClickSaveButton();

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(newGlossaryName),
				"Произошла ошибка:\n глоссарий отсутствует в списке");

			Assert.IsTrue(_glossariesPage.IsGlossaryNotExist(_glossaryUniqueName),
				"Произошла ошибка:\n глоссарий присутствует в списке.");
		}

		[Test]
		public void ChangeGlossaryExistingNameTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(_glossaryUniqueName + "2");

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.FillGlossaryName(_glossaryUniqueName)
				.ClickSaveButtonExpectingError();

			Assert.IsTrue(_newGlossaryDialog.IsExistNameErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'A glossary with this name already exists' не появилось");
		}

		[TestCase("")]
		[TestCase(" ")]
		public void ChangeGlossaryEmptyNameTest(string newGlossaryName)
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.FillGlossaryName(newGlossaryName)
				.ClickSaveButtonExpectingError();

			Assert.IsTrue(_newGlossaryDialog.IsSpecifyGlossaryNameErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'Specify glossary name' не появилось.");
		}

		[Test]
		public void OpenStructureFromPropertiesTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog.ClickAdvancedButton();
		}

		[Test]
		public void CreateMultiLanguageGlossaryTest()
		{
			_glossariesHelper
				.CreateGlossary(
					_glossaryUniqueName,
					languageList: new List<Language>
						{
							 Language.German,
							 Language.French,
							 Language.Japanese,
							 Language.Lithuanian
						});

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(_glossaryUniqueName),
				"Произошла ошибка:\n глоссарий отсутствует в списке");
		}

		private GlossariesHelper _glossariesHelper;
		private WorkspacePage _workspacePage;
		private GlossaryPage _glossaryPage;
		private GlossariesPage _glossariesPage;
		private NewGlossaryDialog _newGlossaryDialog;
		private GlossaryPropertiesDialog _glossaryPropertiesDialog;
		private GlossaryImportDialog _glossaryImportDialog;
		private GlossarySuccessImportDialog _glossarySuccessImportDialog;
		private GlossaryStructureDialog _glossaryStructureDialog;
		private string _glossaryUniqueName;
	}
}
