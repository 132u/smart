using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Support;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.SupportTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	class WorkspaceSupportButtonsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> 
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetupWorkspaceSupportButtonsTests()
		{
			_workspacePage = new WorkspacePage(Driver);
			_helpPage = new HelpPage(Driver);
			_videoLessonsPage = new VideoLessonsPage(Driver);
			_supportFeedbackPage = new SupportFeedbackPage(Driver);
			_feedbackDialog = new FeedbackDialog(Driver);
		}

		[Test]
		public void HelpPageTest()
		{
			_workspacePage.ClickHelpMenuButton();
			_workspacePage.ClickHelpPageInNewTab();

			Assert.IsTrue(_helpPage.IsHelpPageOpened(), 
				"Произошла ошибка: \n не открылась страница 'Справка'");
		}

		[Test]
		public void VideoLessonPageTest()
		{
			_workspacePage.ClickHelpMenuButton();
			_workspacePage.ClickVideoLessonsPage();

			Assert.IsTrue(_videoLessonsPage.IsVideoLessonsPageOpened(), 
				"Произошла ошибка: \n не открылась страница 'Видеоуроки'");
		}

		[Test]
		public void SupportFeedbackPageTest()
		{
			_workspacePage.ClickHelpMenuButton();
			_workspacePage.ClickSupportFeedbackPage();

			Assert.IsTrue(_supportFeedbackPage.IsSupportFeedbackPageOpened(), 
				"Произошла ошибка: \n не открылась страница 'Ответы техподдержки'");
		}

		[Test]
		public void FeedbackDialogTest()
		{
			_workspacePage.ClickHelpMenuButton();
			_workspacePage.ClickSupportFeedbackDialog();

			Assert.IsTrue(_feedbackDialog.IsSupportFeedbackDialogOpened(), 
				"Произошла ошибка: \n не открылся диалог 'Обратиться в техподдержку'");
		}

		private WorkspacePage _workspacePage;
		private HelpPage _helpPage;
		private VideoLessonsPage _videoLessonsPage;
		private SupportFeedbackPage _supportFeedbackPage;
		private FeedbackDialog _feedbackDialog;
	}
}