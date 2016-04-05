using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes
{
	class LoadTestsAttribute : CategoryAttribute
	{
		public LoadTestsAttribute()
			: base("LoadTests")
		{
		}
	}
}
