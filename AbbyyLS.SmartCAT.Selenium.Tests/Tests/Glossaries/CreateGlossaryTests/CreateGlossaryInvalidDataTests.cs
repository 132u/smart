using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Glossaries]
	class CreateGlossaryInvalidDataTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateGlossaryWithoutNameTest()
		{
			_glossariesHelper.CreateGlossary("", errorExpected: true);

			Assert.IsTrue(_newGlossaryDialog.IsSpecifyGlossaryNameErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'Specify glossary name' не появилось.");
		}

		[Test]
		public void CreateGlossaryWithExistingNameTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(_glossaryUniqueName, errorExpected: true);

			Assert.IsTrue(_newGlossaryDialog.IsExistNameErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'A glossary with this name already exists' не появилось");
		}
	}
}
