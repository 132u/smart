using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	class PriorityMajorAttribute : CategoryAttribute
	{
		public PriorityMajorAttribute()
			: base("PriorityMajor")
		{

		}
	}
}
