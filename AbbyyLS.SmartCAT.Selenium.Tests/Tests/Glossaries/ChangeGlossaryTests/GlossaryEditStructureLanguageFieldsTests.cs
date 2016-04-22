using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Glossaries]
	class GlossaryEditStructureLanguageFieldsTests<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpGlossaryEditStructureLanguageFieldsTests()
		{
			_glossaryHelper = new GlossariesHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_glossaryHelper = new GlossariesHelper(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);

			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.GoToGlossariesPage();

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog
				.SelectLevelGlossaryStructure(GlossaryStructureLevel.Language)
				.AddLanguageFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();
		}

		///<summary>
		///Метод тестирования изменения структуры на уровне Languages - поле Comment
		///</summary>
		[Test]
		public void AddCommentFieldTest()
		{
			var comment = "Comment Example";

			_glossaryPage
				.AddLanguageComment(comment)
				.ClickSaveEntryButton()
				.OpenLanguageDetailsViewMode();

			Assert.IsTrue(_glossaryPage.IsCommentFilled(comment),
				"Произошла ошибка:\n неверный текст в поле комментария");
		}

		[Test]
		public void AddDefinitionFieldTest()
		{
			var definition = "Definition Example";

			_glossaryPage
				.AddDefinition(definition)
				.ClickSaveEntryButton()
				.OpenLanguageDetailsViewMode();

			Assert.IsTrue(_glossaryPage.IsDefinitionFilled(definition),
				"Произошла ошибка:\n неверный текст в поле Definition");
		}

		[Test]
		public void AddDefinitionSourceFieldTest()
		{
			var definitionSource = "Definition source example";

			_glossaryPage
				.AddDefinitionSource(definitionSource)
				.ClickSaveEntryButton()
				.OpenLanguageDetailsViewMode();

			Assert.IsTrue(_glossaryPage.IsDefinitionSourceFilled(definitionSource),
				"Произошла ошибка:\n неверный текст в поле 'Definition source'");
		}

		private GlossariesHelper _glossaryHelper;
		private WorkspacePage _workspacePage;
		private GlossaryPage _glossaryPage;
		private GlossaryStructureDialog _glossaryStructureDialog;
		private string _glossaryUniqueName;
	}
}
