using System;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class QualityAssuranceAdvancedSettingsSection : AdvancedSettingsSection, IAbstractPage<QualityAssuranceAdvancedSettingsSection>
	{
		public QualityAssuranceAdvancedSettingsSection(WebDriver driver)
			: base(driver)
		{
		}

		public new QualityAssuranceAdvancedSettingsSection LoadPage()
		{
			if (!IsQualityAssuranceAdvancedSettingsSectionOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылись расширенные настройки 'Quality Assurance'.");
			}

			return this;
		}
		
		#region Простые методы страницы

		/// <summary>
		/// Получить список всех ошибок
		/// </summary>
		/// <returns></returns>
		public IList<IWebElement> GetAllErrorTitles()
		{
			CustomTestContext.WriteLine("Получить список всех ошибок.");

			return Driver.GetElementList(By.XPath(ALL_ERROR_TITLES));
		}

		/// <summary>
		/// Выбрать тип ошибки.
		/// </summary>
		/// <param name="errorType">тип ошибки</param>
		public QualityAssuranceAdvancedSettingsSection SelectErrorType(ErrorType errorType)
		{
			CustomTestContext.WriteLine("Выбрать тип ошибки.");
			ErrorType = Driver.SetDynamicValue(How.XPath, ERROR_TYPE, errorType.ToString());

			try
			{
				ErrorType.Scroll();
				ErrorType.Click();
			}
			catch (InvalidOperationException)
			{
				ErrorType.ScrollDown();
				ErrorType.Click();
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун со списком типов ошибок.
		/// </summary>
		/// <param name="errorTitle">название ошибки</param>
		public QualityAssuranceAdvancedSettingsSection ClickErrorTypeDropdown(string errorTitle)
		{
			CustomTestContext.WriteLine("Открыть дропдаун '{0}' со списком типов ошибок.", errorTitle);
			if (errorTitle.Contains("%"))
			{
				errorTitle = errorTitle.Replace("%", "");
			}
			ErrorTypeDropdown = Driver.SetDynamicValue(How.XPath, ERROR_TYPE_DROPDOWN, errorTitle);
			try
			{
				ErrorTypeDropdown.Scroll();
				ErrorTypeDropdown.Click();
			}
			catch (InvalidOperationException)
			{
				ErrorTypeDropdown.ScrollDown();
				ErrorTypeDropdown.Click();
			}

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Выбрать один и тот же тип ошибки для всех ошибок
		/// </summary>
		/// <param name="errorTitle">название ошибки</param>
		/// <param name="errorType">тип ошибки</param>
		public QualityAssuranceAdvancedSettingsSection SetErrorTypeForAllErrors(ErrorType errorType = DataStructures.ErrorType.disableError)
		{
			var errorList = GetAllErrorTitles();
			for (int i = 0; i < errorList.Count; i++)
			{
				SetErrorType(errorList[i].Text, errorType);
			}

			return LoadPage();
		}

		/// <summary>
		/// Выбрать тип ошибки
		/// </summary>
		/// <param name="errorTitle">название ошибки</param>
		/// <param name="errorType">тип ошибки</param>
		public QualityAssuranceAdvancedSettingsSection SetErrorType(string errorTitle, ErrorType errorType)
		{
			ClickErrorTypeDropdown(errorTitle);
			SelectErrorType(errorType);

			return LoadPage();
		}

		/// <summary>
		/// Проверить, что открылись расширенные настройки 'Quality Assurance'.
		/// </summary>
		public bool IsQualityAssuranceAdvancedSettingsSectionOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_SETTINGS));
		}

		#endregion


		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ERROR_SETTINGS)]
		protected IWebElement ErrorSettings { get; set; }
		protected IWebElement ErrorType { get; set; }
		protected IWebElement ErrorTypeDropdown { get; set; }
		#endregion

		#region Описания XPath элементов

		protected const string ERROR_SETTINGS = "//div[contains(@data-bind, 'errorSettings')]";
		protected const string ALL_ERROR_TITLES = "//span[contains(@class, 'mistake-title')]";
		protected const string ERROR_TYPE = "//div[@class='mistake-popup']//div[contains(@data-bind, '*#*')]";
		protected const string ERROR_TYPE_DROPDOWN = "//span[contains(text(), '*#*')]/../../..//span[@class='pull-right']";

		#endregion
	}
}
