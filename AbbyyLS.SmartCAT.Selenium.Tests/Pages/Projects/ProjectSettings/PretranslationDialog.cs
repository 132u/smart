using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings
{
	public class PretranslationDialog : ProjectSettingsPage, IAbstractPage<PretranslationDialog>
	{
		public PretranslationDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new PretranslationDialog LoadPage()
		{
			if (!IsPretranslationDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\nДиалог претранслейта не открылся.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку создания нового правила
		/// </summary>
		public PretranslationDialog ClickAddInsertionRuleButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку создания нового правила");
			AddInsertionRuleButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Раскрыть дропдаун ресурсов.
		/// </summary>
		public PretranslationDialog ExpandResourceDropdown()
		{
			CustomTestContext.WriteLine("Раскрыть дропдаун ресурсов.");
			ResourceDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения.
		/// </summary>
		public ProjectSettingsPage ClickSaveAndRunButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения.");
			SaveAndRunButton.Click();

			return new ProjectSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть опцию в дропдауне ресурсов.
		/// </summary>
		/// <param name="resourceOption">ресурс</param>
		public PretranslationDialog ClickResourceOption(string resourceOption)
		{
			CustomTestContext.WriteLine("Кликнуть опцию в дропдауне ресурсов.");
			ResourceOption = Driver.SetDynamicValue(How.XPath, RESOURCE_OPTION,  resourceOption);
			ResourceOption.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel.
		/// </summary>
		public ProjectSettingsPage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel.");
			CancelButton.Click();

			return new ProjectSettingsPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать ресурс
		/// </summary>
		/// <param name="resourceOption">ресурс</param>
		public PretranslationDialog SelectResourceOption(CatType resourceOption)
		{
			ExpandResourceDropdown();
			ClickResourceOption(resourceOption.Description());
		
			return LoadPage();
		}

		/// <summary>
		/// Выбрать ресурс памяти перевода
		/// </summary>
		/// <param name="resourceOption">имя памяти перевода</param>
		public PretranslationDialog SelectTranslationMemoryResourceOption(string translationMemory)
		{
			ExpandResourceDropdown();
			ClickResourceOption(translationMemory);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что диалог претранслейта открылся.
		/// </summary>
		public bool IsPretranslationDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(ADD_INSERTION_RULE_BUTTON));
		}

		#endregion
		
		#region Вспомогательные методы


		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ADD_INSERTION_RULE_BUTTON)]
		protected IWebElement AddInsertionRuleButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = RESOURCE_DROPDOWN)]
		protected IWebElement ResourceDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_AND_RUN_BUTTON)]
		protected IWebElement SaveAndRunButton { get; set; }

		protected IWebElement ResourceOption { get; set; }

		#endregion
		
		#region Описание XPath элементов

		protected const string ADD_INSERTION_RULE_BUTTON = "//div[contains(@class,'pretranslate')][2]//div[contains(@class, 'new-rule')]";
		protected const string RESOURCE_DROPDOWN = "//div[contains(@class,'pretranslate')]//span[contains(@class, 'js-dropdown')]";
		protected const string RESOURCE_OPTION = "//span[@title='*#*']";
		protected const string SAVE_AND_RUN_BUTTON = "//div[contains(@class,'pretranslate')][2]//div[contains(@class, 'js-save')]";
		protected const string CANCEL_BUTTON = "//div[contains(@class, 'js-popup-pretranslate')][2]//a[contains(@class, 'js-popup-close')]";
		#endregion
	}
}
