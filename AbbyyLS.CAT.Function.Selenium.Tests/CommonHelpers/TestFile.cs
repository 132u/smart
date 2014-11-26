using System.IO;

namespace AbbyyLS.CAT.Function.Selenium.Tests.CommonHelpers
{
	public class TestFile
	{
		public TestFile(FilesRootCfg cfgRoot)
		{
			DocumentFile = Path.GetFullPath(cfgRoot.Root + "/littleEarth.docx");
			DocumentFileToConfirm = Path.GetFullPath(cfgRoot.Root + "/FilesForConfirm/testToConfirm.txt");
			DocumentFileToConfirm2 = Path.GetFullPath(cfgRoot.Root + "/FilesForConfirm/testToConfirm2.txt");

			EditorTXTFile = Path.GetFullPath(cfgRoot.Root + "/FileForTestTM/textWithoutTags.txt");
			EditorTMXFile = Path.GetFullPath(cfgRoot.Root + "/FileForTestTM/textWithoutTags.tmx");

			LongTxtFile = Path.GetFullPath(cfgRoot.Root + "/LongTxtTmx/LongText.txt");
			LongTmxFile = Path.GetFullPath(cfgRoot.Root + "/LongTxtTmx/LongTM.tmx");

			SecondTmFile = Path.GetFullPath(cfgRoot.Root + "/TextEngTestAddTMX.tmx");
			ImportGlossaryFile = Path.GetFullPath(cfgRoot.Root + "/TestGlossary.xlsx");
			ImageFile = Path.GetFullPath(cfgRoot.Root + "/TestImage.jpg");
			AudioFile = Path.GetFullPath(cfgRoot.Root + "/TestAudio.mp3");
			TxtFileForMatchTest = Path.GetFullPath(cfgRoot.Root + "/FilesForMatchTest/TxtFileForMatchTest.docx");
			TmxFileForMatchTest = Path.GetFullPath(cfgRoot.Root + "/FilesForMatchTest/TmxFileForMatchTest.tmx");
			PhotoLoad = Path.GetFullPath(cfgRoot.Root + "/FilesForLoadPhotoInRegistration/");
			TestUserFile = Path.GetFullPath(cfgRoot.RootToConfig + "/TestUsers.xml");
		}

		/// Полный путь к документу для загрузки
		public string DocumentFile { get; private set; }

		/// Полный путь к документу без тегов
		public string DocumentFileToConfirm { get; private set; }

		/// Полный путь ко второму документу без тегов
		public string DocumentFileToConfirm2 { get; private set; }

		/// Полный путь к файлу TXT для работы в редакторе
		public  string EditorTXTFile { get; private set; }

		/// Полный путь к файлу TMX для работы в редакторе
		public string EditorTMXFile { get; private set; }

		/// Полный путь к файлу TXT из 25 строк для работы в редакторе
		public string LongTxtFile { get; private set; }

		/// Полный путь к файлу TMX для longTxt
		public string LongTmxFile { get; private set; }

		/// Полный путь ко второму файлу TMX
		public string SecondTmFile { get; private set; }

		/// Полный путь к Txt для match теста
		public string TxtFileForMatchTest { get; private set; }

		/// Полный путь к файлу для импорта глоссария
		public string ImportGlossaryFile { get; private set; }

		/// Путь к изображению
		public string ImageFile { get; private set; }

		/// Путь к аудиофайлу (медиа)
		public string AudioFile { get; private set; }

		/// Полный путь к Tmx для match теста
		public string TmxFileForMatchTest { get; private set; }

		/// Полный путь к фото для загрузки на стр регистрации
		public string PhotoLoad { get; private set; }

		///  Файл со списком пользователей , имеющие аккаунты на аол/курсера/передем
		public string TestUserFile { get; private set; }
	}
}
