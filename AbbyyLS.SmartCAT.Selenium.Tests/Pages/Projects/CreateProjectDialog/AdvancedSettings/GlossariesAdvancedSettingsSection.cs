using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class GlossariesAdvancedSettingsSection : AdvancedSettingsSection, IAbstractPage<GlossariesAdvancedSettingsSection>
	{
		public GlossariesAdvancedSettingsSection(WebDriver driver)
			: base(driver)
		{
		}

		public new GlossariesAdvancedSettingsSection LoadPage()
		{
			if (!IsGlossariesAdvancedSettingsSectionOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылись расширенные настройки Glossaries.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на кнопку 'Create Glossary' в панели 'Advanced Settings'.
		/// </summary>
		public GlossariesAdvancedSettingsSection ClickCreateGlossaryButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Create Glossary' в панели 'Advanced Settings'.");
			CreateGlossaryButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Edit Glossary' в панели 'Advanced Settings'.
		/// </summary>
		public NewProjectEditGlossaryDialog ClickEditGlossaryButton(int glossaryNumber)
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Edit Glossary' в панели 'Advanced Settings' для глоссария №{0}.", glossaryNumber);
			EditGlossaryButton = Driver.SetDynamicValue(How.XPath, EDIT_GLOSSARY_BUTTON, glossaryNumber.ToString());
			EditGlossaryButton.Click();

			return new NewProjectEditGlossaryDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Навести курсор на глоссарий в панели 'Advanced Settings'.
		/// </summary>
		public NewProjectSettingsPage HoverGlossaryRow(int glossaryNumber)
		{
			CustomTestContext.WriteLine("Навести курсор на глоссарий №{0} в панели 'Advanced Settings'.", glossaryNumber);
			GlossaryRow = Driver.SetDynamicValue(How.XPath, GLOSSARY_ROW, glossaryNumber.ToString());
			GlossaryRow.ScrollDown();
			GlossaryRow.HoverElement();

			return new NewProjectSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Получить количество глоссариев.
		/// </summary>
		public int GetGlossariesCount()
		{
			CustomTestContext.WriteLine("Получить количество глоссариев.");

			return Driver.GetElementsCount(By.XPath(PROJECT_GLOSSARY_ROW));
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Переименовать глоссарий
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <param name="glossaryNumber">номер глоссария</param>
		public NewProjectEditGlossaryDialog OpenEditGlossaryDialog(int glossaryNumber = 1)
		{
			HoverGlossaryRow(glossaryNumber);
			ClickEditGlossaryButton(glossaryNumber);

			return new NewProjectEditGlossaryDialog(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылись расширенные настройки Glossaries.
		/// </summary>
		public bool IsGlossariesAdvancedSettingsSectionOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SELECT_GLOSSARY_BUTTON));
		}

		/// <summary>
		/// Проверить, что отображается сообщение 'The glossary cannot be created until a client is selected'.
		/// </summary>
		public bool IsClientErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается сообщение 'The glossary cannot be created until a client is selected.'.");

			return ClientError.Displayed;
		}

		/// <summary>
		/// Проверить, что отображается сообщение 'You cannot create glossaries for the selected client.'.
		/// </summary>
		public bool IsWrongClientErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что отображается сообщение 'You cannot create glossaries for the selected client.'.");

			return WrongClientError.Displayed;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CREATE_GLOSSARY_BUTTON)]
		protected IWebElement CreateGlossaryButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDIT_GLOSSARY_BUTTON)]
		protected IWebElement EditGlossaryButton { get; set; }

		[FindsBy(How = How.XPath, Using = SELECT_GLOSSARY_BUTTON)]
		protected IWebElement SelectGlossaryButton { get; set; }
		[FindsBy(How = How.XPath, Using = PROJECT_GLOSSARY_ROW)]
		protected IWebElement ProjectGlossaryRow { get; set; }
		[FindsBy(How = How.XPath, Using = CLIENT_ERROR)]
		protected IWebElement ClientError { get; set; }

		[FindsBy(How = How.XPath, Using = WRONG_CLIENT_ERROR)]
		protected IWebElement WrongClientError { get; set; }

		protected IWebElement GlossaryRow { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string GLOSSARY_ROW = "//div[contains(@data-bind, 'switchDetailMode')][*#*]";
		protected const string CREATE_GLOSSARY_BUTTON = "//div[contains(@data-bind, 'createGlossary')]";
		protected const string EDIT_GLOSSARY_BUTTON = "//div[contains(@data-bind, 'switchDetailMode')][*#*]//div[contains(@class, 'right l-settings-icons')]//span[contains(@data-bind, 'edit')]";
		protected const string SELECT_GLOSSARY_BUTTON = "//div[contains(@data-bind, 'addExistingGlossary')]";
		protected const string PROJECT_GLOSSARY_ROW = "//div[contains(@data-bind, 'projectGlossaries')]//div[contains(@data-bind, 'switchDetailMode')]";
		protected const string CLIENT_ERROR = "//div[text()='The glossary cannot be created until a client is selected.']";
		protected const string WRONG_CLIENT_ERROR = "//div[text()='You cannot create glossaries for the selected client.']";

		#endregion
	}
}
