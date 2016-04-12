using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;


namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	public class TranslationMemoriesFilterDialog : WorkspacePage, IAbstractPage<TranslationMemoriesFilterDialog>
	{
		public TranslationMemoriesFilterDialog(WebDriver driver) : base(driver)
		{
		}

		public new TranslationMemoriesFilterDialog LoadPage()
		{
			if (!IsTranslationMemoriesFilterDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузился диалог фильтров");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку очистки полей
		/// </summary>
		public TranslationMemoriesFilterDialog ClickClearFieldsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку очистки полей");
			ClearFieldsButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку отмены
		/// </summary>
		public TranslationMemoriesPage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку отмены");
			CancelButton.Click();

			return new TranslationMemoriesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку применения фильтра
		/// </summary>
		public TranslationMemoriesPage ClickApplyButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку применения фильтра");
			ApplyButton.Click();

			return new TranslationMemoriesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Кликнуть по раскрывающемуся списку source-языков
		/// </summary>
		public TranslationMemoriesFilterDialog ClickSourceLanguageList()
		{
			CustomTestContext.WriteLine("Кликнуть по раскрывающемуся списку source-языков");
			SourceLanguageList.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по раскрывающемуся списку target-языков
		/// </summary>
		public TranslationMemoriesFilterDialog ClickTargetLanguageList()
		{
			CustomTestContext.WriteLine("Кликнуть по раскрывающемуся списку target-языков");
			TargetLanguageList.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать source-язык в открытом списке
		/// </summary>
		/// <param name="language">язык</param>
		public TranslationMemoriesFilterDialog SelectSourceLanguage(Language? language)
		{
			CustomTestContext.WriteLine("Выбрать {0} в выпадающем списке source-языков.", language);
			SourceLanguage = Driver.SetDynamicValue(How.XPath, SOURCE_LANGUAGE, language.ToString());
			SourceLanguage.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать target-язык в открытом списке
		/// </summary>
		/// <param name="language">язык</param>
		public TranslationMemoriesFilterDialog SelectTargetLanguage(Language? language)
		{
			CustomTestContext.WriteLine("Выбрать {0} в выпадающем списке target-языков.", language);
			TargetLanguage = Driver.SetDynamicValue(How.XPath, TARGET_LANGUAGE, language.ToString());
			TargetLanguage.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать дату создания (поле 'от')
		/// </summary>
		/// <param name="creationDate">дата создания</param>
		public TranslationMemoriesFilterDialog SetCreationDateTMFilterFrom(DateTime creationDate)
		{
			var stringDate = string.Format(@"{0}/{1}/{2}", creationDate.Month, creationDate.Day, creationDate.Year);
			CustomTestContext.WriteLine("Задать дату создания в ТМ фильтрах. Дата: {0}", stringDate);
			CreationDate.SetText(stringDate);
			CreationDate.Click();
			
			return LoadPage();
		}

		/// <summary>
		/// Выбрать тему в открытом списке
		/// </summary>
		/// <param name="topicName">тема</param>
		public TranslationMemoriesFilterDialog SelectTopic(string topicName)
		{
			CustomTestContext.WriteLine("Выбрать {0} в выпадающем списке тем.", topicName);
			Topic = Driver.SetDynamicValue(How.XPath, TOPIC, topicName);
			Topic.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по раскрывающемуся списку тем
		/// </summary>
		public TranslationMemoriesFilterDialog ClickTopicList()
		{
			CustomTestContext.WriteLine("Кликнуть по раскрывающемуся списку тем");
			TopicList.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать группу проектов в открытом списке
		/// </summary>
		/// <param name="projectGroup">группа проектов</param>
		public TranslationMemoriesFilterDialog SelectProjectGroup(string projectGroup)
		{
			CustomTestContext.WriteLine("Выбрать {0} в выпадающем списке групп проектов.", projectGroup);
			ProjectGroup = Driver.SetDynamicValue(How.XPath, PROJECT_GROUP, projectGroup);
			ProjectGroup.Scroll();
			ProjectGroup.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по раскрывающемуся списку групп проектов
		/// </summary>
		public TranslationMemoriesFilterDialog ClickProjectGroupList()
		{
			CustomTestContext.WriteLine("Кликнуть по раскрывающемуся списку групп проектов");
			ProjectGroupList.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать клиента в открытом списке
		/// </summary>
		/// <param name="client">клиент</param>
		public TranslationMemoriesFilterDialog SelectClient(string client)
		{
			CustomTestContext.WriteLine("Выбрать {0} в выпадающем списке клиентов.", client);
			Client = Driver.SetDynamicValue(How.XPath, CLIENT, client);
			Client.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по раскрывающемуся списку клиентов
		/// </summary>
		public TranslationMemoriesFilterDialog ClickClientList()
		{
			CustomTestContext.WriteLine("Кликнуть по раскрывающемуся списку клиентов");
			ClientList.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать автора в открытом списке
		/// </summary>
		/// <param name="author">автора</param>
		public TranslationMemoriesFilterDialog SelectAuthor(string author)
		{
			CustomTestContext.WriteLine("Выбрать {0} в выпадающем списке авторов.", author);
			Author = Driver.SetDynamicValue(How.XPath, AUTHOR, author);
			Author.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по раскрывающемуся списку авторов
		/// </summary>
		public TranslationMemoriesFilterDialog ClickAuthorList()
		{
			CustomTestContext.WriteLine("Кликнуть по раскрывающемуся списку авторов");
			AuthorList.Click();

			return LoadPage();
		}

		/// <summary>
		/// Кликнуть по хидеру диалога создания фильтра, чтоб закрыть календарь.
		/// </summary>
		public TranslationMemoriesFilterDialog ClickFilterDialogHeader()
		{
			CustomTestContext.WriteLine("Кликнуть по хидеру диалога создания фильтра, чтоб закрыть календарь.");
			FilterHeaderInDialog.Click();

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Установить фильтр по автору
		/// </summary>
		/// <param name="author">автор</param>
		public TranslationMemoriesFilterDialog SetAuthorFilter(string author)
		{
			ClickAuthorList();
			SelectAuthor(author);
			ClickAuthorList();

			return LoadPage();
		}

		/// <summary>
		/// Установить фильтр по языку сорса
		/// </summary>
		/// <param name="language">язык сорса</param>
		public TranslationMemoriesFilterDialog SetSourceLanguageFilter(Language language)
		{
			ClickSourceLanguageList();
			SelectSourceLanguage(language);
			ClickSourceLanguageList();

			return LoadPage();
		}

		/// <summary>
		/// Установить фильтр по языку таргета
		/// </summary>
		/// <param name="language">язык таргета</param>
		public TranslationMemoriesFilterDialog SetTargetLanguageFilter(Language language)
		{
			ClickTargetLanguageList();
			SelectTargetLanguage(language);
			ClickTargetLanguageList();

			return LoadPage();
		}

		/// <summary>
		/// Установить фильтр по топику
		/// </summary>
		/// <param name="topicName">имя топика</param>
		public TranslationMemoriesFilterDialog SetTopicFilter(string topicName)
		{
			ClickTopicList();
			SelectTopic(topicName);
			ClickTopicList();

			return LoadPage();
		}

		/// <summary>
		/// Установить фильтр по группе проекта
		/// </summary>
		/// <param name="projectGroup">имя группы проекта</param>
		public TranslationMemoriesFilterDialog SetProjectGroupFilter(string projectGroup)
		{
			ClickProjectGroupList();
			SelectProjectGroup(projectGroup);
			ClickProjectGroupList();

			return LoadPage();
		}

		/// <summary>
		/// Установить фильтр по клиенту
		/// </summary>
		/// <param name="client">имя клиента</param>
		public TranslationMemoriesFilterDialog SetClientFilter(string client)
		{
			ClickClientList();
			SelectClient(client);
			ClickClientList();

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли диалог выбора фильтров
		/// </summary>
		public bool IsTranslationMemoriesFilterDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(FILTER_DIALOG));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CLEAR_FIELDS_BUTTON)]
		protected IWebElement ClearFieldsButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = APPLY_BUTTON)]
		protected IWebElement ApplyButton { get; set; }

		[FindsBy(How = How.XPath, Using = SOURCE_LANGUAGE_LIST)]
		protected IWebElement SourceLanguageList { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_LANGUAGE_LIST)]
		protected IWebElement TargetLanguageList { get; set; }

		[FindsBy(How = How.XPath, Using = TOPIC_LIST)]
		protected IWebElement TopicList { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUP_LIST)]
		protected IWebElement ProjectGroupList { get; set; }

		[FindsBy(How = How.XPath, Using = CREATION_DATE)]
		protected IWebElement CreationDate { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENT_LIST)]
		protected IWebElement ClientList { get; set; }

		[FindsBy(How = How.XPath, Using = AUTHOR_LIST)]
		protected IWebElement AuthorList { get; set; }

		[FindsBy(How = How.XPath, Using = FILTER_HEADER_IN_DIALOG)]
		protected IWebElement FilterHeaderInDialog { get; set; }

		protected IWebElement Author { get; set; }

		protected IWebElement SourceLanguage { get; set; }

		protected IWebElement TargetLanguage { get; set; }

		protected IWebElement Topic { get; set; }

		protected IWebElement ProjectGroup { get; set; }

		protected IWebElement Client { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string FILTER_DIALOG = "//div[contains(@class, 'js-filter-popup')]";
		protected const string CLEAR_FIELDS_BUTTON = "//a[contains(@class, 'js-clear-all')]";
		protected const string CANCEL_BUTTON = "//div[contains(@class, 'js-filter-popup')]//a[contains(@class, 'js-popup-close')]";
		protected const string APPLY_BUTTON = "//div[contains(@class, 'js-filter-popup')]//div[contains(@class, 'js-search-button')]";
		protected const string SOURCE_LANGUAGE_LIST = "//div[contains(@class, 'lang first')]/div";
		protected const string SOURCE_LANGUAGE = "//div[contains(@class, 'ui-multiselect-menu')][1]//input[@title='*#*']";
		protected const string TARGET_LANGUAGE = "//div[contains(@class, 'ui-multiselect-menu')][2]//input[@title='*#*']";
		protected const string TARGET_LANGUAGE_LIST = "//div[@class='l-filtersrc__control lang']/div";
		protected const string CREATION_DATE = "//input[contains(@class, 'js-from-date')]";
		protected const string TOPIC = "//div[@class='js-topics']/div/div//span[contains(@class,'nodetext') and text()='*#*']";
		protected const string TOPIC_LIST = "//div[@class='js-topics']/div/div";
		protected const string PROJECT_GROUP = "//div[contains(@class, 'ui-multiselect-menu')][4]//input[@title='*#*']";
		protected const string PROJECT_GROUP_LIST = "//div[@class='l-filtersrc__control'][2]/div";
		protected const string CLIENT = "//div[contains(@class, 'ui-multiselect-menu')][5]//input[@title='*#*']";
		protected const string CLIENT_LIST = "//div[@class='l-filtersrc__control'][3]/div";
		protected const string AUTHOR_LIST = "//div[@class='l-filtersrc__control creator']/div";
		protected const string AUTHOR = "//div[contains(@class, 'ui-multiselect-menu')][3]//input[@title='*#*']";
		protected const string FILTER_HEADER_IN_DIALOG = "//div[contains(@class, 'js-filter-popup')]//h2";
		#endregion
	}
}
