using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorConfirmTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test(Description = "Проверяет подтверждение сегмента с помощью кнопки")]
		public void ConfirmButtonTest()
		{
			EditorPage
				.FillSegmentTargetField()
				.CloseTutorialIfExist()
				.ConfirmSegmentTranslation();

			Assert.IsTrue(EditorPage.IsSegmentConfirmed(),
				"Произошла ошибка:\n не удалось подтвердить сегмент");
		}

		[Test(Description = "Проверяет подтверждение сегмента нажатием Ctrl+Enter")]
		public void ConfirmHotkeyTest()
		{
			EditorPage
				.FillSegmentTargetField("Some words for example")
				.ClickOnTargetCellInSegment()
				.ConfirmSegmentByHotkeys();

			Assert.IsTrue(EditorPage.IsSegmentConfirmed(),
				"Произошла ошибка:\n не удалось подтвердить сегмент");
		}
	}
}
