using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	public class GlossaryEditStructureTermFieldsTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_workspaceHelper = new WorkspaceHelper(Driver);

			_glossaryHelper = _workspaceHelper
				.GoToGlossariesPage()
				.CreateGlossary(_glossaryUniqueName);

			_glossaryPage = new GlossaryPage(Driver).GetPage();

			_glossaryHelper
				.OpenGlossaryStructure()
				.SelectLevelGlossaryStructure(GlossaryStructureLevel.Term);

		}

		[TestCase(GlossarySystemField.Source)]
		[TestCase(GlossarySystemField.Interpretation)]
		[TestCase(GlossarySystemField.InterpretationSource)]
		[TestCase(GlossarySystemField.Context)]
		[TestCase(GlossarySystemField.ContextSource)]
		public void AddSystemTermFieldTest(GlossarySystemField termField)
		{
			var termValue = "testTermSystemField";

			_glossaryHelper
				.AddTermField(termField)
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsTermFieldEditModeDisplayed(termField),
				"Произошла ошибка:\nПоле {0} не отображается в режиме редактирования термина.", termField);

			_glossaryHelper
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
			_glossaryHelper
				.AddTermField(termField)
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsDropdownTermFieldEditModeDisplayed(termField),
				"Произошла ошибка:\nПоле {0} не отображается в режиме редактирования термина.", termField);

			var option = _glossaryHelper.OptionValueText(termField);

			_glossaryHelper
				.SpecifyDropdownTermField(termField, option)
				.ClickSaveEntryButton()
				.ClickTermInLanguagesAndTermsColumn();

			Assert.AreEqual(option, _glossaryPage.DropdownTermFieldViewModelText(termField),
				"Произошла ошибка:\nВ поле {0} неверное значение.", termField);
		}

		private WorkspaceHelper _workspaceHelper;
		private GlossariesHelper _glossaryHelper;
		private string _glossaryUniqueName;
		private GlossaryPage _glossaryPage;
	}
}