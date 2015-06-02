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
						_newTranslationMemoryDialog.ClickSaveTranslationMemory<TranslationMemoriesPage>();
					}
					
					break;
				case DialogButtonType.Cancel:
					_newTranslationMemoryDialog.ClickCancelTranslationMemoryCreation();
					break;
				case DialogButtonType.None:
					break;
				default:
					throw new InvalidEnumArgumentException();
			}
			

			return this;
		}

		public TranslationMemoriesHelper AssertClientExistInClientsList(string clientName)
		{
			BaseObject.InitPage(_translationMemoriesPage);
			_translationMemoriesPage
				.ClickTranslationMemoriesButton()
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
				.ClickTranslationMemoriesButton()
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
				.ClickTranslationMemoriesButton()
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
				.ClickTranslationMemoriesButton()
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

		public static string GetTranslationMemoryUniqueName()
		{
			return "TM_" + DateTime.UtcNow.Ticks;
		}

		private readonly TranslationMemoriesPage _translationMemoriesPage = new TranslationMemoriesPage();
		private readonly NewTranslationMemoryDialog _newTranslationMemoryDialog = new NewTranslationMemoryDialog();
	}
}
