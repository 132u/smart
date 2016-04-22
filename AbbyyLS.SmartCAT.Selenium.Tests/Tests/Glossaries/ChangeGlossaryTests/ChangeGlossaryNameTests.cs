using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Glossaries]
	class ChangeGlossaryNameTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void ChangeGlossaryNameTest()
		{
			var newGlossaryName = GlossariesHelper.UniqueGlossaryName();

			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.FillGlossaryName(newGlossaryName)
				.ClickSaveButton();

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(newGlossaryName),
				"Произошла ошибка:\n глоссарий отсутствует в списке");

			Assert.IsTrue(_glossariesPage.IsGlossaryNotExist(_glossaryUniqueName),
				"Произошла ошибка:\n глоссарий присутствует в списке.");
		}

		[Test]
		public void ChangeGlossaryExistingNameTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			_glossariesHelper.CreateGlossary(_glossaryUniqueName + "2");

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.FillGlossaryName(_glossaryUniqueName)
				.ClickSaveButtonExpectingError();

			Assert.IsTrue(_newGlossaryDialog.IsExistNameErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'A glossary with this name already exists' не появилось");
		}

		[TestCase("")]
		[TestCase(" ")]
		public void ChangeGlossaryEmptyNameTest(string newGlossaryName)
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog
				.FillGlossaryName(newGlossaryName)
				.ClickSaveButtonExpectingError();

			Assert.IsTrue(_newGlossaryDialog.IsSpecifyGlossaryNameErrorDisplayed(),
				"Произошла ошибка:\n сообщение 'Specify glossary name' не появилось.");
		}
	}
}
