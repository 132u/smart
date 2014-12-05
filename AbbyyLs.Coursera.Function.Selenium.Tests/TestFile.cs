using System.IO;

namespace AbbyyLS.Coursera.Function.Selenium.Tests
{
	public class TestFile
	{
		public TestFile(FilesRootCfg cfgRoot)
		{
			TestUserFile = Path.GetFullPath(cfgRoot.Root + "CourseraUsers.xml");

		}
		///  Файл со списком пользователей Coursera
		public string TestUserFile { get; private set; }
	}
}
