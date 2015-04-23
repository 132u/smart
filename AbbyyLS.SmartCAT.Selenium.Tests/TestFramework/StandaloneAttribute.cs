using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	class StandaloneAttribute : CategoryAttribute
	{
		public StandaloneAttribute()
			: base("Standalone")
		{

		}
	}
}
