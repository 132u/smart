using System.IO;

using NConfiguration;

using AbbyyLS.SmartCAT.Selenium.Tests.Configs;

namespace AbbyyLS.SmartCAT.Selenium.Tests
{
	public static class PathProvider
	{		
		/// <summary>
		/// Путь к тестовому файлу txt для проверки TM
		/// </summary>
		public static string EditorTxtFile
		{
			get 
			{
				return Path.Combine(editorFilesFolder, "textWithoutTags.txt"); 
			}
		}

		/// <summary>
		/// Путь к тестовому файлу tmx для проверки TM
		/// </summary>
		public static string EditorTmxFile 
		{
			get 
			{ 
				return Path.Combine(editorFilesFolder, "textWithoutTags.tmx");
			}
		}

		/// <summary>
		/// Полный путь к документу без тегов
		/// </summary>
		public static string DocumentFileToConfirm1 
		{
			get 
			{ 
				return Path.Combine(filesForConfirmFolder, "testToConfirm.txt");
			}
		}

		/// <summary>
		/// Полный путь ко второму документу без тегов
		/// </summary>
		public static string DocumentFileToConfirm2
		{
			get 
			{ 
				return Path.Combine(filesForConfirmFolder, "testToConfirm2.txt");
			}
		}

		/// <summary>
		/// Путь к тестовому файлу txt на match'и
		/// </summary>
		public static string TxtFileForMatchTest 
		{
			get 
			{ 
				return Path.Combine(filesForMatchTestFolder, "TxtFileForMatchTest.docx");
			} 
		}

		/// <summary>
		/// Путь к тестовому файлу tmx на match'и
		/// </summary>
		public static string TmxFileForMatchTest
		{
			get 
			{ 
				return Path.Combine(filesForMatchTestFolder, "TmxFileForMatchTest.tmx");
			}
		}

		/// <summary>
		/// Путь к длинному тестовому текстовому файлу
		/// </summary>
		public static string LongTxtFile
		{
			get 
			{ 
				return Path.Combine(longFilesTestFolder, "LongText.txt");
			}
		}

		/// <summary>
		/// Путь к длинному тестовому файлу tmx
		/// </summary>
		public static string LongTmxFile
		{
			get 
			{ 
				return Path.Combine(longFilesTestFolder, "LongTM.tmx");
			}
		}

		/// <summary>
		/// Путь к длинному тестовому файлу tmx в одну строку
		/// </summary>
		public static string OneLineTmxFile
		{
			get 
			{ 
				return Path.Combine(longFilesTestFolder, "OneLineTmx.tmx");
			}
		}

