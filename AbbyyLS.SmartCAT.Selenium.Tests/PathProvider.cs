using System;
using System.Collections.Generic;
using System.IO;

using NConfiguration;

using AbbyyLS.SmartCAT.Selenium.Tests.Configs;

namespace AbbyyLS.SmartCAT.Selenium.Tests
{
	public static class PathProvider
	{
		#region Файлы для тестов редактора

		/// <summary>
		/// Путь к тестовому файлу xlf
		/// </summary>
		public static string EditorXliffFile
		{
			get
			{
				return new Uri(Path.Combine(editorFilesFolder, "ТС-42.xlf")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к файлу с тегами, репитшенами и цифрами
		/// </summary>
		public static string EditorTagsRepetitionsNumbersFile
		{
			get
			{
				return new Uri(Path.Combine(editorFilesFolder, "TagsRepetitionsNumbers.docx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к файлу с тегами, репитшенами и цифрами
		/// </summary>
		public static string EditorCrossFileRepetitionsFirstFile
		{
			get
			{
				return new Uri(Path.Combine(editorFilesFolder, "CrossFileRepititions1.docx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к файлу с тегами, репитшенами и цифрами
		/// </summary>
		public static string EditorCrossFileRepetitionsSecondFile
		{
			get
			{
				return new Uri(Path.Combine(editorFilesFolder, "CrossFileRepititions2.docx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому файлу txt
		/// </summary>
		public static string EditorTxtFile
		{
			get
			{
				return new Uri(Path.Combine(editorFilesFolder, "EditorTxtFile.txt")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому файлу QANavigationFile
		/// </summary>
		public static string QANavigationFile
		{
			get
			{
				return new Uri(Path.Combine(editorFilesFolder, "QANavigationFile.txt")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому файлу txt с повторами
		/// </summary>
		public static string RepetitionsTxtFile
		{
			get
			{
				return new Uri(Path.Combine(editorFilesFolder, "repetitionsTxtFile.txt")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому файлу tmx
		/// </summary>
		public static string EditorTmxFile
		{
			get
			{
				return new Uri(Path.Combine(editorFilesFolder, "EditorTmxFile.tmx")).LocalPath;
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
		/// Путь к папке тестовых файлов на match'и
		/// </summary>
		private static string filesForMatchTestFolder
		{
			get
			{
				return new Uri(Path.Combine(editorFilesFolder, "FilesForMatchTest")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке файлов для тестирования редактора
		/// </summary>
		private static string editorFilesFolder
		{
			get
			{
				return new Uri(Path.Combine(FilesDirectory, "Editor")).LocalPath;
			}
		}

		#endregion

		#region Файлы для тестов проектов

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
		/// Полный путь к документу без тегов
		/// </summary>
		public static string DocumentFileToConfirm1
		{
			get
			{
				return new Uri(Path.Combine(projectsFilesFolder, "testToConfirm.txt")).LocalPath;
			}
		}

		/// <summary>
		/// Полный путь ко второму документу без тегов
		/// </summary>
		public static string DocumentFileToConfirm2
		{
			get
			{
				return new Uri(Path.Combine(projectsFilesFolder, "testToConfirm2.txt")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к длинному тестовому текстовому файлу
		/// </summary>
		public static string LongTxtFile
		{
			get
			{
				return new Uri(Path.Combine(projectsFilesFolder, "LongText.txt")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому ttx файлу
		/// </summary>
		public static string TtxFile
		{
			get
			{
				return new Uri(Path.Combine(projectsFilesFolder, "test.ttx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке файлов для тестирования форматов, поддерживаемых системой
		/// </summary>
		public static string SupportedFormatsFiles
		{
			get
			{
				return new Uri(Path.Combine(projectsFilesFolder, "SupportedFormatsFiles")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке файлов для тестирования форматов, не поддерживаемых системой
		/// </summary>
		public static string UnSupportedFormatsFiles
		{
			get
			{
				return new Uri(Path.Combine(projectsFilesFolder, "UnSupportedFormatsFiles")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке файлов для тестирования редактора
		/// </summary>
		private static string projectsFilesFolder
		{
			get
			{
				return new Uri(Path.Combine(FilesDirectory, "Projects")).LocalPath;
			}
		}

		#endregion

		#region Файлы для тестов TM

		/// <summary>
		/// Путь к тестовому файлу tmx с примером перевода
		/// </summary>
		public static string TmForSearchTest
		{
			get
			{
				return new Uri(Path.Combine(tmFilesFolder, "SearchTranslationExampleTest.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к длинному тестовому файлу tmx
		/// </summary>
		public static string LongTmxFile
		{
			get
			{
				return new Uri(Path.Combine(tmFilesFolder, "LongTM.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к длинному тестовому файлу tmx в одну строку
		/// </summary>
		public static string OneLineTmxFile
		{
			get
			{
				return new Uri(Path.Combine(tmFilesFolder, "OneLineTmx.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь ко второму тестовому tmx файлу
		/// </summary>
		public static string TmxFile
		{
			get
			{
				return new Uri(Path.Combine(tmFilesFolder, "CorrectTmxEnRu.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому файлу tmx
		/// </summary>
		public static string SecondTmxFile
		{
			get
			{
				return new Uri(Path.Combine(tmFilesFolder, "SecondCorrectTmxEnRu.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к файлу tmx с Unicode символами
		/// </summary>
		public static string WithUnicodeCharacters
		{
			get
			{
				return new Uri(Path.Combine(tmFilesFolder, "withUnicodeCharacters.tmx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке с файлами для тестов TM
		/// </summary>
		public static string IncorrectTmxFilesFolder
		{
			get
			{
				return new Uri(Path.Combine(tmFilesFolder, "IncorrectTmxFiles")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке с файлами для тестов TM
		/// </summary>
		private static string tmFilesFolder
		{
			get
			{
				return new Uri(Path.Combine(FilesDirectory, "TM")).LocalPath;
			}
		}

		#endregion

		#region Файлы для тестов глоссариев

		/// <summary>
		/// Путь к тестовому файлу xlsx
		/// </summary>
		public static string GlossaryFileForImport
		{
			get
			{
				return new Uri(Path.Combine(glossariesFilesFolder, "TestGlossary.xlsx")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому файлу jpg
		/// </summary>
		public static string ImageFileForGlossariesTests
		{
			get
			{
				return new Uri(Path.Combine(glossariesFilesFolder, "TestImage.jpg")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к тестовому файлу mp3
		/// </summary>
		public static string AudioFileForGlossariesTests
		{
			get
			{
				return new Uri(Path.Combine(glossariesFilesFolder, "TestAudio.mp3")).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке с файлами для тестов глоссария
		/// </summary>
		private static string glossariesFilesFolder
		{
			get
			{
				return new Uri(Path.Combine(FilesDirectory, "Glossaries")).LocalPath;
			}
		}

		#endregion

		#region Файлы для тестов курсеры

		/// <summary>
		/// Получить файлы из папки FilesForCourseraProject
		/// </summary>
		/// <returns></returns>
		public static IList<string> GetFilesFromCourseraFolder()
		{
			var files = new List<string>();
			var filesInFolder = new DirectoryInfo(
				Path.Combine(сourseraFilesTestFolder, "FilesForCourseraProject")).GetFiles();

			foreach (FileInfo file in filesInFolder)
			{
				files.Add(file.FullName);
			}

			return files;
		}

		/// <summary>
		/// Путь к папке тестовых файлов для курсеры
		/// </summary>
		private static string сourseraFilesTestFolder
		{
			get
			{
				return new Uri(Path.Combine(FilesDirectory, "Coursera")).LocalPath;
			}
		}

		#endregion

		/// <summary>
		/// Путь к папке с тестовыми файлами
		/// </summary>
		public static string FilesDirectory
		{
			get
			{
				return new Uri(Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory, _pathConfig.FilesDirectory)).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папке результатов тестов
		/// </summary>
		public static string ResultsFolderPath
		{
			get
			{
				return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
					_pathConfig.ResultDirectory)).LocalPath;
			}
		}

		/// <summary>
		/// Путь к папкам с драйверами
		/// </summary>
		public static string DriversTemporaryFolder
		{
			get
			{
				return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
					"WebDrivers")).LocalPath;
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

		static private readonly FilesRootCfg _pathConfig = TestSettingDefinition.Instance.Get<FilesRootCfg>();
	}
}
