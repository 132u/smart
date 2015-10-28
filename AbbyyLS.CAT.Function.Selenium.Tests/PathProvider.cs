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
		/// Полный путь к документу для загрузки
		/// </summary>
		public static string DocumentFile 
		{
			get { return Path.GetFullPath(CfgRoot.FilesDirectory + "/littleEarth.docx"); }
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