		/// <summary>
		/// Путь к папке тестовых tmx
		/// </summary>
		public static string TMTestFolder
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/TMTestFiles");
			}
		}

		/// <summary>
		/// Путь к папке файлов для тестирования форматов, поддерживаемые ОР
		/// </summary>
		public static string FilesForStandaloneDifferentFormatsFolder
		{
			get
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/FilesForStandaloneDifferentFormatsFolder");
			}
		}

		/// <summary>
		/// Путь ко второму тестовому tmx файлу
		/// </summary>
		public static string TMTestFile2 
		{
			get 
			{ 
				return Path.Combine(TMTestFolder, "TMFile2.tmx");
			}
		}
		
		/// <summary>
		/// Путь к тестовому файлу xlf
		/// </summary>
		public static string EditorXlfFile 
		{
			get 
			{
				return Path.Combine(Path.GetFullPath(_pathConfig.FilesDirectory + "/Xliff"), "ТС-42.xlf");
			}
		}

		/// <summary>
		/// Полный путь к документу для загрузки
		/// </summary>
		public static string DocumentFile 
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/littleEarth.docx");
			}
		}

		/// <summary>
		/// Полный путь ко второму документу для загрузки
		/// </summary>
		public static string DocumentFile2
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/English.docx");
			}
		}

		/// <summary>
		/// Путь к тестовому ttx файлу
		/// </summary>
		public static string TtxFile 
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/test.ttx");
			}
		}

		/// <summary>
		/// Путь к тестовому txt файлу
		/// </summary>
		public static string TxtFile 
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/test.txt");
			}
		}

		/// <summary>
		/// Путь к тестовому srt файлу
		/// </summary>
		public static string SrtFile 
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/test.srt");
			}
		}

		/// <summary>
		/// Путь к тестовому xliff файлу
		/// </summary>
		public static string XliffFile 
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/TC-10En.xliff");
			}
		}

		/// <summary>
		/// Путь к тестовому файлу tmx
		/// </summary>
		public static string SecondTmFile 
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/TextEngTestAddTMX.tmx");
			}
		}

		/// <summary>
		/// Путь к файлу txt с расширением tmx
		/// </summary>
		public static string TxtWithTmxExtension
		{
			get
			{
				return Path.Combine(TMTestFolder, "TxtWithTmxExtension.tmx");
			}
		}

		/// <summary>
		/// Путь к файлу tmx с длинным значением в поле Seg
		/// </summary>
		public static string LongSegValue
		{
			get
			{
				return Path.Combine(TMTestFolder, "longSegValue.tmx");
			}
		}

		/// <summary>
		/// Путь к файлу tmx с отсутствующим закрывающим тегом tmx
		/// </summary>
		public static string WithoutTmxEndTag
		{
			get
			{
				return Path.Combine(TMTestFolder, "withoutTmxEndTag.tmx");
			}
		}

		/// <summary>
		/// Путь к файлу tmx с отсутствующим закрывающим тегом body
		/// </summary>
		public static string WithoutBodyEndTag
		{
			get
			{
				return Path.Combine(TMTestFolder, "withoutBodyEndTag.tmx");
			}
		}

		/// <summary>
		/// Путь к файлу tmx с отсутствующим закрывающим тегом tu
		/// </summary>
		public static string WithoutTuEndTag
		{
			get
			{
				return Path.Combine(TMTestFolder, "withoutTuEndTag.tmx");
			}
		}

		/// <summary>
		/// Путь к файлу tmx с отсутствующим закрывающим тегом seg
		/// </summary>
		public static string WithoutSegEndTag
		{
			get
			{
				return Path.Combine(TMTestFolder, "withoutSegEndTag.tmx");
			}
		}

		/// <summary>
		/// Путь к файлу tmx с Unicode символами
		/// </summary>
		public static string WithUnicodeCharacters
		{
			get
			{
				return Path.Combine(TMTestFolder, "withUnicodeCharacters.tmx");
			}
		}

		/// <summary>
		/// Путь к тестовому файлу xlsx
		/// </summary>
		public static string ImportGlossaryFile 
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/TestGlossary.xlsx");
			}
		}
		
		/// <summary>
		/// Путь к тестовому файлу jpg
		/// </summary>
		public static string ImageFile 
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/TestImage.jpg");
			}
		}
		
		/// <summary>
		/// Путь к тестовому файлу mp3
		/// </summary>
		public static string AudioFile 
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/TestAudio.mp3");
			}
		}

		/// <summary>
		/// Путь к файлу тестовых юзеров
		/// </summary>
		public static string TestUserFile 
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.ConfigDirectory + "/TestUsers.xml");
			}
		}

		public static string AolUserFile
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.ConfigDirectory + "/AolUsers.xml");
			}
		}

		public static string CourseraUserFile
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.ConfigDirectory + "/CourseraUsers.xml");
			}
		}

		/// <summary>
		/// Путь к экспортированным файлам
		/// </summary>
		public static string ExportFiles
		{
			get
			{
				return Path.GetFullPath(_pathConfig.ResultDirectory + "/ExportFiles");
			}
		}

		/// <summary>
		/// Путь к папке результатов тестов
		/// </summary>
		public static string ResultsFolderPath 
		{
			get 
			{
				return Path.GetFullPath(_pathConfig.ResultDirectory);
			}
		}

		public static string DriversTemporaryFolder
		{
			get
			{
				return Path.Combine(Directory.GetCurrentDirectory(), "WebDrivers");

			}
		}

		/// <summary>
		/// Путь к папке тестовых "длинных" файлов
		/// </summary>
		private static string longFilesTestFolder
		{
			get
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/LongTxtTmx");
			}
		}

		/// <summary>
		/// Путь к папке тестовых файлов на match'и
		/// </summary>
		private static string filesForMatchTestFolder
		{
			get
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/FilesForMatchTest");
			}
		}

		/// <summary>
		/// Путь к папке документов без тегов
		/// </summary>
		private static string filesForConfirmFolder
		{
			get
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/FilesForConfirm");
			}
		}

		/// <summary>
		/// Путь к папке файлов для тестирования редактора
		/// </summary>
		private static string editorFilesFolder
		{
			get
			{
				return Path.GetFullPath(_pathConfig.FilesDirectory + "/FileForTestTM");
			}
		}

		static private readonly FilesRootCfg _pathConfig = TestSettingDefinition.Instance.Get<FilesRootCfg>();
	}
}
