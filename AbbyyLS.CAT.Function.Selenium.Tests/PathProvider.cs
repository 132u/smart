using System.IO;
using NConfiguration;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	public static class PathProvider
	{
		static private readonly FilesRootCfg CfgRoot = TestSettingDefinition.Instance.Get<FilesRootCfg>();
		
		/// <summary>
		/// Путь к папке файлов для тестирования редактора
		/// </summary>
		public static string FilesEditorTestFolder
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/FileForTestTM"); }
		}

		/// <summary>
		/// Путь к тестовому файлу txt для проверки TM
		/// </summary>
		public static string EditorTxtFile
		{
			get { return Path.Combine(FilesEditorTestFolder, "textWithoutTags.txt"); }
		}

		/// <summary>
		/// Путь к тестовому файлу tmx для проверки TM
		/// </summary>
		public static string EditorTmxFile 
		{
			get { return Path.Combine(FilesEditorTestFolder, "textWithoutTags.tmx"); }
		}

		/// <summary>
		/// Путь к папке документов без тегов
		/// </summary>
		public static string FilesForConfirmFolder
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/FilesForConfirm"); }
		}

		/// <summary>
		/// Полный путь к документу без тегов
		/// </summary>
		public static string DocumentFileToConfirm1 
		{
			get { return Path.Combine(FilesForConfirmFolder, "testToConfirm.txt"); }
		}

		/// <summary>
		/// Полный путь ко второму документу без тегов
		/// </summary>
		public static string DocumentFileToConfirm2
		{
			get { return Path.Combine(FilesForConfirmFolder, "testToConfirm2.txt"); }
		}

		/// <summary>
		/// Путь к папке тестовых файлов для проверки фото при регистрации
		/// </summary>
		public static string PhotoLoadFolder
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/FilesForLoadPhotoInRegistration/"); }
		}

		/// <summary>
		/// Путь к папке тестовых файлов на match'и
		/// </summary>
		public static string FilesForMatchTestFolder
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/FilesForMatchTest"); }
		}

		/// <summary>
		/// Путь к тестовому файлу txt на match'и
		/// </summary>
		public static string TxtFileForMatchTest 
		{
			get { return Path.Combine(FilesForMatchTestFolder, "TxtFileForMatchTest.docx"); } 
		}

		/// <summary>
		/// Путь к тестовому файлу tmx на match'и
		/// </summary>
		public static string TmxFileForMatchTest
		{
			get { return Path.Combine(FilesForMatchTestFolder, "TmxFileForMatchTest.tmx"); }
		}

		/// <summary>
		/// Путь к папке тестовых "длинных" файлов
		/// </summary>
		public static string LongFilesTestFolder 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/LongTxtTmx"); }
		}

		/// <summary>
		/// Путь к длинному тестовому текстовому файлу
		/// </summary>
		public static string LongTxtFile
		{
			get { return Path.Combine(LongFilesTestFolder, "LongText.txt"); }
		}

		/// <summary>
		/// Путь к длинному тестовому файлу tmx
		/// </summary>
		public static string LongTmxFile
		{
			get { return Path.Combine(LongFilesTestFolder, "LongTM.tmx"); }
		}

		/// <summary>
		/// Путь к длинному тестовому файлу tmx в одну строку
		/// </summary>
		public static string OneLineTmxFile
		{
			get { return Path.Combine(LongFilesTestFolder, "OneLineTmx.tmx"); }
		}

		/// <summary>
		/// Путь к папке тестовых tmx
		/// </summary>
		public static string TMTestFolder
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/TMTestFiles"); }
		}

		/// <summary>
		/// Путь к первому тестовому tmx файлу
		/// </summary>
		public static string TMTestFile1
		{
			get { return Path.Combine(TMTestFolder, "TMFile1.tmx"); }
		}

		/// <summary>
		/// Путь ко второму тестовому tmx файлу
		/// </summary>
		public static string TMTestFile2 
		{
			get { return Path.Combine(TMTestFolder, "TMFile2.tmx"); }
		}

		/// <summary>
		/// Путь к третьему тестовому tmx файлу
		/// </summary>
		public static string TMTestFile3 
		{
			get { return Path.Combine(TMTestFolder, "TMFile1.tmx"); }
		}

		/// <summary>
		/// Путь к папке тестовых Xliff
		/// </summary>
		public static string XliffTestFolder 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/Xliff"); }
		}
		
		/// <summary>
		/// Путь к тестовому файлу xlf
		/// </summary>
		public static string EditorXlfFile 
		{
			get { return Path.Combine(XliffTestFolder, "ТС-42.xlf"); }
		}

		/// <summary>
		/// Путь к поврежденному doc-файлу
		/// </summary>
		public static string DocumentFileWrong
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/doc98.doc"); }
		}

		/// <summary>
		/// Полный путь к документу для загрузки
		/// </summary>
		public static string DocumentFile 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/littleEarth.docx"); }
		}

		/// <summary>
		/// Путь к тестовому ttx файлу
		/// </summary>
		public static string TtxFile 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/test.ttx"); }
		}

		/// <summary>
		/// Путь к тестовому txt файлу
		/// </summary>
		public static string TxtFile 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/test.txt"); }
		}

		/// <summary>
		/// Путь к тестовому srt файлу
		/// </summary>
		public static string SrtFile 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/test.srt"); }
		}

		/// <summary>
		/// Путь к тестовому xliff файлу
		/// </summary>
		public static string XliffFile 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/TC-10En.xliff"); }
		}

		/// <summary>
		/// Путь к тестовому файлу tmx
		/// </summary>
		public static string SecondTmFile 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/TextEngTestAddTMX.tmx"); }
		}

		/// <summary>
		/// Путь к тестовому файлу xlsx
		/// </summary>
		public static string ImportGlossaryFile 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/TestGlossary.xlsx"); }
		}
		
		/// <summary>
		/// Путь к тестовому файлу jpg
		/// </summary>
		public static string ImageFile 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/TestImage.jpg"); }
		}
		
		/// <summary>
		/// Путь к тестовому файлу mp3
		/// </summary>
		public static string AudioFile 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/TestAudio.mp3"); }
		}



		/// <summary>
		/// Путь к файлу тестовых юзеров
		/// </summary>
		public static string TestUserFile 
		{
			get { return Path.GetFullPath(CfgRoot.ConfigDirectory + "/TestUsers.xml"); }
		}



		/// <summary>
		/// Путь к папке результатов тестов
		/// </summary>
		public static string ResultsFolderPath 
		{
			get { return Path.GetFullPath(CfgRoot.ResultDirectory); }
		}
	}
}
