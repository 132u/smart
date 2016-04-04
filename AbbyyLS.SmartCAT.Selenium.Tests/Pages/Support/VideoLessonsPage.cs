using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Support
{
	public class VideoLessonsPage : WorkspacePage, IAbstractPage<VideoLessonsPage>
	{
		public VideoLessonsPage(WebDriver driver)
			: base(driver)
		{
		}

		public new VideoLessonsPage LoadPage()
		{
			if (!IsVideoLessonsPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылась страница видеоуроков.");
			}

			return this;
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница
		/// </summary>
		public bool IsVideoLessonsPageOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(VIDEO_LESSONS));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = VIDEO_LESSONS)]
		protected IWebElement VideoLessons { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string VIDEO_LESSONS = "//div[contains(@class, 'g-top-nav')]//span['Video Lessons']";

		#endregion
	}
}