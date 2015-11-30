using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorCopySourceToTargetTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test(Description = "Проверяет копирование текста из сорса в таргет с помощью кнопки")]
		public void CopySourceToTargetButtonTest()
		{
			EditorPage.ClickCopySourceToTargetButton();

			Assert.AreEqual(EditorPage.GetSourceText(rowNumber: 1), EditorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка:\n в сегменте исходный текст не совпадает с таргетом");

		}

		[Test(Description = "Проверяет копирование текста из сорса в таргет нажатием Ctrl+Insert")]
		public void CopySourceToTargetHotkeyTest()
		{
			EditorPage.CopySourceToTargetHotkey();

			Assert.AreEqual(EditorPage.GetSourceText(rowNumber: 1), EditorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка:\n в сегменте исходный текст не совпадает с таргетом");
		}
	}
}
