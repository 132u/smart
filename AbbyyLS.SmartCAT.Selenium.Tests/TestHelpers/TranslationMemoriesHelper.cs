using System;
using System.Collections.Generic;
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
				.OpenSourceLanguagesList()
				.SelectSourceLanguage(sourceLanguage);

			BaseObject.InitPage(_newTranslationMemoryDialog);

			if (targetLanguage != Language.NoLanguage)
			{
				_newTranslationMemoryDialog
					.OpenTargetLanguagesList()
					.SelectTargetLanguage(targetLanguage)
					.OpenTargetLanguagesList();
			}

			if (secondTargetLanguage != Language.NoLanguage)
			{
				_newTranslationMemoryDialog
					.OpenTargetLanguagesList()
					.SelectTargetLanguage(secondTargetLanguage)
					.OpenTargetLanguagesList();
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
							.AssertNewTMDialogDisappeared()
							.AssertDialogBackgroundDissapeared<TranslationMemoriesPage>();
					}

					break;
				case DialogButtonType.Cancel:
					_newTranslationMemoryDialog
						.ClickCancelTMCreation()
						.AssertNewTMDialogDisappeared()
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
			FindTranslationMemory(translationMemoryName);

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
		
		public TranslationMemoriesHelper RenameTranslationMemory(string oldName, string newName)
		{
			OpenTranslationMemoryInformation(oldName);

			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickEditButton()
				.CleanTranslationMemoryName()
				.AddTranslationMemoryName(newName)
				.ClickSaveTranslationMemoryButton();

			return this;
		}

		public TranslationMemoriesHelper AssertEditionFormDisappeared()
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.AssertEditionFormDisappeared();

			return this;
		}

		public TranslationMemoriesHelper EditComment(string tmName, string text)
		{
			OpenTranslationMemoryInformation(tmName);

			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickEditButton()
				.CleanComment()
				.AddComment(text)
				.ClickSaveTranslationMemoryButton()
				.AssertEditionFormDisappeared();

			return this;
		}

		public TranslationMemoriesHelper AssertCommentExist(string text)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.AssertCommentExists(text);

			return this;
		}

		public TranslationMemoriesHelper FindTranslationMemory(string tmName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			
			_translationMemoriesPage
				.FillSearch(tmName)
				.ClickSearchTMButton();

			return this;
		}

		public TranslationMemoriesHelper CloseAllNotifications()
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.CloseAllNotifications<TranslationMemoriesPage>();

			return this;
		}

		public TranslationMemoriesHelper AssertClientExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.OpenClientsList()
				.AssertClientsListDisplayed()
				.AssertClientExists(clientName);

			return this;
		}

		public TranslationMemoriesHelper AssertClientNotExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.OpenClientsList()
				.AssertClientsListDisplayed()
				.AssertClientNotExists(clientName);

			return this;
		}

		public TranslationMemoriesHelper AssertProjectGroupExist(string projectGroup)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.OpenProjectGroupsList()
				.AssertProjectGroupsListDisplayed()
				.AssertProjectGroupExists(projectGroup);

			return this;
		}

		public TranslationMemoriesHelper AssertProjectGroupNotExist(string projectGroup)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickCreateNewTmButton()
				.OpenProjectGroupsList()
				.AssertProjectGroupsListDisplayed()
				.AssertProjectGroupNotExists(projectGroup);

			return this;
		}

		public TranslationMemoriesHelper AssertTranslationMemoryExists(string tmName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.AssertTranslationMemoryExists(tmName);

			return this;
		}

		public TranslationMemoriesHelper AssertTranslationMemoryNotExists(string tmName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.AssertTranslationMemoryNotExists(tmName);

			return this;
		}

		public TranslationMemoriesHelper AssertExistNameErrorAppearedInCreationDialog()
		{
			BaseObject.InitPage(_newTranslationMemoryDialog);
			_newTranslationMemoryDialog.AssertExistingNameErrorAppeared();

			return this;
		}

		public TranslationMemoriesHelper AssertNoNameErrorAppearedInCreationDialog()
		{
			BaseObject.InitPage(_newTranslationMemoryDialog);
			_newTranslationMemoryDialog.AssertNoNameErrorAppeared();

			return this;
		}

		public TranslationMemoriesHelper AssertNoTargetErrorAppearedInCreationDialog()
		{
			BaseObject.InitPage(_newTranslationMemoryDialog);
			_newTranslationMemoryDialog.AssertNoTargetErrorAppeared();

			return this;
		}

		public TranslationMemoriesHelper AssertNotTmxFileErrorAppearedInCreationDialog()
		{
			BaseObject.InitPage(_newTranslationMemoryDialog);
			_newTranslationMemoryDialog.AssertNotTmxFileErrorAppeared();

			return this;
		}

		public TranslationMemoriesHelper AssertNoNameErrorAppearInEditionForm()
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.AssertNoNameErrorAppear();

			return this;
		}

		public TranslationMemoriesHelper AssertExistNameErrorAppearInEditionForm()
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.AssertExistingNameErrorAppeared();

			return this;
		}

		public TranslationMemoriesHelper GetTranslationMemoryUniqueName(ref string baseTranslationMemoryName)
		{
			baseTranslationMemoryName = baseTranslationMemoryName + DateTime.UtcNow.Ticks;

			return this;
		}

		public TranslationMemoriesHelper AddTargetLanguageToTranslationMemory(string translationMemoryName, Language language)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickTranslationMemoryRow(translationMemoryName)
				.ClickEditButton()
				.ClickToTargetLanguages()
				.SelectTargetLanguage(language)
				.ClickSaveTranslationMemoryButton();

			return this;
		}

		public TranslationMemoriesHelper AssertLanguagesForTranslationMemory(
			string translationMemoryName, 
			string sourceLanguage, 
			List<string> targetlanguages)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.AssertLanguagesForTranslationMemory(translationMemoryName, sourceLanguage, targetlanguages);

			return this;
		}

		public TranslationMemoriesHelper AddFirstProjectGroupToTranslationMemory(string translationMemoryName, out string projectGroupName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickEditButton()
				.ClickToProjectGroupsField()
				.SelectFirstProjectGroup(out projectGroupName)
				.ClickSaveTranslationMemoryButton()
				.AssertEditionFormDisappeared();

			return this;
		}

		public TranslationMemoriesHelper AssertTMXFileIsImported()
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage.AssertFileImportCompleteNotifierDisplayed();

				return this;
		}

		public TranslationMemoriesHelper AssertProjectGroupExistForTranslationMemory(
			string translationMemoryName,
			string projectGroup)
		{
			OpenTranslationMemoryInformation(translationMemoryName);

			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.AssertProjectGroupSelectedForTM(translationMemoryName, projectGroup);

			return this;
		}

		public TranslationMemoriesHelper OpenTranslationMemoryInformation(string translationMemoryName)
		{
			BaseObject.InitPage(_translationMemoriesPage);

			if (!_translationMemoriesPage.IsTranslationMemoryInformationOpen(translationMemoryName))
			{
				_translationMemoriesPage.ClickTranslationMemoryRow(translationMemoryName);
			}

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
