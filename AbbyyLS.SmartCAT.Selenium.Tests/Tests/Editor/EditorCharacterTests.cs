using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorCharacterTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CharacterButtonTest()
		{
			EditorHelper.OpenSpecialCharacters();
		}

		[Test]
		public void CharacterHotkeyTest()
		{
			EditorHelper.OpenSpecialCharactersByHotKey();
		}
	}
}
