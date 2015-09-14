using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Standalone]
	class GlossaryEditStructureLanguageFieldsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpGlossaryEditStructureLanguageFieldsTests()
		{
			_workspaceHelper = new WorkspaceHelper();
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_glossaryHelper = _workspaceHelper
								.GoToGlossariesPage()
								.CreateGlossary(_glossaryUniqueName)
								.OpenGlossaryStructure()
								.SelectLevelGlossaryStructure(GlossaryStructureLevel.Language)
								.AddLanguageFields()
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

			_glossaryHelper
				.FillLanguageComment(comment)
				.ClickSaveEntryButton()
				.AssertLanguageCommentIsFilled(comment);
		}

		[Test]
		public void AddDefinitionFieldTest()
		{
			var definition = "Definition Example";

			_glossaryHelper
				.FillDefinition(definition)
				.ClickSaveEntryButton()
				.AssertDefinitionFilled(definition);
		}

		[Test]
		public void AddDefinitionSourceFieldTest()
		{
			var definitionSource = "Definition source example";

			_glossaryHelper
				.FillDefinitionSource(definitionSource)
				.ClickSaveEntryButton()
				.AssertDefinitionSourceFilled(definitionSource);
		}

		private GlossariesHelper _glossaryHelper = new GlossariesHelper();
		private WorkspaceHelper _workspaceHelper;
		private string _glossaryUniqueName;
	}
}
