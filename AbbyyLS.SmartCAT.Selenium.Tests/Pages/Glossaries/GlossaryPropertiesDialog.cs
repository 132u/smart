﻿using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossaryPropertiesDialog : WorkspacePage, IAbstractPage<GlossaryPropertiesDialog>
	{
		public new GlossaryPropertiesDialog GetPage()
		{
			var glossaryPropertiesDialog = new GlossaryPropertiesDialog();
			InitPage(glossaryPropertiesDialog);

			return glossaryPropertiesDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(GLOSSARY_PROPERTIES_DIALOG)))
			{
				Assert.Fail("Произошла ошибка:\n диалог свойств глоссария не открылся.");
			}
		}

		/// <summary>
		/// Ввести имя глоссария в диалоге свойств глоссария
		/// </summary>
		public GlossaryPropertiesDialog FillGlossaryName(string glossaryName)
		{
			Logger.Debug("Ввести имя глоссария {0} в диалоге свойств глоссария.", glossaryName);
			GlossaryName.SetText(glossaryName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Delete Glossary'
		/// </summary>
		public GlossaryPropertiesDialog ClickDeleteGlossaryButton()
		{
			Logger.Debug("Нажать кнопку 'Delete Glossary'.");
			DeleteGlossaryButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Delete для подтверждения удаления глоссария
		/// </summary>
		public GlossariesPage ClickConfirmDeleteGlossaryButton()
		{
			Logger.Debug("Нажать кнопку Delete для подтверждения удаления глоссария.");
			ConfirmDeleteButton.Click();

			return new GlossariesPage().GetPage();
		}


		/// <summary>
		/// Нажать кнопку Save 
		/// </summary>
		public T ClickSaveButton<T>() where T : class, IAbstractPage<T>, new()
		{
			Logger.Debug("Нажать кнопку Save в диалоге свойств глоссария.");
			SaveButton.Click();

			return new T().GetPage();
		}

		/// <summary>
		/// Проверить, что сообщение подтверждения удаления глоссария появилось
		/// </summary>
		public GlossaryPropertiesDialog AssertConfirmDeleteMessageDisplay()
		{
			Logger.Trace("Проверить, что сообщение подтверждения удаления глоссария появилось.");

			Assert.IsTrue(ConfirmDeleteMessage.Displayed, "Произошла ошибка:\n сообщение подтверждения удаления глоссария не появилось.");

			return GetPage();
		}

		/// <summary>
		/// Получить количество языков в диалоге свойств глоссария
		/// </summary>
		public int LanguagesCount()
		{
			Logger.Trace("Получить количество языков в диалоге свойств глоссария.");

			return Driver.GetElementsCount(By.XPath(LANGUAGE_LIST));
		}

		/// <summary>
		/// Нажать на крестик для удаления языка в диалоге свойств глоссария
		/// </summary>
		/// <param name="languageNumber">номер языка</param>
		public GlossaryPropertiesDialog ClickDeleteLanguageButton(int languageNumber)
		{
			Logger.Debug("Нажать на крестик для удаления {0} языка в диалоге свойств глоссария.", languageNumber);
			var deleteButton = Driver.SetDynamicValue(How.XPath, DELETE_LANGUAGE_BUTTON, languageNumber.ToString());

			deleteButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось предупреждение при удалении языка в диалоге свойств глоссария
		/// </summary>
		public GlossaryPropertiesDialog AssertDeleteLanguageWarningDisplay()
		{
			Logger.Trace("Проверить, что появилось предупреждение при удалении языка в диалоге свойств глоссария.");

			Assert.IsTrue(WarningDeleteLanguage.Displayed,
				"Произошла ошибка:\n не появилось предупреждение при удалении языка в диалоге свойств глоссария.");

			return GetPage();
		}

		/// <summary>
		/// Нажать Cancel в сообщении при удалении языка в диалоге свойств глоссария
		/// </summary>
		public GlossaryPropertiesDialog ClickCancelInDeleteLanguageWarning()
		{
			Logger.Debug("Нажать Cancel в сообщении при удалении языка в диалоге свойств глоссария.");
			CancelLanguageDeleteButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Advanced в диалоге свойств глоссария
		/// </summary>
		public GlossaryStructureDialog ClickAdvancedButton()
		{
			Logger.Debug("Нажать кнопку Advanced в диалоге свойств глоссария.");
			AdvancedButton.Click();

			return new GlossaryStructureDialog().GetPage();
		}

		[FindsBy(How = How.XPath, Using = DELETE_GLOSSARY_BUTTON)]
		protected IWebElement DeleteGlossaryButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_DELETE_BUTTON)]
		protected IWebElement ConfirmDeleteButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_DELETE_MESSAGE)]
		protected IWebElement ConfirmDeleteMessage { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_LANGUAGE_BUTTON)]
		protected IWebElement DeleteLanguageButton{ get; set; }

		[FindsBy(How = How.XPath, Using = WARNING_DELETE_LANGUAGE)]
		protected IWebElement WarningDeleteLanguage { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_LANGUAGE_DELETE)]
		protected IWebElement CancelLanguageDeleteButton { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_NAME)]
		protected IWebElement GlossaryName { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADVANCED_BUTTON)]
		protected IWebElement AdvancedButton { get; set; }

		protected const string GLOSSARY_PROPERTIES_DIALOG = ".//div[contains(@class,'js-popup-edit-glossary')][2]";
		protected const string DELETE_GLOSSARY_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[contains(@data-bind, 'click: deleteGlossary')]";
		protected const string CONFIRM_DELETE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//a[contains(@data-bind, 'click: deleteGlossary')]";
		protected const string CONFIRM_DELETE_MESSAGE = "//div[contains(@class, 'popup-edit-glossary')][2]//p[@data-message-id='confirm-delete-glossary']";
		protected const string LANGUAGE_LIST = "//div[@class='l-editgloss__contrbox'][1]//span[@class='g-iblock l-editgloss__control l-editgloss__lang']";
		protected const string DELETE_LANGUAGE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[@class='l-editgloss__contrbox'][1]//span[*#*]//em";
		protected const string WARNING_DELETE_LANGUAGE = "//div[contains(@class, 'popup-edit-glossary')][2]//p[@data-message-id='language-deleted-warning']";
		protected const string CANCEL_LANGUAGE_DELETE = ".//div[contains(@class,'js-popup-edit-glossary')][2]//a[contains(@data-bind, 'click: undoDeleteLanguage')]";
		protected const string GLOSSARY_NAME = ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[@class='g-bold l-editgloss__nmtext']";
		protected const string SAVE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[@class='g-btn g-redbtn ']";
		protected const string ADVANCED_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//a[contains(@data-bind,'click: saveAndEditStructure')]";
	}
}