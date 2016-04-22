using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Glossaries]
	class DeleteTermTests<TWebDriverSettings>
		: BaseGlossaryTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);
		}

		[Test]
		public void DeleteTermTest()
		{
			var term1 = "Term1";
			var term2 = "Term2";

			_glossaryPage
				.CreateTerm(term1, term2)
				.DeleteTerm(term1, term2);

			Assert.IsTrue(_glossaryPage.IsDeleteButtonDisappeared(term1, term2),
				"Произошла ошибка: \nне исчезла кнопка удаления");

			Assert.IsTrue(_glossaryPage.IsDefaultTermsCountMatchExpected(expectedTermCount: 0),
				"Произошла ошибка:\nневерное количество терминов");
		}
	}
}
