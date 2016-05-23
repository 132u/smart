using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor.TopPanelTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	class SearchInDictionariesButtonTests<TWebDriverProvider> :
		EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_editorPage.ClickConcordanceSearchButton();
		}

		[Test]
		public void OpenDictionariesTabByButton()
		{
			_editorPage.ClickSearchInLingvoDictionariesButton();

			Assert.IsTrue(_editorPage.IsDictionariesTabOpened(),
				"Произошла ошибка: не открылась вкладка Dictionaries на панели справа");
		}

		[Test]
		public void OpenDictionariesTabByHotkey()
		{
			_editorPage.ClickSearchInLingvoDictionariesButtonByHotkey();

			Assert.IsTrue(_editorPage.IsDictionariesTabOpened(),
				"Произошла ошибка: не открылась вкладка Dictionaries на панели справа");
		}
	}
}
