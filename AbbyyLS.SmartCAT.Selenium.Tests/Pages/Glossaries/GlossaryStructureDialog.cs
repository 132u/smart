using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossaryStructureDialog : WorkspacePage, IAbstractPage<GlossaryStructureDialog>
	{
		public new GlossaryStructureDialog GetPage()
		{
			var glossaryStructureDialog = new GlossaryStructureDialog();
			InitPage(glossaryStructureDialog);

			return glossaryStructureDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(ADD_TO_LIST_BUTTON)))
			{
				Assert.Fail("Произошла ошибка:\n диалог изменения структуры глоссария не открылся.");
			}
		}

		/// <summary>
		/// Выбрать поле для изменения структуры глоссария
		/// </summary>
		/// <param name="systemField"></param>
		/// <returns></returns>
		public GlossaryStructureDialog SelectSystemField(GlossarySystemField systemField)
		{
			Logger.Debug("Выбрать поле {0} для изменения структуры глоссария.", systemField);
			var field = Driver.SetDynamicValue(How.XPath, SYSTEM_FIELD, systemField.ToString());

			field.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Save в диалоге изменения сруктуры глоссария.
		/// </summary>
		public GlossaryPage ClickSaveButton()
		{
			Logger.Debug("Нажать кнопку Save в диалоге изменения сруктуры глоссария.");
			SaveButton.Click();

			return new GlossaryPage().GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Add To List' в диалоге изменения сруктуры глоссария.
		/// </summary>
		public GlossaryStructureDialog ClickAddToListButton()
		{
			Logger.Debug("Нажать кнопку 'Add To List' в диалоге изменения сруктуры глоссария.");
			AddToListButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Добавить все поля уровня Language
		/// </summary>
		public GlossaryStructureDialog AddLanguageFields()
		{
			Logger.Debug("Добавить все поля уровня Language.");
			var fieldList = Driver.GetElementList(By.XPath(LANGUAGE_FIELDS_LIST));

			foreach (var field in fieldList)
			{
				field.Click();
				AddToListButton.Click();
			}
			
			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле добавлено
		/// </summary>
		public GlossaryStructureDialog AssertSystemFieldIsAdded(GlossarySystemField systemField)
		{
			Logger.Trace("Проверить, что поле {0} добавлено.", systemField);
			var field = Driver.SetDynamicValue(How.XPath, ADDED_SYSTEM_FIELD, systemField.ToString());

			Assert.IsTrue(field.Displayed, "Произошла ошибка:\n поле {0} не добавлено.", systemField);

			return GetPage();
		}

		/// <summary>
		/// Раскрыть комбобокс с уровнями полей
		/// </summary>
		public GlossaryStructureDialog ExpandLevelDropdown()
		{
			Logger.Debug("Раскрыть комбобокс с уровнями полей.");
			LevelDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать уровень полей
		/// </summary>
		/// <param name="level">уровень</param>
		public GlossaryStructureDialog SelectLevel(GlossaryStructureLevel level)
		{
			Logger.Debug("Выбрать {0} уровень полей.", level);
			var levelOption = Driver.SetDynamicValue(How.XPath, LEVEL_OPTION, level.ToString());

			levelOption.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = ADD_TO_LIST_BUTTON)]
		protected IWebElement AddToListButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = LEVEL_DROPDOWN)]
		protected IWebElement LevelDropdown { get; set; }

		protected const string SAVE_BUTTON = "//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]";
		protected const string ADD_TO_LIST_BUTTON = "//span[contains(@class,'js-add-tbx-attribute')]";
		protected const string SYSTEM_FIELD = "//table[contains(@class, 'table concept')]//tr[@data-attr-type='*#*']";
		protected const string ADDED_SYSTEM_FIELD = "//div[@class='l-editgloss__tbxreslt']//td[contains(text(), '*#*')]";
		protected const string LEVEL_DROPDOWN = "//span[contains(@class, 'js-dropdown__text level')]";
		protected const string LANGUAGE_FIELDS_LIST = "//table[contains(@class, 'language')]/tbody//tr";
		protected const string LEVEL_OPTION = "//span[contains(@class, 'js-dropdown__item level') and @title='*#*']";
	}
}
