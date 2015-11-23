﻿using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class GlossaryEditStructureLanguageFieldsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpGlossaryEditStructureLanguageFieldsTests()
		{
			_glossaryHelper = new GlossariesHelper(Driver);
			_workspaceHelper = new WorkspaceHelper(Driver);

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
		[Test, Ignore("PRX-10924")]
		public void AddCommentFieldTest()
		{
			var comment = "Comment Example";

			_glossaryHelper
				.FillLanguageComment(comment)
				.SaveEntry()
				.AssertLanguageCommentIsFilled(comment);
		}

		[Test, Ignore("PRX-10924")]
		public void AddDefinitionFieldTest()
		{
			var definition = "Definition Example";

			_glossaryHelper
				.FillDefinition(definition)
				.SaveEntry()
				.AssertDefinitionFilled(definition);
		}

		[Test, Ignore("PRX-10924")]
		public void AddDefinitionSourceFieldTest()
		{
			var definitionSource = "Definition source example";

			_glossaryHelper
				.FillDefinitionSource(definitionSource)
				.SaveEntry()
				.AssertDefinitionSourceFilled(definitionSource);
		}

		private GlossariesHelper _glossaryHelper;
		private WorkspaceHelper _workspaceHelper;
		private string _glossaryUniqueName;
	}
}
