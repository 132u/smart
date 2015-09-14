using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.ExplicitAttributes
{
	class HotkeyExplicitAttribute : CategoryAttribute
	{
		public HotkeyExplicitAttribute()
			: base("Тест использует хоткеи и не прогоняется в тимсити")
		{

		}
	}
}
