using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorConcordanceSearchTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test(Description = "Проверяет открытие вкладки 'Конкордансный поиск' с помощью кнопки")]
		public void ConcordanceSearchButtonTest()
		{
			EditorPage.ClickConcordanceSearchButton();

			Assert.IsTrue(EditorPage.IsConcordanceSearchDisplayed(),
				"Произошла ошибка:\n Конкордансный поиск не появился");
		}

		[Test(Description = "Проверяет открытие вкладки 'Конкордансный поиск' нажатием Ctrl+K")]
		public void ConcordanceSearchHotkeyTest()
		{
			EditorPage.OpenConcordanceSearchByHotKey();

			Assert.IsTrue(EditorPage.IsConcordanceSearchDisplayed(),
				"Произошла ошибка:\n Конкордансный поиск не появился");
		}
	}
}
