using System;
using System.ComponentModel;
using System.Threading;

using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class TranslationMemoriesHelper : WorkspaceHelper
	{
		public TranslationMemoriesHelper(WebDriver driver) : base(driver)
		{
			_newTranslationMemoryDialog = new NewTranslationMemoryDialog(Driver);
			_translationMemoriesPage = new TranslationMemoriesPage(Driver);
			_translationMemoriesFilterDialog = new TranslationMemoriesFilterDialog(Driver);
		}

		public TranslationMemoriesHelper CreateTranslationMemory(
			string translationMemoryName,
			Language sourceLanguage = Language.English,
			Language targetLanguage = Language.Russian,
			Language secondTargetLanguage = Language.NoLanguage,
			string importFilePath = null,
			DialogButtonType finalButtonType = DialogButtonType.Save,
			bool isCreationErrorExpected = false)
		{
			_translationMemoriesPage.ClickCreateNewTmButton();

			_newTranslationMemoryDialog
				.SetTranslationMemoryName(translationMemoryName)
				.SetSourceLanguage(sourceLanguage);

			if (targetLanguage != Language.NoLanguage)
			{
				_newTranslationMemoryDialog.SetTargetLanguage(targetLanguage);
			}

			if (secondTargetLanguage != Language.NoLanguage)
			{
				_newTranslationMemoryDialog.SetTargetLanguage(secondTargetLanguage);
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
						_newTranslationMemoryDialog.ClickSaveTranslationMemoryExpectingError();
						return this;
					}
					else
					{
						_newTranslationMemoryDialog.ClickSaveTranslationMemory();
					}
					break;
				case DialogButtonType.Cancel:
					_newTranslationMemoryDialog.ClickCancelTMCreation();
					break;
				case DialogButtonType.None:
					return this;
				default:
					throw new InvalidEnumArgumentException();
			}

			if (!_translationMemoriesPage.IsNewTMDialogDisappeared())
			{
				throw new XPathLookupException("Произошла ошибка:\n диалог создания ТМ не закрылся");
			}

			_translationMemoriesPage.WaitUntilDialogBackgroundDisappeared();

			return this;
		}

		public string GetTranslationMemoryUniqueName()
		{
			return "TM_" + Guid.NewGuid();
		}

		public TranslationMemoriesHelper CreateNewTMFilter(
			string setAuthorFilter = null,
			Language? setSourceLanguageFilter = null,
			Language? setTargetLanguageFilter = null,
			string setTopicFilter = null,
			string setProjectGroupFilter = null,
			string setClientFilter = null,
			DateTime? setCreationDateTMFilterFrom = null,
			bool clearFilters = true,
			bool cancelFilterCreation = false)
		{
			_translationMemoriesPage.ClickFilterButton();
			//Sleep нужен,чтобы форма успела перейти в режим редактирования.
			Thread.Sleep(1000);

			if (clearFilters)
			{
				_translationMemoriesFilterDialog.ClickClearFieldsButton();
			}

			if (setAuthorFilter != null)
			{
				_translationMemoriesFilterDialog.SelectAuthor(setAuthorFilter);
			}

			if (setSourceLanguageFilter != null)
			{
				_translationMemoriesFilterDialog.SetSourceLanguageFilter(setSourceLanguageFilter.Value);
			}

			if (setTargetLanguageFilter != null)
			{
				_translationMemoriesFilterDialog.SetTargetLanguageFilter(setTargetLanguageFilter.Value);
			}

			if (setTopicFilter != null)
			{
				_translationMemoriesFilterDialog.SetTopicFilter(setTopicFilter);
			}

			if (setProjectGroupFilter != null)
			{
				_translationMemoriesFilterDialog.SetProjectGroupFilter(setProjectGroupFilter);
			}

			if (setClientFilter != null)
			{
				_translationMemoriesFilterDialog.SetClientFilter(setClientFilter);
			}

			if (setCreationDateTMFilterFrom != null)
			{
				_translationMemoriesFilterDialog.SetCreationDateTMFilterFrom(setCreationDateTMFilterFrom.Value);
			}

			if (cancelFilterCreation)
			{
				_translationMemoriesFilterDialog.ClickCancelButton();
				
				return this;
			}

			_translationMemoriesFilterDialog.ClickApplyButton();

			return this;
		}

		private TranslationMemoriesFilterDialog _translationMemoriesFilterDialog;
		private TranslationMemoriesPage _translationMemoriesPage;
		private NewTranslationMemoryDialog _newTranslationMemoryDialog;
	}
}
