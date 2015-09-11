using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.ExplicitAttributes
{
	class FileUploadExplicitAttribute : ExplicitAttribute
	{
		public FileUploadExplicitAttribute()
			: base("Тест использует системную загрузку файлов и не прогоняется в тимсити")
		{

		}
	}
}
