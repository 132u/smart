using System.Linq;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor.SegmentsPanelTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	class FilterSegmentsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_editorPage = new EditorPage(Driver);

			var projectName = _createProjectHelper.GetProjectUniqueName();
			var document = PathProvider.EditorFilterFile;

			_createProjectHelper.CreateNewProject(
				projectName: projectName,
				filesPaths: new[] { document });

			_projectsPage
				.OpenProjectInfo(projectName)
				.ClickDocumentRefExpectingEditorPage(document);
		}

		[Test]
		public void FilterSegmentsBySourceTest()
		{
			var word = "is";

			_editorPage.FilterSegmentsBySource(word);

			Assert.AreEqual(3, _editorPage.GetVisibleSegmentsCount(),
				"Произошла ошибка: не верное кол-во сегментов в выдаче.");
			
			Assert.Contains(word, _editorPage.GetSourceText(rowNumber: 1).ToLower().Split(' ').ToList(),
				"Произошла ошибка: сегменты сорса не содержат искомое слово.");

			Assert.Contains(word, _editorPage.GetSourceText(rowNumber: 2).ToLower().Split(' ').ToList(),
				"Произошла ошибка: сегменты сорса не содержат искомое слово.");

			Assert.Contains(word, _editorPage.GetSourceText(rowNumber: 3).ToLower().Split(' ').ToList(),
				"Произошла ошибка: сегменты сорса не содержат искомое слово.");
		}

		[Test]
		public void FilterSegmentsByTargetTest()
		{
			var word = "translation";

			_editorPage
				.FillSegmentTargetField(rowNumber: 1)
				.FillSegmentTargetField(rowNumber: 2)
				.FilterSegmentsByTarget(word);

			Assert.AreEqual(2, _editorPage.GetVisibleSegmentsCount(),
				"Произошла ошибка: не верное кол-во сегментов в выдаче.");

			Assert.Contains(word, _editorPage.GetTargetText(rowNumber: 1).ToLower().Split(' ').ToList(),
				"Произошла ошибка: сегменты сорса не содержат искомое слово.");

			Assert.Contains(word, _editorPage.GetTargetText(rowNumber: 2).ToLower().Split(' ').ToList(),
				"Произошла ошибка: сегменты сорса не содержат искомое слово.");
		}

		[Test, Description("S-7250"), ShortCheckList]
		public void ReplaceTest()
		{
			var word = "translation";
			var wordForReplace = "replace";

			_editorPage
				.FillSegmentTargetField(rowNumber: 1)
				.OpenReplaceMenu()
				.FilterSegmentsByTarget(word)
				.FillTextForReplace(wordForReplace)
				.ClickReplaceButton();

			Assert.AreEqual(1, _editorPage.GetVisibleSegmentsCount(),
				"Произошла ошибка: не верное кол-во сегментов в выдаче.");

			Assert.Contains(wordForReplace, _editorPage.GetTargetText(rowNumber: 1).ToLower().Split(' ').ToList(),
				"Произошла ошибка: сегменты сорса не содержат искомое слово.");
		}

		[Test, Description("S-7251"), ShortCheckList]
		public void ReplaceAllTest()
		{
			var word = "translation";
			var wordForReplace = "replace";

			_editorPage
				.FillSegmentTargetField(rowNumber: 1)
				.FillSegmentTargetField(rowNumber: 2)
				.OpenReplaceMenu()
				.FilterSegmentsByTarget(word)
				.FillTextForReplace(wordForReplace)
				.ClickReplaceAllButton();

			Assert.AreEqual(2, _editorPage.GetVisibleSegmentsCount(),
				"Произошла ошибка: не верное кол-во сегментов в выдаче.");

			Assert.Contains(wordForReplace, _editorPage.GetTargetText(rowNumber: 1).ToLower().Split(' ').ToList(),
				"Произошла ошибка: сегменты сорса не содержат искомое слово.");

			Assert.Contains(wordForReplace, _editorPage.GetTargetText(rowNumber: 2).ToLower().Split(' ').ToList(),
				"Произошла ошибка: сегменты сорса не содержат искомое слово.");
		}

		protected CreateProjectHelper _createProjectHelper;
		protected ProjectsPage _projectsPage;
		protected EditorPage _editorPage;
	}
}
