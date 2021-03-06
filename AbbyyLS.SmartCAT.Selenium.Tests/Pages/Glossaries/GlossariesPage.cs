﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries.SuggestedTerms;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossariesPage : WorkspacePage, IAbstractPage<GlossariesPage>
	{
		public GlossariesPage(WebDriver driver) : base(driver)
		{
		}

		public new GlossariesPage LoadPage()
		{
			if (!IsGlossariesPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась страница с глоссариями");
			}

			return this;
		}

		#region Простые методы страницы
		
		/// <summary>
		/// Нажать кнопку создания глоссария
		/// </summary>
		public NewGlossaryDialog ClickCreateGlossaryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку создания глоссария.");
			CreateGlossaryButton.JavaScriptClick();

			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SAVE_GLOSSARY_BUTTON)))
			{
				CustomTestContext.WriteLine("Повторный клик по кнопке создания глоссария.");
				CreateGlossaryButton.JavaScriptClick();
			}

			return new NewGlossaryDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по названию
		/// </summary>
		public GlossariesPage ClickSortByName()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по названию");
			SortByName.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по названию, ожидается алерт.
		/// </summary>
		public void ClickSortByNameAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по названию, ожидается алерт.");

			SortByName.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по языкам
		/// </summary>
		public GlossariesPage ClickSortByLanguages()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по языкам");
			SortByLanguages.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по языкам, ожидая алерт.
		/// </summary>
		public void ClickSortByLanguagesAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по языкам, ожидая алерт.");

			SortByLanguages.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по кол-ву добавленных терминов
		/// </summary>
		public GlossariesPage ClickSortByTermsAdded()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по кол-ву добавленных терминов");
			SortByTermsAdded.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сортировки по кол-ву добавленных терминов, ожидая алерт.
		/// </summary>
		public void ClickSortByTermsAddedAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по кол-ву добавленных терминов, ожидая алерт.");

			SortByTermsAdded.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по кол-ву терминов на рассмотрении, ожидая алерт.
		/// </summary>
		public void ClickSortByTermsUnderReviewAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по кол-ву терминов на рассмотрении, ожидая алерт.");

			SortByTermsUnderReview.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по комментариям, ожидая алерт.
		/// </summary>
		public void ClickSortByCommentAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по комментариям, ожидая алерт.");

			SortByComment.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по группа проектов, ожидая алерт.
		/// </summary>
		public void ClickSortByProjectGroupsAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по группа проектов, ожидая алерт.");

			SortByProjectGroups.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по клиенту, ожидая алерт.
		/// </summary>
		public void ClickSortByClientAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по клиенту, ожидая алерт.");

			SortByClient.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по дате изменения, ожидая алерт.
		/// </summary>
		public void ClickSortByDateModifiedAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по дате изменения, ожидая алерт.");

			SortByDateModified.Click();
		}

		/// <summary>
		/// Нажать кнопку сортировки по автору изменения, ожидая алерт.
		/// </summary>
		public void ClickSortByModifiedByAssumingAlert()
		{
			CustomTestContext.WriteLine("Нажать кнопку сортировки по автору изменения, ожидая алерт.");

			SortByModifiedBy.Click();
		}

		/// <summary>
		/// Нажать по имени глоссария
		/// </summary>
		public GlossaryPage ClickGlossaryRow(string glossaryName)
		{
			CustomTestContext.WriteLine("Нажать по имени глоссария {0}.", glossaryName);
			var glossaryRow = Driver.SetDynamicValue(How.XPath, GLOSSARY_ROW, glossaryName);

			glossaryRow.ScrollAndClick();

			return new GlossaryPage(Driver).LoadPage();
		}

		/// <summary>
		/// Получить целевые языки глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		public List<string> GetTargetLanguages(string glossaryName)
		{
			CustomTestContext.WriteLine("Получить целевые языки глоссария {0}.", glossaryName);
			TargetLanguagesColumn = Driver.SetDynamicValue(How.XPath, TARGET_LANGUAGES_COLUMN, glossaryName);
			var languagesColumn = TargetLanguagesColumn.Text;
			var languagesList = languagesColumn.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower()).ToList();

			languagesList.Sort();

			return languagesList;
		}

		/// <summary>
		/// Получить дату изменения глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <returns>дата изменения</returns>
		public DateTime GlossaryDateModified(string glossaryName)
		{
			CustomTestContext.WriteLine("Получить дату изменения глоссария {0}.", glossaryName);
			
			return DateTime.ParseExact(Driver.SetDynamicValue(How.XPath, MODIFIED_DATE, glossaryName).Text,
				"M/d/yyyy h:mm tt",CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Получить имя автора глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		/// <returns>имя автор</returns>
		public string GetModifiedByAuthor(string glossaryName)
		{
			CustomTestContext.WriteLine("Получить имя автора глоссария.");
			var author = Driver.SetDynamicValue(How.XPath, AUTHOR, glossaryName);

			return author.Text;
		}

		/// <summary>
		/// Нажать на кнопку 'Suggested Terms'
		/// </summary>
		public SuggestedTermsGlossariesPage ClickSuggestedTermsButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Suggested Terms'.");
			SuggestedTermsButton.Click();

			return new SuggestedTermsGlossariesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Suggest Term'
		/// </summary>
		public SuggestTermDialog ClickSuggestTermButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Suggest Term'.");
			SuggestTermButton.Click();

			return new SuggestTermDialog(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что кнопка 'Suggest Term' присутствует
		/// </summary>
		public bool IsSuggestTermButtonExist()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Suggest Term' присутствует");

			return Driver.GetIsElementExist(By.XPath(SUGGEST_TERM_BUTTON));
		}

		/// <summary>
		/// Проверить, что кнопка 'Suggested Terms' присутствует
		/// </summary>
		public bool IsSuggestedTermsButtonExist()
		{
			CustomTestContext.WriteLine("Проверить, что кнопка 'Suggested Terms' присутствует");

			return Driver.GetIsElementExist(By.XPath(SUGGESTED_TERMS_BUTTON));
		}

		/// <summary>
		/// Проверить, что дата изменения глоссария совпадает с текущей датой.
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public bool IsDateModifiedMatchCurrentDate(string glossaryName)
		{
			CustomTestContext.WriteLine("Проверить, что дата изменения глоссария {0} совпадает с текущей датой", glossaryName);

			DateTime convertModifiedDate = GlossaryDateModified(glossaryName);
			TimeSpan result = DateTime.Now - convertModifiedDate;
			var minutesDifference = result.TotalMinutes;

			return minutesDifference < 5;
		}

		/// <summary>
		/// Проверить, что глоссарий присутствует в списке
		/// </summary>
		public bool IsGlossaryExist(string glossaryName)
		{
			CustomTestContext.WriteLine("Проверить, что глоссарий {0} присутствует в списке.", glossaryName);

			return Driver.GetIsElementExist(By.XPath(GLOSSARY_ROW.Replace("*#*", glossaryName)));
		}

		///<summary>
		///Проверить, что глоссарий отсутствует в списке
		///</summary>
		public bool IsGlossaryNotExist(string glossaryName)
		{
			CustomTestContext.WriteLine("Проверить, что глоссарий {0} отсутствует в списке.", glossaryName);

			return Driver.WaitUntilElementIsDisappeared(By.XPath(GLOSSARY_ROW.Replace("*#*", glossaryName)));
		}

		/// <summary>
		/// Проверить, открыта ли страница со списком глоссариев
		/// </summary>
		public bool IsGlossariesPageOpened()
		{
			return IsDialogBackgroundDisappeared() &&
				Driver.WaitUntilElementIsDisplay(By.XPath(GLOSSARY_TABLE));
		}

		/// <summary>
		/// Проверить, отображается ли кнопка создания глоссария
		/// </summary>
		public bool IsCreateGlossaryButtonDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, отображается ли кнопка создания глоссария.");

			return Driver.GetIsElementExist(By.XPath(CREATE_GLOSSARY_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SORT_BY_NAME)]
		protected IWebElement SortByName { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_LANGUAGES)]
		protected IWebElement SortByLanguages { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_TERMS_ADDED)]
		protected IWebElement SortByTermsAdded { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_TERMS_UNDER_REVIEW)]
		protected IWebElement SortByTermsUnderReview { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_COMMENT)]
		protected IWebElement SortByComment { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_PROJECT_GROUPS)]
		protected IWebElement SortByProjectGroups { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_CLIENT)]
		protected IWebElement SortByClient { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_DATE_MODIFIED)]
		protected IWebElement SortByDateModified { get; set; }

		[FindsBy(How = How.XPath, Using = SORT_BY_MODIFIED_BY)]
		protected IWebElement SortByModifiedBy { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_GLOSSARY_BUTTON)]
		protected IWebElement CreateGlossaryButton { get; set; }

		[FindsBy(How = How.XPath, Using = SUGGESTED_TERMS_BUTTON)]
		protected IWebElement SuggestedTermsButton { get; set; }

		[FindsBy(How = How.XPath, Using = SUGGEST_TERM_BUTTON)]
		protected IWebElement SuggestTermButton { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_LANGUAGES_COLUMN)]
		protected IWebElement TargetLanguagesColumn { get; set; }

		protected IWebElement GlossaryRow { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string GLOSSARY_CREATION_DIALOG_XPATH = ".//div[contains(@class,'js-popup-edit-glossary')][2]";
		protected const string CREATE_GLOSSARY_BUTTON = ".//div[contains(@class,'js-create-glossary-button')]//a";
		protected const string GLOSSARY_TABLE = "//table[contains(@class,'js-sortable-table') and contains(@data-sort-action, 'Glossaries')]";
		protected const string GLOSSARY_ROW = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text()='*#*']";
		protected const string MODIFIED_DATE = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text() = '*#*']/../../td[last()]/preceding::td[1]";
		protected const string AUTHOR = "//tr[contains(@class, 'js-glossary-row')]/td[1]/p[text() = '*#*']/../../td[last()]/p";

		protected const string SAVE_GLOSSARY_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[@data-bind='click: save']";

		protected const string SORT_BY_NAME = "//th[contains(@data-sort-by,'Name')]//a";
		protected const string SORT_BY_LANGUAGES = "//th[contains(@data-sort-by,'Languages')]//a";
		protected const string SORT_BY_TERMS_ADDED = "//th[contains(@data-sort-by,'TermsCount')]//a";
		protected const string SORT_BY_TERMS_UNDER_REVIEW = "//th[contains(@data-sort-by,'SuggestsCount')]//a";
		protected const string SORT_BY_COMMENT = "//th[contains(@data-sort-by,'Comment')]//a";
		protected const string SORT_BY_PROJECT_GROUPS = "//th[contains(@data-sort-by,'Domains')]//a";
		protected const string SORT_BY_CLIENT = "//th[contains(@data-sort-by,'Client')]//a";
		protected const string SORT_BY_DATE_MODIFIED = "//th[contains(@data-sort-by,'LastModifiedDate')]//a";
		protected const string SORT_BY_MODIFIED_BY = "//th[contains(@data-sort-by,'LastModifiedBy')]//a";

		protected const string SUGGESTED_TERMS_BUTTON = ".//a[contains(@href,'/Suggests')]";
		protected const string SUGGEST_TERM_BUTTON = "//div[contains(@class,'js-add-suggest')]";
		protected const string TARGET_LANGUAGES_COLUMN = "//p[text()='*#*']/../..//td[2]//p";

		#endregion
	}
}
