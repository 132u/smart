using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	class EditorCommentsTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void SetupTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_document = PathProvider.EditorTxtFile;

			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { _document });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(_document, ThreadUser.NickName, _projectUniqueName);
			_projectSettingsHelper.AssignTasksOnDocument(_document, _secondUser.NickName, _projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_document);

			_selectTaskDialog.SelectTask();
		}

		[Test, Description("S-7227"), ShortCheckList]
		public void AddSegmentCommentTest()
		{
			_editorPage
				.ClickSegmentCommentTab()
				.SendSegmentComment(_comment);

			Assert.IsTrue(_editorPage.IsCommentDisplayed(ThreadUser.NickName, _comment),
				"Произошла ошибка: не отображается комментарий '{0}'.", _comment);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();
			_workspacePage.SignOut();

			_loginHelper.Authorize(StartPage.Workspace, _secondUser);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_document);

			_selectTaskDialog.SelectTask();

			Assert.IsTrue(_editorPage.IsSegmentCommentTabHighlighted(),
				"Произошла ошибка: вкладка комментариев сегмента не подсвечена.");

			_editorPage.ClickSegmentCommentTab();

			Assert.IsTrue(_editorPage.IsCommentDisplayed(ThreadUser.NickName, _comment),
				"Произошла ошибка: не отображается комментарий '{0}'.", _comment);
		}

		[Test, Description("S-7228"), ShortCheckList]
		public void AddDocumentCommentTest()
		{
			_editorPage
				.ClickDocumentCommentTab()
				.SendDocumentComment(_comment);

			Assert.IsTrue(_editorPage.IsCommentDisplayed(ThreadUser.NickName, _comment),
				"Произошла ошибка: не отображается комментарий '{0}'.", _comment);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.SignOut();

			_loginHelper.Authorize(StartPage.Workspace, _secondUser);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_document);

			_selectTaskDialog.SelectTask();

			Assert.IsTrue(_editorPage.IsDocumentCommentTabHighlighted(),
				"Произошла ошибка: вкладка комментариев документа не подсвечена.");

			_editorPage.ClickDocumentCommentTab();

			Assert.IsTrue(_editorPage.IsCommentDisplayed(ThreadUser.NickName, _comment),
				"Произошла ошибка: не отображается комментарий '{0}'.", _comment);
		}


		[Test, Description("S-11725"), ShortCheckList]
		public void DeleteDocumentCommentTest()
		{
			_editorPage
				.ClickDocumentCommentTab()
				.SendDocumentComment(_comment)
				.DeleteComment(ThreadUser.NickName, _comment);

			Assert.IsTrue(_editorPage.IsCommentNotDisplayed(ThreadUser.NickName, _comment),
				"Произошла ошибка: отображается комментарий '{0}'.", _comment);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_workspacePage.SignOut();

			_loginHelper.Authorize(StartPage.Workspace, _secondUser);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_document);

			_selectTaskDialog.SelectTask();

			_editorPage.ClickDocumentCommentTab();

			Assert.IsTrue(_editorPage.IsCommentNotDisplayed(ThreadUser.NickName, _comment),
				"Произошла ошибка: отображается комментарий '{0}'.", _comment);
		}

		[Test, ShortCheckList] //TODO Добавить Description после разделения кейса S-11725
		public void DeleteSegmentCommentTest()
		{
			_editorPage
				.ClickSegmentCommentTab()
				.SendSegmentComment(_comment)
				.DeleteComment(ThreadUser.NickName, _comment);

			Assert.IsTrue(_editorPage.IsCommentNotDisplayed(ThreadUser.NickName, _comment),
				"Произошла ошибка: отображается комментарий '{0}'.", _comment);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();
			_workspacePage.SignOut();

			_loginHelper.Authorize(StartPage.Workspace, _secondUser);

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithTaskSelect(_document);

			_selectTaskDialog.SelectTask();

			_editorPage.ClickSegmentCommentTab();

			Assert.IsTrue(_editorPage.IsCommentNotDisplayed(ThreadUser.NickName, _comment),
				"Произошла ошибка: отображается комментарий '{0}'.", _comment);
		}

		[Test, Description("S-7230"), ShortCheckList]
		public void PasteToCommentTest()
		{
			var text = _editorPage.GetSourceText(rowNumber: 1);

			_editorPage
				.ClickDocumentCommentTab()
				.SelectAllTextByHotkeyInSource(segmentNumber: 1)
				.CopyByHotkey()
				.ClickDocumentCommentTextarea()
				.PasteByHotkey()
				.ClickSendDocumentCommentButton();

			Assert.IsTrue(_editorPage.IsCommentDisplayed(ThreadUser.NickName, text),
				"Произошла ошибка: не отображается комментарий '{0}'.", _comment);
		}
		
		[TearDown]
		public void TestFixtureTearDown()
		{
			if (_secondUser != null)
			{
				ReturnUser(ConfigurationManager.Users, _secondUser);
			}
		}

		private TestUser _secondUser;
		private const string _comment = "comment";
	}
}
