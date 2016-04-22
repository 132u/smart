using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Admin
{
	public class AdminCreateAccountPage : AdminLingvoProPage, IAbstractPage<AdminCreateAccountPage>
	{
		public AdminCreateAccountPage(WebDriver driver) : base(driver)
		{
		}

		public new AdminCreateAccountPage LoadPage()
		{
			if (!IsAdminCreateAccountPageOpened())
			{
				throw new Exception("Произошла ошибка:\n не загружена страница создания аккаунта.");
			}

			return this;
		}

		#region Простые методы

		/// <summary>
		/// Выбрать дату окончания действия словаря
		/// </summary>
		public AdminCreateAccountPage SelectExpirationDate()
		{
			CustomTestContext.WriteLine("Выбрать дату окончания действия словаря");
			var nextYear = DateTime.Today.AddYears(1).ToString("dd.MM.yyyy");
			CustomTestContext.WriteLine("Установить дату '{0}'", nextYear);
			DictionariesExtDate.SetText(nextYear);

			return LoadPage();
		}
		
		/// <summary>
		/// Нажать кнопку 'Сохранить' 
		/// </summary>
		public AdminCreateAccountPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Сохранить'.");
			SaveButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Поставить галочку в Workflow чекбокс
		/// </summary>
		public AdminCreateAccountPage CheckWorkflowCheckbox()
		{
			CustomTestContext.WriteLine("Поставить галочку в Workflow чекбокс");
			WorkflowCheckbox.Click();

			return this;
		}

		/// <summary>
		/// Ввести название аккаунта
		/// </summary>
		/// <param name="name">название</param>
		public AdminCreateAccountPage FillAccountName(string name)
		{
			CustomTestContext.WriteLine("Ввести название аккаунта " + name);
			Name.SetText(name);

			return LoadPage();
		}

		/// <summary>
		/// Выбрать затею
		/// </summary>
		/// <param name="name">название</param>
		public AdminCreateAccountPage SetVenture(string name)
		{
			CustomTestContext.WriteLine("Выбрать нужную затею: {0}", name);
			Venture.SelectOptionByText(name);

			return LoadPage();
		}

		/// <summary>
		/// Выбрать тип корпоративного аккаунта
		/// </summary>
		public AdminCreateAccountPage SetEnterpriseAccountType(AccountType accountType)
		{
			CustomTestContext.WriteLine("Выбрать тип корпоративного аккаунта: {0}", accountType);
			EnterpriseAccountTypeDropdown.SelectOptionByText(accountType.ToString());

			return LoadPage();
		}

		/// <summary>
		/// Ввести название поддомена
		/// </summary>
		/// <param name="name">название</param>
		public AdminCreateAccountPage FillSubdomainName(string name)
		{
			CustomTestContext.WriteLine("Ввести название поддомена {0}", name);
			Subdomain.SetText(name);

			return LoadPage();
		}

		/// <summary>
		/// Выбрать функцию
		/// </summary>
		public AdminCreateAccountPage SelectFeature(string feature)
		{
			CustomTestContext.WriteLine("Выбрать функцию {0}", feature);
			FeaturesOptions = Driver.SetDynamicValue(How.XPath, FEATURES_OPTIONS, feature);
			FeaturesOptions.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть стрелку 'вправо', чтоб добавить функию
		/// </summary>
		public AdminCreateAccountPage ClickRightArrowToAddFeature()
		{
			CustomTestContext.WriteLine("Кликнуть по стрелке 'вправо', чтоб добавить функцию");
			FeaturesToRightArrow.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку добавления всех пакетов словарей
		/// </summary>
		public AdminCreateAccountPage AddAllPackages()
		{
			CustomTestContext.WriteLine("Нажать на кнопку добавления всех пакетов словарей.");
			AddAllDictionariesPackagesButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить список включенных в аккаунт фич из правой таблицы.
		/// </summary>
		public List<string> GetFeaturesListInAccount()
		{
			CustomTestContext.WriteLine("Получить список включенных в аккаунт фич из правой таблицы.");

			return Driver.GetTextListElement(By.XPath(FEATURES_OPTIONS_IN_RIGHT_TABLE));
		}

		/// <summary>
		/// Нажать на кнопку добавления всех алгоритмов разбора документов 
		/// </summary>
		public AdminCreateAccountPage ClickAddAllDisassemblesButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку добавления всех алгоритмов разбора документов.");
			AddAllDisassemblesButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по ссылке 'Управление платными услугами'
		/// </summary>
		public AdminManagementPaidServicesPage ClickManagementPaidServicesReference()
		{
			CustomTestContext.WriteLine("Кликнуть по ссылке 'Управление платными услугами'");
			ManagementPaidServicesReference.Click();

			return new AdminManagementPaidServicesPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Добавить фичи
		/// </summary>
		/// <param name="features">список фич</param>
		public AdminCreateAccountPage AddFeatures(IList<string> features)
		{
			foreach (var feature in features.ToList())
			{
				SelectFeature(feature);
				ClickRightArrowToAddFeature();
			}

			return LoadPage();
		}

		/// <summary>
		/// Добавить все пакеты словарей
		/// </summary>
		public AdminCreateAccountPage AddAllDictionariesPackages()
		{
			AddAllPackages();
			ClickSaveButton();

			return LoadPage();
		}

		/// <summary>
		/// Закрыть окно с вопросом о Workflow
		/// </summary>
		public AdminCreateAccountPage AcceptWorkflowModalDialog()
		{
			var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));

			try
			{
				var alert = wait.Until(ExpectedConditions.AlertIsPresent());

				if (alert.Text.Contains("Включение функции"
					 + " workflow для аккаунта необратимо, обратное выключение "
					 + "будет невозможно. Продолжить?"))
				{
					CustomTestContext.WriteLine("Закрыть модальное диалоговое окно");
					alert.Accept();
				}
			}
			catch (NoAlertPresentException)
			{
				CustomTestContext.WriteLine("Закрыть открытый диалог");
				SendKeys.SendWait(@"{Enter}");
			}
			catch (WebDriverTimeoutException)
			{
				CustomTestContext.WriteLine("Алерт не появился");
			}

			return LoadPage();
		}

		/// <summary>
		/// Заполнить форму создания нового аккаунта
		/// </summary>
		/// <param name="venture">затея</param>
		/// <param name="accountName">название аккаунта</param>
		/// <param name="workflow">включить workflow</param>
		/// <param name="features">набор фич</param>
		/// <param name="packagesNeed">нужны ли пакеты словарей</param>
		/// <param name="accountType">тип аккаунта</param>
		public AdminCreateAccountPage FillAccountCreationForm(
			string venture,
			string accountName,
			bool workflow,
			List<string> features,
			bool packagesNeed,
			AccountType accountType = AccountType.LanguageServiceProvider)
		{
			FillAccountName(accountName);
			SetVenture(venture);
			FillSubdomainName(accountName);
			SetEnterpriseAccountType(accountType);

			if (workflow)
			{
				CheckWorkflowCheckbox();
				AcceptWorkflowModalDialog();
			}

			AddFeatures(features);
			ClickAddAllDisassemblesButton();

			if (features.Contains(Feature.LingvoDictionaries.ToString()))
			{
				SelectExpirationDate();

				if (packagesNeed)
				{
					AddAllPackages();
				}
			}

			ClickSaveButton();

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница создания аккаунта
		/// </summary>
		public bool IsAdminCreateAccountPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(INPUT_NAME));
		}

		#endregion

		#region Объявление элементов страниц

		[FindsBy(How = How.XPath, Using = FEATURES_OPTIONS)]
		protected IWebElement FeaturesOptions { get; set; }

		[FindsBy(How = How.XPath, Using = FEATURES_TO_RIGHT_ARROW)]
		protected IWebElement FeaturesToRightArrow { get; set; }

		[FindsBy(How = How.XPath, Using = DICTIONARIES_EXP_DATE)]
		protected IWebElement DictionariesExtDate {get; set;}

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = INPUT_NAME)]
		protected IWebElement Name { get; set; }

		[FindsBy(How = How.XPath, Using = INPUT_VENTURE)]
		protected IWebElement Venture { get; set; }

		[FindsBy(How = How.XPath, Using = SUBDOMAIN_NAME)]
		protected IWebElement Subdomain { get; set; }

		[FindsBy(How = How.XPath, Using = WORKFLOW_CHECKBOX)]
		protected IWebElement WorkflowCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_ALL_DICTIONARIES_PACKAGES_BUTTON)]
		protected IWebElement AddAllDictionariesPackagesButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_ALL_DISASSEMBLES_BUTTON)]
		protected IWebElement AddAllDisassemblesButton { get; set; }

		[FindsBy(How = How.XPath, Using = MANAGEMENT_PAID_SERVICES_REFERENCE)]
		protected IWebElement ManagementPaidServicesReference { get; set; }

		[FindsBy(How = How.XPath, Using = ENTERPRISE_ACCOUNT_TYPE_DROPDOWN)]
		protected IWebElement EnterpriseAccountTypeDropdown { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string INPUT_NAME = "//input[(@id='Name')]";
		protected const string FEATURES_OPTIONS = "//table[@name='Features']//select[@id='left']//option[@value='*#*']";
		protected const string FEATURES_OPTIONS_IN_RIGHT_TABLE = "//table[@name='Features']//select[@id='right']//option";
		protected const string FEATURES_TO_RIGHT_ARROW = "//table[@name='Features']//input[@name='toRight']";
		protected const string DICTIONARIES_EXP_DATE = "//input[@id='DictionariesExpirationDate']";
		protected const string SAVE_BUTTON = "//p[@class='submit-area']/input";
		protected const string INPUT_VENTURE = "//select[@id='VentureId']";
		protected const string SUBDOMAIN_NAME = "//input[@name='SubDomain']";
		protected const string WORKFLOW_CHECKBOX = "//input[@id='WorkflowEnabled']";
		protected const string ADD_ALL_DICTIONARIES_PACKAGES_BUTTON = "//table[@name='dictionariesPackages']//input[@name='allToRight']";
		protected const string ADD_ALL_DISASSEMBLES_BUTTON = "//table[@name='DisassembleDocumentMethodNames']//input[@name='allToRight']";
		protected const string MANAGEMENT_PAID_SERVICES_REFERENCE = "//a[contains(@href,'PaidServices')]";
		protected const string ENTERPRISE_ACCOUNT_TYPE_DROPDOWN = "//select[@id='EnterpriseAccountType']";

		#endregion
	}
}
