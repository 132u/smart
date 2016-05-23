using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.QualityAssurance
{
	[Parallelizable(ParallelScope.Fixtures)]
	class QualityAssuranceNavigationTests<TWebDriverProvider> : QualityAssuranceBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-10531"), ShortCheckList] // TODO немного переделать xpath-ы стрелок после фикса PRX-16527
		public void TerminologicalErrorsNavigationTest()
		{
			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			string[] term1 = {"sentence", "предложение"};
			string[] term2 = {"phrase", "фраза"};
			string[] term3 = {"second", "второй"};

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(glossaryUniqueName);

			_glossaryPage
				.CreateTerm(term1[0], term1[1])
				.CreateTerm(term2[0], term2[1])
				.CreateTerm(term3[0], term3[1]);

			_workspacePage.GoToProjectsPage();
			_createProjectHelper.CreateNewProject(_projectUniqueName, filesPaths: new[] { PathProvider.QANavigationFile });

			_projectsPage.OpenProjectSettingsPage(_projectUniqueName);

			_projectSettingsHelper.AssignTasksOnDocument(PathProvider.QANavigationFile, ThreadUser.NickName, _projectUniqueName);
			
			_projectSettingsPage
				.SelectGlossaryByName(glossaryUniqueName)
				.OpenDocumentInEditorWithTaskSelect(PathProvider.QANavigationFile);

			_selectTaskDialog.SelectTask();

			_editorPage
				.FillTarget(term3[1], rowNumber: 1)
				.ConfirmSegmentTranslation()
				.FillTarget(term1[1] + " " + term2[1], rowNumber: 2)
				.ConfirmSegmentTranslation()
				.FillTarget(term2[1] + " " + term3[1], rowNumber: 3)
				.ConfirmSegmentTranslation()
				.ClickOnTargetCellInSegment()
				.ClickQualityAssuranceCheckTab();

			Assert.IsTrue(_editorPage.IsTerminilogyErrorDisplayed(term1[0]),
				"Произошла ошибка: отсутствует терминологическая ошибка термина {0}", term1[0]);

			Assert.IsTrue(_editorPage.IsTerminilogyErrorDisplayed(term2[0]),
				"Произошла ошибка: отсутствует терминологическая ошибка термина {0}", term2[0]);

			_editorPage.ClickNextTerminologyErrorArrow(term1[0]);

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: 3),
				"Произошла ошибка: третий сегмент не выделен.");

			_editorPage.ClickNextTerminologyErrorArrow(term1[0]);

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: 1),
				"Произошла ошибка: первый сегмент не выделен.");

			_editorPage.ClickNextTerminologyErrorArrow(term2[0]);

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: 1),
				"Произошла ошибка: первый сегмент не выделен.");

			_editorPage
				.ClickOnTargetCellInSegment(rowNumber: 2)
				.ClickNextTerminologyErrorArrow(term3[0]);

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: 2),
				"Произошла ошибка: второй сегмент не выделен.");

			_editorPage.ClickPreviousTerminologyErrorArrow(term3[0]);

			Assert.IsTrue(_editorPage.IsSegmentSelected(segmentNumber: 2),
				"Произошла ошибка: второй сегмент не выделен.");
		}
	}
}
