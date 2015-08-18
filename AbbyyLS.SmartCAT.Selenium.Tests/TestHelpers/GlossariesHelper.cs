using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

using NLog;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class GlossariesHelper : WorkspaceHelper
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

		public GlossariesHelper AssertClientExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickCreateGlossaryButton()
				.OpenClientsList()
				.AssertClientsListOpened()
				.AssertClientExistInList(clientName);

			return this;
		}

		public GlossariesHelper AssertClientNotExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickCreateGlossaryButton()
				.OpenClientsList()
				.AssertClientsListOpened()
				.AssertClientNotExistInList(clientName);

			return this;
		}

		public GlossariesHelper AssertProjectGroupExist (string projectGroupName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickCreateGlossaryButton()
				.ClickProjectGroupsList()
				.AssertProjectGroupsListOpened()
				.AssertProjectGroupExistInList(projectGroupName);

			return this;
		}

		public GlossariesHelper AssertProjectGroupNotExist(string projectGroupName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickCreateGlossaryButton()
				.ClickProjectGroupsList()
				.AssertProjectGroupsListOpened()
				.AssertProjectGroupNotExistInList(projectGroupName);

			return this;
		}

		public GlossariesHelper ClickSortByName()
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickSortByName();

			return this;
		}

		public GlossariesHelper ClickSortByLanguages()
		{			
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickSortByLanguages();

			return this;
		}

		public GlossariesHelper ClickSortByTermsAdded()
		{			
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickSortByTermsAdded();

			return this;
		}

		public GlossariesHelper ClickSortByTermsUnderReview()
		{			
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickSortByTermsUnderReview();

			return this;
		}

		public GlossariesHelper ClickSortByComment()
		{			
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickSortByComment();

			return this;
		}

		public GlossariesHelper ClickSortByProjectGroups()
		{			
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickSortByProjectGroups();

			return this;
		}

		public GlossariesHelper ClickSortByClient()
		{			
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickSortByClient();

			return this;
		}

		public GlossariesHelper ClickSortGlossariesToDateModified()
		{			
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickSortByDateModified();

			return this;
		}

		public GlossariesHelper ClickSortTermsToDateModified()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickSortByDateModified();

			return this;
		}

		public GlossariesHelper ClickSortByModifiedBy()
		{			
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickSortByModifiedBy();

			return this;
		}

		public GlossariesHelper ClickSortByEnglishTerm()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickSortByEnglishTerm();

			return this;
		}

		public GlossariesHelper ClickSortByRussianTerm()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickSortByRussianTerm();

			return this;
		}

		public GlossariesHelper AssertGlossaryExist(string glossaryName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.AssertGlossaryExist(glossaryName);

			return this;
		}

		public GlossariesHelper AssertGlossaryNotExist(string glossaryName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.AssertGlossaryNotExist(glossaryName);

			return this;
		}

		public GlossariesHelper AssertModifiedByMatch(string glossaryName, string userName)
		{
			BaseObject.InitPage(_glossariesPage);
			var modifiedBy = _glossariesPage.GetModifiedByAuthor(glossaryName);

			Assert.AreEqual(modifiedBy, userName,
				"Произошла ошибка:\n имя {0} не совпадает с {1}.", modifiedBy, userName);

			return this;
		}

		public GlossariesHelper FillLanguageComment(string comment)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.OpenLanguageAndTermDetailsEditMode()
				.FillLanguageComment(comment);

			return this;
		}

		public GlossariesHelper FillDefinitionSource(string definitionSource)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.OpenLanguageAndTermDetailsEditMode()
				.FillDefinitionSource(definitionSource);

			return this;
		}

		public GlossariesHelper ClickSaveEntryButton()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickSaveEntryButton();

			return this;
		}

		public GlossariesHelper AssertLanguageCommentIsFilled(string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.OpenLanguageAndTermDetailsViewMode()
				.AssertCommentIsFilled(text);

			return this;
		}
		
		public GlossariesHelper AssertDefinitionFilled(string definition)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.OpenLanguageAndTermDetailsViewMode()
				.AssertDefinitionIsFilled(definition);

			return this;
		}

		public GlossariesHelper AssertDefinitionSourceFilled(string definition)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.OpenLanguageAndTermDetailsViewMode()
				.AssertDefinitionSourceIsFilled(definition);

			return this;
		}

		public GlossariesHelper CreateTerm(
			string firstTerm = "firstTerm",
			string secondTerm = "secondTerm")
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.ClickNewEntryButton()
				.FillTerm(1, firstTerm)
				.FillTerm(2, secondTerm)
				.ClickSaveTermButton();
			
			return this;
		}

		public GlossariesHelper ExportGlossary()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickExportGlossary();

			return this;
		}

		public static string UniqueGlossaryName()
		{
			return "TestGlossary" + DateTime.Now.ToString("MM.dd.yyyy HH:mm:ss", new CultureInfo("en-US"));
		}

		public GlossariesHelper CreateGlossary(
			string glossaryName,
			string comment = "Test Glossary Generated by Selenium",
			bool errorExpected = false,
			List<Language> languageList = null,
			string projectGroupName = null)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickCreateGlossaryButton()
				.FillGlossaryName(glossaryName)
				.FillComment(comment);

			BaseObject.InitPage(_newGlossaryDialog);
			if (languageList != null && languageList.Count > 0)
			{
				_newGlossaryDialog
					.ClickDeleteLanguageButton()
					.ExpandLanguageDropdown(1)
					.SelectLanguage(languageList[0]);

				for (var i = 2; i <= languageList.Count; i++)
				{
					_newGlossaryDialog
						.ClickAddButton()
						.ExpandLanguageDropdown(i)
						.SelectLanguage(languageList[i - 1]);
				}
			}

			if (projectGroupName != null)
			{
				_newGlossaryDialog
					.ClickProjectGroupsList()
					.SelectProjectGroup(projectGroupName)
					.ClickProjectGroupsList();
			}

			if (errorExpected)
			{
				_newGlossaryDialog.ClickSaveGlossaryButton<NewGlossaryDialog>();
			}
			else
			{
				_newGlossaryDialog
					.ClickSaveGlossaryButton<GlossaryPage>()
					.AssertDialogBackgroundDisappeared<GlossaryPage>();
			}

			return this;
		}

		public GlossariesHelper AssertEmptyNameErrorDisplay()
		{
			BaseObject.InitPage(_newGlossaryDialog);
			_newGlossaryDialog.AssertEmptyNamyErrorDisplay();

			return this;
		}

		public GlossariesHelper AssertExistNameErrorDisplay()
		{
			BaseObject.InitPage(_newGlossaryDialog);
			_newGlossaryDialog.AssertExistNameErrorDisplay();

			return this;
		}

		public GlossariesHelper AssertLanguageNotExistInDropdown(Language language, int dropdownNumber)
		{
			BaseObject.InitPage(_newGlossaryDialog);
			_newGlossaryDialog
				.ExpandLanguageDropdown(dropdownNumber)
				.AssertLanguageNotExistInDropdown(language);

			return this;
		}

		public GlossariesHelper OpenNewGlossaryDialog()
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickCreateGlossaryButton();

			return this;
		}

		public GlossariesHelper SelectLanguage(Language language, int dropdownNumber)
		{
			BaseObject.InitPage(_newGlossaryDialog);
			_newGlossaryDialog
				.ExpandLanguageDropdown(dropdownNumber)
				.SelectLanguage(language);

			return this;
		}

		public GlossariesHelper DeleteLanguage(int dropdownNumber = 1)
		{
			BaseObject.InitPage(_newGlossaryDialog);
			_newGlossaryDialog.ClickDeleteLanguageButton(dropdownNumber);

			return this;
		}

		public GlossariesHelper AssertDateModifiedMatchCurrentDate(string glossaryName)
		{
			Logger.Trace("Проверить, что дата изменения глоссария {0} совпадает с текущей датой.", glossaryName);
			BaseObject.InitPage(_glossariesPage);

			DateTime convertModifiedDate = _glossariesPage.GlossaryDateModified(glossaryName);
			TimeSpan result = DateTime.Now - convertModifiedDate;
			var minutesDifference = result.TotalMinutes;

			Assert.IsTrue(minutesDifference < 5,
				"Произошла ошибка:\n дата создания {0} глоссария {1} не совпадет с текущей датой."
				+ "\nРазница во времени в минутах составляет = {2}.",
				convertModifiedDate.ToShortDateString(), glossaryName, minutesDifference);

			return this;
		}

		public GlossariesHelper AssertLanguagesCountChanged(int countBefore, int countAfter)
		{
			Assert.AreNotEqual(countAfter, countBefore, "Произошла ошибка:\n количество языков не изменилось.");

			return this;
		}

		public GlossariesHelper SwitchToGlossaryStructureDialog()
		{
			BaseObject.InitPage(_glossaryPropertiesDialog);
			_glossaryPropertiesDialog.ClickAdvancedButton();

			return this;
		}

		public GlossariesHelper OpenGlossaryProperties()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.ExpandEditGlossaryMenu()
				.ClickGlossaryProperties();

			return this;
		}

		public GlossariesHelper OpenGlossaryStructure()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.ExpandEditGlossaryMenu()
				.ClickGlossaryStructure();

			return this;
		}

		public GlossariesHelper DeleteGlossaryInPropertiesDialog()
		{
			BaseObject.InitPage(_glossaryPropertiesDialog);
			_glossaryPropertiesDialog
				.ClickDeleteGlossaryButton()
				.AssertConfirmDeleteMessageDisplay()
				.ClickConfirmDeleteGlossaryButton();

			return this;
		}

		public GlossariesHelper ClickSaveButtonInPropetiesDialog(bool errorExpected = false)
		{
			BaseObject.InitPage(_glossaryPropertiesDialog);
			if (errorExpected)
			{
				_glossaryPropertiesDialog.ClickSaveButton<GlossaryPropertiesDialog>();
			}
			else
			{
				_glossaryPropertiesDialog
					.ClickSaveButton<GlossaryPage>()
					.AssertDialogBackgroundDisappeared<GlossaryPage>();
			}
			

			return this;
		}

		public int LanguagesCountInPropertiesDialog(out int languagesCountBefore)
		{
			BaseObject.InitPage(_glossaryPropertiesDialog);

			return languagesCountBefore = _glossaryPropertiesDialog.LanguagesCount();
		}

		public GlossariesHelper CancelDeleteLanguageInPropertiesDialog(int languagesNumber = 2)
		{
			BaseObject.InitPage(_glossaryPropertiesDialog);
			_glossaryPropertiesDialog
				.ClickDeleteLanguageButton(languagesNumber)
				.AssertDeleteLanguageWarningDisplay()
				.ClickCancelInDeleteLanguageWarning();

			return this;
		}

		public GlossariesHelper FillGlossaryNameInPropertiesDialog(string glossaryName)
		{
			BaseObject.InitPage(_glossaryPropertiesDialog);
			_glossaryPropertiesDialog.FillGlossaryName(glossaryName);

			return this;
		}

		public GlossariesHelper GoToGlossaryPage(string glossaryName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickGlossaryRow(glossaryName);

			return this;
		}

		public GlossariesHelper AssertDateTimeModifiedNotMatch(DateTime modifiedDateBefore, DateTime modifiedDateAfter)
		{
			Assert.AreNotEqual(modifiedDateBefore, modifiedDateAfter,
				"Произошла ошибка:\n дата изменния глоссария {0} совпадает с {1}", modifiedDateBefore, modifiedDateAfter);

			return this;
		}

		public GlossariesHelper AssertLanguagesCountMatch(int countBefore, int countAfter)
		{
			BaseObject.InitPage(_glossaryPropertiesDialog);

			Assert.AreEqual(countBefore, countAfter,
				"Произошла ошибка:\n количество языков {0} не совпадает с {1} в диалоге свойств глоссария.", countBefore, countAfter);

			return this;
		}

		public GlossariesHelper GetLanguagesCount(out int languagesCount)
		{
			BaseObject.InitPage(_newGlossaryDialog);

			languagesCount = _newGlossaryDialog.GetGlossaryLanguageCount();

			return this;
		}

		public DateTime ModifiedDateTime(string glossaryName)
		{
			return _glossariesPage.GlossaryDateModified(glossaryName);
		}

		public GlossariesHelper AssertExtendModeOpen()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.AssertExtendModeOpen();

			return this;
		}

		public GlossariesHelper ClickNewEntryButton()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickNewEntryButton();

			return this;
		}

		public GlossariesHelper AddNewSystemField(GlossarySystemField systemField)
		{
			BaseObject.InitPage(_glossaryStructureDialog);
			_glossaryStructureDialog
				.SelectSystemField(systemField)
				.ClickAddToListButton()
				.AssertSystemFieldIsAdded(systemField)
				.ClickSaveButton()
				.AssertDialogBackgroundDisappeared<GlossaryPage>();

			return this;
		}

		public GlossariesHelper SelectLevelGlossaryStructure(GlossaryStructureLevel level)
		{
			BaseObject.InitPage(_glossaryStructureDialog);
			_glossaryStructureDialog
				.ExpandLevelDropdown()
				.SelectLevel(level);

			return this;
		}

		public GlossariesHelper AddLanguageFields()
		{
			BaseObject.InitPage(_glossaryStructureDialog);
			_glossaryStructureDialog
				.AddLanguageFields()
				.ClickSaveButton()
				.AssertDialogBackgroundDisappeared<GlossaryPage>();

			return this;
		}

		public GlossariesHelper FillTermInLanguagesAndTermsSection(string text = "text")
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.FillTermInLanguagesAndTermsSection(text);

			return this;
		}

		public GlossariesHelper AssertGlossaryExportedSuccesfully(string pathToFile)
		{
			var timeout = 0;

			while (!File.Exists(pathToFile) && (timeout < 10))
			{
				timeout++;
				Thread.Sleep(1000);
			}

			Assert.IsTrue(File.Exists(pathToFile),
				"Произошла ошибка:\n файл {0} не был скачен за отведенный таймаут {1} сек.", pathToFile, timeout);

			return this;
		}

		public GlossariesHelper ImportGlossary(string pathFile)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickImportButton()
				.ImportGlossary(pathFile)
				.ClickImportButtonInImportDialog()
				.ClickCloseButton();

			return this;
		}

		public GlossariesHelper ImportGlossaryWithReplaceTerms(string pathFile)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.ClickImportButton()
				.ClickReplaceTermsButton()
				.ImportGlossary(pathFile)
				.ClickImportButtonInImportDialog()
				.ClickCloseButton();

			return this;
		}

		public GlossariesHelper AssertGlossaryContainsCorrectTermsCount(int termsCount)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertGlossaryContainsCorrectTermsCount(termsCount);

			return this;
		}

		public GlossariesHelper FillDefinition(string definition)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.OpenLanguageAndTermDetailsEditMode()
				.FillDefinition(definition);

			return this;
		}

		private readonly GlossariesPage _glossariesPage = new GlossariesPage();
		private readonly GlossaryPage _glossaryPage = new GlossaryPage();
		private readonly NewGlossaryDialog _newGlossaryDialog = new NewGlossaryDialog();
		private readonly GlossaryPropertiesDialog _glossaryPropertiesDialog = new GlossaryPropertiesDialog();
		private readonly GlossaryStructureDialog _glossaryStructureDialog = new GlossaryStructureDialog();


	}
}
