using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Standalone]
	[Parallelizable(ParallelScope.Fixtures)]
	public class GlossaryEditStructureTermFieldsTest<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_glossariesHelper = new GlossariesHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);
			_glossaryPage = new GlossaryPage(Driver);

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(glossaryUniqueName);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.SelectLevelGlossaryStructure(GlossaryStructureLevel.Term);

		}

		[TestCase(GlossarySystemField.Source)]
		[TestCase(GlossarySystemField.Interpretation)]
		[TestCase(GlossarySystemField.InterpretationSource)]
		[TestCase(GlossarySystemField.Context)]
		[TestCase(GlossarySystemField.ContextSource)]
		public void AddSystemTermFieldTest(GlossarySystemField termField)
		{
			var termValue = "testTermSystemField";

			_glossaryStructureDialog.AddTermField(termField);				

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsTermFieldEditModeDisplayed(termField),
				"Произошла ошибка:\nПоле {0} не отображается в режиме редактирования термина.", termField);

			_glossaryPage
				.FillTermField(termField, termValue)
				.ClickSaveEntryButton()
				.ClickTermInLanguagesAndTermsColumn();

			Assert.AreEqual(termValue, _glossaryPage.TermFieldViewModelText(termField),
				"Произошла ошибка:\nВ поле {0} неверное значение.", termField);
		}

		[TestCase(GlossarySystemField.Label)]
		[TestCase(GlossarySystemField.Gender)]
		[TestCase(GlossarySystemField.Number)]
		[TestCase(GlossarySystemField.PartOfSpeech)]
		public void AddDropdownSystemTermFieldTest(GlossarySystemField termField)
		{
			_glossaryStructureDialog.AddTermField(termField);

			_glossaryPage				
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsDropdownTermFieldEditModeDisplayed(termField),
				"Произошла ошибка:\nПоле {0} не отображается в режиме редактирования термина.", termField);

			var option = _glossaryPage.GetOptionValue(termField);

			_glossaryPage
				.SpecifyDropdownTermField(termField, option)
				.ClickSaveEntryButton()
				.ClickTermInLanguagesAndTermsColumn();

			Assert.AreEqual(option, _glossaryPage.GetDropdownTermFieldViewModelText(termField),
				"Произошла ошибка:\nВ поле {0} неверное значение.", termField);
		}

		private WorkspacePage _workspacePage;
		private GlossaryPage _glossaryPage;
		private GlossaryStructureDialog _glossaryStructureDialog;

		private GlossariesHelper _glossariesHelper;
	}
}