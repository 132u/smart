using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes
{
	/// <summary>
	/// Тесты, отмеченные этим аттрибутом, не должны запускать на стендах с доменной авторизацией
	/// </summary>
	class NotSupportedWithDomainAuthenticationAttribute : CategoryAttribute
	{
		public NotSupportedWithDomainAuthenticationAttribute()
			: base("NotSupportedWithDomainAuthentication")
		{

		}
	}
}
