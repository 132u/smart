using System;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Glossaries]
	public class EditTermTests<TWebDriverSettings>
		: BaseGlossaryTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);
		}

		[Test]
		public void EditDefaultTerm()
		{
			var term1 = "Term 1";
			var term2 = "Term 2";

			_glossaryPage
				.CreateTerm(term1, term2)
				.EditDefaultTerm(term1, term2, term1 + DateTime.Now);

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 1),
				"Произошла ошибка:\n неверное количество терминов");
		}

		[Test]
		public void EditCustomTerm()
		{
			var newTerm = "Term Example" + DateTime.Now;
			var languages = new List<Language> 
			{
				Language.German,
				Language.French,
				Language.Japanese,
				Language.Lithuanian
			};

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddNewSystemField(GlossarySystemField.Topic);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton()
				.EditCustomTerms(newTerm);

			Assert.IsTrue(_glossaryPage.IsTermsTextMatchExpected(newTerm),
				"Произошла ошибка:\n один или более терминов не соответствуют ожидаемому значению");

			Assert.IsTrue(_glossaryPage.IsTermDisplayedInLanguagesAndTermsSection(newTerm),
				"Произошла ошибка:\n Термин {0} отсутствует в секции 'Languages And Terms'.", newTerm);
		}
	}
}
