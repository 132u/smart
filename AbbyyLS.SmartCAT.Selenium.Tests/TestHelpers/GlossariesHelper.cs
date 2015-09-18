using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

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
			_glossariesPage.ClickCreateGlossaryButton()
				.OpenClientsList()
				.AssertClientsListOpened()
				.AssertClientExistInList(clientName);

			return this;
		}

		public GlossariesHelper AssertClientNotExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickCreateGlossaryButton()
				.OpenClientsList()
				.AssertClientsListOpened()
				.AssertClientNotExistInList(clientName);

			return this;
		}

		public GlossariesHelper AssertProjectGroupExist(string projectGroupName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickCreateGlossaryButton()
				.ClickProjectGroupsList()
				.AssertProjectGroupsListOpened()
				.AssertProjectGroupExistInList(projectGroupName);

			return this;
		}

		public GlossariesHelper AssertProjectGroupNotExist(string projectGroupName)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickCreateGlossaryButton()
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

		public GlossariesHelper OpenSuggestTermDialogFromGlossariesPage()
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickSuggestTermButton();

			return this;
		}

		public GlossariesHelper ClickSaveButtonInSuggestTermDialogFromGlossaryPage(bool errorExpected = false)
		{
			BaseObject.InitPage(_suggestTermDialog);
			if (!errorExpected)
			{
				_suggestTermDialog
					.ClickSaveButton<GlossaryPage>()
					.AssertDialogBackgroundDisappeared<GlossaryPage>();
			}
			else
			{
				_suggestTermDialog.ClickSaveButton<SuggestTermDialog>();
			}

			return this;
		}

		public GlossariesHelper FillSuggestTermDialog(
			string term1 = "term1",
			string term2 = "term2",
			Language language1 = Language.English,
			Language language2 = Language.Russian,
			string glossary = null)
		{
			BaseObject.InitPage(_suggestTermDialog);
			_suggestTermDialog
				.FillTerm(termNumber: 1, term: term1)
				.FillTerm(termNumber: 2, term: term2)
				.ClickLanguageList(languageNumber: 1)
				.SelectLanguageInList(language1)
				.ClickLanguageList(languageNumber: 2)
				.SelectLanguageInList(language2);

			if (glossary != null)
			{
				BaseObject.InitPage(_suggestTermDialog);
				_suggestTermDialog
					.ClickGlossariesDropdown()
					.SelectGlossariesInDropdown(glossary);
			}

			return this;
		}

		public GlossariesHelper ClickCancelButtonInSuggestedTermDialogFromGlossariesPage()
		{
			BaseObject.InitPage(_suggestTermDialog);
			_suggestTermDialog
				.ClickCancelButton<GlossariesPage>()
				.AssertDialogBackgroundDisappeared<GlossariesPage>();

			return this;
		}

		public GlossariesHelper ClickSaveButtonInSuggestTermDialogFromGlossariesPage(bool errorExpected = false)
		{
			BaseObject.InitPage(_suggestTermDialog);
			if (!errorExpected)
			{
				_suggestTermDialog
					.ClickSaveButton<GlossariesPage>()
					.AssertDialogBackgroundDisappeared<GlossariesPage>();
			}
			else
			{
				_suggestTermDialog
					.ClickSaveButton<SuggestTermDialog>();
			}

			return this;
		}

		public GlossariesHelper ClickSaveTermAnywayInSuggestTermDialogFromGlossaryPage()
		{
			BaseObject.InitPage(_suggestTermDialog);
			_suggestTermDialog
				.ClickSaveTermAnywayButton<GlossaryPage>()
				.AssertDialogBackgroundDisappeared<GlossaryPage>();

			return this;
		}

		public GlossariesHelper AssertLanguageInSuggestTermDialogMatch(int languageNumber, Language language)
		{
			Logger.Trace("Проверить, что указан {0} язык №{1}.", language, languageNumber);
			BaseObject.InitPage(_suggestTermDialog);

			Assert.AreEqual(language.ToString(), _suggestTermDialog.LanguageText(languageNumber),
				"Произошла ошибка:\nНеверный язык №{0} в диалоге предложения термина.");

			return this;
		}

		public GlossariesHelper ClickSaveTermAnywayInSuggestTermDialogFromGlossariesPage()
		{
			BaseObject.InitPage(_suggestTermDialog);
			_suggestTermDialog
				.ClickSaveTermAnywayButton<GlossariesPage>()
				.AssertDialogBackgroundDisappeared<GlossariesPage>();

			return this;
		}

		public GlossariesHelper SelectLanguageToSuggestTerm(int languageNumber, Language language)
		{
			BaseObject.InitPage(_suggestTermDialog);
			_suggestTermDialog
				.ClickLanguageList(languageNumber)
				.SelectLanguageInList(language);

			return this;
		}

		public GlossariesHelper ClickCancelButtonInSuggestedTermDialogFromGlossaryPage()
		{
			BaseObject.InitPage(_suggestTermDialog);
			_suggestTermDialog
				.ClickCancelButton<GlossaryPage>()
				.AssertDialogBackgroundDisappeared<GlossaryPage>();

			return this;
		}

		public GlossariesHelper OpenSuggestTermDialogFromGlossaryPage()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickSuggestTermButton();

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

			Assert.AreEqual(modifiedBy, userName, "Произошла ошибка:\n имя {0} не совпадает с {1}.", modifiedBy, userName);

			return this;
		}

		public GlossariesHelper AssertExtendTermsCountMatch(int expectedTermCount)
		{
			Logger.Trace("Проверить, что количество терминов = {0}.", expectedTermCount);
			BaseObject.InitPage(_glossaryPage);

			Assert.AreEqual(
				expectedTermCount,
				_glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");

			return this;
		}

		public GlossariesHelper AssertSynonymCountMatch(int expectedCount, int termNumber, int columnNumber)
		{
			Logger.Trace("Проверить, что количество синонимов в термине = {0}.", expectedCount);
			BaseObject.InitPage(_glossaryPage);

			Assert.AreEqual(
				expectedCount,
				_glossaryPage.SynonymFieldsCount(termNumber, columnNumber),
				"Произошла ошибка:\n неверное количество синонимов в термине №{0} и столбце №{1}",
				termNumber,
				columnNumber);

			return this;
		}

		public GlossariesHelper AssertTermsTextMatch(string text)
		{
			Logger.Trace("Проверить, что все термины совпадают с {0}.", text);
			BaseObject.InitPage(_glossaryPage);

			var termsList = _glossaryPage.TermsList();

			foreach (var term in termsList)
			{
				Assert.AreEqual(
					text,
					term.Trim(),
					"Произошла ошибка:\n Термин {0} не соответствует ожидаемому значению {1}.",
					term.Trim(),
					text);
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

			Assert.AreEqual(
				expectedTermCount,
				_glossaryPage.DefaultTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");

			return this;
		}

		public GlossariesHelper AssertAlreadyExistTermErrorDisplayed()
		{
			Logger.Trace("Проверить, что сообщение 'The term already exists' появилось.");
			BaseObject.InitPage(_glossaryPage);

			Assert.IsTrue(
				_glossaryPage.AlreadyExistTermErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'The term already exists' не появилось.");

			return this;
		}
		

		public GlossariesHelper AssertTermMatch(string expectedText)
		{
			Logger.Trace("Проверить, что текст в термине совпадает с {0}.", expectedText);
			BaseObject.InitPage(_glossaryPage);

			Assert.AreEqual(
				expectedText,
				_glossaryPage.FirstTermText(),
				"Произошла ошибка:\n текст в термине не совпадает с ожидаемым.");

			return this;
		}

		public GlossariesHelper DeleteTerm(string source, string target)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.HoverTermRow(source, target)
				.ClickDeleteButton(source, target)
				.AssertDeleteButtonDisappeared(source, target);

			return this;
		}

		public GlossariesHelper EditDefaultTerm(string source, string target, string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.HoverTermRow(source, target)
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
				_glossaryPage.ClickTermInLanguagesAndTermsSection(i)
					.EditTermInLanguagesAndTermsSection(text, i);
			}

			_glossaryPage.ClickSaveEntryButton();

			return this;
		}

		public GlossariesHelper AssertTermDisplayedInLanguagesAndTermsSection(string term)
		{
			BaseObject.InitPage(_glossaryPage);

			Assert.IsTrue(
				_glossaryPage.AssertTermDisplayedInLanguagesAndTermsSection(term),
				"Произошла ошибка:\n Термин {0} отсутствует в секции 'Languages And Terms'.",
				term);

			return this;
		}

		public GlossariesHelper SearchTerm(string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.FillSearchField(text)
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
			_glossaryPage.OpenLanguageAndTermDetailsEditMode()
				.FillLanguageComment(comment);

			return this;
		}

		public GlossariesHelper FillDefinitionSource(string definitionSource)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.OpenLanguageAndTermDetailsEditMode()
				.FillDefinitionSource(definitionSource);

			return this;
		}

		public GlossariesHelper ClickSaveEntryButton()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickSaveEntryButton();

			return this;
		}

		public GlossariesHelper FillField(string fieldName, string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.FillField(fieldName, text);

			return this;
		}

		public GlossariesHelper FillNumberField(string fieldName, string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.FillNumberCustomField(fieldName, text);

			return this;
		}

		public GlossariesHelper ClickYesNoCheckBox()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickYesNoCheckbox();

			return this;
		}

		public GlossariesHelper AssertYesNoCheckboxChecked(string yesNo, string fieldName)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertYesNoCheckboxChecked(yesNo, fieldName);

			return this;
		}

		public GlossariesHelper FillDateField(string fieldName)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.OpenCalendar(fieldName)
				.ClickTodayInCalendar();

			return this;
		}

		public GlossariesHelper UploadImage(string fieldName, string imageFile)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickImageField(fieldName);

			uploadFile(imageFile);

			return this;
		}

		public GlossariesHelper UploadMediaFile(string fieldName, string mediaFile)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickAddMediaButton(fieldName);

			uploadFile(mediaFile);

			_glossaryPage.AssertProgressUploadDissapeared(fieldName);

			return this;
		}

		public GlossariesHelper AssertImageFieldFilled(string fieldName)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertImageFieldFilled(fieldName);

			return this;
		}

		public GlossariesHelper AssertMediaFieldFilled(string fieldName, string fileName)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertMediaFileMatch(fileName, fieldName);

			return this;
		}

		public GlossariesHelper AssertSystemTextAreaFieldDisplayed(GlossarySystemField fieldName)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertSystemTextAreaFieldDisplayed(fieldName);

			return this;
		}

		public GlossariesHelper AssertSystemDropdownFieldDisplayed(GlossarySystemField fieldName)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertSystemDropdownFieldDisplayed(fieldName);

			return this;
		}


		public GlossariesHelper FillSystemField(GlossarySystemField systemField, string value)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.FillSystemField(systemField, value);

			return this;
		}

		public GlossariesHelper AssertGeneralFieldValueMatch(GlossarySystemField fieldName, string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertFieldValueMatch(fieldName, text);

			return this;
		}

		public GlossariesHelper AssertCustomFieldValueMatch(string fieldName, string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertCustomFieldValueMatch(fieldName, text);

			return this;
		}

		public GlossariesHelper AssertLanguageCommentIsFilled(string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.OpenLanguageAndTermDetailsViewMode()
				.AssertCommentIsFilled(text);

			return this;
		}

		public GlossariesHelper AssertDefinitionFilled(string definition)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.OpenLanguageAndTermDetailsViewMode()
				.AssertDefinitionIsFilled(definition);

			return this;
		}

		public GlossariesHelper AssertDefinitionSourceFilled(string definition)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.OpenLanguageAndTermDetailsViewMode()
				.AssertDefinitionSourceIsFilled(definition);

			return this;
		}

		public GlossariesHelper CreateTerm(string firstTerm = "firstTerm", string secondTerm = "secondTerm")
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickNewEntryButton()
				.FillTerm(1, firstTerm)
				.FillTerm(2, secondTerm)
				.ClickSaveTermButton();

			return this;
		}

		public GlossariesHelper FillTerm(string firstTerm = "firstTerm", string secondTerm = "secondTerm")
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.FillTerm(1, firstTerm)
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
			_glossariesPage.ClickCreateGlossaryButton()
				.FillGlossaryName(glossaryName)
				.FillComment(comment);

			BaseObject.InitPage(_newGlossaryDialog);
			if (languageList != null && languageList.Count > 0)
			{
				_newGlossaryDialog.ClickDeleteLanguageButton()
					.ExpandLanguageDropdown(1)
					.SelectLanguage(languageList[0]);

				for (var i = 2; i <= languageList.Count; i++)
				{
					_newGlossaryDialog.ClickAddButton()
						.ExpandLanguageDropdown(i)
						.SelectLanguage(languageList[i - 1]);
				}
			}

			if (projectGroupName != null)
			{
				_newGlossaryDialog.ClickProjectGroupsList()
					.SelectProjectGroup(projectGroupName)
					.ClickProjectGroupsList();
			}

			if (errorExpected)
			{
				_newGlossaryDialog.ClickSaveGlossaryButton<NewGlossaryDialog>();
			}
			else
			{
				_newGlossaryDialog.ClickSaveGlossaryButton<GlossaryPage>()
					.AssertDialogBackgroundDisappeared<GlossaryPage>();
			}

			return this;
		}

		public GlossariesHelper AddAtLeastOnTermErrorDisplay()
		{
			BaseObject.InitPage(_glossaryPage);

			Assert.IsTrue(
				_glossaryPage.EmptyTermErrorDisplayed(),
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
			_newGlossaryDialog.ExpandLanguageDropdown(dropdownNumber)
				.AssertLanguageNotExistInDropdown(language);

			return this;
		}

		public GlossariesHelper AssertLanguageColumnCountMatch(int count)
		{
			Logger.Trace("Проверить, что количество колонок с языками = {0}.", count);
			BaseObject.InitPage(_glossaryPage);

			Assert.AreEqual(
				count,
				_glossaryPage.LanguageColumnCount(),
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
			_newGlossaryDialog.ExpandLanguageDropdown(dropdownNumber)
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

			Assert.IsTrue(
				minutesDifference < 5,
				"Произошла ошибка:\n дата создания {0} глоссария {1} не совпадет с текущей датой."
				+ "\nРазница во времени в минутах составляет = {2}.",
				convertModifiedDate.ToShortDateString(),
				glossaryName,
				minutesDifference);

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
			_glossaryPage.ExpandEditGlossaryMenu()
				.ClickGlossaryProperties();

			return this;
		}

		public GlossariesHelper OpenGlossaryStructure()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ExpandEditGlossaryMenu()
				.ClickGlossaryStructure();

			return this;
		}

		public GlossariesHelper DeleteGlossaryInPropertiesDialog()
		{
			BaseObject.InitPage(_glossaryPropertiesDialog);
			_glossaryPropertiesDialog.ClickDeleteGlossaryButton()
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
				_glossaryPropertiesDialog.ClickSaveButton<GlossaryPage>()
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
			_glossaryPropertiesDialog.ClickDeleteLanguageButton(languagesNumber)
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
			Assert.AreNotEqual(
				modifiedDateBefore,
				modifiedDateAfter,
				"Произошла ошибка:\n неверная дата изменния глоссария.");

			return this;
		}

		public GlossariesHelper AssertLanguagesCountMatch(int countBefore, int countAfter)
		{
			BaseObject.InitPage(_glossaryPropertiesDialog);

			Assert.AreEqual(
				countBefore,
				countAfter,
				"Произошла ошибка:\n количество языков {0} не совпадает с {1} в диалоге свойств глоссария.",
				countBefore,
				countAfter);

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
			_glossaryStructureDialog.SelectSystemField(systemField)
				.ClickAddToListButton()
				.AssertSystemFieldIsAdded(systemField)
				.ClickSaveButton()
				.AssertDialogBackgroundDisappeared<GlossaryPage>();

			return this;
		}

		public GlossariesHelper SelectLevelGlossaryStructure(GlossaryStructureLevel level)
		{
			BaseObject.InitPage(_glossaryStructureDialog);
			_glossaryStructureDialog.ExpandLevelDropdown()
				.SelectLevel(level);

			return this;
		}

		public GlossariesHelper AddLanguageFields()
		{
			BaseObject.InitPage(_glossaryStructureDialog);
			_glossaryStructureDialog.AddLanguageFields()
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

			Assert.IsTrue(
				File.Exists(pathToFile),
				"Произошла ошибка:\n файл {0} не был скачен за отведенный таймаут {1} сек.",
				pathToFile,
				timeout);

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
			_glossaryPage.ClickImportButton()
				.ClickReplaceTermsButton()
				.ImportGlossary(pathFile)
				.ClickImportButtonInImportDialog()
				.ClickCloseButton();

			return this;
		}

		public GlossariesHelper AssertSynonymsMatch(List<string> synonyms, int columnNumber = 1)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertSynonymsMatch(columnNumber, synonyms);

			return this;
		}

		public GlossariesHelper AssertGlossaryContainsCorrectTermsCount(int termsCount)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertGlossaryContainsCorrectTermsCount(termsCount);

			return this;
		}

		public GlossariesHelper SelectGlossaryInSuggestedTermsPageForAllGlossaries(string glossaryName)
		{
			BaseObject.InitPage(_suggestedTermsPageForAllGlossaries);
			_suggestedTermsPageForAllGlossaries
				.ClickGlossariesDropdown()
				.SelectGlossariesInDropdown(glossaryName);

			return this;
		}

		public GlossariesHelper SelectGlossaryInSuggestedTermsPageForCurrentGlossary(string glossaryName)
		{
			BaseObject.InitPage(_suggestedTermsPageForCurrentGlossaries);
			_suggestedTermsPageForCurrentGlossaries
				.ClickGlossariesDropdown()
				.SelectGlossariesInDropdown(glossaryName);

			return this;
		}

		public GlossariesHelper FillDefinition(string definition)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.OpenLanguageAndTermDetailsEditMode()
				.FillDefinition(definition);

			return this;
		}

		public GlossariesHelper AddSynonym(int columnNumber, string text)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickSynonymPlusButton(columnNumber)
				.FillSynonym(text, columnNumber);

			return this;
		}

		public GlossariesHelper DeleteTerm(string source)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.DeleteTerm(source);

			return this;
		}

		public GlossariesHelper AssertIsSingleTermWithTranslationExists(string glossaryName, string source, string target)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickGlossaryRow(glossaryName)
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
			_glossariesPage.ClickGlossaryRow(glossaryName)
				.AssertIsSingleTermExists(source);

			return this;
		}

		public GlossariesHelper AssertIsTermWithCommentExists(string glossaryName, string source, string comment)
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickGlossaryRow(glossaryName)
				.AssertIsTermWithCommentExists(source, comment);

			return this;
		}

		public GlossariesHelper AssertSuggestedTermsButtonNotExist(bool glossariesPage)
		{
			if (glossariesPage)
			{
				BaseObject.InitPage(_glossariesPage);
				_glossariesPage.AssertSuggestedTermsButtonNotExist<GlossariesPage>();
			}
			else
			{
				BaseObject.InitPage(_glossaryPage);
				_glossaryPage.AssertSuggestedTermsButtonNotExist<GlossaryPage>();
			}
			return this;
		}

		public GlossariesHelper AssertSuggestTermButtonNotExist(bool glossariesPage)
		{
			if (glossariesPage)
			{
				BaseObject.InitPage(_glossariesPage);
				_glossariesPage.AssertSuggestTermButtonNotExist<GlossariesPage>();
			}
			else
			{
				BaseObject.InitPage(_glossaryPage);
				_glossaryPage.AssertSuggestTermButtonNotExist<GlossaryPage>();
			}

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

		public GlossariesHelper AssertImageFieldExistInNewEntry(string fieldName)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertImageFieldExistInNewEntry(fieldName);

			return this;
		}

		public GlossariesHelper AssertMediaFieldExistInNewEntry(string fieldName)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertMediaFieldExistInNewEntry(fieldName);

			return this;
		}

		public GlossariesHelper AddCustomField(
			string fieldName,
			GlossaryCustomFieldType type,
			bool isRequiredField = false,
			string defaultValue = null,
			List<string> itemsList = null,
			bool multi = false)
		{
			BaseObject.InitPage(_glossaryStructureDialog);
			_glossaryStructureDialog.SwitchToCustomFieldsTab()
				.ExpandCustomFieldType()
				.SelectCustomFieldType(type)
				.FillCustomFieldName(fieldName);

			if (itemsList != null)
			{
				var items = string.Join(";", itemsList);
				_glossaryStructureDialog.FillItemsList(items);
			}

			if (isRequiredField)
			{
				_glossaryStructureDialog.ClickRequiredCheckbox();
			}

			if (defaultValue != null)
			{
				_glossaryStructureDialog.FillDefaultValue(defaultValue);
			}

			_glossaryStructureDialog.ClickAddCustoFieldButton()
				.ClickSaveButton()
				.AssertDialogBackgroundDisappeared<GlossaryPage>();

			return this;
		}

		public GlossariesHelper AssertFieldExistInNewEntry(string fieldName)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertFieldExistInNewEntry(fieldName);

			return this;
		}

		public GlossariesHelper SelectItemInListDropdown(string fieldName, string item)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ExpandItemsListDropdown(fieldName)
				.SelectItemInListDropdown(item);

			return this;
		}

		public GlossariesHelper SelectItemInMultiSelectListDropdown(string fieldName, string item)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickMultiselectListDropdown(fieldName)
				.SelectItemInMultiselectListDropdown(item)
				.ClickMultiselectListDropdown(fieldName);

			return this;
		}

		public GlossariesHelper AssertCustomDefaultValueMatch(string fieldName, string defaultValue)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertCustomDefaultValueMatch(fieldName, defaultValue);

			return this;
		}

		public GlossariesHelper AssertFieldErrorDisplayed(string fieldName)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertFieldErrorDisplayed(fieldName);

			return this;
		}

		public GlossariesHelper AssertCustomImageFieldErrorDisplayed(string fieldName)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.AssertImageFieldErrorDisplayed(fieldName);

			return this;
		}

		public GlossariesHelper AddAllSystemFields()
		{
			Logger.Trace("Добавить все системные поля в диалоге изменения структуры глоссария.");
			BaseObject.InitPage(_glossaryStructureDialog);
			var fieldnames = _glossaryStructureDialog.SystemFieldNames();

			foreach (var field in fieldnames)
			{
				field.Click();
				_glossaryStructureDialog.ClickAddSystemFieldButton();
			}

			_glossaryStructureDialog
				.ClickSaveButton()
				.AssertDialogBackgroundDisappeared<GlossaryPage>();

			return this;
		}

		public GlossariesHelper SelectOptionInTopic(string option)
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage
				.ExpandTopicDropdown()
				.ClickOptionInTopicDropdown(option);

			return this;
		}

		private void uploadFile(string imageFile)
		{
			Logger.Trace("Загрузка файла {0}.\nВвести путь к файлу в системное окно.", imageFile);
			Thread.Sleep(3000); // слип необходим, так как не всегда успевает открыться окно загрузки

			var txt = Regex.Replace(imageFile, "[+^%~()]", "{$0}");
			SendKeys.SendWait(txt);
			Thread.Sleep(2000);
			SendKeys.SendWait(@"{Enter}");
			Thread.Sleep(2000);
		}

		public GlossariesHelper SuggestedTermsByGlossaryCountMatch(int suggestedTermsCount, string glossary = "")
		{
			BaseObject.InitPage(_suggestedTermsPageForAllGlossaries);

			Assert.AreEqual(suggestedTermsCount, _suggestedTermsPageForAllGlossaries.TermsByGlossaryNameCount(glossary),
				"Произошла Ошибка:\n Неверное количество терминов.");

			return this;
		}

		public GlossariesHelper GoToSuggestedTermsPageFromGlossariesPage()
		{
			BaseObject.InitPage(_glossariesPage);
			_glossariesPage.ClickSuggestedTermsButton();

			return this;
		}

		public GlossariesHelper GoToSuggestedTermsPageFromGlossaryPage()
		{
			BaseObject.InitPage(_glossaryPage);
			_glossaryPage.ClickSuggestedTermsButton();

			return this;
		}

		public GlossariesHelper AssertSuggestedTermValueMatch(string term, int rowNumber, int columnNumber)
		{
			BaseObject.InitPage(_suggestedTermsPageForCurrentGlossaries);
			_suggestedTermsPageForCurrentGlossaries.AssertTermValueMatch(term, rowNumber, columnNumber);

			return this;
		}

		public int SuggestedTermsCountForGlossary(string glossaryName = "")
		{
			BaseObject.InitPage(_suggestedTermsPageForAllGlossaries);

			return _suggestedTermsPageForAllGlossaries.TermRowNumberByGlossaryName(glossaryName);
		}

		public GlossariesHelper AcceptSuggestTermInSuggestedTermsPageForAllGlossaries(bool chooseGlossary = false, string glossaryName = "")
		{
			BaseObject.InitPage(_suggestedTermsPageForAllGlossaries);
			var termRowNumber = _suggestedTermsPageForAllGlossaries.TermRowNumberByGlossaryName(glossaryName);
			var termsCountBeforeAccept = _suggestedTermsPageForAllGlossaries.SuggestedTermsCount();

			_suggestedTermsPageForAllGlossaries
				.HoverSuggestedTermRow(termRowNumber)
				.ClickAcceptSuggestButton(termRowNumber);

			// Sleep не убирать, иначе термин не исчезнет
			Thread.Sleep(1000);

			if (!chooseGlossary)
			{
				var termsCountAfterAccept = _suggestedTermsPageForAllGlossaries.SuggestedTermsCount();

				Assert.AreEqual(termsCountBeforeAccept - termsCountAfterAccept, 1,
					"Произошла ошибка:\nПодтвержденный предложенный термин не исчез из списка.");
			}
			else
			{
				_suggestedTermsPageForAllGlossaries.AssertSelectGlossaryDialogDisplayed();
			}

			return this;
		}

		public GlossariesHelper DeleteSuggestTermInSuggestedTermsPageForAllGlossaries(string glossaryName = "")
		{
			BaseObject.InitPage(_suggestedTermsPageForAllGlossaries);
			var termRowNumber = _suggestedTermsPageForAllGlossaries.TermRowNumberByGlossaryName(glossaryName);
			var termsCountBeforeDelete = _suggestedTermsPageForAllGlossaries.SuggestedTermsCount();

			_suggestedTermsPageForAllGlossaries
				.HoverSuggestedTermRow(termRowNumber)
				.ClickDeleteSuggestTermButton(termRowNumber);

			// Sleep не убирать, иначе термин не исчезнет
			Thread.Sleep(1000);

			var termsCountAfterDelete = _suggestedTermsPageForAllGlossaries.SuggestedTermsCount();

			Assert.AreEqual(termsCountBeforeDelete - termsCountAfterDelete, 1,
				"Произошла ошибка:\nПодтвержденный предложенный термин не исчез из списка.");
			
			return this;
		}


		public GlossariesHelper AcceptSuggestTermInSuggestedTermsPageForCurrentGlossary(int termRowNumber)
		{
			BaseObject.InitPage(_suggestedTermsPageForCurrentGlossaries);
			var termsCountBeforeAccept = _suggestedTermsPageForCurrentGlossaries.SuggestedTermsCount();

			_suggestedTermsPageForCurrentGlossaries
				.HoverSuggestedTermRow(termRowNumber)
				.ClickAcceptSuggestButton(termRowNumber);

			// Sleep не убирать, иначе термин не исчезнет
			Thread.Sleep(1000);

			var termsCountAfterAccept = _suggestedTermsPageForCurrentGlossaries.SuggestedTermsCount();

			Assert.AreEqual(termsCountBeforeAccept - termsCountAfterAccept, 1,
				"Произошла ошибка:\nПодтвержденный предложенный термин не исчез из списка.");

			return this;
		}

		public GlossariesHelper DeleteSuggestTermInSuggestedTermsPageForCurrentGlossary(int termRowNumber)
		{
			BaseObject.InitPage(_suggestedTermsPageForCurrentGlossaries);
			var termsCountBeforeDelete = _suggestedTermsPageForCurrentGlossaries.SuggestedTermsCount();

			_suggestedTermsPageForCurrentGlossaries
				.HoverSuggestedTermRow(termRowNumber)
				.ClickDeleteSuggestTermButton(termRowNumber);

			// Sleep не убирать, иначе термин не исчезнет
			Thread.Sleep(1000);

			var termsCountAfterDelete = _suggestedTermsPageForCurrentGlossaries.SuggestedTermsCount();

			Assert.AreEqual(termsCountBeforeDelete - termsCountAfterDelete, 1,
				"Произошла ошибка:\nПодтвержденный предложенный термин не исчез из списка.");

			return this;
		}

		public GlossariesHelper EditSuggestTermInSuggestedTermsPageForCurrentGlossary(int termRowNumber, string termValue)
		{
			BaseObject.InitPage(_suggestedTermsPageForCurrentGlossaries);

			_suggestedTermsPageForCurrentGlossaries
				.HoverSuggestedTermRow(termRowNumber)
				.ClickEditSuggestTermButton(termRowNumber)
				.AssertEditFormDisplayed()
				.FillSuggestedTermInEditMode(termNumber: 1, termValue: termValue)
				.FillSuggestedTermInEditMode(termNumber: 2, termValue: termValue)
				.ClickAcceptTermButtonInEditMode()
				.AssertEditFormDisappeared();

			return this;
		}

		public GlossariesHelper AddSynonimInSuggestedTermsPageForCurrentGlossary(int termRowNumber, int addButtonNumber, string synonymValue)
		{
			BaseObject.InitPage(_suggestedTermsPageForCurrentGlossaries);

			_suggestedTermsPageForCurrentGlossaries
				.HoverSuggestedTermRow(termRowNumber)
				.ClickEditSuggestTermButton(termRowNumber)
				.AssertEditFormDisplayed()
				.ClickAddSynonymButton(addButtonNumber)
				.FillSuggestedTermInEditMode(addButtonNumber, synonymValue)
				.ClickAcceptTermButtonInEditMode()
				.AssertEditFormDisappeared();

			return this;
		}

		public GlossariesHelper EditSuggestTermInSuggestedTermsPageForAllGlossaries(
			string glossaryName,
			string termValue,
			bool chooseGlossary = false,
			string glossaryToChoose = null)
		{
			BaseObject.InitPage(_suggestedTermsPageForAllGlossaries);
			var termRowNumber = _suggestedTermsPageForAllGlossaries.TermRowNumberByGlossaryName(glossaryName);

			_suggestedTermsPageForAllGlossaries
				.HoverSuggestedTermRow(termRowNumber)
				.ClickEditSuggestTermButton(termRowNumber);

			if (chooseGlossary && glossaryToChoose != null)
			{
				_suggestedTermsPageForAllGlossaries
					.AssertSelectDialogDisplayed()
					.ClickSelectGlossaryDropdownInSelectDialog()
					.SelectGlossaryInSelectDialog(glossaryToChoose)
					.ClickOkButton()
					.AssertDialogBackgroundDisappeared<SuggestedTermsPageForAllGlossaries>();
			}

			_suggestedTermsPageForAllGlossaries
				.AssertEditFormDisplayed()
				.FillSuggestedTermInEditMode(termNumber: 1, termValue: termValue)
				.FillSuggestedTermInEditMode(termNumber: 2, termValue: termValue)
				.ClickAcceptTermButtonInEditMode()
				.AssertEditFormDisappeared();

			return this;
		}

		public GlossariesHelper AssertEmptyTermErrorDisplayed()
		{
			BaseObject.InitPage(_suggestTermDialog);
			_suggestTermDialog.AssertEmptyTermErrorDisplayed();

			return this;
		}

		public GlossariesHelper AssertSuggestedTermsCountMatch(int expectedTermCount)
		{
			BaseObject.InitPage(_suggestedTermsPageForCurrentGlossaries);

			Assert.AreEqual(expectedTermCount, _suggestedTermsPageForCurrentGlossaries.SuggestedTermsCount(),
				"Произошла ошибка:\nНеверное количество терминов.");

			return this;
		}

		public GlossariesHelper AssertDublicateErrorDisplayed()
		{
			BaseObject.InitPage(_suggestTermDialog);
			_suggestTermDialog.AssertDublicateErrorDisplayed();

			return this;
		}

		public GlossariesHelper SelectGlossaryForSuggestedTerm(string glossaryName)
		{
			BaseObject.InitPage(_suggestedTermsPageForAllGlossaries);
			_suggestedTermsPageForAllGlossaries
				.ClickSelectGlossaryDropdownInSelectDialog()
				.SelectGlossaryInSelectDialog(glossaryName)
				.ClickOkButton()
				.AssertDialogBackgroundDisappeared<SuggestedTermsPageForAllGlossaries>();

			return this;
		}
		
		public int SuggestedTermsByGlossaryCount(string glossary = "")
		{
			return _suggestedTermsPageForAllGlossaries.TermsByGlossaryNameCount(glossary);
		}

		private readonly SuggestedTermsPageForCurrentGlossaries _suggestedTermsPageForCurrentGlossaries = new SuggestedTermsPageForCurrentGlossaries();
		private readonly SuggestTermDialog _suggestTermDialog = new SuggestTermDialog();
		private readonly SuggestedTermsPageForAllGlossaries _suggestedTermsPageForAllGlossaries = new SuggestedTermsPageForAllGlossaries();
		private readonly GlossariesPage _glossariesPage = new GlossariesPage();

		private readonly GlossaryPage _glossaryPage = new GlossaryPage();

		private readonly NewGlossaryDialog _newGlossaryDialog = new NewGlossaryDialog();

		private readonly GlossaryPropertiesDialog _glossaryPropertiesDialog = new GlossaryPropertiesDialog();

		private readonly GlossaryStructureDialog _glossaryStructureDialog = new GlossaryStructureDialog();

		private readonly WorkspaceHelper _workspaceHelper = new WorkspaceHelper();
	}
}