﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

﻿using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class FilterDialog : GlossaryPage, IAbstractPage<FilterDialog>
	{
		public FilterDialog(WebDriver driver): base(driver)
		{
		}

		public new FilterDialog LoadPage()
		{
			if (!IsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузился FilterDialog.");
			}

			return this;
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыта ли страница
		/// </summary>
		public bool IsPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CLEAR_FILTER_BUTTON));
		}

		/// <summary>
		/// Получить список элементов из дроплауна языков.
		/// </summary>
		public IList<IWebElement> LanguageCheckboxes()
		{
			CustomTestContext.WriteLine("Получить список жлементов из дроплауна языков.");
			LanguageDropdown.Click();
			
			return Driver.GetElementList(By.XPath(LANGUAGES_CHECKBOXES));
		}

		/// <summary>
		/// Проверить, что все чекбоксы в дропдауне языков чекнуты.
		/// </summary>
		public bool AreLanguagesCheckedInDropdown()
		{
			CustomTestContext.WriteLine("Проверить, что все чекбоксы в дропдауне языков чекнуты.");
			
			foreach (var checkbox in LanguageCheckboxes())
			{
				if (checkbox.GetAttribute("checked") == "false")
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Получить дату из поля даты изменения
		/// </summary>
		public DateTime GetModifiedDataText(DateRange period)
		{
			CustomTestContext.WriteLine("Получить дату из поля даты изменения.");
			var dateString = Driver.SetDynamicValue(How.XPath, TODAY_CREATED_DATE, period.ToString().ToLower()).GetAttribute("innerHTML");
			
			return DateTime.ParseExact(dateString, "M/d/yyyy", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Получить список чекбоксов из дропдауна автора.
		/// </summary>
		public IList<IWebElement> AuthorCheckboxes()
		{
			CustomTestContext.WriteLine("Получить список чекбоксов из дропдауна автора..");
			
			return Driver.GetElementList(By.XPath(AUTHOR_CHECKBOXES));
		}

		/// <summary>
		/// Проверить, что нет галочек в дропдауне Автора.
		/// </summary>
		public bool AreAuthorChecboxesUnchecked()
		{
			CustomTestContext.WriteLine("Проверить, что нет галочек в дропдауне Автора.");
			ClickAuthorDropdown();

			return AuthorCheckboxes().Count(c => c.Selected) == 0;
		}

		/// <summary>
		/// Проверить, что нет галочек в дропдауне автора изменений.
		/// </summary>
		public bool AreModifierChecboxesUnchecked()
		{
			CustomTestContext.WriteLine("Проверить, что нет галочек в дропдауне автора изменений.");
			ClickModifierDropdown();

			foreach (var checkbox in ModifierCheckboxes())
			{
				if (checkbox.Selected)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Получить выбранное значение в комбобоксе Modified
		/// </summary>
		public string GetDateModifiedDropdownText()
		{
			CustomTestContext.WriteLine("Получить выбранное значение в комбобоксе Modified.");

			return DateModifiedDropdown.Text;
		}

		/// <summary>
		/// Получить список чекбоксов из дропдауна автора изменений
		/// </summary>
		public IList<IWebElement> ModifierCheckboxes()
		{
			CustomTestContext.WriteLine("Получить список чекбоксов из дропдауна автора изменений.");

			return Driver.GetElementList(By.XPath(MODIFIER_CHECKBOXES));
		}

		/// <summary>
		/// Получить значение из поля даты создания
		/// </summary>
		public string GetCreatedDateDropdownText()
		{
			CustomTestContext.WriteLine("Получить установленное значение из дроплауна даты создания.");

			return CreatedDate.Text.Trim();
		}

		/// <summary>
		/// Получить значение из поля даты изменения
		/// </summary>
		public string GetModifiedDateDropdownText()
		{
			CustomTestContext.WriteLine("Получить установленное значение из дроплауна даты изменения.");

			return ModifiedDate.Text.Trim();
		}

		/// <summary>
		/// Получить дату из поля даты создания
		/// </summary>
		/// <param name="period">период</param>
		public DateTime GetCreatedDateTime(DateRange period)
		{
			CustomTestContext.WriteLine("Получить дату из поля даты создания.");
			var dateString = Driver.SetDynamicValue(How.XPath, TODAY_CREATED_DATE, period.ToString().ToLower()).GetAttribute("innerHTML");

			return DateTime.ParseExact(dateString, "M/d/yyyy", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Получить список элементов из дропдауна языков
		/// </summary>
		public IList<IWebElement> LanguagesListInDropdown()
		{
			CustomTestContext.WriteLine("Получить список элементов из дропдауна языков.");

			return Driver.GetElementList(By.XPath(LANGUAGES_CHECKBOXES));
		}

		/// <summary>
		/// Проверить, что сообщение 'The Language field cannot be empty.' появилось
		/// </summary>
		public bool IsEmptyLanguageerrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что сообщение 'The Language field cannot be empty.' появилось.");

			return EmptyLanguageError.Displayed;
		}

		/// <summary>
		/// Получить список выбранных элементов
		/// </summary>
		public List<string> SelectedLanguagesInDropdown()
		{
			CustomTestContext.WriteLine("Получить список выбранных элементов в дропдауне языков.");
			IList<IWebElement> selectedLanguages = Driver.GetElementList(By.XPath(SELECTED_LANGUAES));

			return selectedLanguages.Select(l => l.Text.Trim()).ToList();
		}
		#endregion

		#region Простые методы страницы

		/// <summary>
		/// Нажать на дропдаун языков
		/// </summary>
		public FilterDialog ClickLanguageDropdown()
		{
			CustomTestContext.WriteLine("Нажать на дропдаун языков.");
			LanguageDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Apply
		/// </summary>
		public GlossaryPage ClickApplyButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Apply.");
			ApplyButton.Click();

			return new GlossaryPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Apply, ожидая ошибку
		/// </summary>
		public FilterDialog ClickApplyButtonFailureExpected()
		{
			CustomTestContext.WriteLine("Нажать кнопку Apply, ожидая ошибку.");
			ApplyButton.Click();

			return new FilterDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун автора
		/// </summary>
		public FilterDialog ClickAuthorDropdown()
		{
			CustomTestContext.WriteLine("Нажать на дропдаун автора.");
			AuthorDropdown.Click();

			return LoadPage();
		}
		
		/// <summary>
		/// Нажать на дропдаун автора изменений
		/// </summary>
		public FilterDialog ClickModifierDropdown()
		{
			CustomTestContext.WriteLine("Нажать на дропдаун автора изменений.");
			ModifierDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Clear
		/// </summary>
		public FilterDialog ClickClearButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Clear.");
			ClearFilterButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel
		/// </summary>
		public GlossaryPage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel.");
			CancelFilterButton.Click();

			return new GlossaryPage(Driver).LoadPage();
		}

		/// <summary>
		/// Ввести конечную дату в фильтр Created
		/// </summary>
		/// <param name="createdEndDate">конечная дата создания</param>
		public FilterDialog SetCreatedEndDate(DateTime createdEndDate)
		{
			CustomTestContext.WriteLine("Ввести {0} в конечную дату создания.", createdEndDate);
			CreatedEndDate.SetText(createdEndDate.ToString("M/d/yyyy").Replace(".", "/"));

			return LoadPage();
		}

		/// <summary>
		/// Ввести начальную дату в фильтр Created
		/// </summary>
		/// <param name="createdStartDate">начальная дата создания</param>
		public FilterDialog SetCreatedStartDate(DateTime createdStartDate)
		{
			CustomTestContext.WriteLine("Ввести {0} в начальную дату создания.", createdStartDate);
			CreatedStartDate.SetText(createdStartDate.ToString("M/d/yyyy").Replace(".", "/"));

			return LoadPage();
		}

		/// <summary>
		/// Ввести конечную дату в фильтр Modified
		/// </summary>
		/// <param name="modifiedEndDate">конечная дата изменения</param>
		public FilterDialog SetModifiedEndDate(DateTime modifiedEndDate)
		{
			CustomTestContext.WriteLine("Ввести {0} в конечную дату изменения.", modifiedEndDate);
			ModifiedEndDate.SetText(modifiedEndDate.ToString("M/d/yyyy").Replace(".", "/"));

			return LoadPage();
		}

		/// <summary>
		/// Ввести начальную дату в фильтр Modified
		/// </summary>
		/// <param name="modifiedStartDate">начальная дата изменения</param>
		public FilterDialog SetModifiedStartDate(DateTime modifiedStartDate)
		{
			CustomTestContext.WriteLine("Ввести {0} в начальную дату изменения.", modifiedStartDate);
			ModifiedStartDate.SetText(modifiedStartDate.ToString("M/d/yyyy").Replace(".", "/"));
			
			return LoadPage();
		}

		/// <summary>
		/// Нажать на начальную дату создания
		/// </summary>
		public FilterDialog ClickCreatedStartDate()
		{
			CustomTestContext.WriteLine("Нажать на начальную дату создания.");
			CreatedStartDate.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на конечную дату создания
		/// </summary>
		public FilterDialog ClickCreatedEndDate()
		{
			CustomTestContext.WriteLine("Нажать на конечную дату создания.");
			CreatedEndDate.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на начальную дату изменения
		/// </summary>
		public FilterDialog ClickModifiedStartDate()
		{
			CustomTestContext.WriteLine("Нажать на начальную дату изменения.");
			ModifiedStartDate.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на конечную дату изменения
		/// </summary>
		public FilterDialog ClickModifiedEndDate()
		{
			CustomTestContext.WriteLine("Нажать на конечную дату изменения.");
			ModifiedEndDate.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на дату в календаре
		/// </summary>
		public FilterDialog ClickDayInCalendar()
		{
			CustomTestContext.WriteLine("Нажать на дату в календаре.");
			DayInCalendar.Click();

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать язык.
		/// </summary>
		public FilterDialog SelectLanguage(Language language)
		{
			CustomTestContext.WriteLine("Выбрать язык {0}.", language);
			LanguageDropdown.Click();
			Driver.WaitUntilElementIsDisplay(By.XPath(LANGUAGES_CHECKBOXES));
			var checkboxList = Driver.GetElementList(By.XPath(LANGUAGES_CHECKBOXES));

			foreach (var checkbox in checkboxList)
			{
				if ((checkbox.GetAttribute("title") != language.ToString()) && (checkbox.GetAttribute("checked") == "true"))
				{
					checkbox.Click();
				}
			}
			LanguageDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать дату в дропдауне Modified.
		/// </summary>
		public FilterDialog SelectModifiedDateOption(DateRange option)
		{
			CustomTestContext.WriteLine("Выбрать дату {0} в дропдауне Modified.", option.Description());
			DateModifiedDropdown.Click();
			Driver.SetDynamicValue(How.XPath, DATE_OPTION, option.Description()).Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать дату в дропдауне Created.
		/// </summary>
		public FilterDialog SelectCreatedDateOption(DateRange option)
		{
			CustomTestContext.WriteLine("Выбрать дату {0} в дропдауне Created.", option.Description());
			DateCreatedDropdown.Click();
			Driver.SetDynamicValue(How.XPath, DATE_OPTION, option.Description()).Click();

			return LoadPage();
		}

		/// <summary>
		/// Снять все галочки в дропдауне языков
		/// </summary>
		public FilterDialog UncheckAllFilterLanguages()
		{
			CustomTestContext.WriteLine("Снять все галочки в дропдауне языков.");
			var checkboxList = Driver.GetElementList(By.XPath(LANGUAGES_CHECKBOXES));
			var checkedCheckboxList = checkboxList.Where(checkbox => checkbox.GetAttribute("checked") == "true");

			foreach (var checkbox in checkedCheckboxList)
			{
				checkbox.Click();
			}

			return LoadPage();
		}
		
		/// <summary>
		/// Выбрать автора изменений
		/// </summary>
		public FilterDialog SelectModifier(string userName)
		{
			CustomTestContext.WriteLine("Выбрать автора изменений {0}.", userName);
			var split = userName.Split(' ');
			var modifier = Driver.GetElementList(By.XPath(MODIFIER_CHECKBOXES)).FirstOrDefault(item => split.Count(word => item.GetAttribute("title").Contains(word)) == split.Count());

			if (modifier == null)
			{
				throw new NullReferenceException("В дропдауне нет автора изменений.");
			}
			
			modifier.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать автора
		/// </summary>
		/// <param name="userName">имя</param>
		public FilterDialog SelectAuthor(string userName)
		{
			CustomTestContext.WriteLine("Выбрать автора {0}.", userName);
			var author = Driver.GetElementList(By.XPath(AUTHOR_CHECKBOXES)).FirstOrDefault();

			if (author == null)
			{
				throw new NullReferenceException("В дропдауне нет автора.");
			}

			author.Click();

			return LoadPage();
		}
		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = LANGUAGE_DROPDOWN)]
		protected IWebElement LanguageDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = APPLY_BUTTON)]
		protected IWebElement ApplyButton { get; set; }

		[FindsBy(How = How.XPath, Using = EMPTY_LANGUAGE_ERROR)]
		protected IWebElement EmptyLanguageError { get; set; }

		[FindsBy(How = How.XPath, Using = AUTHOR_DROPDOWN)]
		protected IWebElement AuthorDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = MODIFIER_DROPDOWN)]
		protected IWebElement ModifierDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = DATE_CREATED_DROPDOWN)]
		protected IWebElement DateCreatedDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = DATE_MODIFIED_DROPDOWN)]
		protected IWebElement DateModifiedDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = CLEAR_FILTER_BUTTON)]
		protected IWebElement ClearFilterButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_FILTER_BUTTON)]
		protected IWebElement CancelFilterButton { get; set; }

		[FindsBy(How = How.XPath, Using = CREATED_DATE)]
		protected IWebElement CreatedDate { get; set; }

		[FindsBy(How = How.XPath, Using = MODIFIED_DATE)]
		protected IWebElement ModifiedDate { get; set; }

		[FindsBy(How = How.XPath, Using = CREATED_END_DATE)]
		protected IWebElement CreatedEndDate { get; set; }

		[FindsBy(How = How.XPath, Using = CREATED_START_DATE)]
		protected IWebElement CreatedStartDate { get; set; }

		[FindsBy(How = How.XPath, Using = MODIFIED_END_DATE)]
		protected IWebElement ModifiedEndDate { get; set; }

		[FindsBy(How = How.XPath, Using = MODIFIED_START_DATE)]
		protected IWebElement ModifiedStartDate { get; set; }

		[FindsBy(How = How.XPath, Using = DAY_IN_CALENDAR)]
		protected IWebElement DayInCalendar { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string CLEAR_FILTER_BUTTON = "//a[contains(text(),'Clear fields')]";
		protected const string CANCEL_FILTER_BUTTON = "//div[@class='g-popupbox__bd']//a[contains(@class, 'js-popup-close')]";
		protected const string SELECTED_LANGUAES = "//span[@class='g-iblock g-bold ui-multiselect-value']";
		protected const string APPLY_BUTTON = "//input[@value='Apply']";
		protected const string LANGUAGES_CHECKBOXES = "//input[contains(@id,'ui-multiselect-languages')]";
		protected const string FILTER_FORM = "//div[@class='g-popupbox l-filtersrc']//h2[contains(text(),'Filter')]";
		protected const string LANGUAGE_DROPDOWN = "//div[contains(@class, 'js-languages-multiselect')]//div[@class='ui-multiselect-text']";
		protected const string EMPTY_LANGUAGE_ERROR = "//div[@style='display: block;']//p[text()='The \"Language\" field cannot be empty.']";
		protected const string AUTHOR_DROPDOWN = "//div[@class='l-filtersrc__control creator']//div[@class='ui-multiselect-text']//span";
		protected const string AUTHOR_CHECKBOXES = "//input[contains(@id,'ui-multiselect-creator-option')]";
		protected const string MODIFIER_DROPDOWN = "//div[contains(@class,'modifier')]//div[contains(@class, 'ui-multiselect ui-widget')]";
		protected const string MODIFIER_CHECKBOXES = "//input[contains(@id,'ui-multiselect-modifier-option')]";
		protected const string DATE_CREATED_DROPDOWN = "//div[@class='l-filtersrc__lside']//i";
		protected const string DATE_OPTION = "//span[contains(@title, '*#*')]";
		protected const string TODAY_CREATED_DATE = "//div[@class='l-filtersrc__lside']//span[@class='js-data *#*']";
		protected const string DATE_MODIFIED_DROPDOWN = "//div[@class='l-filtersrc__rside']//i";
		protected const string TODAY_MODIFIED_DATE = "//div[@class='l-filtersrc__rside']//span[@class='js-data *#*']";
		protected const string CREATED_DATE = "//div[@class='l-filtersrc__lside']//span[contains(@class, 'js-dropdown filterDate')]";
		protected const string MODIFIED_DATE = "//div[@class='l-filtersrc__rside']//span[contains(@class, 'js-dropdown filterDate')]";
		protected const string CREATED_END_DATE = "//div[@class='l-filtersrc__lside']//span[6]/input";
		protected const string CREATED_START_DATE = "//div[@class='l-filtersrc__lside']//span[5]/input";
		protected const string MODIFIED_END_DATE = "//div[@class='l-filtersrc__rside']//span[6]/input";
		protected const string MODIFIED_START_DATE = "//div[@class='l-filtersrc__rside']//span[5]/input";
		protected const string DATE_CREATED_DROPDOWN_TEXT = "//div[@class='l-filtersrc__lside']//span[@class='js-dropdown filterDate g-drpdwn g-iblock ']//span";
		protected const string DAY_IN_CALENDAR = "//table[@class='ui-datepicker-calendar']//tr[3]/td[1]/a";

		#endregion
	}
}