using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Editor]
	public class EditorHomeButtonTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test(Description = "Проверяет переход из редактора на страницу настроек проекта с помощью кнопки")]
		public void HomeButtonTest()
		{
			var projectSettingsPage = _editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			Assert.IsTrue(projectSettingsPage.IsProjectSettingsPageOpened(),
				"Произошла ошибка:\n не удалось перейти на вкладку проекта");
		}
	}
}
