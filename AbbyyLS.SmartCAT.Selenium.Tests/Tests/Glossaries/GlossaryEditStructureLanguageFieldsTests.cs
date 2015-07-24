using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	class GlossaryEditStructureLanguageFieldsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		[Standalone]
		public void SetUpGlossaryEditStructureLanguageFieldsTests()
		{
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_glossaryHelper = WorkspaceHelper
								.GoToGlossariesPage()
								.CreateGlossary(_glossaryUniqueName);
		}

		///<summary>
		///Метод тестирования изменения структуры на уровне Languages - поле Comment
		///</summary>
		[Test]
		public void AddCommentFieldTest()
		{
			var comment = "Comment Example";

			_glossaryHelper
				.OpenGlossaryStructure()
				.SelectLevelGlossaryStructure(GlossaryStructureLevel.Language)
				.AddLanguageFields()
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.FillLanguageComment(comment)
				.ClickSaveTermButton()
				.AssertLanguageCommentIsFilled(comment);
		}

		[Test]
		public void AddDefinitionFieldTest()
		{
			var definition = "Definition Example";

			_glossaryHelper
				.OpenGlossaryStructure()
				.SelectLevelGlossaryStructure(GlossaryStructureLevel.Language)
				.AddLanguageFields()
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.FillDefinition(definition)
				.ClickSaveTermButton()
				.AssertDefinitionFilled(definition);
		}

		[Test]
		public void AddDefinitionSourceFieldTest()
		{
			var definitionSource = "Definition source example";

			_glossaryHelper
				.OpenGlossaryStructure()
				.SelectLevelGlossaryStructure(GlossaryStructureLevel.Language)
				.AddLanguageFields()
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.FillDefinitionSource(definitionSource)
				.ClickSaveTermButton()
				.AssertDefinitionSourceFilled(definitionSource);
		}

		private GlossariesHelper _glossaryHelper = new GlossariesHelper();
		private string _glossaryUniqueName;
	}
}
