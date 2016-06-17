using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Coursera
{
	class CourseraEditorPage : EditorPage, IAbstractPage<CourseraEditorPage>
	{
		public CourseraEditorPage(WebDriver driver, bool needCloseTutorial = true) : base(driver, needCloseTutorial)
		{
			PageFactory.InitElements(Driver, this);
		}

		public CourseraEditorPage LoadPage()
		{
			if (!IsCourseraEditorPageOpened(_needCloseTutorial))
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся редактор в курсере.");
			}

			return this;
		}

		public CourseraEditorPage LoadPageFromAnotherPage()
		{
			Driver.WaitUntilElementIsDisplay(By.XPath(FINISH_TUTORIAL_BUTTON), 3);

			if (IsCloseTutorialButtonDisplay() && _needCloseTutorial)
			{
				CloseTutorial();
			}

			return LoadPage();
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку "Домой" для перехода на страницу курсов курсеры.
		/// </summary>
		public CoursesPage ClickHomeButtonExpectingCourseraCoursesPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Домой' для перехода на страницу курсов курсеры.");
			HomeButton.Click();

			return new CoursesPage(Driver).LoadPage();
		}
		
		/// <summary>
		/// Нажать кнопку удаления перевода пользователя
		/// </summary>
		/// <param name="author">автор</param>
		///  <param name="translation">перевод</param>
		public DeleteTranslationDialog ClickDeleteTranslateButton(string author, string translation)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления перевода {0} пользователя {1}.", translation, author);
			DeleteTranslateButton = Driver.SetDynamicValue(How.XPath, DELETE_TRANSLATE_BUTTON, author, translation);
			DeleteTranslateButton.Click();

			return new DeleteTranslationDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Проскролить до кнопки голосования Против.
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="translation">перевод</param>
		public CourseraEditorPage ScrollToVoteDownButton(string author, string translation)
		{
			CustomTestContext.WriteLine("Проскролить до кнопки голосования Против перевода '{0}' автора {1}.", translation, author);
			VoteDownButton = Driver.SetDynamicValue(How.XPath, VOTE_DOWN_BUTTON, author, translation);
			VoteDownButton.ScrollAndClickViaElementBlock();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку голосования Против.
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="translation">перевод</param>
		public CourseraEditorPage ClickVoteDownButton(string author, string translation)
		{
			CustomTestContext.WriteLine("Нажать кнопку голосования Против перевода '{0}' автора {1}.", translation, author);
			VoteDownButton = Driver.SetDynamicValue(How.XPath, VOTE_DOWN_BUTTON, author, translation);
			VoteDownButton.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Проскролить до кнопки голосования За.
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="translation">перевод</param>
		public CourseraEditorPage ScrollToVoteUpButton(string author, string translation)
		{
			CustomTestContext.WriteLine("Нажать кнопку голосования За перевода '{0}' автора {1}.", translation, author);
			VoteUpButton = Driver.SetDynamicValue(How.XPath, VOTE_UP_BUTTON, author, translation);
			VoteUpButton.Scroll();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку голосования За.
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="translation">перевод</param>
		public CourseraEditorPage ClickVoteUpButton(string author, string translation)
		{
			CustomTestContext.WriteLine("Нажать кнопку голосования За перевода '{0}' автора {1}.", translation, author);
			VoteUpButton = Driver.SetDynamicValue(How.XPath, VOTE_UP_BUTTON, author, translation);
			VoteUpButton.Click();

			return LoadPage();
		}
		
		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Проскролить и нажать кнопку голосования За.
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="translation">перевод</param>
		public CourseraEditorPage ScrollAndClickVoteUpButton(string author, string translation)
		{
			CustomTestContext.WriteLine("Нажать кнопку голосования За перевода '{0}' автора {1}.", translation, author);
			ScrollToVoteUpButton(author, translation);
			ClickVoteUpButton(author, translation);

			return LoadPage();
		}

		/// <summary>
		/// Проскролить и нажать кнопку голосования Против.
		/// </summary>
		/// <param name="author">автор</param>
		/// <param name="translation">перевод</param>
		public CourseraEditorPage ScrollAndClickVoteDownButton(string author, string translation)
		{
			CustomTestContext.WriteLine("Проскролить и нажать кнопку голосования Против перевода '{0}' автора {1}.", translation, author);
			ScrollToVoteDownButton(author, translation);
			ClickVoteDownButton(author, translation);

			return LoadPage();
		}

		/// <summary>
		/// Добавить перевод для прогресса
		/// </summary>
		/// <param name="translationText">перевод</param>
		public CoursesPage AddTranslationForCourseraProgress(string translationText, int rowNumber = 1)
		{
			FillTarget(translationText, rowNumber);
			ConfirmSegmentTranslation();
			ClickHomeButtonExpectingCourseraCoursesPage();

			return new CoursesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на таргет, проскролить и кликнуть кнопку голосования за перевод.
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="author">автор</param>
		/// <param name="translation">перевод</param>
		public CourseraEditorPage ClickOnTargetAndScrollAndClickVoteUpButton(string author, string translation, int segmentNumber = 1)
		{
			ClickOnTargetCellInSegment(segmentNumber);
			ScrollAndClickVoteUpButton(author, translation);

			return LoadPage();
		}

		/// <summary>
		/// Нажать на таргет, проскролить и кликнуть кнопку голосования против перевода.
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="author">автор</param>
		/// <param name="translation">перевод</param>
		public CourseraEditorPage ClickOnTargetAndScrollAndClickVoteDownButton(string author, string translation, int segmentNumber = 1)
		{
			ClickOnTargetCellInSegment(segmentNumber);
			ScrollAndClickVoteDownButton(author, translation);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить и подтвердить таргет
		/// </summary>
		/// <param name="translation">перевод</param>
		/// <param name="segmentNumber">номер сегмента</param>
		public CourseraEditorPage FillAndConfirmTarget(string translation, int segmentNumber = 1)
		{
			FillTarget(translation, segmentNumber);
			ConfirmSegmentTranslation();

			return LoadPage();
		}

		/// <summary>
		///  Удалить перевод
		/// </summary>
		/// <param name="translation">перевод</param>
		/// <param name="author">автор</param>
		/// <param name="segmentNumber">номер сегмента</param>
		public CourseraEditorPage DeleteTranslation(string translation, string author, int segmentNumber = 1)
		{
			ClickOnTargetCellInSegment(segmentNumber);
			ClickDeleteTranslateButton(author, translation);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли редактор
		/// </summary>
		public bool IsCourseraEditorPageOpened(bool closedTutorial = true)
		{
			if (!closedTutorial)
			{
				return IsSavingStatusDisappeared() &&
						Driver.WaitUntilElementIsAppear(By.XPath(SEGMENTS_BODY), timeout: 60);
			}
			else
			{
				return IsSavingStatusDisappeared() &&
						Driver.WaitUntilElementIsAppear(By.XPath(SEGMENTS_BODY), timeout: 60) &&
						Driver.WaitUntilElementIsDisappeared(By.XPath(EDITOR_DIALOG_BACKGROUND));
			}
		}

		#endregion

		#region Объявление элементов страницы

		protected IWebElement DeleteTranslateButton { get; set; }

		#endregion

		#region Описание XPath элементов страницы

		protected const string DELETE_TRANSLATE_BUTTON = "//div[@id='translations-body']//tbody//div[contains(text(), '*#*')]/ancestor::td//following-sibling::td//div[contains(text(), '*##*')]//ancestor::td//following-sibling::td//div[contains(@class, 'sci-delete')]";

		#endregion
	}
}
