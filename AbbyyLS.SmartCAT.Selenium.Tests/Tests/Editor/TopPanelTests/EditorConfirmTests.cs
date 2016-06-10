using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Editor]
	public class EditorConfirmTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test(Description = "Проверяет подтверждение сегмента с помощью кнопки")]
		public void ConfirmButtonTest()
		{
			_editorPage
				.FillSegmentTargetField()
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(),
				"Произошла ошибка:\n не удалось подтвердить сегмент");
		}

		[Test(Description = "Проверяет подтверждение сегмента нажатием Ctrl+Enter"), Ignore("PRX-17386")]
		public void ConfirmHotkeyTest()
		{
			_editorPage
				.FillSegmentTargetField("Some words for example")
				.ClickOnTargetCellInSegment()
				.ConfirmSegmentByHotkeys();

			Assert.IsTrue(_editorPage.IsSegmentConfirmed(),
				"Произошла ошибка:\n не удалось подтвердить сегмент");
		}
	}
}
