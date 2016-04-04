﻿using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog
{
	public class SettingsResourcesStep : DocumentUploadBaseDialog, IAbstractPage<SettingsResourcesStep>
	{
		public SettingsResourcesStep(WebDriver driver): base(driver)
		{
		}

		public new SettingsResourcesStep LoadPage()
		{
			if (!IsSettingsResourcesStepOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся шаг настройки ресурсосв.");
			}

			return this;
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся шаг настройки ресурсов.
		/// </summary>
		public bool IsSettingsResourcesStepOpened()
		{
			CustomTestContext.WriteLine("Проверить, что открылся шаг настройки ресурсов.");

			return Driver.GetIsElementExist(By.XPath(USE_MACHINE_TRANSLATION_CHECKBOX));
		}

		#endregion

		#region Объявление элементов страницы
		#endregion

		#region Описания XPath элементов
		
		protected const string USE_MACHINE_TRANSLATION_CHECKBOX = "//input[@name='mts-checkbox']";

		#endregion
	}
}
