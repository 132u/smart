using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public enum RightsType
	{
		UsersManagement,
		PaidResources,
		ProjectCreation,
		ProjectResourceManagement,
		ProjectView,
		[Description("Search in all glossaries")]
		GlossarySearch,
		TMManagement,
		TMSearch,
		VendorsManagement,
		ClientsAndDomainsManagement,
		GlossaryManagement,
		TMContentManagement,
		GlossaryContentManagement
	}
}
