using System.Collections.Generic;

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
			var levelOption = Driver.SetDynamicValue(How.XPath, LEVEL_OPTION, level.Description());

			levelOption.Click();

			return GetPage();
		}

		/// <summary>
		/// Переключиться на вкладку 'Custom Fields'
		/// </summary>
		public GlossaryStructureDialog SwitchToCustomFieldsTab()
		{
			Logger.Debug("Переключиться на вкладку ' Custom Fields'.");
			CustomFieldsTab.Click();

			return GetPage();
		}

		/// <summary>
		/// Заполнить название кастомного поля
		/// </summary>
		public GlossaryStructureDialog FillCustomFieldName(string fieldName)
		{
			Logger.Debug("Ввести {0} в название кастомного поля.", fieldName);
			CustomFieldName.SetText(fieldName);

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в поле 'Default value'
		/// </summary>
		public GlossaryStructureDialog FillDefaultValue(string defaultValue)
		{
			Logger.Debug("Ввести {0} в поле 'Default value'.", defaultValue);
			DefaultValue.SetText(defaultValue);

			return GetPage();
		}

		/// <summary>
		/// Заполнить поле 'Items List'
		/// </summary>
		public GlossaryStructureDialog FillItemsList(string items)
		{
			Logger.Debug("Заполнить поле 'Items List'.");
			ItemsListField.SetText(items);

			return GetPage();
		}

		/// <summary>
		/// Выбрать тип кастомного поля
		/// </summary>
		public GlossaryStructureDialog SelectCustomFieldType(GlossaryCustomFieldType type)
		{
			Logger.Debug("Выбрать тип {0} кастомного поля.", type);
			Driver.SetDynamicValue(How.XPath, TYPE_COMBOBOX, type.Description().Trim()).Click();

			return GetPage();
		}

		/// <summary>
		/// Раскрыть комбобокс типа кастомного поля
		/// </summary>
		public GlossaryStructureDialog ExpandCustomFieldType()
		{
			Logger.Debug("Раскрыть комбобокс типа кастомного поля.");
			Type.Click();

			return GetPage();
		}

		/// <summary>
		/// Поставить галочку в чекбоксе 'Required field'
		/// </summary>
		public GlossaryStructureDialog ClickRequiredCheckbox()
		{
			Logger.Debug("Поставить галочку в чекбоксе 'Required field'.");
			RequiredCheckbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Add to List'
		/// </summary>
		public GlossaryStructureDialog ClickAddCustoFieldButton()
		{
			Logger.Debug("Нажать кнопку 'Add to List'.");
			AddCustomFieldButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Add to List' на вкладке 'System Fields'
		/// </summary>
		public GlossaryStructureDialog ClickAddSystemFieldButton()
		{
			Logger.Debug("Нажать кнопку 'Add to List'  на вкладке 'System Fields'.");
			AddSystemFieldButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить имена системных полей на вкладке  'System Fields'
		/// </summary>
		public IList<IWebElement> SystemFieldNames()
		{
			Logger.Trace("Получить список имен системных полей на вкладке  'System Fields'.");

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

		protected const string SAVE_BUTTON = "//div[contains(@class, 'js-popup-buttons')]//span[contains(@class, 'js-save')]";
		protected const string ADD_TO_LIST_BUTTON = "//span[contains(@class,'js-add-tbx-attribute')]";
		protected const string SYSTEM_FIELD = "//table[contains(@class, 'table concept')]//tr[@data-attr-type='*#*']";
		protected const string ADDED_SYSTEM_FIELD = "//div[@class='l-editgloss__tbxreslt']//td[contains(text(), '*#*')]";
		protected const string LEVEL_DROPDOWN = "//span[contains(@class, 'js-dropdown__text level')]";
		protected const string LANGUAGE_FIELDS_LIST = "//table[contains(@class, 'language')]/tbody//tr";
		protected const string LEVEL_OPTION = "//span[contains(@class, 'js-dropdown__item level') and @title='*#*']";
		protected const string CUSTOM_FIELDS_TAB = "//a[contains(@class,'js-type-tab js-custom-tab')]";
		protected const string CUSTOM_FIELD_NAME = "//div[contains(@class,'js-custom-attrs')]//input[contains(@class,'js-name')]";
		protected const string TYPE_COMBOBOX = "//span[contains(@class,'js-dropdown__item type')][@title='*#*']";
		protected const string TYPE = "//table[contains(@class,'l-editgloss__tblEditStructure')]//span[contains(@class,'js-dropdown__text type')]";
		protected const string REQUIRED_CHECKBOX = "//input[contains(@class,'js-required')]";
		protected const string DEFAULT_VALUE = "//td[contains(@class,'js-default-editor-placeholder')]//input[contains(@class,'js-submit-input')]";
		protected const string ADD_CUSTOM_FIELD_BUTTON = "//span[contains(@class,'js-add-custom-attribute')]";
		protected const string GLOSSARY_STRUCTURE_DIALOG_HEADER = "//h2[contains(text(), 'Structure')]";
		protected const string ITEMS_LIST_FIELD = "//table[contains(@class,'l-editgloss__tblEditStructure')]//input[contains(@class,'js-choice-values')]";

		protected const string ADD_SYSTEM_FIELD_BUTTON = "//div[contains(@class, 'js-popup-edit-structure')]//div[contains(@class, 'addinlist')]//span//a";
		protected const string FIELD_NAME_LIST_IN_SYSTEM_FILEDS_TAB = "//table[contains(@class,'js-predefined-attrs-table')][contains(@style,'table')]//tr[contains(@class,'js-attr-row') and not(contains(@class,'g-hidden'))]/td[1]";
	}
}
