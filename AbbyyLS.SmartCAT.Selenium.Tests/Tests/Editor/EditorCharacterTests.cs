using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	public class EditorCharacterTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CharacterButtonTest()
		{
			EditorHelper.OpenSpecialCharacters();
		}

		[Test]
		[Category("SCAT_102")]
		public void CharacterHotkeyTest()
		{
			EditorHelper.OpenSpecialCharactersByHotKey();
		}
	}
}
