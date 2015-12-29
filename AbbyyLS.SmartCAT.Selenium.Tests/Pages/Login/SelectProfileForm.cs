﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Login
{
	public class SelectProfileForm : BaseObject, IAbstractPage<SelectProfileForm>
	{
		
		public WebDriver Driver { get; protected set; }

		public SelectProfileForm(WebDriver driver)
		{
			Driver = driver;
			PageFactory.InitElements(Driver, this);
		}

		public SelectProfileForm GetPage()
		{
			var selectProfileForm = new SelectProfileForm(Driver);
			InitPage(selectProfileForm, Driver);

			return selectProfileForm;
		}

		public void LoadPage()
		{
			Driver.WaitPageTotalLoad();
			if (!IsSelectProfileFormOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась форма выбора профиля.");
			}
		}

		#region Простые методы страницы


		#endregion


		#region Составные методы страницы


		#endregion


		#region Методы, проверяющие состояние страницы

		public bool IsSelectProfileFormOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(FREELANCE_PROFILE))
				&& Driver.WaitUntilElementIsDisplay(By.XPath(CORPORATE_PROFILE));
		}

		#endregion


		#region Вспомогательные методы


		#endregion


		#region Объявление элементов страницы


		#endregion



		#region Описание XPath элементов

		protected const string FREELANCE_PROFILE = "//div[contains(@data-bind, 'chooseFreelancerProfile')]";
		protected const string CORPORATE_PROFILE = "//div[contains(@data-bind, 'chooseCorporateProfile')]";

		#endregion
	}
}
