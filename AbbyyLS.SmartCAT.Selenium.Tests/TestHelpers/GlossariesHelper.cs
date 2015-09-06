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
			Logger.Trace("Проверить, что имя автора совпадает с {0}.", userName);
			BaseObject.InitPage(_glossariesPage);
			var modifiedBy = _glossariesPage.GetModifiedByAuthor(glossaryName);

			Assert.AreEqual(modifiedBy, userName,
				"Произошла ошибка:\n имя {0} не совпадает с {1}.", modifiedBy, userName);

			return this;
		}

		public GlossariesHelper AssertExtendTermsCountMatch(int expectedTermCount)
		{
			Logger.Trace("Проверить, что количество терминов = {0}.", expectedTermCount);
			BaseObject.InitPage(_glossaryPage);

			Assert.AreEqual(expectedTermCount, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");

			return this;
		}

		public GlossariesHelper AssertSynonymCountMatch(int expectedCount, int termNumber, int columnNumber)
		{
			Logger.Trace("Проверить, что количество синонимов в термине = {0}.", expectedCount);
			BaseObject.InitPage(_glossaryPage);

			Assert.AreEqual(expectedCount, _glossaryPage.SynonymFieldsCount(termNumber, columnNumber),
				"Произошла ошибка:\n неверное количество синонимов в термине №{0} и столбце №{1}", termNumber, columnNumber);

			return this;
		}

		public GlossariesHelper AssertTermsTextMatch(string text)
		{
			Logger.Trace("Проверить, что все термины совпадают с {0}.", text);
			BaseObject.InitPage(_glossaryPage);

			var termsList = _glossaryPage.TermsList();

			foreach (var term in termsList)
			{
				Assert.AreEqual(text, term.Trim(), "Произошла ошибка:\n Термин {0} не соответствует ожидаемому значению {1}.", term.Trim(), text);
			}

			return this;
		}

		public GlossariesHelper AssertSynonumUniqueErrorDisplayed(int columnNumber)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertSynonumUniqueErrorDisplayed(columnNumber);

			return this;
		}

		public GlossariesHelper AssertDefaultTermsCountMatch(int expectedTermCount)
		{
			Logger.Trace("Проверить, что количество терминов равно {0}.", expectedTermCount);
			BaseObject.InitPage(_glossaryPage);

			Assert.AreEqual(expectedTermCount, _glossaryPage.DefaultTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");

			return this;
		}

		public GlossariesHelper AssertAlreadyExistTermErrorDisplayed()
		{
			Logger.Trace("Проверить, что сообщение 'The term already exists' появилось.");
			BaseObject.InitPage(_glossaryPage);
			
			Assert.IsTrue(_glossaryPage.AlreadyExistTermErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'The term already exists' не появилось.");

			return this;
		}

		public GlossariesHelper AssertEmptyTermErrorDisplayed()
		{
			Logger.Trace("Проверить, что сообщение 'Please add at least one term' появилось.");
			BaseObject.InitPage(_glossaryPage);

			Assert.IsTrue(_glossaryPage.EmptyTermErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'Please add at least one term' не появилось.");

			return this;
		}

		public GlossariesHelper AssertTermMatch(string expectedText)
		{
			Logger.Trace("Проверить, что текст в термине не совпадает с {0}.", expectedText);
			BaseObject.InitPage(_glossaryPage);

			Assert.AreEqual(expectedText, _glossaryPage.FirstTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");

			return this;
		}

		public GlossariesHelper DeleteTerm(string source, string target)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.HoverTermRow(source, target)
				.ClickDeleteButton(source, target)
				.AssertDeleteButtonDisappeared(source, target);

			return this;
		}

		public GlossariesHelper EditDefaultTerm(string source, string target, string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.HoverTermRow(source, target)
				.ClickEditButton()
				.FillTerm(1, source + text)
				.FillTerm(2, target + text)
				.ClickSaveTermButton();

			return this;
		}

		public GlossariesHelper EditCustomTerms(string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickEditEntryButton();

			var termsCount = _glossaryPage.TermsCountInLanguagesAndTermsSection();

			for (int i = 1; i <= termsCount; i++)
			{
				_glossaryPage
					.ClickTermInLanguagesAndTermsSection(i)
					.EditTermInLanguagesAndTermsSection(text, i);
			}

			_glossaryPage.ClickSaveEntryButton();

			return this;
		}

		public GlossariesHelper AssertTermDisplayedInLanguagesAndTermsSection(string term)
		{
			BaseObject.InitPage(_glossaryPage);

			Assert.IsTrue(_glossaryPage.AssertTermDisplayedInLanguagesAndTermsSection(term),
				"Произошла ошибка:\n Термин {0} отсутствует в секции 'Languages And Terms'.", term);

			return this;
		}

		public GlossariesHelper SearchTerm(string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.FillSearchField(text)
				.ClickSearchButton();

			return this;
		}
		
		public GlossariesHelper CancelEditTerm()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickCancelButton();

			return this;
		}

		public GlossariesHelper CloseTermsInfo()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.CloseExpandedTerms();

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

		public GlossariesHelper FillTerm(
			string firstTerm = "firstTerm",
			string secondTerm = "secondTerm")
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.FillTerm(1, firstTerm)
				.FillTerm(2, secondTerm);

			return this;
		}

		public GlossariesHelper ClickSaveButton()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickSaveTermButton();

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
			return "TestGlossary" + "-" + Guid.NewGuid();
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

		public GlossariesHelper AddAtLeastOnTermErrorDisplay()
		{
			BaseObject.InitPage(_glossaryPage);

			Assert.IsTrue(_glossaryPage.EmptyTermErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'Please add at least one term' не появилось.");

			return this;
		}

		public GlossariesHelper AssertSpecifyGlossaryNameErrorDisplay()
		{
			BaseObject.InitPage(_newGlossaryDialog);

			Assert.IsTrue(_newGlossaryDialog.SpecifyGlossaryNameErrorDisplay(),
				"Произошла ошибка:\n сообщение 'Specify glossary name' не появилось.");

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

		public GlossariesHelper AssertLanguageColumnCountMatch(int count)
		{
			Logger.Trace("Проверить, что количество колонок с языками = {0}.", count);
			BaseObject.InitPage(_glossaryPage);

			Assert.AreEqual(count, _glossaryPage.LanguageColumnCount(),
				"Произошла ошибка:\n неверное количество колонок с языками.");

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
			_glossaryPage.AssertExtendModeOpen();

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

		public GlossariesHelper FillTermInLanguagesAndTermsSection(string text = "Term Example")
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

		public GlossariesHelper AddSynonym(int columnNumber, string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.ClickSynonymPlusButton(columnNumber)
				.FillSynonym(text, columnNumber);

			return this;
		}

		public GlossariesHelper DeleteTerm(string source)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.DeleteTerm(source);

			return this;
		}

		public GlossariesHelper AssertIsSingleTermWithTranslationExists(string glossaryName, string source , string target)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickGlossaryRow(glossaryName)
				.AssertIsSingleTermWithTranslationExists(source, target);

			return this;
		}

		public GlossariesHelper AssertIsTermWithTranslationAndCommentExists(
			string glossaryName,
			string source,
			string target,
			string comment)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.AssertIsTermWithTranslationAndCommentExists(source, target, comment);

			return this;
		}

		public GlossariesHelper AssertIsSingleTermExists(string glossaryName, string source)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickGlossaryRow(glossaryName)
				.AssertIsSingleTermExists(source);

			return this;
		}

		public GlossariesHelper AssertIsTermWithCommentExists(
			string glossaryName,
			string source,
			string comment)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage
				.ClickGlossaryRow(glossaryName)
				.AssertIsTermWithCommentExists(source, comment);

			return this;
		}

		public GlossariesHelper CheckTermInGlossary(
			string glossaryName,
			string source,
			string target = null,
			string comment = null,
			int termsCount = 1)
		{

			if (target != null)
			{
				AssertIsSingleTermWithTranslationExists(glossaryName, source, target);

				if (comment != null)
				{
					AssertIsTermWithTranslationAndCommentExists(glossaryName, source, target, comment);
				}
			}
			else
			{
				AssertIsSingleTermExists(glossaryName, source);

				if (comment != null)
				{
					AssertIsTermWithCommentExists(glossaryName, source, comment);
				}
			}

			AssertGlossaryContainsCorrectTermsCount(termsCount);

			return new GlossariesHelper();
		}

		private readonly GlossariesPage _glossariesPage = new GlossariesPage();
		private readonly GlossaryPage _glossaryPage = new GlossaryPage();
		private readonly NewGlossaryDialog _newGlossaryDialog = new NewGlossaryDialog();
		private readonly GlossaryPropertiesDialog _glossaryPropertiesDialog = new GlossaryPropertiesDialog();
		private readonly GlossaryStructureDialog _glossaryStructureDialog = new GlossaryStructureDialog();
		private readonly WorkspaceHelper _workspaceHelper = new WorkspaceHelper();
	}
}
