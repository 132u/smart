using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.ExplicitAttributes
{
	class HotkeyExplicitAttribute : ExplicitAttribute
	{
		public HotkeyExplicitAttribute()
			: base("Тест использует хоткеи и не прогоняется в тимсити")
		{

		}
	}
}
