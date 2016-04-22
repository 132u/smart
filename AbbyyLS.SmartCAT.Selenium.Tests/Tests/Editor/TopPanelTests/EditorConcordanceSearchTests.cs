using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Editor]
	public class EditorConcordanceSearchTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test(Description = "Проверяет открытие вкладки 'Конкордансный поиск' с помощью кнопки")]
		public void ConcordanceSearchButtonTest()
		{
			_editorPage.ClickConcordanceSearchButton();

			Assert.IsTrue(_editorPage.IsConcordanceSearchDisplayed(),
				"Произошла ошибка:\n Конкордансный поиск не появился");
		}

		[Test(Description = "Проверяет открытие вкладки 'Конкордансный поиск' нажатием Ctrl+K")]
		public void ConcordanceSearchHotkeyTest()
		{
			_editorPage.OpenConcordanceSearchByHotKey();

			Assert.IsTrue(_editorPage.IsConcordanceSearchDisplayed(),
				"Произошла ошибка:\n Конкордансный поиск не появился");
		}
	}
}
