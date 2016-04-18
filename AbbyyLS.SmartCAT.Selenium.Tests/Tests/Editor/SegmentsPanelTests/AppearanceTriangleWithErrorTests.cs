using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class AppearanceTriangleWithErrorTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> 
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7231")]
		public void IdenticalSourceAndTargetErrorTest()
		{
			_editorPage
				.ClickCopySourceToTargetButton()
				.ConfirmSegmentTranslation();

			Assert.IsTrue(_editorPage.IsYellowTriangleErrorLogoDisplayed(),
				"Логотип с ошибкой не появился, после подтверждения таргета содержащего ошибку.");
		}
	}
}