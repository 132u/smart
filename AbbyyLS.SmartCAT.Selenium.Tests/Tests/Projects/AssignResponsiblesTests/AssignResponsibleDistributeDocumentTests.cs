using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Projects]
	class AssignResponsibleDistributeDocumentTests<TWebDriverProvider> : AssignResponsiblesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void AssignResponsibleDistributeDocumentTestsSetUp()
		{
			_startRange = 1;
			_endRange = 3;
			
			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new []{ PathProvider.LongTxtFile });

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.LongTxtFile);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName)
				.SelectDistributeDocumentAssignmentType();
		}

		[Test(Description = "ТС-19")]
		[Standalone]
		public void OpenDistributeDocumentPageTest()
		{
			Assert.IsTrue(_distributeDocumentBetweenAssigneesPage.IsDistributeSegmentsBetweenAssigneesPageOpened(),
				"Произошла ошибка:\n Не открылась страница распределения сегментов между исполнителями.");
		}

		[Test(Description = "ТС-34")]
		[Standalone]
		public void AssignSegmentRangeOneUserTest()
		{
			_distributeDocumentBetweenAssigneesPage.ClickSelectSegmentsAndAssignLink();

			_distributeSegmentsBetweenAssigneesPage
				.AssignSegmentsRange(_startRange, _endRange)
				.ClickSaveButton();

			Assert.AreEqual(_startRange + "-" + _endRange, _distributeDocumentBetweenAssigneesPage.GetSegmentsRange(),
				"Произошла ошибка:\nНеверный диапазон сегментов в таблице.");

			Assert.AreEqual(ThreadUser.NickName, _distributeDocumentBetweenAssigneesPage.GetAssigneeName(),
				"Произошла ошибка:\nНеверное имя исполнителя в таблице.");

			Assert.IsTrue(_distributeDocumentBetweenAssigneesPage.IsDeleteAssigneeButtonDisplayed(ThreadUser.NickName),
				"Произошла ошибка:\nНе отображается кнопка удаления исполнителя.");
		}

		[Test(Description = "ТС-35")]
		[Standalone]
		public void AnotherAssigneeButtonTest()
		{
			_distributeDocumentBetweenAssigneesPage.ClickAnotherAssigneeButton();

			Assert.IsTrue(_distributeDocumentBetweenAssigneesPage.IsAssigneeDropdownDisplayed(),
				"Произошла ошибка:\nНе отображается кнопка выбора другого исполнителя.");
		}

		[Test(Description = "ТС-36")]
		[Standalone]
		public void DeleteAssigneeTest()
		{
			_distributeDocumentBetweenAssigneesPage
				.ClickDeleteButtonEditMode(rowNumber: 1)
				.ClickBackToTaskButton();

			Assert.IsTrue(_taskAssignmentPage.IsAssigneeDropdownDisplayed(),
				"Произошла ошибка:\nНе отображается кнопка выбора другого исполнителя.");

			Assert.IsTrue(_taskAssignmentPage.IsAssignedUserDisappeared(ThreadUser.NickName),
				"Произошла ошибка:\n назначений быть не должно.");
		}

		[Test]
		[Standalone]
		public void CancelAssigneeTest()
		{
			_distributeDocumentBetweenAssigneesPage.ClickSelectSegmentsAndAssignLink();

			_distributeSegmentsBetweenAssigneesPage
				.AssignSegmentsRange(_startRange, _endRange)
				.ClickRemoveRangeButton()
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(PathProvider.LongTxtFile);

			Assert.IsFalse(_editorPage.IsStageNameIsEmpty(),
				"Произошла ошибка:\n название этапа проставлено.");
		}

		[Test(Description = "ТС-37")]
		[Ignore("PRX-16792")]
		[Standalone]
		public void DistributeSegmentsBetweenFewAssigneesTest()
		{
			var startRangeForSecondAssignee = 4;
			var endRangeForSecondAssignee = 6;
			var startRangeForThirdAssignee = 7;
			var endRangeForThirdAssignee = 10;

			_secondUser = TakeUser(ConfigurationManager.Users);
			_thirdUser = TakeUser(ConfigurationManager.Users);

			_distributeDocumentBetweenAssigneesPage.ClickSelectSegmentsAndAssignLink();

			_distributeSegmentsBetweenAssigneesPage.SelectSegmentsRange(rangeStart: _startRange, rangeEnd: _endRange);
			var wordsCountForFirstAssignee = _distributeSegmentsBetweenAssigneesPage.GetWordsCount();

			_distributeSegmentsBetweenAssigneesPage
				.ClickAssignButton()
				.ClickSaveButton();

			_distributeDocumentBetweenAssigneesPage
				.SelectAssignee(_secondUser.NickName)
				.ClickSelectSegmentsAndAssignLink(assigneeNumber: 2);

			_distributeSegmentsBetweenAssigneesPage.SelectSegmentsRange(rangeStart: startRangeForSecondAssignee, rangeEnd: endRangeForSecondAssignee);
			var wordsCountForSecondAssignee = _distributeSegmentsBetweenAssigneesPage.GetWordsCount();

			_distributeSegmentsBetweenAssigneesPage
				.ClickAssignButton()
				.ClickSaveButton();

			_distributeDocumentBetweenAssigneesPage
				.SelectAssignee(_thirdUser.NickName)
				.ClickSelectSegmentsAndAssignLink(assigneeNumber: 3);

			_distributeSegmentsBetweenAssigneesPage.SelectSegmentsRange(rangeStart: startRangeForThirdAssignee, rangeEnd: endRangeForThirdAssignee);
			var wordsCountForThirdAssignee = _distributeSegmentsBetweenAssigneesPage.GetWordsCount();

			_distributeSegmentsBetweenAssigneesPage
				.ClickAssignButton()
				.ClickSaveButton();

			Assert.AreEqual(_startRange + "-" + _endRange, _distributeDocumentBetweenAssigneesPage.GetSegmentsRange(assigneeNumber: 3),
				"Произошла ошибка:\nНеверный диапазон сегментов в таблице.");

			Assert.AreEqual(startRangeForSecondAssignee + "-" + endRangeForSecondAssignee, _distributeDocumentBetweenAssigneesPage.GetSegmentsRange(assigneeNumber: 2),
				"Произошла ошибка:\nНеверный диапазон сегментов в таблице.");

			Assert.AreEqual(startRangeForThirdAssignee + "-" + endRangeForThirdAssignee, _distributeDocumentBetweenAssigneesPage.GetSegmentsRange(assigneeNumber: 1),
				"Произошла ошибка:\nНеверный диапазон сегментов в таблице.");

			Assert.AreEqual(wordsCountForFirstAssignee, _distributeDocumentBetweenAssigneesPage.GetWordsCount(assigneeNumber: 3),
				"Произошла ошибка:\n Неверное количество слов в строке №1.");

			Assert.AreEqual(wordsCountForSecondAssignee, _distributeDocumentBetweenAssigneesPage.GetWordsCount(assigneeNumber: 2),
				"Произошла ошибка:\n Неверное количество слов в строке №2.");

			Assert.AreEqual(wordsCountForThirdAssignee, _distributeDocumentBetweenAssigneesPage.GetWordsCount(assigneeNumber: 1),
				"Произошла ошибка:\n Неверное количество слов в строке №3.");
		}

		[Test(Description = "ТС-38")]
		[Standalone]
		public void OneSegmentNotDistributedTest()
		{
			_distributeDocumentBetweenAssigneesPage.ClickSelectSegmentsAndAssignLink();

			_distributeSegmentsBetweenAssigneesPage
				.AssignSegmentsRange(rangeStart: _startRange, rangeEnd: _distributeSegmentsBetweenAssigneesPage.GetSegmentsCountInDocumnent() - 1);

			Assert.AreEqual(
				_distributeSegmentsBetweenAssigneesPage.GetSegmentsCountInDocumnent(),
				_distributeSegmentsBetweenAssigneesPage.GetNotDistributedSegmentNumber(),
				"Произошла ошибка:\n Неверный номер нераспределенного сегмента.");

			_distributeSegmentsBetweenAssigneesPage.ClickSaveButton();
		}

		[Test(Description = "ТС-38")]
		[Standalone]
		public void TwoSegmentRangeNotDistributedTest()
		{
			var startSecondRange = _startRange + 5;
			var endSecondRange = _startRange + 8;

			_distributeDocumentBetweenAssigneesPage.ClickSelectSegmentsAndAssignLink();

			_distributeSegmentsBetweenAssigneesPage.AssignSegmentsRange(rangeStart: startSecondRange, rangeEnd: endSecondRange);

			Assert.AreEqual(2, _distributeSegmentsBetweenAssigneesPage.GetNotDistributedSegmentsRangeCount(),
				"Произошла ошибка:\n Неверное количество нераспределнных диапазонов.");

			Assert.AreEqual(_startRange + "-" + (startSecondRange - 1),
				_distributeSegmentsBetweenAssigneesPage.GetNotDistributedRange(rangeNumber: 1),
				"Произошла ошибка:\nНеверный первый нераспрделенный диапазон.");

			Assert.AreEqual((endSecondRange + 1) + "-" + _distributeSegmentsBetweenAssigneesPage.GetSegmentsCountInDocumnent(),
				_distributeSegmentsBetweenAssigneesPage.GetNotDistributedRange(rangeNumber: 2),
				"Произошла ошибка:\nНеверный второй нераспрделенный диапазон.");

			_distributeSegmentsBetweenAssigneesPage.ClickSaveButton();
		}

		[Test(Description = "ТС-48")]
		[Ignore("PRX-16792")]
		[Standalone]
		public void CancelReassigneSegmentsBetweenAssigneesTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);
			_thirdUser = TakeUser(ConfigurationManager.Users);

			_distributeDocumentBetweenAssigneesPage.ClickSelectSegmentsAndAssignLink();

			_distributeSegmentsBetweenAssigneesPage
				.AssignSegmentsRange(_startRange, _endRange)
				.ClickSaveButton();

			_distributeDocumentBetweenAssigneesPage
				.SelectAssignee(_secondUser.NickName)
				.ClickSelectSegmentsAndAssignLink(assigneeNumber: 2);

			_distributeSegmentsBetweenAssigneesPage
				.AssignSegmentsRange(_endRange + 1, _endRange + 4)
				.ClickSaveButton();
			
			_distributeDocumentBetweenAssigneesPage
				.SelectAssignee(_thirdUser.NickName)
				.ClickSelectSegmentsAndAssignLink(assigneeNumber: 3);

			_distributeSegmentsBetweenAssigneesPage.AssignSegmentsRange(_startRange + 1, _endRange + 3);
				
			Assert.IsTrue(
				_reassigneDialog.IsReassigneDialogOpened(),
				"Произошла ошибка:\n Не открылось окно переназначения сегментов.");

			_reassigneDialog.ClickCancelReassignePopUpButton();

			Assert.IsTrue(
				_reassigneDialog.IsReassignePopUpDisappeared(),
				"Произошла ошибка:\n Не закрылось окно переназначения сегментов.");
		}

		[Test(Description = "ТС-48")]
		[Ignore("PRX-16792")]
		[Standalone]
		public void ContinueReassigneSegmentsBetweenAssigneesTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);
			_thirdUser = TakeUser(ConfigurationManager.Users);

			_distributeDocumentBetweenAssigneesPage
				.ClickSelectSegmentsAndAssignLink();

			_distributeSegmentsBetweenAssigneesPage
				.AssignSegmentsRange(_startRange, _endRange)
				.ClickSaveButton();

			_distributeDocumentBetweenAssigneesPage
				.SelectAssignee(_secondUser.NickName)
				.ClickSelectSegmentsAndAssignLink(assigneeNumber: 2);

			_distributeSegmentsBetweenAssigneesPage
				.AssignSegmentsRange(_endRange + 1, _endRange + 4)
				.ClickSaveButton();

			_distributeDocumentBetweenAssigneesPage
				.SelectAssignee(_thirdUser.NickName)
				.ClickSelectSegmentsAndAssignLink(assigneeNumber: 3);

			_distributeSegmentsBetweenAssigneesPage.AssignSegmentsRange(_startRange + 1, _endRange + 3);

			Assert.IsTrue(
				_reassigneDialog.IsReassigneDialogOpened(),
				"Произошла ошибка:\n Не открылось окно переназначения сегментов.");

			_reassigneDialog.ClickContinueReassignePopUpButton();

			Assert.AreEqual((_startRange + 1) + "-" + (_endRange + 3), 
				_distributeSegmentsBetweenAssigneesPage.GetDistributedRange());
		}

		[Test(Description = "ТС-411")]
		[Standalone]
		public void AssigneAutomaticallyGeneratedRangeFromNotDistributedTest()
		{
			_distributeDocumentBetweenAssigneesPage
				.ClickSelectSegmentsAndAssignLink();

			_distributeSegmentsBetweenAssigneesPage
				.AssignSegmentsRange(_startRange, _endRange)
				.AssignSegmentsRange(_distributeSegmentsBetweenAssigneesPage.GetSegmentsCountInDocumnent() - 4, _distributeSegmentsBetweenAssigneesPage.GetSegmentsCountInDocumnent());

			var distributedRangeCount = _distributeSegmentsBetweenAssigneesPage.GetDistributedSegmentsRangeCount();
			var notDistributedRangeCount = _distributeSegmentsBetweenAssigneesPage.GetNotDistributedSegmentsRangeCount();

			_distributeSegmentsBetweenAssigneesPage.ClickAssignButtonInNotDistributedrange();

			Assert.AreEqual(distributedRangeCount - 1, 1,
				"Произошла ошибка:\n Неверное количество распределенных диапазонов.");

			Assert.AreEqual(notDistributedRangeCount - 1, _distributeSegmentsBetweenAssigneesPage.GetNotDistributedSegmentsRangeCount(),
				"Произошла ошибка:\n Неверное количество нераспределенных диапазонов.");
		}

		[Test(Description = "ТС-413")]
		[Standalone]
		public void ChangeDistributedRangeTest()
		{
			var newStartRange = _startRange + 2;
			var newEndRange = _endRange +5;

			_distributeDocumentBetweenAssigneesPage
				.ClickSelectSegmentsAndAssignLink();

			_distributeSegmentsBetweenAssigneesPage
				.AssignSegmentsRange(_startRange, _endRange)
				.ChangeRange(rangeNumber: 1, newRangeStart: newStartRange, newRangeEnd: newEndRange);

			Assert.AreEqual(newStartRange + "-" + newEndRange, _distributeSegmentsBetweenAssigneesPage.GetDistributedRange(rangeNumber: 1),
				"Произошла ошибка:\n Неверный распределенный диапазон после редактирования.");
		}

		private int _startRange;
		private int _endRange;
	}
}
