using System;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages
{
	public class DistributeSegmentsBetweenAssigneesPage : ProjectsPage, IAbstractPage<DistributeSegmentsBetweenAssigneesPage>
	{
		public DistributeSegmentsBetweenAssigneesPage(WebDriver driver)
			: base(driver)
		{
		}

		public new DistributeSegmentsBetweenAssigneesPage LoadPage()
		{
			if (!IsDistributeSegmentsBetweenAssigneesPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Не открылась страница распределения сегментов между исполнителями.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Кликнуть по сегменту
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		public DistributeSegmentsBetweenAssigneesPage ClickSegment(int segmentNumber = 1)
		{
			CustomTestContext.WriteLine("Попытаться кликнуть по сегменту №{0}", segmentNumber);
			Driver.FindElement(By.XPath(SEGMENT.Replace("*#*", segmentNumber.ToString()))).Scroll();
			Segment = Driver.WaitUntilElementIsClickable(By.XPath(SEGMENT.Replace("*#*", segmentNumber.ToString())));

			if (Segment != null)
			{
				CustomTestContext.WriteLine("Кликнуть по сегменту №{0}", segmentNumber);
				Segment.Click();
			}
			else
			{
				throw new Exception(String.Format("Сегмент №{0} не стал кликабельным.", segmentNumber));
			}

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Remove.
		/// </summary>
		public DistributeSegmentsBetweenAssigneesPage ClickRemoveRangeButton(int rangeNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать на кнопку Remove.");
			RemoveRangeButton = Driver.SetDynamicValue(How.XPath, REMOVE_RANGE_BUTTON, rangeNumber.ToString());
			RemoveRangeButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Assign.
		/// </summary>
		public DistributeSegmentsBetweenAssigneesPage ClickAssignButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Assign.");
			AssignButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Change.
		/// </summary>
		/// <param name="rangeNumber">номер диапазона</param>
		public DistributeSegmentsBetweenAssigneesPage ClickChangeRangeButton(int rangeNumber)
		{
			CustomTestContext.WriteLine("Нажать на кнопку Change для диапазона №{0}.", rangeNumber);
			Driver.SetDynamicValue(How.XPath, CHANGE_RANGE_BUTTON, rangeNumber.ToString()).Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Begin With a Different Start Segment'.
		/// </summary>
		public DistributeSegmentsBetweenAssigneesPage ClickBeginWithDifferentStartSegmentButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Begin With a Different Start Segment'.");
			BeginWithDifferentStartSegmentButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку 'Begin With a Different End Segment'.
		/// </summary>
		public DistributeSegmentsBetweenAssigneesPage ClickBeginWithDifferentEndSegmentButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Begin With a Different Start Segment'.");
			BeginWithDifferentEndSegmentButton.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на кнопку Save.
		/// </summary>
		public DistributeDocumentBetweenAssigneesPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Save.");
			SaveButton.Click();

			return new DistributeDocumentBetweenAssigneesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать по 'Select End Of Range' в контекстном меню
		/// </summary>
		public DistributeSegmentsBetweenAssigneesPage ClickSelectEndOfRange()
		{
			CustomTestContext.WriteLine("Нажать по 'Select End Of Range' в контекстном меню.");
			SelectEndOfRange.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать по Assign в списке нераспределнных диапазанов
		/// </summary>
		/// <param name="rangeNumber">номер диапазона</param>
		public DistributeSegmentsBetweenAssigneesPage ClickAssignButtonInNotDistributedRange(int rangeNumber = 1)
		{
			CustomTestContext.WriteLine("Нажать по Assign в списке нераспределнных диапазанов.");
			Driver.SetDynamicValue(How.XPath, ASSIGN_BUTTON_IN_NOT_DISTRIBUTED_RANGE, rangeNumber.ToString()).Click();

			return LoadPage();
		}

		/// <summary>
		/// Получить количество сегментов в документе.
		/// </summary>
		public int GetSegmentsCountInDocumnent()
		{
			CustomTestContext.WriteLine("Получить количество сегментов в документе.");

			return Driver.GetElementsCount(By.XPath(ROWS_IN_TABLE));
		}

		/// <summary>
		/// Получить количество нераспределенных диапазонов.
		/// </summary>
		public int GetNotDistributedSegmentsRangeCount()
		{
			CustomTestContext.WriteLine("Получить количество нераспределенных диапазонов.");

			return Driver.GetElementsCount(By.XPath(NOT_DISTRIBUTED_SEGMENTS_RANGE_COUNT));
		}

		/// <summary>
		/// Получить количество распределенных диапазонов.
		/// </summary>
		public int GetDistributedSegmentsRangeCount()
		{
			CustomTestContext.WriteLine("Получить количество распределенных диапазонов.");

			return Driver.GetElementsCount(By.XPath(DISTRIBUTED_SEGMENTS_RANGE_COUNT));
		}

		/// <summary>
		/// Получить номер нераспределенного сегмента.
		/// </summary>
		public int GetNotDistributedSegmentNumber()
		{
			CustomTestContext.WriteLine("Получить номер нераспределенного сегмента.");
			int notDistributedSegmentNumber;

			if (!int.TryParse(NotDistributedSegmentNumber.Text, out notDistributedSegmentNumber))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование номера нераспределенного сегмента в число."));
			}
			
			return notDistributedSegmentNumber;
		}

		/// <summary>
		/// Получить номер первого и последнего сегмента для распределенного диапазона.
		/// </summary>
		/// <param name="rangeNumber">номер диапазона</param>
		public string GetDistributedRange(int rangeNumber = 1)
		{
			CustomTestContext.WriteLine("Получить номер первого и последнего сегмента для {0} распределенного диапазона.", rangeNumber);
			var DistributedStartRange = Driver.SetDynamicValue(How.XPath, DISTRIBUTED_START_RANGE, rangeNumber.ToString());
			var DistributedEndRange = Driver.SetDynamicValue(How.XPath, DISTRIBUTED_END_RANGE, rangeNumber.ToString());

			DistributedStartRange.Scroll();

			return DistributedStartRange.Text + "-" + DistributedEndRange.Text;
		}

		/// <summary>
		/// Получить номер первого нераспределенного сегмента.
		/// </summary>
		/// <param name="rangeNumber">номер диапазона</param>
		public string GetNotDistributedRange(int rangeNumber)
		{
			CustomTestContext.WriteLine("Получить номер первого нераспределенного сегмента для {0} нераспределенного диапазона.", rangeNumber);

			return 
				Driver.SetDynamicValue(How.XPath, NOT_DISTRIBUTED_START_RANGE, rangeNumber.ToString()).Text
				+ "-" + Driver.SetDynamicValue(How.XPath, NOT_DISTRIBUTED_END_RANGE, rangeNumber.ToString()).Text;
		}
		
		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Получить количество слов в выбранном диапазоне
		/// </summary>
		public int GetWordsCount()
		{
			CustomTestContext.WriteLine("Получить количество слов в выбранном диапазоне");
			var timer = 0;
			int count;

			while (WordsCount.Text == String.Empty && timer < 5)
			{
				Thread.Sleep(1000);
				timer++;
			}
			
			if (!int.TryParse(WordsCount.Text, out count))
			{
				throw new Exception(string.Format("Произошла ошибка:\n не удалось преобразование количества слов в число."));
			}

			return count;
		}

		/// <summary>
		/// Выбрать диапазон сегментов
		/// </summary>
		/// <param name="rangeStart">начальный сегмент</param>
		/// <param name="rangeEnd">конечный сегмент</param>
		public DistributeSegmentsBetweenAssigneesPage SelectSegmentsRange(int rangeStart = 1, int rangeEnd = 3)
		{
			CustomTestContext.WriteLine("Выбрать диапазон сегментов от {0} до {1}.", rangeStart.ToString(), rangeEnd.ToString());
			ClickSegment(rangeStart);
			ClickSelectEndOfRange();
			ClickSegment(rangeEnd);
			
			return LoadPage();
		}

		/// <summary>
		/// Назначить диапазон сегментов
		/// </summary>
		/// <param name="rangeStart">начальный сегмент</param>
		/// <param name="rangeEnd">конечный сегмент</param>
		public DistributeSegmentsBetweenAssigneesPage AssignSegmentsRange(int rangeStart = 1, int rangeEnd = 3)
		{
			SelectSegmentsRange(rangeStart, rangeEnd);
			Driver.WaitUntilElementIsDisplay(By.XPath(ASSIGN_BUTTON));
			ClickAssignButton();

			return LoadPage();
		}

		/// <summary>
		/// Поменять диапазон
		/// </summary>
		/// <param name="newRangeStart">первый сегмент</param>
		/// <param name="newRangeEnd">последний сегмент</param>
		/// <param name="rangeNumber">номер редактируемого диапазона</param>
		public DistributeSegmentsBetweenAssigneesPage ChangeRange(int rangeNumber, int newRangeStart, int newRangeEnd = 3)
		{
			CustomTestContext.WriteLine("Редактировать диапазон №{0}, поменять первый сегмент на {1} и поменять последний сегмент на {2}",
				rangeNumber, newRangeStart, newRangeEnd);
			ClickChangeRangeButton(rangeNumber);
			ClickBeginWithDifferentStartSegmentButton();
			ClickSegment(newRangeStart);
			BackButton.Click();
			ClickBeginWithDifferentEndSegmentButton();
			ClickSegment(newRangeEnd);
			ClickAssignButton();

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		public bool IsDistributeSegmentsBetweenAssigneesPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SEGMENTS_TABLE));
		}
		
		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SELECT_END_OF_RANGE)]
		protected IWebElement SelectEndOfRange { get; set; }

		[FindsBy(How = How.XPath, Using = ASSIGN_BUTTON)]
		protected IWebElement AssignButton { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = REMOVE_RANGE_BUTTON)]
		protected IWebElement RemoveRangeButton { get; set; }

		[FindsBy(How = How.XPath, Using = WORDS_COUNT)]
		protected IWebElement WordsCount { get; set; }
		
		[FindsBy(How = How.XPath, Using = NOT_DISTRIBUTED_SEGMENT_NUMBER)]
		protected IWebElement NotDistributedSegmentNumber { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_REASSIGNE_POP_UP_BUTTON)]
		protected IWebElement CancelReassignePopUpButton { get; set; }

		[FindsBy(How = How.XPath, Using = BEGIN_WITH_DIFFERENT_END_SEGMENT_BUTTON)]
		protected IWebElement BeginWithDifferentEndSegmentButton { get; set; }

		[FindsBy(How = How.XPath, Using = BEGIN_WITH_DIFFERENT_START_SEGMENT_BUTTON)]
		protected IWebElement BeginWithDifferentStartSegmentButton { get; set; }

		[FindsBy(How = How.XPath, Using = CHANGE_RANGE_BUTTON)]
		protected IWebElement ChangeRangeButton { get; set; }

		[FindsBy(How = How.XPath, Using = BACK_BUTTON)]
		protected IWebElement BackButton { get; set; }

		protected IWebElement DistributedStartRange { get; set; }
		protected IWebElement DistributedEndRange { get; set; }
		protected IWebElement Segment { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string SEGMENTS_TABLE = "//tbody[contains(@data-bind, 'segmentsFrame.items')]";
		protected const string SEGMENT = "//tr[*#*]//td[@class='segment-num']/following-sibling::td[1]";
		protected const string SELECT_END_OF_RANGE = "//div[contains(@data-bind, 'SelectRangeModes.EndOfRange')]";
		protected const string ASSIGN_BUTTON = "//a[contains(@data-bind, 'click: assign')]";
		protected const string BACK_BUTTON = "//a[contains(@data-bind, 'click: back')]";
		protected const string SAVE_BUTTON = "//div[contains(@data-bind, 'click: save')]";
		protected const string REMOVE_RANGE_BUTTON = "(//ul[contains(@class, 'ranges-list_assigned')]//li[contains(@class, 'ranges-item')]//span[contains(@class, 'icon_remove')])[*#*]";
		protected const string WORDS_COUNT = "//span[contains(@data-bind, 'range().wordsCount')]";
		protected const string ROWS_IN_TABLE = "//tbody[contains(@data-bind, 'segmentsFrame.items')]//tr";
		protected const string NOT_DISTRIBUTED_SEGMENT_NUMBER = "//ul[contains(@data-bind, 'notAssignedRanges')]//span[contains(@data-bind, 'goToSegment')]";
		protected const string NOT_DISTRIBUTED_SEGMENTS_RANGE_COUNT = "//ul[contains(@data-bind, 'notAssignedRanges')]//li";
		protected const string DISTRIBUTED_SEGMENTS_RANGE_COUNT = "//table[contains(@data-bind, 'foreach: assignedRanges')]//tbody";
		protected const string NOT_DISTRIBUTED_START_RANGE = "//ul[contains(@data-bind, 'notAssignedRanges')]//li[*#*]//span[contains(@data-bind, 'goToSegment(from)')]";
		protected const string NOT_DISTRIBUTED_END_RANGE = "//ul[contains(@data-bind, 'notAssignedRanges')]//li[*#*]//span[contains(@data-bind, 'goToSegment(to)')]";
		protected const string DISTRIBUTED_START_RANGE = "//ul[contains(@data-bind, 'foreach: assignedRanges')]//li[*#*]//span[contains(@data-bind, 'from')]";
		protected const string DISTRIBUTED_END_RANGE = "//ul[contains(@data-bind, 'foreach: assignedRanges')]//li[*#*]//span[contains(@data-bind, 'to')]";
		protected const string CHANGE_RANGE_BUTTON = "//tr[*#*]//a[contains(@data-bind, 'editRange')]";
		protected const string REASSIGNE_POP_UP = "//form[contains(@class, 'ajax-form-submit')]//div[contains(string(), 'Reassign the segments')]";
		protected const string CANCEL_REASSIGNE_POP_UP_BUTTON = "//form[contains(@class, 'ajax-form-submit')]//a[contains(@class, 'js-popup-close')]";

		protected const string ASSIGN_BUTTON_IN_NOT_DISTRIBUTED_RANGE = "//table[contains(@data-bind, 'notAssignedRanges')]//tbody[*#*]//a[contains(@data-bind, 'assignRange')]";

		protected const string BEGIN_WITH_DIFFERENT_START_SEGMENT_BUTTON = "//div[contains(@data-bind, '.SelectRangeModes.StartOfRange')]";
		protected const string BEGIN_WITH_DIFFERENT_END_SEGMENT_BUTTON = "//div[contains(@data-bind, '.SelectRangeModes.EndOfRange')]";

		#endregion
	}
}
