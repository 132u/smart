using System;
using System.ComponentModel;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class TranslationMemoriesHelper : WorkspaceHelper
	{
		public TranslationMemoriesHelper CreateTranslationMemory(
			string translationMemoryName,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian,
			Language secondTargetLanguage = Language.NoLanguage,
			string importFilePath = null,
			DialogButtonType finalButtonType = DialogButtonType.Save,
			bool isCreationErrorExpected = false)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.SetTranslationMemoryName(translationMemoryName)
				.ClickOpenSourceLanguageList()
				.SelectSourceLanguage(sourceLanguage);

			BaseObject.InitPage(_newTranslationMemoryDialog);

			if (targetLanguage != Language.NoLanguage)
			{
				_newTranslationMemoryDialog
					.ClickTargetLanguageList()
					.SelectTargetLanguage(targetLanguage)
					.ClickTargetLanguageList();
			}

			if (secondTargetLanguage != Language.NoLanguage)
			{
				_newTranslationMemoryDialog
					.ClickTargetLanguageList()
					.SelectTargetLanguage(secondTargetLanguage)
					.ClickTargetLanguageList();
			}

			if (importFilePath != null)
			{
				_newTranslationMemoryDialog.UploadFile(importFilePath);
			}

			switch (finalButtonType)
			{
				case DialogButtonType.Save:
					if (isCreationErrorExpected)
					{
						_newTranslationMemoryDialog.ClickSaveTranslationMemory<NewTranslationMemoryDialog>();
					}
					else
					{
						_newTranslationMemoryDialog
							.ClickSaveTranslationMemory<TranslationMemoriesPage>()
							.AssertNewTranslationMemoryDialogDisappear()
							.AssertDialogBackgroundDissapeared<TranslationMemoriesPage>();
					}

					break;
				case DialogButtonType.Cancel:
					_newTranslationMemoryDialog
						.ClickCancelTranslationMemoryCreation()
						.AssertNewTranslationMemoryDialogDisappear()
						.AssertDialogBackgroundDissapeared<TranslationMemoriesPage>();
					break;
				case DialogButtonType.None:
					break;
				default:
					throw new InvalidEnumArgumentException();
			}

			return this;
		}

		public TranslationMemoriesHelper CreateTranslationMemoryIfNotExist(
			string translationMemoryName,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian,
			Language secondTargetLanguage = Language.NoLanguage,
			string importFilePath = null,
			DialogButtonType finalButtonType = DialogButtonType.Save,
			bool isCreationErrorExpected = false)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.FillSearch(translationMemoryName)
				.ClickSearchButton();

			if (!_translationMemoriesPage.TranslationMemoryExists(translationMemoryName))
			{
				CreateTranslationMemory(
					translationMemoryName,
					sourceLanguage,
					targetLanguage,
					secondTargetLanguage,
					importFilePath,
					finalButtonType,
					isCreationErrorExpected);
			}

			return this;
		}
		
		public TranslationMemoriesHelper AssertClientExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.ClickOpenClientsList()
				.AssertClientsListDisplayed()
				.AssertClientExistInTmCreationDialog(clientName);

			return this;
		}

		public TranslationMemoriesHelper AssertClientNotExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.ClickOpenClientsList()
				.AssertClientsListDisplayed()
				.AssertClientNotExistInTmCreationDialog(clientName);

			return this;
		}

		public TranslationMemoriesHelper AssertProjectGroupExist(string projectGroup)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.OpenProjectGroupsList()
				.AssertProjectGroupListDisplayed()
				.AssertProjectGroupExist(projectGroup);

			return this;
		}

		public TranslationMemoriesHelper AssertProjectGroupNotExist(string projectGroup)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.OpenProjectGroupsList()
				.AssertProjectGroupListDisplayed()
				.AssertProjectGroupNotExist(projectGroup);

			return this;
		}

		public TranslationMemoriesHelper AssertTranslationMemoryExist(string tmName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.AssertTranslationMemoryExist(tmName);

			return this;
		}

		public TranslationMemoriesHelper AssertTranslationMemoryNotExist(string tmName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.AssertTranslationMemoryNotExist(tmName);

			return this;
		}

		public TranslationMemoriesHelper AssertExistNameErrorAppear()
		{
			BaseObject.InitPage(_newTranslationMemoryDialog);
			_newTranslationMemoryDialog.AssertExistNameErrorAppear();

			return this;
		}

		public TranslationMemoriesHelper AssertNoNameErrorAppear()
		{
			BaseObject.InitPage(_newTranslationMemoryDialog);
			_newTranslationMemoryDialog.AssertNoNameErrorAppear();

			return this;
		}

		public TranslationMemoriesHelper AssertNoTargetErrorAppear()
		{
			BaseObject.InitPage(_newTranslationMemoryDialog);
			_newTranslationMemoryDialog.AssertNoTargetErrorAppear();

			return this;
		}

		public TranslationMemoriesHelper AssertNotTmxFileErrorAppear()
		{
			BaseObject.InitPage(_newTranslationMemoryDialog);
			_newTranslationMemoryDialog.AssertNotTmxFileErrorAppear();

			return this;
		}

		public TranslationMemoriesHelper GetTranslationMemoryUniqueName(ref string baseTranslationMemoryName)
		{
			baseTranslationMemoryName = baseTranslationMemoryName + DateTime.UtcNow.Ticks;

			return this;
		}

		public TranslationMemoriesHelper OpenTranslationMemoryInformation(string translationMemoryName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.ClickTranslationMemoryRow(translationMemoryName);

			return this;
		}

		public TranslationMemoriesHelper DeleteTranslationMemory(string translationMemoryName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickDeleteButtonInTMInfo()
				.AssertDeleteConfirmatonDialogPresent()
				.ClickDeleteButtonInConfirmationDialog()
				.AssertDeleteConfirmatonDialogDisappear();

			return this;
		}

		public static string GetTranslationMemoryUniqueName()
		{
			return "TM_" + DateTime.UtcNow.Ticks;
		}

		public TranslationMemoriesHelper ClickSortByTMName()
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.ClickSortByTMName();

			return this;
		}

		public TranslationMemoriesHelper ClickSortByLanguages()
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.ClickSortByLanguages();

			return this;
		}

		public TranslationMemoriesHelper ClickSortByAuthor()
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.ClickSortByAuthor();

			return this;
		}

		public TranslationMemoriesHelper ClickSortByCreationDate()
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.ClickSortByCreationDate();

			return this;
		}

		private readonly TranslationMemoriesPage _translationMemoriesPage = new TranslationMemoriesPage();
		private readonly NewTranslationMemoryDialog _newTranslationMemoryDialog = new NewTranslationMemoryDialog();
	}
}
