using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Editor]
	public class EditorCopySourceToTargetTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_editorPage.ClickOnTargetCellInSegment(1);
		}

		[Test(Description = "Проверяет копирование текста из сорса в таргет с помощью кнопки")]
		public void CopySourceToTargetButtonTest()
		{
			_editorPage.ClickCopySourceToTargetButton();

			Assert.AreEqual(_editorPage.GetSourceText(rowNumber: 1), _editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка:\n в сегменте исходный текст не совпадает с таргетом");

		}

		[Test(Description = "Проверяет копирование текста из сорса в таргет нажатием Ctrl+Insert")]
		public void CopySourceToTargetHotkeyTest()
		{
			_editorPage.CopySourceToTargetHotkey();

			Assert.AreEqual(_editorPage.GetSourceText(rowNumber: 1), _editorPage.GetTargetText(rowNumber: 1),
				"Произошла ошибка:\n в сегменте исходный текст не совпадает с таргетом");
		}
	}
}
