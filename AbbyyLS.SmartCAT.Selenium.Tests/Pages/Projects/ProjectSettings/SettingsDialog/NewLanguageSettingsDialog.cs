using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog
{
	public class NewLanguageSettingsDialog: ProjectSettingsDialog, IAbstractPage<NewLanguageSettingsDialog>
	{
		public NewLanguageSettingsDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewLanguageSettingsDialog LoadPage()
		{
			if (!IsNewLanguageSettingsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Не открылся диалог настройки новых языков.");
			}

			return this;
		}
		
		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Save.
		/// </summary>
		public ProjectSettingsDialog ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save.");
			SaveButton.Click();

			return new ProjectSettingsDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Поставить галочку для добавления глоссария
		/// </summary>
		/// <param name="glossary">имя глоссария</param>
		public NewLanguageSettingsDialog CheckGlossaryCheckbox(string glossary)
		{
			CustomTestContext.WriteLine("Поставить галочку для добавления глоссария {0}.", glossary);
			GlossaryCheckbox = Driver.SetDynamicValue(How.XPath, GLOSSARY_CHECKBOX, glossary);
			GlossaryCheckbox.Click();
			
			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что стоит галочка у глоссария
		/// </summary>
		/// <param name="glossary">имя глоссария</param>
		public bool IsGlossaryCheckboxChecked(string glossary)
		{
			CustomTestContext.WriteLine("Проверить, что стоит галочка у глоссария {0}.", glossary);
			GlossaryCheckbox = Driver.SetDynamicValue(How.XPath, GLOSSARY_CHECKBOX, glossary);

			return GlossaryCheckbox.GetIsInputChecked();
		}

		/// <summary>
		/// Проверить, что открылся диалог настройки новых языков.
		/// </summary>
		public bool IsNewLanguageSettingsDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DIALOG_HEADER));
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Выбрать глоссарий
		/// </summary>
		/// <param name="glossary">имя глоссария</param>
		public NewLanguageSettingsDialog SelectGlossary(string glossary)
		{
			if (!IsGlossaryCheckboxChecked(glossary))
			{
				CheckGlossaryCheckbox(glossary);
			}

			return LoadPage();
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = GLOSSARY_CHECKBOX)]
		protected IWebElement GlossaryCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string DIALOG_HEADER = "//div[contains(@style, 'display: block')]//h2[text()='New Language Settings']";
		protected const string GLOSSARY_CHECKBOX = "//td[text() = '*#*']/..//input";
		protected const string SAVE_BUTTON = "(//div[contains(@style, 'display: block')]//div[contains(@data-bind, 'save')])[2]";

		#endregion
	}
}
