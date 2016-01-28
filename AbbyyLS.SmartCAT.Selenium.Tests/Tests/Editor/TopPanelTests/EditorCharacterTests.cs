using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorCharacterTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_specialCharactersForm = new SpecialCharactersForm(Driver);
		}

		[Test(Description = "Проверяет открытие формы 'Специальные символы' с помощью кнопки")]
		public void CharacterButtonTest()
		{
			_editorPage.ClickCharacterButton();

			Assert.IsTrue(_specialCharactersForm.IsSpecialCharactersFormOpened(),
				"Произошла ошибка:\n не появилась форма 'Специальные символы'");
		}

		[Test(Description = "Проверяет открытие формы 'Специальные символы' нажатием Ctrl+Shift+I")]
		public void CharacterHotkeyTest()
		{
			_editorPage.OpenSpecialCharacterFormByHotKey();

			Assert.IsTrue(_specialCharactersForm.IsSpecialCharactersFormOpened(),
				"Произошла ошибка:\n не появилась форма 'Специальные символы'");
		}

		private SpecialCharactersForm _specialCharactersForm;
	}
}
