using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.MachineTranslation;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.MachineTranslations.FastMTTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	class TranslatingTextTests<TWebDriverProvider> : BaseMTTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_workspacePage
				.GoToMachineTranslationPage()
				.GoToTextTab();
				
			_fastMTTextPage = new FastMTTextPage(Driver);
		}

		[Test, Description("S-29773"), ShortCheckList]
		public void TranslateTextUsingFastMTTest()
		{
			_fastMTTextPage
				.SetTranslationSettings()
				.SetTextToTranslate("test")
				.ClickTranslateButton();

			Assert.IsTrue(_fastMTTextPage.IsTranslationAppeared(),
				"Произошла ошибка:\n текст перевода не появился");

			var translation = _fastMTTextPage.GetTranslationText();

			Assert.IsFalse(string.IsNullOrEmpty(translation),
				"Произошла ошибка:\n пришел пустой текст перевода");
		}

		protected FastMTTextPage _fastMTTextPage;
	}
}
