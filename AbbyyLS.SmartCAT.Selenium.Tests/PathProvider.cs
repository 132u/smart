using System;
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
				return new Uri(Path.Combine(editorFilesFolder, "textWithoutTags.txt")).LocalPath; 
			}
		}

		/// <summary>
		/// Путь к тестовому файлу tmx для проверки TM
		/// </summary>
		public static string EditorTmxFile 
		{
			get 
			{ 
				return new Uri(Path.Combine(editorFilesFolder, "textWithoutTags.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Полный путь к документу без тегов
		/// </summary>
		public static string DocumentFileToConfirm1 
		{
			get 
			{ 
				return new Uri(Path.Combine(filesForConfirmFolder, "testToConfirm.txt")).LocalPath;
			}
		}

		/// <summary>
		/// Полный путь ко второму документу без тегов
		/// </summary>
		public static string DocumentFileToConfirm2
		{
			get 
			{ 
				return new Uri(Path.Combine(filesForConfirmFolder, "testToConfirm2.txt")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому файлу txt на match'и
		/// </summary>
		public static string TxtFileForMatchTest 
		{
			get 
			{ 
				return new Uri(Path.Combine(filesForMatchTestFolder, "TxtFileForMatchTest.docx")).LocalPath;
			} 
		}

		/// <summary>
		/// Путь к тестовому файлу tmx на match'и
		/// </summary>
		public static string TmxFileForMatchTest
		{
			get 
			{ 
				return new Uri(Path.Combine(filesForMatchTestFolder, "TmxFileForMatchTest.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к длинному тестовому текстовому файлу
		/// </summary>
		public static string LongTxtFile
		{
			get 
			{ 
				return new Uri(Path.Combine(longFilesTestFolder, "LongText.txt")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к длинному тестовому файлу tmx
		/// </summary>
		public static string LongTmxFile
		{
			get 
			{ 
				return new Uri(Path.Combine(longFilesTestFolder, "LongTM.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к длинному тестовому файлу tmx в одну строку
		/// </summary>
		public static string OneLineTmxFile
		{
			get 
			{ 
				return new Uri(Path.Combine(longFilesTestFolder, "OneLineTmx.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке тестовых tmx
		/// </summary>
		public static string TMTestFolder
		{
			get
			{
				return new Uri(Path.Combine(FilesDirectory, "TMTestFiles")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке файлов для тестирования форматов, поддерживаемые ОР
		/// </summary>
		public static string FilesForStandaloneDifferentFormatsFolder
		{
			get
			{
				return new Uri(Path.Combine(FilesDirectory, "FilesForStandaloneDifferentFormatsFolder")).LocalPath;
			}
		}

		/// <summary>
		/// Путь ко второму тестовому tmx файлу
		/// </summary>
		public static string TMTestFile2 
		{
			get 
			{ 
				return new Uri(Path.Combine(TMTestFolder, "TMFile2.tmx")).LocalPath;
			}
		}
		
		/// <summary>
		/// Путь к тестовому файлу xlf
		/// </summary>
		public static string EditorXlfFile 
		{
			get
			{
				return new Uri(Path.Combine(Path.Combine(FilesDirectory, "Xliff"), "ТС-42.xlf")).LocalPath;
			}
		}

		/// <summary>
		/// Полный путь к документу для загрузки
		/// </summary>
		public static string DocumentFile 
		{
			get 
			{
				return new Uri(Path.Combine(FilesDirectory, "littleEarth.docx")).LocalPath;
			}
		}

		/// <summary>
		/// Полный путь ко второму документу для загрузки
		/// </summary>
		public static string DocumentFile2
		{
			get 
			{
				return new Uri(Path.Combine(FilesDirectory, "English.docx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому ttx файлу
		/// </summary>
		public static string TtxFile 
		{
			get 
			{
				return new Uri(Path.Combine(FilesDirectory, "test.ttx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому txt файлу
		/// </summary>
		public static string TxtFile 
		{
			get 
			{
				return new Uri(Path.Combine(FilesDirectory, "test.txt")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому srt файлу
		/// </summary>
		public static string SrtFile 
		{
			get 
			{
				return new Uri(Path.Combine(FilesDirectory, "test.srt")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому xliff файлу
		/// </summary>
		public static string XliffFile 
		{
			get 
			{
				return new Uri(Path.Combine(FilesDirectory, "TC-10En.xliff")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому файлу tmx
		/// </summary>
		public static string SecondTmFile 
		{
			get 
			{
				return new Uri(Path.Combine(FilesDirectory, "TextEngTestAddTMX.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к файлу txt с расширением tmx
		/// </summary>
		public static string TxtWithTmxExtension
		{
			get
			{
				return new Uri(Path.Combine(TMTestFolder, "TxtWithTmxExtension.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к файлу tmx с длинным значением в поле Seg
		/// </summary>
		public static string LongSegValue
		{
			get
			{
				return new Uri(Path.Combine(TMTestFolder, "longSegValue.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к файлу tmx с отсутствующим закрывающим тегом tmx
		/// </summary>
		public static string WithoutTmxEndTag
		{
			get
			{
				return new Uri(Path.Combine(TMTestFolder, "withoutTmxEndTag.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к файлу tmx с отсутствующим закрывающим тегом body
		/// </summary>
		public static string WithoutBodyEndTag
		{
			get
			{
				return new Uri(Path.Combine(TMTestFolder, "withoutBodyEndTag.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к файлу tmx с отсутствующим закрывающим тегом tu
		/// </summary>
		public static string WithoutTuEndTag
		{
			get
			{
				return new Uri(Path.Combine(TMTestFolder, "withoutTuEndTag.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к файлу tmx с отсутствующим закрывающим тегом seg
		/// </summary>
		public static string WithoutSegEndTag
		{
			get
			{
				return new Uri(Path.Combine(TMTestFolder, "withoutSegEndTag.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к файлу tmx с Unicode символами
		/// </summary>
		public static string WithUnicodeCharacters
		{
			get
			{
				return new Uri(Path.Combine(TMTestFolder, "withUnicodeCharacters.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому файлу xlsx
		/// </summary>
		public static string ImportGlossaryFile 
		{
			get 
			{
				return new Uri(Path.Combine(FilesDirectory, "TestGlossary.xlsx")).LocalPath;
			}
		}
		
		/// <summary>
		/// Путь к тестовому файлу jpg
		/// </summary>
		public static string ImageFile 
		{
			get 
			{
				return new Uri(Path.Combine(FilesDirectory, "TestImage.jpg")).LocalPath;
			}
		}
		
		/// <summary>
		/// Путь к тестовому файлу mp3
		/// </summary>
		public static string AudioFile 
		{
			get 
			{
				return new Uri(Path.Combine(FilesDirectory, "TestAudio.mp3")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к экспортированным файлам
		/// </summary>
		public static string ExportFiles
		{
			get
			{
				return new Uri(Path.Combine(ResultsFolderPath, "ExportFiles")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке результатов тестов
		/// </summary>
		public static string ResultsFolderPath 
		{
			get
			{
				return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _pathConfig.ResultDirectory)).LocalPath;
			}
		}

		public static string DriversTemporaryFolder
		{
			get
			{
				return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebDrivers")).LocalPath;

			}
		}

		/// <summary>
		/// Путь к папке тестовых "длинных" файлов
		/// </summary>
		private static string longFilesTestFolder
		{
			get
			{
				return new Uri(Path.Combine(FilesDirectory, "LongTxtTmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке тестовых файлов на match'и
		/// </summary>
		private static string filesForMatchTestFolder
		{
			get
			{
				return new Uri(Path.Combine(FilesDirectory, "FilesForMatchTest")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке документов без тегов
		/// </summary>
		private static string filesForConfirmFolder
		{
			get
			{
				return new Uri(Path.Combine(FilesDirectory, "FilesForConfirm")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке файлов для тестирования редактора
		/// </summary>
		private static string editorFilesFolder
		{
			get
			{
				return new Uri(Path.Combine(FilesDirectory, "FileForTestTM")).LocalPath;
			}
		}

		public static string FilesDirectory
		{
			get
			{
				return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _pathConfig.FilesDirectory)).LocalPath;
			}
		}

		static private readonly FilesRootCfg _pathConfig = TestSettingDefinition.Instance.Get<FilesRootCfg>();
	}
}
