using System.Collections.Generic;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossaryStructureDialog : WorkspacePage, IAbstractPage<GlossaryStructureDialog>
	{
		public GlossaryStructureDialog(WebDriver driver) : base(driver)
		{
		}

		public new GlossaryStructureDialog GetPage()
		{
			var glossaryStructureDialog = new GlossaryStructureDialog(Driver);
			InitPage(glossaryStructureDialog, Driver);

			return glossaryStructureDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(GLOSSARY_STRUCTURE_DIALOG_HEADER)))
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
			CustomTestContext.WriteLine("Выбрать поле {0} для изменения структуры глоссария.", systemField);
			var field = Driver.SetDynamicValue(How.XPath, SYSTEM_FIELD, systemField.ToString());

			field.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Save в диалоге изменения сруктуры глоссария.
		/// </summary>
		public GlossaryPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save в диалоге изменения сруктуры глоссария.");
			SaveButton.Click();

			return new GlossaryPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Add To List' в диалоге изменения сруктуры глоссария.
		/// </summary>
		public GlossaryStructureDialog ClickAddToListButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Add To List' в диалоге изменения сруктуры глоссария.");
			AddToListButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Добавить все поля уровня Language
		/// </summary>
		public GlossaryStructureDialog AddLanguageFields()
		{
			CustomTestContext.WriteLine("Добавить все поля уровня Language.");
			var fieldList = Driver.GetElementList(By.XPath(LANGUAGE_FIELDS_LIST));

			foreach (var field in fieldList)
			{
				field.Click();
				AddSystemFieldButton.Click();
			}

			return GetPage();
		}

		/// <summary>
		/// Проверить, что поле добавлено
		/// </summary>
		public GlossaryStructureDialog AssertSystemFieldIsAdded(GlossarySystemField systemField)
		{
			CustomTestContext.WriteLine("Проверить, что поле {0} добавлено.", systemField);
			var field = Driver.SetDynamicValue(How.XPath, ADDED_SYSTEM_FIELD, systemField.ToString());

			Assert.IsTrue(field.Displayed, "Произошла ошибка:\n поле {0} не добавлено.", systemField);

			return GetPage();
		}

		/// <summary>
		/// Раскрыть комбобокс с уровнями полей
		/// </summary>
		public GlossaryStructureDialog ExpandLevelDropdown()
		{
			CustomTestContext.WriteLine("Раскрыть комбобокс с уровнями полей.");
			LevelDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать уровень полей
		/// </summary>
		/// <param name="level">уровень</param>
		public GlossaryStructureDialog SelectLevel(GlossaryStructureLevel level)
		{
			CustomTestContext.WriteLine("Выбрать {0} уровень полей.", level);
			var levelOption = Driver.SetDynamicValue(How.XPath, LEVEL_OPTION, level.Description());

			levelOption.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать поле типа термин
		/// </summary>
		/// <returns></returns>
		public GlossaryStructureDialog SelectTermField(GlossarySystemField termField)
		{
			CustomTestContext.WriteLine("Выбрать поле {0} типа термин.", termField);
			Driver.SetDynamicValue(How.XPath, TERM_FIELD_OPTION, termField.Description()).ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Переключиться на вкладку 'Custom Fields'
		/// </summary>
		public GlossaryStructureDialog SwitchToCustomFieldsTab()
		{
			CustomTestContext.WriteLine("Переключиться на вкладку ' Custom Fields'.");
			CustomFieldsTab.Click();

			return GetPage();
		}

		/// <summary>
		/// Заполнить название кастомного поля
		/// </summary>
		public GlossaryStructureDialog FillCustomFieldName(string fieldName)
		{
			CustomTestContext.WriteLine("Ввести {0} в название кастомного поля.", fieldName);
			CustomFieldName.SetText(fieldName);

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в поле 'Default value'
		/// </summary>
		public GlossaryStructureDialog FillDefaultValue(string defaultValue)
		{
			CustomTestContext.WriteLine("Ввести {0} в поле 'Default value'.", defaultValue);
			DefaultValue.SetText(defaultValue);

			return GetPage();
		}

		/// <summary>
		/// Заполнить поле 'Items List'
		/// </summary>
		public GlossaryStructureDialog FillItemsList(string items)
		{
			CustomTestContext.WriteLine("Заполнить поле 'Items List'.");
			ItemsListField.SetText(items);

			return GetPage();
		}

		/// <summary>
		/// Выбрать тип кастомного поля
		/// </summary>
		public GlossaryStructureDialog SelectCustomFieldType(GlossaryCustomFieldType type)
		{
			CustomTestContext.WriteLine("Выбрать тип {0} кастомного поля.", type);
			Driver.SetDynamicValue(How.XPath, TYPE_COMBOBOX, type.Description().Trim()).Click();

			return GetPage();
		}

		/// <summary>
		/// Раскрыть комбобокс типа кастомного поля
		/// </summary>
		public GlossaryStructureDialog ExpandCustomFieldType()
		{
			CustomTestContext.WriteLine("Раскрыть комбобокс типа кастомного поля.");
			Type.Click();

			return GetPage();
		}

		/// <summary>
		/// Поставить галочку в чекбоксе 'Required field'
		/// </summary>
		public GlossaryStructureDialog ClickRequiredCheckbox()
		{
			CustomTestContext.WriteLine("Поставить галочку в чекбоксе 'Required field'.");
			RequiredCheckbox.JavaScriptClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Add to List'
		/// </summary>
		public GlossaryStructureDialog ClickAddCustomFieldButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Add to List' на вкладке 'Custom Fields'.");
			AddCustomFieldButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Add to List' на вкладке 'System Fields'
		/// </summary>
		public GlossaryStructureDialog ClickAddSystemFieldButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Add to List'  на вкладке 'System Fields'.");
			AddSystemFieldButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить имена системных полей на вкладке  'System Fields'
		/// </summary>
		public IList<IWebElement> SystemFieldNames()
		{
			CustomTestContext.WriteLine("Получить список имен системных полей на вкладке  'System Fields'.");

			return Driver.GetElementList(By.XPath(FIELD_NAME_LIST_IN_SYSTEM_FILEDS_TAB));
		}

		[FindsBy(How = How.XPath, Using = ADD_TO_LIST_BUTTON)]
		protected IWebElement AddToListButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = LEVEL_DROPDOWN)]
		protected IWebElement LevelDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = CUSTOM_FIELDS_TAB)]
		protected IWebElement CustomFieldsTab { get; set; }

		[FindsBy(How = How.XPath, Using = CUSTOM_FIELD_NAME)]
		protected IWebElement CustomFieldName { get; set; }

		[FindsBy(How = How.XPath, Using = TYPE)]
		protected IWebElement Type { get; set; }

		[FindsBy(How = How.XPath, Using = REQUIRED_CHECKBOX)]
		protected IWebElement RequiredCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = DEFAULT_VALUE)]
		protected IWebElement DefaultValue { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_CUSTOM_FIELD_BUTTON)]
		protected IWebElement AddCustomFieldButton { get; set; }

		[FindsBy(How = How.XPath, Using = ITEMS_LIST_FIELD)]
		protected IWebElement ItemsListField { get; set; }
		
		[FindsBy(How = How.XPath, Using = ADD_SYSTEM_FIELD_BUTTON)]
		protected IWebElement AddSystemFieldButton { get; set; }

		protected const string SAVE_BUTTON = "//div[contains(@class, 'js-popup-buttons')]//div[contains(@class, 'js-save')]";
		protected const string ADD_TO_LIST_BUTTON = "//span[contains(@class,'js-add-tbx-attribute')]";
		protected const string SYSTEM_FIELD = "//table[contains(@class, 'table concept')]//tr[@data-attr-type='*#*']";
		protected const string ADDED_SYSTEM_FIELD = "//div[contains(@class,'l-editgloss__tbxreslt')]//td[contains(text(), '*#*')]";
		protected const string LEVEL_DROPDOWN = "//span[contains(@class, 'js-dropdown__text level')]";
		protected const string LANGUAGE_FIELDS_LIST = "//table[contains(@class, 'language')]/tbody//tr";
		protected const string LEVEL_OPTION = "//span[contains(@class, 'js-dropdown__item level') and @title='*#*']";
		protected const string CUSTOM_FIELDS_TAB = "//a[contains(@class,'js-type-tab js-custom-tab')]";
		protected const string CUSTOM_FIELD_NAME = "//div[contains(@class,'js-custom-attrs')]//input[contains(@class,'js-name')]";
		protected const string TYPE_COMBOBOX = "//span[contains(@class,'js-dropdown__item type')][@title='*#*']";
		protected const string TYPE = "//table[contains(@class,'l-editgloss__tblEditStructure')]//span[contains(@class,'js-dropdown__text type')]";
		protected const string REQUIRED_CHECKBOX = "//input[contains(@class,'js-required')]";
		protected const string DEFAULT_VALUE = "//td[contains(@class,'js-default-editor-placeholder')]//input[contains(@class,'js-submit-input')]";
		protected const string ADD_CUSTOM_FIELD_BUTTON = "//div[contains(@class,'js-add-custom-attribute')]";
		protected const string GLOSSARY_STRUCTURE_DIALOG_HEADER = "//h2[contains(text(), 'Structure')]";
		protected const string ITEMS_LIST_FIELD = "//table[contains(@class,'l-editgloss__tblEditStructure')]//input[contains(@class,'js-choice-values')]";

		protected const string ADD_SYSTEM_FIELD_BUTTON = "//div[contains(@class, 'js-popup-edit-structure')]//div[contains(@class, 'addinlist')]//div//a";
		protected const string FIELD_NAME_LIST_IN_SYSTEM_FILEDS_TAB = "//table[contains(@class,'js-predefined-attrs-table')][contains(@style,'table')]//tr[contains(@class,'js-attr-row') and not(contains(@class,'g-hidden'))]/td[1]";
		protected const string TERM_FIELD_OPTION = "//table[contains(@class, 'table term')]//td[text()='*#*']";
	}
}
