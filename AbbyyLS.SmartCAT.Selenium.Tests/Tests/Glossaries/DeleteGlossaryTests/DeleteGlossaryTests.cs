using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	class DeleteGlossaryTests<TWebDriverSettings>
		: BaseGlossaryTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverProvider, new()
	{
		[Test]
		public void DeleteGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog.ClickDeleteGlossaryButton();

			Assert.IsTrue(_glossaryPropertiesDialog.IsConfirmDeleteMessageDisplayed(),
				"Произошла ошибка: \nне появилось сообщение с подтверждением удаления глоссария");

			_glossaryPropertiesDialog.ClickConfirmDeleteGlossaryButton();

			Assert.IsTrue(_glossariesPage.IsGlossaryNotExist(_glossaryUniqueName),
				"Произошла ошибка:\n глоссарий присутствует в списке.");
		}
	}
}
