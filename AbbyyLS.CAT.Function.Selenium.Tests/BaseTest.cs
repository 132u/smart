using System;
using System.Threading;
using AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Imaging;
using NConfiguration;
using System.Text.RegularExpressions;
using NLog;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	using NUnit.Framework.Constraints;

	/// <summary>
	/// Базовый тест
	/// </summary>

	[TestFixture("Firefox")]

	public class BaseTest
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();
		/// <summary>
		/// Конструктор базового теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public BaseTest(string browserName)
		{
			try
			{
				var cfgAgentSpecific = TestSettingDefinition.Instance.Get<TargetServerConfig>();
				var cfgUserInfo = TestSettingDefinition.Instance.Get<UserInfoConfig>();
				var cfgRoot = TestSettingDefinition.Instance.Get<FilesRootCfg>();
				_browserName = browserName;
				_url = "https://" + cfgAgentSpecific.Url;
				_workspaceUrl = cfgAgentSpecific.Workspace;
				if (string.IsNullOrWhiteSpace(_workspaceUrl))
					_workspaceUrl = "https://" + cfgAgentSpecific.Url + "/workspace";
				_adminUrl = "http://" + cfgAgentSpecific.Url + ":81";


				CreateDriver();


				_login = cfgUserInfo.Login;
				_password = cfgUserInfo.Password;
				_userName = cfgUserInfo.UserName;

				_login2 = cfgUserInfo.Login2;
				_password2 = cfgUserInfo.Password2;
				_userName2 = cfgUserInfo.UserName2;

				_deadlineDate = "03/03/2016";
				_documentFile = Path.GetFullPath(cfgRoot.Root + "/littleEarth.docx");
				_documentFileToConfirm = Path.GetFullPath(cfgRoot.Root + "/FilesForConfirm/testToConfirm.txt");
				_documentFileToConfirm2 = Path.GetFullPath(cfgRoot.Root + "/FilesForConfirm/testToConfirm2.txt");

				_constTmName = "TestTM";
				_glossaryName = "TestGlossary";
				_projectNameExportTestOneDoc = "TestProjectTestExportOneDocumentUniqueName";
				_projectNameExportTestMultiDoc = "TestProjectTestExportMultiDocumentsUniqueName";

				CreateUniqueNamesByDatetime();

				_pathTestFiles = cfgRoot.Root;

				_editorTXTFile = Path.GetFullPath(cfgRoot.Root + "/FileForTestTM/textWithoutTags.txt");
				_editorTMXFile = Path.GetFullPath(cfgRoot.Root + "/FileForTestTM/textWithoutTags.tmx");

				_longTxtFile = Path.GetFullPath(cfgRoot.Root + "/LongTxtTmx/LongText.txt");
				_longTmxFile = Path.GetFullPath(cfgRoot.Root + "/LongTxtTmx/LongTM.tmx");

				_tmFile = Path.GetFullPath(cfgRoot.Root + "/Earth.tmx");
				_secondTmFile = Path.GetFullPath(cfgRoot.Root + "/TextEngTestAddTMX.tmx");
				_importGlossaryFile = Path.GetFullPath(cfgRoot.Root + "/TestGlossary.xlsx");
				_imageFile = Path.GetFullPath(cfgRoot.Root + "/TestImage.jpg");
				_audioFile = Path.GetFullPath(cfgRoot.Root + "/TestAudio.mp3");
				_rtfFile = Path.GetFullPath(cfgRoot.Root + "/rtf1.rtf");
				_txtFileForMatchTest = Path.GetFullPath(cfgRoot.Root + "/FilesForMatchTest/TxtFileForMatchTest.docx");
				_tmxFileForMatchTest = Path.GetFullPath(cfgRoot.Root + "/FilesForMatchTest/TmxFileForMatchTest.tmx");
				_photoLoad = Path.GetFullPath(cfgRoot.Root + "/FilesForLoadPhotoInRegistration/");
				_testUserFile = Path.GetFullPath(cfgRoot.RootToConfig + "/TestUsers.xml");
			
				if (TestUserFileExist())
				{
					var _cfgTestUser = TestSettingDefinition.Instance.Get<TestUserConfig>();
					var _cfgTestCompany = TestSettingDefinition.Instance.Get<TestUserConfig>();

					_testUserList = new List<UserInfo>();
					_testCompanyList = new List<UserInfo>();
					// Добавление пользователей в _testUserList из конфига
					for (int v = 0; v < _cfgTestUser.Users.Count; v++)
						_testUserList.Add(
								new UserInfo(_cfgTestUser.Users[v].Login, _cfgTestUser.Users[v].Password, _cfgTestUser.Users[v].Activated));

					for (int v = 0; v < _cfgTestCompany.Companies.Count; v++)
						_testCompanyList.Add(
							new UserInfo(_cfgTestCompany.Companies[v].Login, _cfgTestCompany.Companies[v].Password, _cfgTestCompany.Companies[v].Activated));
				}
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Ошибка в конструкторе : " + ex.Message, ex);
				throw;
			}
		}
		public enum RegistrationType
		{
			User,
			Company
		}

		public enum RegistrationField
		{
			FirstName,
			LastName,
			CompanyName,
			DomainName,
			PhoneNumber,
			CompanyType
		}
		public bool TestUserFileExist()
		{
			return File.Exists(_testUserFile);
		}

		/// <summary>
		/// Файл со списком пользователей , имеющие аккаунты на аол/курсера/передем
		/// </summary>
		private string _testUserFile;
		/// <summary>
		///  Файл со списком пользователей , имеющие аккаунты на аол/курсера/передем
		/// </summary>
		protected string TestUserFile
		{
			get
			{
				return _testUserFile;
			}
		}
		/// <summary>
		/// WebDriver
		/// </summary>
		private IWebDriver _driver;
		/// <summary>
		/// WebDriver
		/// </summary>
		protected IWebDriver Driver
		{
			get
			{
				return _driver;
			}
			set
			{
				_driver = value;
			}
		}

		/// <summary>
		/// Wait
		/// </summary>
		private WebDriverWait _wait;
		/// <summary>
		/// Wait
		/// </summary>
		protected WebDriverWait Wait
		{
			get
			{
				return _wait;
			}
		}

		/// <summary>
		/// Профиль
		/// </summary>
		private FirefoxProfile _profile;

		/// <summary>
		/// Url
		/// </summary>
		private string _url;
		/// <summary>
		/// Url
		/// </summary>
		protected string Url
		{
			get
			{
				return _url;
			}
		}

		/// <summary>
		/// Url workspace
		/// </summary>
		private string _workspaceUrl;
		/// <summary>
		/// Url workspace
		/// </summary>
		protected string workspaceUrl
		{
			get
			{
				return _workspaceUrl;
			}
		}

		/// <summary>
		/// URL соответствующей админки
		/// </summary>
		private string _adminUrl;
		/// <summary>
		/// URL соответствующей админки
		/// </summary>
		protected string AdminUrl
		{
			get
			{
				return _adminUrl;
			}
		}

		/// <summary>
		/// Логин пользователя
		/// </summary>
		private string _login;
		/// <summary>
		/// Логин пользователя
		/// </summary>
		protected string Login
		{
			get
			{
				return _login;
			}
		}

		/// <summary>
		/// Пароль пользователя
		/// </summary>
		private string _password;
		/// <summary>
		/// Пароль пользователя
		/// </summary>
		protected string Password
		{
			get
			{
				return _password;
			}
		}

		/// <summary>
		/// Имя пользователя
		/// </summary>
		private string _userName;
		/// <summary>
		/// Имя пользователя
		/// </summary>
		protected string UserName
		{
			get
			{
				return _userName;
			}
		}

		/// <summary>
		/// Логин второго пользователя
		/// </summary>
		private string _login2;
		/// <summary>
		/// Логин второго пользователя
		/// </summary>
		protected string Login2
		{
			get
			{
				return _login2;
			}
		}

		/// <summary>
		/// Пароль второго пользователя
		/// </summary>
		private string _password2;
		/// <summary>
		/// Пароль второго пользователя
		/// </summary>
		protected string Password2
		{
			get
			{
				return _password2;
			}
		}

		/// <summary>
		/// Имя второго пользователя
		/// </summary>
		private string _userName2;
		/// <summary>
		/// Имя второго пользователя
		/// </summary>
		protected string UserName2
		{
			get
			{
				return _userName2;
			}
		}

		private List<UserInfo> _testUserList;
		protected List<UserInfo> TestUserList
		{
			get
			{
				return _testUserList;
			}
		}

		private List<UserInfo> _testCompanyList;
		protected List<UserInfo> TestCompanyList
		{
			get
			{
				return _testCompanyList;
			}
		}
		/// <summary>
		/// Уникальное для теста название проекта
		/// </summary>
		private string _projectName;
		/// <summary>
		/// Уникальное для теста название проекта
		/// </summary>
		protected string ProjectName
		{
			get
			{
				return _projectName;
			}
		}

		/// <summary>
		/// Общее/постоянное название ТМ
		/// </summary>
		private string _constTmName;
		/// <summary>
		/// Общее/постоянное название ТМ
		/// </summary>
		protected string ConstTMName
		{
			get
			{
				return _constTmName;
			}
		}

		/// <summary>
		/// Общая часть названия глоссария
		/// </summary>
		private string _glossaryName;
		/// <summary>
		/// Общая часть названия глоссария
		/// </summary>
		protected string GlossaryName
		{
			get
			{
				return _glossaryName;
			}
		}

		/// <summary>
		/// Path результатов теста
		/// </summary>
		protected string PathTestResults
		{
			get
			{
				DirectoryInfo directoryInfo =
					Directory.GetParent(@"..\TestResults\");

				return directoryInfo.ToString();
			}
		}

		/// <summary>
		/// Путь к файлам для тестирования, взятый из AgentSpecific.xml
		/// </summary>
		private string _pathTestFiles;

		/// <summary>
		/// Path файлов для тестирования
		/// </summary>
		public string PathTestFiles
		{
			get
			{
				return Directory.GetParent(
					string.Concat(
						_pathTestFiles, 
						Path.DirectorySeparatorChar))
					.ToString();
			}
		}
		
		/// <summary>
		/// Общее имя проекта с одним документом
		/// </summary>
		private string _projectNameExportTestOneDoc;
		/// <summary>
		/// Общее имя проекта с одним документом
		/// </summary>
		protected string ProjectNameExportTestOneDoc
		{
			get
			{
				return _projectNameExportTestOneDoc;
			}
		}

		/// <summary>
		/// Общее имя проекта с несколькими документами
		/// </summary>
		private string _projectNameExportTestMultiDoc;
		/// <summary>
		/// Общее имя проекта с несколькими документами
		/// </summary>
		protected string ProjectNameExportTestMultiDoc
		{
			get
			{
				return _projectNameExportTestMultiDoc;
			}
		}

		/// <summary>
		/// Deadline дата в английской локали
		/// </summary>
		private string _deadlineDate;
		/// <summary>
		/// Deadline дата в английской локали
		/// </summary>
		protected string DeadlineDate
		{
			get
			{
				return _deadlineDate;
			}
		}

		/// <summary>
		/// Полный путь к документу для загрузки
		/// </summary>
		private string _documentFile;
		/// <summary>
		/// Полный путь к документу для загрузки
		/// </summary>
		protected string DocumentFile
		{
			get
			{
				return _documentFile;
			}
		}

		///// <summary>
		///// Полный путь к документу тестовых юзеров
		///// </summary>
		//private string _testUserFile;
		///// <summary>
		///// Полный путь к документу тестовых юзеров
		///// </summary>
		//protected string TestUserFile
		//{
		//	get
		//	{
		//		return _testUserFile;
		//	}
		//}
		/// <summary>
		/// Полный путь к документу без тегов
		/// </summary>
		private string _documentFileToConfirm;
		/// <summary>
		/// Полный путь к документу без тегов
		/// </summary>
		protected string DocumentFileToConfirm
		{
			get
			{
				return _documentFileToConfirm;
			}
		}

		/// <summary>
		/// Полный путь ко второму документу без тегов
		/// </summary>
		private string _documentFileToConfirm2;
		/// <summary>
		/// Полный путь ко второму документу без тегов
		/// </summary>
		protected string DocumentFileToConfirm2
		{
			get
			{
				return _documentFileToConfirm2;
			}
		}

		/// <summary>
		/// Полный путь к файлу TMX
		/// </summary>
		private string _tmFile;
		/// <summary>
		/// Полный путь к файлу TMX
		/// </summary>
		protected string TmFile
		{
			get
			{
				return _tmFile;
			}
		}

		/// <summary>
		/// Полный путь ко второму файлу TMX
		/// </summary>
		private string _secondTmFile;
		/// <summary>
		/// Полный путь ко второму файлу TMX
		/// </summary>
		protected string SecondTmFile
		{
			get
			{
				return _secondTmFile;
			}
		}

		/// <summary>
		/// Полный путь к файлу TXT для работы в редакторе
		/// </summary>
		private string _editorTXTFile;
		/// <summary>
		/// Полный путь к файлу TXT для работы в редакторе
		/// </summary>
		protected string EditorTXTFile
		{
			get
			{
				return _editorTXTFile;
			}
		}

		/// <summary>
		/// Полный путь к файлу TXT из 25 строк для работы в редакторе
		/// </summary>
		private string _longTxtFile;
		/// <summary>
		/// Полный путь к файлу TXT из 25 строк для работы в редакторе
		/// </summary>
		protected string LongTxtFile
		{
			get
			{
				return _longTxtFile;
			}
		}

		/// <summary>
		/// Полный путь к файлу TMX для работы в редакторе
		/// </summary>
		private string _editorTMXFile;
		/// <summary>
		/// Полный путь к файлу TMX для работы в редакторе
		/// </summary>
		protected string EditorTMXFile
		{
			get
			{
				return _editorTMXFile;
			}
		}

		/// <summary>
		/// Полный путь к файлу TMX для longTxt 
		/// </summary>
		private string _longTmxFile;
		/// <summary>
		/// Полный путь к файлу TMX для longTxt
		/// </summary>
		protected string LongTmxFile
		{
			get
			{
				return _longTmxFile;
			}
		}

		/// <summary>
		/// Полный путь к RTF
		/// </summary>
		private string _rtfFile;
		/// <summary>
		/// Полный путь к RTF
		/// </summary>
		protected string RtfFile
		{
			get
			{
				return _rtfFile;
			}
		}
		/// <summary>
		/// Полный путь к Txt для match теста
		/// </summary>
		private string _txtFileForMatchTest;
		/// <summary>
		/// Полный путь к Txt для match теста
		/// </summary>
		protected string TxtFileForMatchTest
		{
			get
			{
				return _txtFileForMatchTest;
			}
		}
		/// <summary>
		/// Полный путь к Tmx для match теста
		/// </summary>
		private string _tmxFileForMatchTest;
		/// <summary>
		/// Полный путь к Tmx для match теста
		/// </summary>
		protected string TmxFileForMatchTest
		{
			get
			{
				return _tmxFileForMatchTest;
			}
		}
		/// <summary>
		/// Полный путь к файлу для импорта глоссария
		/// </summary>
		private string _importGlossaryFile;
		/// <summary>
		/// Полный путь к файлу для импорта глоссария
		/// </summary>
		protected string ImportGlossaryFile
		{
			get
			{
				return _importGlossaryFile;
			}
		}

		/// <summary>
		/// Путь к изображению
		/// </summary>
		private string _imageFile;
		/// <summary>
		/// Путь к изображению
		/// </summary>
		protected string ImageFile
		{
			get
			{
				return _imageFile;
			}
		}

		/// <summary>
		/// Путь к аудиофайлу (медиа)
		/// </summary>
		private string _audioFile;
		/// <summary>
		/// Путь к аудиофайлу (медиа)
		/// </summary>
		protected string AudioFile
		{
			get
			{
				return _audioFile;
			}
		}

		/// <summary>
		/// Имя браузера
		/// </summary>
		private string _browserName;
		/// <summary>
		/// Имя браузера
		/// </summary>
		protected string BrowserName
		{
			get
			{
				return _browserName;
			}
		}


		/// <summary>
		/// Полный путь к фото для загрузки на стр регистрации
		/// </summary>
		private string _photoLoad;
		/// <summary>
		/// Полный путь к фото для загрузки на стр регистрации
		/// </summary>
		protected string PhotoLoad
		{
			get
			{
				return _photoLoad;
			}
		}

		/// <summary>
		/// Страница с проектом
		/// </summary>
		private ProjectPageHelper _projectPageHelper;
		/// <summary>
		/// Страница с проектом
		/// </summary>
		protected ProjectPageHelper ProjectPage
		{
			get
			{
				return _projectPageHelper;
			}
		}

		/// <summary>
		/// Страница редактора
		/// </summary>
		private EditorPageHelper _editorPageHelper;
		/// <summary>
		/// Страница редактора
		/// </summary>
		protected EditorPageHelper EditorPage
		{
			get
			{
				return _editorPageHelper;
			}
		}

		/// <summary>
		/// Страница входа (Login)
		/// </summary>
		private LoginPageHelper _loginPageHelper;
		/// <summary>
		/// Страница входа (Login)
		/// </summary>
		protected LoginPageHelper LoginPage
		{
			get
			{
				return _loginPageHelper;
			}
		}

		/// <summary>
		/// Страница со списком проектов (Workspace)
		/// </summary>
		private WorkSpacePageHelper _workspacePageHelper;
		/// <summary>
		/// Страница со списком проектов (Workspace)
		/// </summary>
		protected WorkSpacePageHelper WorkspacePage
		{
			get
			{
				return _workspacePageHelper;
			}
		}

		/// <summary>
		/// Диалог создания проекта
		/// </summary>
		private Workspace_CreateProjectDialogHelper _workspaceCreateProjectHelper;
		/// <summary>
		/// Диалог создания проекта
		/// </summary>
		protected Workspace_CreateProjectDialogHelper WorkspaceCreateProjectDialog
		{
			get
			{
				return _workspaceCreateProjectHelper;
			}
		}

		/// <summary>
		/// Основной Helper (для работы с переходом между страницами - ссылки в верхнем меню)
		/// </summary>
		private MainHelper _mainHelper;
		/// <summary>
		/// Основной Helper (для работы с переходом между страницами - ссылки в верхнем меню)
		/// </summary>
		protected MainHelper MainHelperClass
		{
			get
			{
				return _mainHelper;
			}
		}

		/// <summary>
		/// Страница Domain
		/// </summary>
		private DomainPageHelper _domainPageHelper;
		/// <summary>
		/// Страница Domain
		/// </summary>
		protected DomainPageHelper DomainPage
		{
			get
			{
				return _domainPageHelper;
			}
		}

		/// <summary>
		/// Страница TM
		/// </summary>
		private TMPageHelper _tmPageHelper;
		/// <summary>
		/// Страница TM
		/// </summary>
		protected TMPageHelper TMPage
		{
			get
			{
				return _tmPageHelper;
			}
		}

		/// <summary>
		/// Страница со списком глоссариев
		/// </summary>
		private GlossaryListPageHelper _glossaryListPageHelper;
		/// <summary>
		/// Страница со списком глоссариев
		/// </summary>
		protected GlossaryListPageHelper GlossaryListPage
		{
			get
			{
				return _glossaryListPageHelper;
			}
		}

		/// <summary>
		/// Страница глоссария
		/// </summary>
		private GlossaryPageHelper _glossaryPageHelper;
		/// <summary>
		/// Страница глоссария
		/// </summary>
		protected GlossaryPageHelper GlossaryPage
		{
			get
			{
				return _glossaryPageHelper;
			}
		}

		/// <summary>
		/// Страница Поиска/Перевода
		/// </summary>
		private SearchPageHelper _searchPageHelper;
		/// <summary>
		/// Страница Поиска/Перевода
		/// </summary>
		protected SearchPageHelper SearchPage
		{
			get
			{
				return _searchPageHelper;
			}
		}

		/// <summary>
		/// Страница Client
		/// </summary>
		private ClientPageHelper _clientPageHelper;
		/// <summary>
		/// Страница Client
		/// </summary>
		protected ClientPageHelper ClientPage
		{
			get
			{
				return _clientPageHelper;
			}
		}

		/// <summary>
		/// Работа с админкой
		/// </summary>
		private AdminPageHelper _adminPageHelper;
		/// <summary>
		/// Работа с админкой
		/// </summary>
		protected AdminPageHelper AdminPage
		{
			get
			{
				return _adminPageHelper;
			}
		}

		/// <summary>
		/// Форма изменения структуры глоссария
		/// </summary>
		private GlossaryEditStructureFormHelper _glossaryEditStructureFormHelper;
		/// <summary>
		/// Форма изменения структуры глоссария
		/// </summary>
		protected GlossaryEditStructureFormHelper GlossaryEditStructureForm
		{
			get
			{
				return _glossaryEditStructureFormHelper;
			}
		}

		/// <summary>
		/// Страница со словарями
		/// </summary>
		private DictionaryPageHelper _dictionaryPageHelper;
		/// <summary>
		/// Страница со словарями
		/// </summary>
		protected DictionaryPageHelper DictionaryPage
		{
			get
			{
				return _dictionaryPageHelper;
			}
		}

		/// <summary>
		/// Вкладка Ревизии в редакторе
		/// </summary>
		private Editor_RevisionPageHelper _revisionPageHelper;
		/// <summary>
		/// Вкладка Ревизии в редакторе
		/// </summary>
		protected Editor_RevisionPageHelper RevisionPage
		{
			get
			{
				return _revisionPageHelper;
			}
		}

		/// <summary>
		/// Страница с настройкой прав пользователей
		/// </summary>
		private UserRightsPageHelper _userRightsPageHalper;
		/// <summary>
		/// Страница с настройкой прав пользователей
		/// </summary>
		protected UserRightsPageHelper UserRightsPage
		{
			get
			{
				return _userRightsPageHalper;
			}
		}

		/// <summary>
		/// Диалог предложения термина
		/// </summary>
		private SuggestTermDialogHelper _suggestTermDialogHelper;
		/// <summary>
		/// Диалог предложения термина
		/// </summary>
		protected SuggestTermDialogHelper SuggestTermDialog
		{
			get
			{
				return _suggestTermDialogHelper;
			}
		}

		/// <summary>
		/// Форма редактирования глоссария
		/// </summary>
		private GlossaryEditFormHelper _glossaryEditFormHelper;
		/// <summary>
		/// Форма редактирования глоссария
		/// </summary>
		protected GlossaryEditFormHelper GlossaryEditForm
		{
			get
			{
				return _glossaryEditFormHelper;
			}
		}

		/// <summary>
		/// Страница с предложенными терминами глоссариев
		/// </summary>
		public GlossarySuggestPageHelper _glossarySuggestPageHelper;
		/// <summary>
		/// Страница с предложенными терминами глоссариев
		/// </summary>
		protected GlossarySuggestPageHelper GlossarySuggestPage
		{
			get
			{
				return _glossarySuggestPageHelper;
			}
		}

		/// <summary>
		/// Страница выдачи
		/// </summary>
		private CatPanelResultsHelper _catPanelHelper;
		/// <summary>
		/// Страница выдачи
		/// </summary>
		protected CatPanelResultsHelper CatPanel
		{
			get
			{
				return _catPanelHelper;
			}
		}

		/// <summary>
		/// Окно прав пользователя
		/// </summary>
		private ResponsiblesDialogHelper _responsiblesDialogHelper;
		/// <summary>
		/// Окно прав пользователя
		/// </summary>
		protected ResponsiblesDialogHelper ResponsiblesDialog
		{
			get
			{
				return _responsiblesDialogHelper;
			}
		}

		/// <summary>
		/// Форма добавления нового термина в редакторе
		/// </summary>
		private AddTermFormHelper _addTermFormHelper;
		/// <summary>
		/// Форма добавления нового термина в редакторе
		/// </summary>
		protected AddTermFormHelper AddTermForm
		{
			get
			{
				return _addTermFormHelper;
			}
		}

		private RegistrationPageHelper _registrationPageHelper;
		protected RegistrationPageHelper RegistrationPage
		{
			get
			{
				return _registrationPageHelper;
			}
		}

		// информация о тесте
		protected DateTime testBeginTime;

		/// <summary>
		/// Вкл/откл закрытия драйвера после каждого теста
		/// </summary>
		protected bool quitDriverAfterTest;



		/// <summary>
		/// Начальная подготовка для группы тестов
		/// </summary>
		[TestFixtureSetUp]
		public void SetupAllBase()
		{
		}

		/// <summary>
		/// Начальная подготовка для каждого теста
		/// </summary>
		[SetUp]
		public void SetupBase()
		{
			// По умолчанию выходим из браузера
			quitDriverAfterTest = true;

			// Вывести время начала теста
			testBeginTime = DateTime.Now;
			Console.WriteLine(TestContext.CurrentContext.Test.Name + "\nStart: " + testBeginTime.ToString());

			if (_driver == null)
			{
				// Если конструктор заново не вызывался, то надо заполнить _driver
				CreateDriver();
			}

			// Создание уникального имени проекта
			CreateUniqueNamesByDatetime();
		}

		/// <summary>
		/// Конечные действия для группы тестов
		/// </summary>
		[TestFixtureTearDown]
		public virtual void TeardownAllBase()
		{
			// Закрыть драйвер
			ExitDriver();
		}

		/// <summary>
		/// Конечные действия для каждого теста
		/// </summary>
		[TearDown]
		public void TeardownBase()
		{
			// При вылете браузера возникает ошибка, пытаемся ее словить
			try
			{
				if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
				{
					// Сделать скриншот
					ITakesScreenshot screenshotDriver = _driver as ITakesScreenshot;
					Screenshot screenshot = screenshotDriver.GetScreenshot();

					// Создать папку для скриншотов провалившихся тестов
					string failResultPath = Path.Combine(PathTestResults, "FailedTests");
					Directory.CreateDirectory(failResultPath);
					// Создать имя скриншота по имени теста
					string screenName = TestContext.CurrentContext.Test.Name;
					if (screenName.Contains("("))
					{
						// Убрать из названия теста аргументы (файлы)
						screenName = screenName.Substring(0, screenName.IndexOf("("));
					}
					screenName += DateTime.Now.Ticks + ".png";
					// Создать полное имя файла
					screenName = Path.Combine(failResultPath, screenName);
					// Сохранить скриншот
					screenshot.SaveAsFile(screenName, ImageFormat.Png);
				}
			}
			catch (Exception)
			{
				// Закрыть драйвер
				ExitDriver();
			}

			// Выходим из браузера, если нужно
			if (quitDriverAfterTest)
			{
				ExitDriver();
			}

			// Вывести информацию о прохождении теста
			DateTime testFinishTime = DateTime.Now;
			// Время окончания теста
			Console.WriteLine("Finish: " + testFinishTime.ToString());
			// Длительность теста
			TimeSpan duration = TimeSpan.FromTicks(testFinishTime.Ticks - testBeginTime.Ticks);
			string durResult = "Duration: ";
			if (duration.TotalMinutes > 1)
			{
				durResult += duration.TotalMinutes + "min";
			}
			else
			{
				durResult += duration.TotalSeconds + "sec";
			}

			durResult += " (" + duration.TotalMilliseconds + "ms).";
			Console.WriteLine(durResult);

			if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Failed))
			{
				// Если тест провалился
				Console.WriteLine("Fail!");
			}
		}



		/// <summary>
		/// Обновить уникальные имена для нового теста
		/// </summary>
		protected void CreateUniqueNamesByDatetime()
		{
			_projectName = "Test Project" + "_" + DateTime.UtcNow.Ticks;
		}

		/// <summary>
		/// Метод создания _driver 
		/// </summary>
		private void CreateDriver()
		{
			if (BrowserName == "Firefox")
			{
				if (_driver == null)
				{
					_profile = new FirefoxProfile();
					_profile.AcceptUntrustedCertificates = true;
					// Изменение языка браузера на английский
					_profile.SetPreference("intl.accept_languages", "en");
					_profile.SetPreference("browser.download.dir", PathTestResults);
					_profile.SetPreference("browser.download.folderList", 2);
					_profile.SetPreference("browser.download.useDownloadDir", false);
					//_profile.SetPreference("browser.download.manager.showWhenStarting", false);
					_profile.SetPreference("browser.helperApps.alwaysAsk.force", false);
					_profile.SetPreference
						("browser.helperApps.neverAsk.saveToDisk", "text/xml, text/csv, text/plain, text/log, application/zip, application/x-gzip, application/x-compressed, application/x-gtar, multipart/x-gzip, application/tgz, application/gnutar, application/x-tar, application/x-xliff+xml,  application/msword.docx, application/pdf, application/x-pdf, application/octetstream, application/x-ttx, application/x-tmx, application/octet-stream");
					//_profile.SetPreference("pdfjs.disabled", true);

					_driver = new FirefoxDriver(_profile);
					//string profiledir = "../../../Profile";
					// string profiledir = "TestingFiles/Profile";
					//_profile = new FirefoxProfile(profiledir);
					//_driver = new FirefoxDriver(_profile);
				}
			}
			else if (BrowserName == "Chrome")
			{
				// драйвер работает некорректно
				_driver = new ChromeDriver();
			}
			else if (BrowserName == "IE")
			{
				// не запускается
			}

			setDriverTimeoutDefault();
			_wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

			_driver.Manage().Window.Maximize();

			RecreateDrivers();
		}

		/// <summary>
		/// Пересоздание Helper'ов с новыми Driver, Wait
		/// </summary>










		private void RecreateDrivers()
		{
			_projectPageHelper = new ProjectPageHelper(Driver, Wait);
			_editorPageHelper = new EditorPageHelper(Driver, Wait);
			_loginPageHelper = new LoginPageHelper(Driver, Wait);
			_workspacePageHelper = new WorkSpacePageHelper(Driver, Wait);
			_workspaceCreateProjectHelper = new Workspace_CreateProjectDialogHelper(Driver, Wait);
			_mainHelper = new MainHelper(Driver, Wait);
			_domainPageHelper = new DomainPageHelper(Driver, Wait);
			_tmPageHelper = new TMPageHelper(Driver, Wait);
			_glossaryListPageHelper = new GlossaryListPageHelper(Driver, Wait);
			_glossaryPageHelper = new GlossaryPageHelper(Driver, Wait);
			_searchPageHelper = new SearchPageHelper(Driver, Wait);
			_clientPageHelper = new ClientPageHelper(Driver, Wait);
			_adminPageHelper = new AdminPageHelper(Driver, Wait);
			_glossaryEditStructureFormHelper = new GlossaryEditStructureFormHelper(Driver, Wait);
			_dictionaryPageHelper = new DictionaryPageHelper(Driver, Wait);
			_revisionPageHelper = new Editor_RevisionPageHelper(Driver, Wait);
			_userRightsPageHalper = new UserRightsPageHelper(Driver, Wait);
			_suggestTermDialogHelper = new SuggestTermDialogHelper(Driver, Wait);
			_glossaryEditFormHelper = new GlossaryEditFormHelper(Driver, Wait);
			_glossarySuggestPageHelper = new GlossarySuggestPageHelper(Driver, Wait);
			_catPanelHelper = new CatPanelResultsHelper(Driver, Wait);
			_responsiblesDialogHelper = new ResponsiblesDialogHelper(Driver, Wait);
			_addTermFormHelper = new AddTermFormHelper(Driver, Wait);
			_registrationPageHelper = new RegistrationPageHelper(Driver, Wait);

		}

		/// <summary>
		/// Установить время ожидания драйвера в минимум (для поиска элементов, которых по ожиданию нет)
		/// </summary>
		protected void setDriverTimeoutMinimum()
		{
			_driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
		}

		/// <summary>
		/// Установить стандартное время ожидание драйвера
		/// </summary>
		protected void setDriverTimeoutDefault()
		{
			_driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(15));
		}

		/// <summary>
		/// Метод назначения задачи на пользователя 
		/// </summary>
		/// <param name="documentRowNum">номер документа</param>
		protected void AssignTask(int documentRowNum = 1)
		{
			// Открываем инфо документа 
			ProjectPage.OpenDocumentInfo(documentRowNum);

			// Открываем окно прав исполнителей
			ProjectPage.ClickAssignRessponsibleBtn();

			//// нажать галочку около документа
			//SelectDocumentInProject(documentRowNum);
			//// нажать на Progress
			//ProjectPage.ClickProgressBtn();
			ProjectPage.WaitProgressDialogOpen();

			// Назначить ответственного в окне Progress
			ProjectPage.ClickUserNameCell();
			ProjectPage.WaitAssignUserList();

			// Выбрать нужное имя
			ProjectPage.ClickAssignUserListUser(UserName);
			ProjectPage.WaitAssignBtnDisplay();

			// Нажать на Assign
			ProjectPage.ClickAssignBtn();
			// Дождаться появления Cancel
			ProjectPage.WaitCancelAssignBtnDisplay();

			// Нажать на Close
			ProjectPage.CloseAssignDialogClick();

			SelectDocumentInProject(documentRowNum);

			// Обновить страницу, чтобы активен был переход в редактор
			_driver.Navigate().Refresh();

			// Нажать на Accept
			ProjectPage.ClickAllAcceptBtns();
		}

		/// <summary>
		/// Нажать галочку у документа в проекте
		/// </summary>
		/// <param name="documentNumber"></param>
		protected void SelectDocumentInProject(int documentNumber = 1)
		{
			// Нажать галочку у документа
			Assert.IsTrue(ProjectPage.SelectDocument(documentNumber),
				"Ошибка: на странице проекта нет документа");
		}

		/// <summary>
		/// Метод открытия документа в редакторе
		/// </summary>
		protected void OpenDocument(int documentNumber = 1)
		{
			// Открыть документ
			Assert.IsTrue(ProjectPage.OpenDocument(documentNumber), "Ошибка: на странице проекта нет документа");
			if (ResponsiblesDialog.WaitUntilChooseTaskDialogDisplay())
			{
				Assert.True(EditorPage.GetTaskBtnIsExist(), "Ошибка: Неверный этап в окне выбора ");
				//выбрать задачу перевода и открыть редактор
				EditorPage.ClickTaskBtn();
				EditorPage.ClickContBtn();
			}
			// Дождаться загрузки страницы
			EditorPage.WaitPageLoad();

			Thread.Sleep(1000);

			// Проверить, существует ли хотя бы один сегмент
			Assert.IsTrue(EditorPage.GetSegmentsExist(), "Ошибка: нет сегментов");
		}

		/// <summary>
		/// Авторизация
		/// </summary>
		/// <param name="accountName">аккаунт</param>
		/// <param name="alternativeUser">Использовать альтернативный аккаунт</param>
		public void Authorization(string accountName = "TestAccount", bool alternativeUser = false)
		{
			string authLogin = "";
			string authPassword = "";

			if (!alternativeUser)
			{
				authLogin = _login;
				authPassword = _password;
			}
			else
			{
				authLogin = _login2;
				authPassword = _password2;
			}

			// Перейти на стартовую страницу
			_driver.Navigate().GoToUrl(_url+"/sign-in");

			if (Driver.Url.Contains("pro"))
			{
				// Проверить, загрузилась ли
				Assert.IsTrue(LoginPage.WaitPageLoadLpro(),
					"Не прогрузилась страница Login - возможно, сайт недоступен");

				// Заполнить логин и пароль
				LoginPage.EnterLoginLpro(authLogin);
				LoginPage.EnterPasswordLpro(authPassword);
				Thread.Sleep(1000);
				LoginPage.ClickSubmitLpro();
			}
			else
			{
				// Проверить, загрузилась ли
				Assert.IsTrue(LoginPage.WaitPageLoad(),
					"Не прогрузилась страница Login - возможно, сайт недоступен");
				// Заполнить логин и пароль
				LoginPage.EnterLogin(authLogin);
				LoginPage.EnterPassword(authPassword);
				LoginPage.ClickSubmitCredentials();

				Thread.Sleep(3000);

				// Проверить, появился ли список аккаунтов
				if (LoginPage.WaitAccountExist(accountName, 5))
				{
					// Выбрать аккаунт
					LoginPage.ClickAccountName(accountName);
					// Зайти на сайт
					//LoginPage.ClickSubmitAccount();
				}
				else if (LoginPage.GetIsErrorExist())
				{
					Assert.Fail("Появилась ошибка при входе! М.б.недоступен AOL.");
				}
				// иначе у пользователя только 1 аккаунт
			}
			// Изменили язык на Английский
			Assert.IsTrue(WorkspacePage.WaitAppearLocaleBtn(), "Не дождались загрузки страницы со ссылкой для изменения языка");
			WorkspacePage.SelectLocale(WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.English);
		}

		/// <summary>
		/// Переход на вкладку workspace
		/// Если переадресация на стартовую страницу, то авторизация
		/// </summary>
		public void GoToWorkspace()
		{
			// Отлавливаем Modal Dialog Exception
			// В случае, если для завершения предыдущего теста нужно закрыть дополнительный диалог
			try
			{
				// Перейти на страницу workspace
				_driver.Navigate().GoToUrl(_workspaceUrl);

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					
					// Проходим процедуру авторизации
					Authorization();
				}
			}
			catch
			{
				_driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Пробуем перейти на страницу еще раз
				GoToWorkspace();
			}
		}

		/// <summary>
		/// Переход на admin страницу
		/// </summary>
		public void GoToAdminPage()
		{
			try
			{
				// Перейти на  admin страницу
				_driver.Navigate().GoToUrl(_adminUrl);
				//Assert.IsTrue(AdminPage.WaitPageLoad(), "Ошибка: страница админки не загрузилась");
			}
			catch
			{
				_driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Пробуем перейти на страницу еще раз
				GoToAdminPage();
			}
		}

		/// <summary>
		/// Переход на вкладку глоссариев
		/// Если переадресация на стартовую страницу, то авторизация и затем переход
		/// </summary>
		public void GoToGlossaries()
		{
			// Отлавливаем Modal Dialog Exception
			// В случае, если для завершения предыдущего теста нужно закрыть дополнительный диалог
			try
			{
				// Перейти на страницу глоссариев
				_driver.Navigate().GoToUrl(_url + "/Enterprise/Glossaries");

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					// Проходим процедуру авторизации
					Authorization();
					// Пробуем перейти на глоссарии еще раз
					GoToGlossaries();
				}
			}
			catch
			{
				_driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				Thread.Sleep(2000);

				// Пробуем перейти на глоссарии еще раз
				GoToGlossaries();
			}
		}

		/// <summary>
		/// Переход на вкладку TM
		/// Если переадресация на стартовую страницу, то авторизация и затем переход
		/// </summary>
		public void GoToTranslationMemories()
		{
			// Отлавливаем Modal Dialog Exception
			// В случае, если для завершения предыдущего теста нужно закрыть дополнительный диалог
			try
			{
				// Перейти на страницу
				_driver.Navigate().GoToUrl(_url + "/TranslationMemories/Index");

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					// Проходим процедуру авторизации
					Authorization();
					// Пробуем перейти на страницу еще раз
					GoToTranslationMemories();
				}
			}
			catch
			{
				_driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Пробуем перейти на страницу еще раз
				GoToTranslationMemories();
			}
		}

		/// <summary>
		/// Переход на вкладку Domains
		/// Если переадресация на стартовую страницу, то авторизация и затем переход
		/// </summary>
		public void GoToDomains()
		{
			// Отлавливаем Modal Dialog Exception
			// В случае, если для завершения предыдущего теста нужно закрыть дополнительный диалог
			try
			{
				// Перейти на страницу
				_driver.Navigate().GoToUrl(_url + "/Domains/Index");

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					// Проходим процедуру авторизации
					Authorization();
					// Пробуем перейти на страницу еще раз
					GoToDomains();
				}
			}
			catch
			{
				_driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Пробуем перейти на страницу еще раз
				GoToDomains();
			}
		}

		/// <summary>
		/// Переход на вкладку Clients
		/// Если переадресация на стартовую страницу, то авторизация и затем переход
		/// </summary>
		public void GoToClients()
		{
			// Отлавливаем Modal Dialog Exception
			// В случае, если для завершения предыдущего теста нужно закрыть дополнительный диалог
			try
			{
				// Перейти на страницу
				_driver.Navigate().GoToUrl(_url + "/Clients/Index");

				// Если открылась страница логина
				if (LoginPage.WaitPageLoad(1) || LoginPage.WaitPromoPageLoad())
				{
					// Проходим процедуру авторизации
					Authorization();
					// Пробуем перейти на страницу еще раз
					GoToClients();
				}
			}
			catch
			{
				_driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Пробуем перейти на страницу еще раз
				GoToClients();
			}
		}

		/// <summary>
		/// Заполнение первого шага создания проекта
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <param name="useDefaultTargetLanguage">использовать язык target по умолчанию</param>
		/// <param name="srcLang">язык источника</param>
		/// <param name="trgLang">язык перевода</param>
		protected void FirstStepProjectWizard(string projectName, bool useDefaultTargetLanguage = true,
			CommonHelper.LANGUAGE srcLang = CommonHelper.LANGUAGE.English,
			CommonHelper.LANGUAGE trgLang = CommonHelper.LANGUAGE.Russian)
		{
			Assert.IsTrue(WorkspacePage.WaitPageLoad(), "Страница workspace не прогрузилась");

			// Нажать <Create>
			WorkspacePage.ClickCreateProject();
			// Ждем загрузки формы
			WorkspaceCreateProjectDialog.WaitDialogDisplay();

			// Ввести название проекта
			WorkspaceCreateProjectDialog.FillProjectName(projectName);
			// Ввести deadline дату
			WorkspaceCreateProjectDialog.FillDeadlineDate(_deadlineDate);

			// Выбрать Source - en
			WorkspaceCreateProjectDialog.SelectSourceLanguage(srcLang);

			// Проверить, что в Target выставлен русский язык
			SetRusLanguageTarget();

			// Выбрать Target
			if (!useDefaultTargetLanguage)
			{
				// Открыть список Target
				WorkspaceCreateProjectDialog.ClickTargetList();
				// Убираем русский
				WorkspaceCreateProjectDialog.ClickTargetItem(CommonHelper.LANGUAGE.Russian);
				// Добавляем заданный язык
				WorkspaceCreateProjectDialog.ClickTargetItem(trgLang);
				// Закрыть список Target
				WorkspaceCreateProjectDialog.ClickTargetList();
			}
		}

		/// <summary>
		/// Создать проект
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <param name="downloadFile">документ для загрузки</param>
		/// <param name="createNewTM">создать новый TM</param>
		/// <param name="tmFile">файл для загрузки в ТМ</param>
		/// <param name="setGlossary">тип глоссария: новый, первый в списке, по имени</param>
		/// <param name="glossaryName">имя глоссария, если выбирается по имени</param>
		/// <param name="chooseMT">выбрать МТ</param>
		/// <param name="mtType">тип МТ</param>
		/// <param name="isNeedCheckExist">Нужна проверка проекта в списке</param>
		protected void CreateProject(
			string projectName, 
			string downloadFile = "",
			bool createNewTM = false, 
			string tmFile = "",
			Workspace_CreateProjectDialogHelper.SetGlossary setGlossary = Workspace_CreateProjectDialogHelper.SetGlossary.None, 
			string glossaryName = "",
			bool chooseMT = false, 
			Workspace_CreateProjectDialogHelper.MT_TYPE mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.None,
			bool isNeedCheckExist = true)
		{
			// Заполнение полей на первом шаге
			FirstStepProjectWizard(projectName);
			if (downloadFile.Length > 0)
			{
				// Загрузить файл
				WorkspaceCreateProjectDialog.ClickAddDocumentBtn();
				FillAddDocumentForm(downloadFile);
				WorkspaceCreateProjectDialog.WaitDocumentAppear(Path.GetFileName(downloadFile));
			}
			WorkspaceCreateProjectDialog.ClickNextStep();

			//2 шаг - выбор ТМ
			if (createNewTM)
			{
				// Создать новую ТМ, c файлом или чистую
				CreateNewTM(tmFile);
			}
			else
			{
				// Выбрать существующую ТМ
				ChooseFirstTMInList();
			}

			
			WorkspaceCreateProjectDialog.ClickNextStep();

			//3 шаг - выбор глоссария
			switch (setGlossary)
			{
				case Workspace_CreateProjectDialogHelper.SetGlossary.None:
					
					break;
				
				case Workspace_CreateProjectDialogHelper.SetGlossary.First:
		
					WorkspaceCreateProjectDialog.ClickFirstGlossaryInTable();
					break;
		
				case Workspace_CreateProjectDialogHelper.SetGlossary.New:
		
					CreateAndAddGlossary();
					break;
		
				case Workspace_CreateProjectDialogHelper.SetGlossary.ByName:
		
					WorkspaceCreateProjectDialog.ClickGlossaryByName(glossaryName);
					break;
			}
			
			WorkspaceCreateProjectDialog.ClickNextStep();

			//4 шаг - выбор МТ
			if (chooseMT && mtType != Workspace_CreateProjectDialogHelper.MT_TYPE.None)
			{
				WorkspaceCreateProjectDialog.ChooseMT(mtType);
			}
			
			WorkspaceCreateProjectDialog.ClickNextStep();

			//5 шаг - настройка этапов workflow
			//SetUpWorkflow();
			Thread.Sleep(500);
			WorkspaceCreateProjectDialog.ClickNextStep();

			//5 шаг - настройка Pretranslate
			//Pretranslate();
			//Finish
			Thread.Sleep(500);
			WorkspaceCreateProjectDialog.ClickFinishCreate();

			// Дождаться проекта в списке проектов
			if (isNeedCheckExist)
			{
				Assert.IsTrue(WorkspacePage.WaitProjectAppearInList(projectName), "Ошибка: проект не появился в списке Workspace");
			}
			else
			{
				Thread.Sleep(5000);
			}

			// Дождаться, пока документ догрузится
			Assert.IsTrue(WorkspacePage.WaitProjectLoad(projectName), "Ошибка: документ не загрузился");
		}

		/// <summary>
		/// Создать проект и не проверять, создался ли он
		/// </summary>
		/// <param name="projectName">название проекта</param>
		protected void CreateProjectWithoutCheckExist(string projectName)
		{
			CreateProject(projectName, "", false, "", Workspace_CreateProjectDialogHelper.SetGlossary.None, "", false, Workspace_CreateProjectDialogHelper.MT_TYPE.None, false);
		}

		/// <summary>
		/// Создание проекта, если проект с таким именем не создан
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <param name="downloadFile">документ для загрузки</param>
		/// <param name="createNewTM">создать новый TM</param>
		/// <param name="tmFile">файл для загрузки в ТМ</param>
		/// <param name="setGlossary">тип глоссария: новый, первый в списке, по имени</param>
		/// <param name="glossaryName">имя глоссария, если выбирается по имени</param>
		/// <param name="chooseMT">выбрать МТ</param>
		/// <param name="mtType">тип МТ</param>
		/// <param name="isNeedCheckExist">Нужна проверка проекта в списке</param>
		protected void CreateProjectIfNotCreated(string projectName, string downloadFile = "",
			bool createNewTM = false, string tmFile = "",
			Workspace_CreateProjectDialogHelper.SetGlossary setGlossary = Workspace_CreateProjectDialogHelper.SetGlossary.None,
			string glossaryName = "",
			bool chooseMT = false, Workspace_CreateProjectDialogHelper.MT_TYPE mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.None,
			bool isNeedCheckExist = true)
		{
			if (!GetIsExistProject(projectName))
			{
				// Создание проекта
				CreateProject(projectName, downloadFile, createNewTM, tmFile, setGlossary, glossaryName, chooseMT, mtType, isNeedCheckExist);
			}
		}

		/// <summary>
		/// Создать новую ТМ в диалоге создания проекта
		/// </summary>
		/// <param name="TmFileName">файл для загрузки в ТМ</param>
		/// <param name="fromWorkspaceOrProject">true - из workspace, false - из проекта</param>
		public void CreateNewTM(string TmFileName, bool fromWorkspaceOrProject = true)
		{
			string tmName = "TestTM" + DateTime.Now.Ticks;

			//Создание ТМ
			if (TmFileName.Length > 0)
			{
				//Добавить тмх файл
				WorkspaceCreateProjectDialog.ClickUploadTMX();
				WorkspaceCreateProjectDialog.WaitUploadTMXDialog();
				WorkspaceCreateProjectDialog.FillTMNameDialog(tmName);
				WorkspaceCreateProjectDialog.ClickAddTMXDialog();

				WorkspaceCreateProjectDialog.WaitUploadTMXDialog();

				// Заполнить имя файла для загрузки
				FillAddDocumentForm(TmFileName);

				//Нажать на кнопку Import
				WorkspaceCreateProjectDialog.ClickSaveTMXDialog();

				//Дождаться пока ТМ появится в списке
				WorkspaceCreateProjectDialog.WaitTmxAppear(tmName);

				if (WorkspaceCreateProjectDialog.GetIsExistErrorFileMessage())
				{
					// Диалог загрузки документа не закрылся - закрываем
					TryCloseExternalDialog();

					// кликаем Import снова
					WorkspaceCreateProjectDialog.ClickImportBtn();
				}
				WorkspaceCreateProjectDialog.WaitImportDialogDisappear();
			}
			else
			{
				WorkspaceCreateProjectDialog.ClickCreateTM();
				WorkspaceCreateProjectDialog.WaitCreateTMDialog();
				WorkspaceCreateProjectDialog.FillTMName(tmName);
				WorkspaceCreateProjectDialog.ClickSaveTM();
				WorkspaceCreateProjectDialog.WaitUntilCreateTMDialogDisappear();
				//Дождаться пока ТМ появится в списке
				WorkspaceCreateProjectDialog.WaitTmxAppear(tmName);
			}
		}

		/// <summary>
		/// Выбрать первую ТМ из списка доступных 
		/// </summary>
		public void ChooseFirstTMInList()
		{
			Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsTMTableNotEmpty(),
				"Ошибка: пустая таблица TM");

			WorkspaceCreateProjectDialog.ClickFirstTMInTable();
		}
		
		/// <summary>
		/// Создание и подключение нового глоссария
		/// </summary>
		public void CreateAndAddGlossary()
		{
			string internalGlossaryName = "InternalGlossaryName" + DateTime.UtcNow.Ticks.ToString();
			WorkspaceCreateProjectDialog.ClickCreateGlossary();
			WorkspaceCreateProjectDialog.SetNewGlossaryName(internalGlossaryName);
			WorkspaceCreateProjectDialog.ClickSaveNewGlossary();
			WorkspaceCreateProjectDialog.WaitNewGlossaryAppear(internalGlossaryName);
		}

		// TODO
		/// <summary>
		/// Шаг формирования рабочего процесса диалога создания проекта
		/// </summary>
		public void SetUpWorkflow()
		{
			// Сейчас не изменяем ничего на шаге
			//Настроить этапы workflow
			//_wait.Until(d => _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-new-stage')]")));
			//_driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-new-stage')]")).Click();
		}

		// TODO
		/// <summary>
		/// Шаг Pretranslate диалога создания проекта
		/// </summary>
		public void Pretranslate()
		{
			// Сейчас не изменяем ничего на шаге
			//_wait.Until(d => _driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-finish js-upload-btn')]")));
			//_driver.FindElement(By.XPath(".//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-finish js-upload-btn')]")).Click();
		}

		/// <summary>
		/// метод открытия настроек проекта и загрузки нового документа
		/// </summary>
		/// <param name="filePath">путь в файлу, импортируемого в проект</param>
		/// <param name="projectName">имя проекта</param>
		protected void ImportDocumentProjectSettings(string filePath, string projectName)
		{
			// Зайти в проект
			OpenProjectPage(projectName);

			// Кликнуть Import
			ProjectPage.ClickImportBtn();

			// ждем, когда загрузится окно для загрузки документа 
			ProjectPage.WaitImportDialogDisplay();
			// Нажать Add
			ProjectPage.ClickAddDocumentInImport();
			// Заполнить диалог загрузки
			FillAddDocumentForm(filePath);

			// Нажать Next
			ProjectPage.ClickNextImportDialog();
			Console.WriteLine("кликнули Next");

			// Если появилось сообщение, что не указали файл, значит, Enter не нажался
			if (ProjectPage.GetIsExistNoFileError())
			{
				// Диалог загрузки документа не закрылся - закрываем
				TryCloseExternalDialog();

				// кликнуть Next снова
				ProjectPage.ClickNextImportDialog();
			}
			// Дождаться появления ТМ таблицы
			ProjectPage.WaitImportTMTableDisplay();

			// Next
			ProjectPage.ClickNextImportDialog();
			// Нажать Finish
			ProjectPage.ClickFinishImportDialog();
			// Дождаться окончания загрузки
			Assert.IsTrue(ProjectPage.WaitDocumentDownloadFinish(),
				"Ошибка: документ загружается слишком долго");
		}

		/// <summary>
		/// Проверить, есть ли проект в списке на странице Workspace
		/// </summary>
		/// <param name="projectName">название проекта</param>
		/// <returns>есть</returns>
		protected bool GetIsExistProject(string projectName)
		{
			return WorkspacePage.GetIsProjectInList(projectName);
		}

		/// <summary>
		/// Удаление проект на вкладке проектов по имени
		/// </summary>
		/// <param name="ProjectNameToDelete">имя проекта, который надо удалить</param>
		protected void DeleteProjectFromList(string ProjectNameToDelete)
		{
			// Выбрать этот проект
			SelectProjectInList(ProjectNameToDelete);
			// Нажать Удалить
			WorkspacePage.ClickDeleteProjectBtn();
			// Подтвердить
			Assert.IsTrue(WorkspacePage.ClickConfirmDelete(), "Ошибка: не появилась форма подтверждения удаления проекта");
			// Дождаться, пока пропадет диалог подтверждения удаления
			Assert.IsTrue(WorkspacePage.WaitUntilDeleteConfirmFormDisappear(), "Ошибка: не появилась форма подтверждения удаления проекта");
			Thread.Sleep(500);
		}

		/// <summary>
		/// Работа с диалогом браузера: загрузка документа
		/// </summary>
		/// <param name="DocumentName">полный путь к документу</param>
		protected void FillAddDocumentForm(string DocumentName)
		{
			Thread.Sleep(3000);
			// Заполнить форму для отправки файла
			string txt = Regex.Replace(DocumentName, "[+^%~()]", "{$0}");

			SendKeys.SendWait(txt);
			Thread.Sleep(1000);
			SendKeys.SendWait(@"{Tab}");
			Thread.Sleep(1000);
			SendKeys.SendWait(@"{Tab}");
			Thread.Sleep(1000);
			SendKeys.SendWait(@"{Enter}");

			// заменить в методах, где загружаются объекты, на ожидание появления загруженного объекта, потом убрать слип здесь
			Thread.Sleep(3000);
		}

		/// <summary>
		/// Закрываем диалог
		/// </summary>
		protected void TryCloseExternalDialog()
		{
			SendKeys.SendWait(@"{Enter}");
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Работа с диалогом браузера: сохранение документа
		/// </summary>
		/// <param name="documentName">имя документа</param>
		protected void ExternalDialogSelectSaveDocument(string documentName)
		{
			string txt = Regex.Replace(documentName, "[+^%~()]", "{$0}");

			Thread.Sleep(2000);
			// В открывшемся диалоге выбираем "Сохранить"
			SendKeys.SendWait(@"{DOWN}");
			Thread.Sleep(1000);
			SendKeys.SendWait(@"{Enter}");
			Thread.Sleep(2000);
			// Ввести адрес
			SendKeys.SendWait(txt);
			Thread.Sleep(1000);
			SendKeys.SendWait(@"{Enter}");
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Работа с диалогом браузера: сохранение документа
		/// </summary>
		/// <param name="subFolderName">название папки для выгрузки</param>
		/// <param name="useFileName">использовать название исходного файла</param>
		/// <param name="filePath">название исходного файла</param>
		/// <param name="originalFileExtension">использовать исходное расширение</param>
		/// <param name="fileExtension">расширение</param>
		protected void ExternalDialogSaveDocument(
			string subFolderName,
			bool useFileName = false, 
			string filePath = "",
			bool originalFileExtension = true, 
			string fileExtension = "", 
			string time = "")
		{
			// Заполнить форму для сохранения файла
			string resultPath = Path.Combine(PathTestResults, subFolderName, time);
			Directory.CreateDirectory(resultPath);

			string newFileName = "";
			if (useFileName)
			{
				newFileName = Path.GetFileNameWithoutExtension(filePath) + "_" + DateTime.Now.Ticks.ToString();
			}
			else
			{
				newFileName = DateTime.Now.Ticks.ToString();
			}

			string currentFileExtension = originalFileExtension ? Path.GetExtension(filePath) : fileExtension;

			resultPath = Path.Combine(resultPath, newFileName + currentFileExtension);

			string txt = Regex.Replace(resultPath, "[+^%~()]", "{$0}");

			Thread.Sleep(3000);
			SendKeys.SendWait(txt);
			Thread.Sleep(2000);

			SendKeys.SendWait(@"{Enter}");
			Thread.Sleep(5000);
			Assert.IsTrue(File.Exists(resultPath), "Ошибка: файл не экспортировался\n" + resultPath);
		}

		/// <summary>
		/// Добавить перевод и подтвердить
		/// </summary>
		public void AddTranslationAndConfirm(int segmentRowNumber = 1, string text = "Translation")
		{
			// Написать что-то в target
			EditorPage.AddTextTarget(segmentRowNumber, text);
			// Нажать на кнопку подтвердить
			EditorPage.ClickConfirmBtn();
			// Убедиться что сегмент подтвержден
			Assert.IsTrue(WaitSegmentConfirm(segmentRowNumber),
				"Ошибка: подтверждение (Confirm) не прошло");
		}

		/// <summary>
		/// Дождаться, пока сегмент подтвердится
		/// </summary>
		/// <param name="segmentRowNumber">номер сегмента</param>
		/// <returns>сегмент подтвердился</returns>
		protected bool WaitSegmentConfirm(int segmentRowNumber)
		{
			return EditorPage.WaitSegmentConfirm(segmentRowNumber);
		}

		/// <summary>
		/// Нажать Back в редакторе, проверить возврат в Workspace
		/// </summary>
		/// <param name="backToProject">откуда заходили в редактов: true: страница проекта, false - список проектов</param>
		public void EditorClickHomeBtn()
		{
			// Нажать кнопку назад
			EditorPage.ClickHomeBtn();
			// Проверить, что перешли в Workspace
			Assert.IsTrue(WorkspacePage.WaitPageLoad(), "Ошибка: не зашли в Workspace");
		}

		/// <summary>
		/// Нажать Source, кликнуть Toggle (кнопку)
		/// </summary>
		public void SourceTargetSwitchButton(int segmentNumber)
		{
			//Выбрать target нужного сегмента
			EditorPage.ClickTargetCell(segmentNumber);
			// Нажать Toggle
			EditorPage.ClickToggleBtn();
		}

		/// <summary>
		/// Кликнуть Toggle (хоткей)
		/// </summary>
		public void SourceTargetSwitchHotkey(int segmentNumber)
		{
			// Нажать хоткей Tab
			EditorPage.TabByHotkey(segmentNumber);
		}

		/// <summary>
		/// Проверить Copy (кнопка)
		/// </summary>
		public void ToTargetButton(int rowNumber = 1)
		{
			// Выбрать source нужного сегмента
			EditorPage.ClickSourceCell(rowNumber);

			// Текст source'a нужного сегмента
			string sourcetxt = EditorPage.GetSourceText(rowNumber);

			// Нажать кнопку копирования
			EditorPage.ClickCopyBtn();

			// Проверить, такой ли текст в target'те
			string targetxt = EditorPage.GetTargetText(rowNumber);
			Assert.AreEqual(sourcetxt, targetxt, "Ошибка: после кнопки Copy текст в Source и Target не совпадает");
		}

		/// <summary>
		/// Проверить Copy (хоткей)
		/// </summary>
		public void ToTargetHotkey(int rowNumber = 1)
		{
			//Выбрать source сегмента
			EditorPage.ClickSourceCell(rowNumber);

			// Текст source'a сегмента
			string sourcetxt = EditorPage.GetSourceText(rowNumber);
			// Нажать хоткей копирования
			EditorPage.CopySourceByHotkey(rowNumber);


			// Проверить, такой ли текст в target'те
			string targetxt = EditorPage.GetTargetText(rowNumber);
			Assert.AreEqual(sourcetxt, targetxt, "Ошибка: после хоткея Copy текст в Source и Target не совпадает");
		}

		/// <summary>
		/// Перейти на страницу Domain
		/// </summary>
		protected void SwitchDomainTab()
		{
			MainHelperClass.ClickOpenDomainPage();
			DomainPage.WaitPageLoad();
		}

		/// <summary>
		/// Перейти на страницу TM
		/// </summary>
		protected void SwitchTMTab()
		{
			// Нажать кнопку перехода на страницу Базы Translation memory
			MainHelperClass.ClickOpenTMPage();
			TMPage.WaitPageLoad();
		}

		/// <summary>
		/// Перейти на страницу глоссариев
		/// </summary>
		protected void SwitchGlossaryTab()
		{
			MainHelperClass.ClickOpenGlossaryPage();
			GlossaryListPage.WaitPageLoad();
		}

		/// <summary>
		/// Перейти на страницу поиска
		/// </summary>
		protected void SwitchSearchTab()
		{
			MainHelperClass.ClickOpenSearchPage();
			SearchPage.WaitPageLoad();
		}

		/// <summary>
		/// Перейти на страницу Workspace
		/// </summary>
		protected void SwitchWorkspaceTab()
		{
			MainHelperClass.ClickOpenWorkSpacePage();
			WorkspacePage.WaitPageLoad();
		}

		/// <summary>
		/// Инициировать поиск
		/// </summary>
		/// <param name="searchText"></param>
		protected void InitSearch(string searchText)
		{
			// Ввести слово для поиска
			SearchPage.AddTextSearch(searchText);
			// Нажать Перевести
			SearchPage.ClickTranslateBtn();
			SearchPage.WaitUntilShowResults();
		}

		/// <summary>
		/// Есть ли такой domain
		/// </summary>
		/// <param name="domainName">название</param>
		/// <returns>есть</returns>
		protected bool GetIsDomainExist(string domainName)
		{
			// TODO мб заменить вызов
			return DomainPage.GetIsDomainExist(domainName);
		}

		/// <summary>
		/// Создать Domain
		/// </summary>
		/// <param name="domainName">имя</param>
		/// <param name="shouldCreateOk">должен сохраниться</param>
		protected void CreateDomain(string domainName, bool shouldCreateOk = true)
		{
			// Нажать "Добавить проект"
			DomainPage.ClickCreateDomainBtn();
			// Ввести имя
			DomainPage.EnterNameCreateDomain(domainName);

			// Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
			Driver.Manage().Window.Maximize();

			// Сохранить новый проект
			DomainPage.ClickSaveDomain();
			if (shouldCreateOk)
			{
				Assert.IsTrue(DomainPage.WaitUntilSave(), "Ошибка: domain не сохранился!");
			}
			else
			{
				Thread.Sleep(1000);
			}
		}

		/// <summary>
		/// Кликнуть галочку около проекта в списке проектов
		/// </summary>
		/// <param name="projectName">название проекта</param>
		protected void SelectProjectInList(string projectName)
		{
			WorkspacePage.SelectProject(projectName);
		}

		/// <summary>
		/// Открыть проект (со страницы Workflow)
		/// </summary>
		/// <param name="projectName">название проекта</param>
		protected void OpenProjectPage(string projectName)
		{
			WorkspacePage.OpenProjectPage(projectName);
			ProjectPage.WaitPageLoad();
		}

		/// <summary>
		/// Открыть свертку проекта
		/// </summary>
		/// <param name="projectName">название проекта</param>
		protected void OpenProjectInfo(string projectName)
		{
			WorkspacePage.OpenProjectInfo(projectName);
		}

		/// <summary>
		/// Открыть вкладку workflow настроек из окна проекта
		/// </summary>
		protected void OpenWorkflowSettings()
		{
			//Открываем настройки проекта
			ProjectPage.ClickProjectSettings();
			Thread.Sleep(1000);

			//Переходим на вкладку Workflow
			ProjectPage.ClickProjectSettingsWorkflow();
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Открыть диалог создания глоссария
		/// </summary>
		protected void OpenCreateGlossary()
		{
			GlossaryListPage.ClickCreateGlossary();
			GlossaryEditForm.WaitPageLoad();
		}

		/// <summary>
		/// Создать новый глоссарий
		/// </summary>
		/// <param name="glossaryName">название</param>
		/// <param name="bNeedWaitSuccessSave">ожидается успешное сохранение</param>
		/// <param name="langList">список языков</param>
		protected void CreateGlossaryByName(
			string glossaryName, 
			bool bNeedWaitSuccessSave = true, 
			List<CommonHelper.LANGUAGE> langList = null)
		{
			// Открыть форму создания глоссария
			OpenCreateGlossary();

			// Ввести имя
			GlossaryEditForm.EnterGlossaryName(glossaryName);

			// Добавить комментарий
			GlossaryEditForm.EnterComment("Test Glossary Generated by Selenium");

			// Удалить все языки
			AllLanguagesToDefaultCreateGlossary();

			if (langList != null)
			{
				// Добавить языки
				foreach (CommonHelper.LANGUAGE langID in langList)
				{
					AddLanguageCreateGlossary(langID);
				}
			}

			// Нажать сохранить
			GlossaryEditForm.ClickSaveGlossary();

			if (bNeedWaitSuccessSave)
			{
				// Ожидание успешного сохранения
				Assert.IsTrue(
					GlossaryPage.WaitPageLoad(), 
					"Ошибка: глоссарий не сохранился, не перешли на страницу глоссария");
			}
			else
			{
				Thread.Sleep(1000);
			}
		}
		
		/// <summary>
		/// Получить количество терминовв глоссарии
		/// </summary>
		/// <returns></returns>
		protected int GetCountOfItems()
		{
			return GlossaryPage.GetConceptCount();
		}

		/// <summary>
		/// Зайти в глоссарий
		/// </summary>
		/// <param name="glossaryName"></param>
		protected void SwitchCurrentGlossary(string glossaryName)
		{
			// Перейти на страницу глоссария
			GlossaryListPage.ClickGlossaryRow(glossaryName);
			Assert.IsTrue(GlossaryPage.WaitPageLoad(),
				"Ошибка: не зашли в глоссарий");
		}

		/// <summary>
		/// Открыть свойства глоссария
		/// </summary>
		protected void OpenGlossaryProperties()
		{
			// Нажать Редактирование
			GlossaryPage.OpenEditGlossaryList();
			// Нажать на Properties
			GlossaryPage.ClickOpenProperties();
		}

		/// <summary>
		/// Удаление глоссария
		/// </summary>
		protected void DeleteGlossary()
		{
			// Открыть редактирование свойств глоссария
			OpenGlossaryProperties();
			// Нажать Удалить глоссарий 
			GlossaryEditForm.ClickDeleteGlossary();

			// Нажать Да (удалить)
			GlossaryEditForm.ClickConfirmDeleteGlossary();
			GlossaryListPage.WaitPageLoad();
		}

		/// <summary>
		/// Добавить язык при создании глоссария
		/// </summary>
		/// <param name="lang">код языка</param>
		protected void AddLanguageCreateGlossary(CommonHelper.LANGUAGE lang)
		{
			// Кликнуть по Плюсу
			GlossaryEditForm.ClickAddLanguage();
			// Открыть выпадающий список у добавленного языка
			GlossaryEditForm.ClickLastLangOpenCloseList();
			// Выбрать язык
			Assert.IsTrue(GlossaryEditForm.GetIsExistLanguageInList(lang),
				"Ошибка: указанного языка нет в списке");

			GlossaryEditForm.SelectLanguage(lang);
		}

		/// <summary>
		/// Установить дефолтные языки (английский, русский)
		/// </summary>
		protected void AllLanguagesToDefaultCreateGlossary()
		{
			// Удалить все языки
			for (int i = 1; i < GlossaryEditForm.GetGlossaryLanguageCount(); i++)
			{
				GlossaryEditForm.ClickDeleteLanguage();
			}
			// Выставить первый английский
			GlossaryEditForm.ClickLastLangOpenCloseList();
			// Выбрать язык
			Assert.IsTrue(GlossaryEditForm.GetIsExistLanguageInList(CommonHelper.LANGUAGE.English),
				"Ошибка: указанного языка нет в списке");
			GlossaryEditForm.SelectLanguage(CommonHelper.LANGUAGE.English);
			// Кликнуть по Плюсу
			GlossaryEditForm.ClickAddLanguage();
			// Выставить второй русский
			GlossaryEditForm.ClickLastLangOpenCloseList();
			// Выбрать язык
			Assert.IsTrue(GlossaryEditForm.GetIsExistLanguageInList(CommonHelper.LANGUAGE.Russian),
				"Ошибка: указанного языка нет в списке");
			GlossaryEditForm.SelectLanguage(CommonHelper.LANGUAGE.Russian);
		}

		/// <summary>
		/// Создать проект с документом, открыть документ
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		/// <param name="withTM">с ТМ</param>
		/// <param name="withMT">с МТ</param>
		/// <param name="uploadDocument">путь к документу</param>
		protected void CreateReadyProject(
			string projectName, 
			bool withTM = true, 
			bool withMT = false, 
			string uploadDocument = "",
			Workspace_CreateProjectDialogHelper.MT_TYPE mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT, 
			bool chooseGlossary = false, 
			string glossaryName = "")
		{
			// Создание проекта
			CreateProject(projectName, "", withTM, EditorTMXFile, Workspace_CreateProjectDialogHelper.SetGlossary.None, glossaryName, withMT, mtType);

			//открытие настроек проекта
			uploadDocument = uploadDocument.Length == 0 ? EditorTXTFile : uploadDocument;
			ImportDocumentProjectSettings(uploadDocument, projectName);

			// 3. Назначение задачи на пользователя
			AssignTask();

			// Добавляем созданный глоссарий
			if (glossaryName != "")
				ProjectPage.SetGlossaryByName(glossaryName);

			// 4. Открытие документа по имени созданного проекта
			OpenDocument();
		}

		/// <summary>
		/// Открытие диалога выбора исполнителя
		/// </summary>
		/// <param name="projectName">Имя проекта</param>
		protected void OpenAssignDialog(string projectName)
		{
			// Открываем инфо проекта
			WorkspacePage.OpenProjectInfo(projectName);

			// Открываем инфо документа 
			WorkspacePage.OpenDocumentInfo(1);

			// Открываем окно с правами пользователя через кнопку прав
			WorkspacePage.ClickDocumentAssignBtn();

			// Ожидание открытия диалога выбора исполнителя
			Assert.IsTrue(ResponsiblesDialog.WaitUntilResponsiblesDialogDisplay(),
				"Ошибка: Диалог выбора исполнителя не открылся.");
		}

		/// <summary>
		/// Выбирает из выпадающего списка пользователя или группу по имени
		/// </summary>
		/// <param name="rowNumber">Номер строки задачи</param>
		/// <param name="name">Имя пользователя или группы</param>
		/// <param name="isGroup">Выбор группы</param>
		protected void SetResponsible(int rowNumber, string name, bool isGroup)
		{
			string fullName = "";

			// Открыть выпадающий список
			ResponsiblesDialog.ClickResponsiblesDropboxByRowNumber(rowNumber);

			if (isGroup)
				fullName = "Group: " + name;
			else
				fullName = name;

			ResponsiblesDialog.WaitUntilUsersListDisplay(fullName);

			// Выбрать для заданной задачи имя исполнителя
			ResponsiblesDialog.SetVisibleResponsible(rowNumber, fullName);

			// Кликнуть подтверждение для заданной задачи
			ResponsiblesDialog.ClickAssignBtn(rowNumber);
		}

		/// <summary>
		/// Открывает словарь в редакторе
		/// </summary>
		protected void OpenEditorDictionary()
		{
			// Кликнуть по кнопке
			EditorPage.ClickDictionaryBtn();

			// Проверка, что открылась форма
			Assert.IsTrue(EditorPage.WaitDictionaryFormDisplay(),
				"Ошибка: Форма со словарем не открылась.");
		}

		/// <summary>
		/// Создать уникальное имя
		/// </summary>
		/// <returns>имя</returns>
		protected string GetUniqueGlossaryName()
		{
			// Получить уникальное имя глоссария (т.к. добавляется точная дата и время, то не надо проверять, есть ли такой глоссарий в списке)
			return GlossaryName + DateTime.Now.ToString();
		}

		/// <summary>
		/// Вернуть, есть ли глоссарий с таким именем
		/// </summary>
		/// <param name="glossaryName">навзание</param>
		/// <returns>есть</returns>
		protected bool GetIsExistGlossary(string glossaryName)
		{
			// Получить: существует ли глоссарий с таким именем
			return GlossaryListPage.GetIsExistGlossary(glossaryName);
		}

		/// <summary>
		/// Дождаться автосохранения сегментов
		/// </summary>
		protected void AutoSave()
		{
			// Дожидаемся сохранения сегментов
			Assert.IsTrue(EditorPage.WaitUntilAllSegmentsSave(),
				"Ошибка: Не проходит автосохранение.");
		}

		/// <summary>
		/// Завершение работы с Driver
		/// </summary>
		protected void ExitDriver()
		{
			if (_driver != null)
			{
				// Закрыть драйвер
				_driver.Quit();
				// Очистить, чтобы при следующем тесте пересоздавалось
				_driver = null;
			}
		}

		/// <summary>
		/// Закрываем модальный диалог
		/// </summary>
		protected void AcceptModalDialog()
		{
			try
			{
				if (_driver.SwitchTo().Alert().Text.
					Contains("Эта страница просит вас подтвердить, что вы хотите уйти — при этом введённые вами данные могут не сохраниться.") ||
					_driver.SwitchTo().Alert().Text.
					Contains("This page is asking you to confirm that you want to leave - data you have entered may not be saved."))
					_driver.SwitchTo().Alert().Accept();

				Thread.Sleep(500);

				if (_driver.SwitchTo().Alert().Text.Contains("Failed to send the request to the server. An error occurred while contacting the server."))
					_driver.SwitchTo().Alert().Accept();

				Thread.Sleep(500);
			}
			catch (NoAlertPresentException)
			{
				TryCloseExternalDialog();
			}
		}

		/// <summary>
		/// Задает русский язык в Target при создании проекта
		/// </summary>
		protected void SetRusLanguageTarget()
		{
			// Проверить, что в Target русский язык
			List<string> langList = WorkspaceCreateProjectDialog.GetTargetLanguageList();
			if (langList.Count != 1 || langList[0] != "Russian")
			{
				// Открыть список Target
				WorkspaceCreateProjectDialog.ClickTargetList();
				// Кликаем по всем выбранным языкам, чтобы снять галки
				WorkspaceCreateProjectDialog.ClickAllSelectedTargetItems();
				// Выбираем русский язык
				WorkspaceCreateProjectDialog.ClickTargetItem(CommonHelper.LANGUAGE.Russian);
				// Закрыть список Target
				WorkspaceCreateProjectDialog.ClickTargetList();
			}
		}

		/// <summary>
		/// Пропуск подтверждения отсутствия TM при создании проекта
		/// </summary>
		protected void SkipNotSelectedTM()
		{
			// В случае, если диалог открыт
			if (WorkspaceCreateProjectDialog.WaitUntilConfirmTMDialogDisplay())
			{
				// Жмем Skip
				WorkspaceCreateProjectDialog.ClickSkipBtn();
				// Ждем пока диалог не пропадет
				Assert.IsTrue(WorkspaceCreateProjectDialog.WaitUntilConfirmTMDialogDisappear(),
					"Ошибка: после нажатия кнопки Skip диалог подтверждения не выбранной ТМ не закрылся.");
			}
		}
		
		/// <summary>
		/// Метод подстановки из САТ, возвращает номер строки, из которой произведена подстановка
		/// </summary>
		/// <param name="segmentNumber">номер сегмента таргет для подстановки  в него</param>
		/// <param name="catType">тип подстановки из CAT</param>
		/// <returns>номер строки CAT из которой произвели подстановку</returns>

		protected int PasteFromCatReturnCatLineNumber(int segmentNumber, EditorPageHelper.CAT_TYPE catType)
		{

			//Выбираем сегмент
			EditorPage.ClickTargetCell(segmentNumber);

			int catLineNumber = EditorPage.GetCATTranslationRowNumber(catType);

			//Нажать хоткей для подстановки из TM перевода сегмента
			EditorPage.PutCatMatchByHotkey(segmentNumber, catLineNumber);

			// Дождаться автосохранения
			Assert.IsTrue(EditorPage.WaitUntilAllSegmentsSave(),
				"Ошибка: Не проходит автосохранение.");

			return catLineNumber;
		}

		/// <summary>
		/// Создание нового глоссария из указанных слов словаря
		/// </summary>
		/// <param name="dict">Словарь</param>
		protected void SetGlossaryByDictinary(Dictionary<string, string> dict)
		{
			foreach (KeyValuePair<string, string> pair in dict)
			{
				// Нажать New item
				GlossaryPage.ClickNewItemBtn();
				// Дождаться появления строки для ввода
				GlossaryPage.WaitConceptTableAppear();
				// Заполнить термин
				GlossaryPage.FillTerm(1, pair.Key);
				GlossaryPage.FillTerm(2, pair.Value);

				// Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
				Driver.Manage().Window.Maximize();
				// Нажать Сохранить
				GlossaryPage.ClickSaveTermin();
				GlossaryPage.WaitConceptGeneralSave();
				Thread.Sleep(1000);
			}
		}

		/// <summary>
		/// Переход на страницу регистрации
		/// </summary>
		/// <param name="client">компания или пользователь</param>
		public void GoToRegistrationPage(RegistrationType client)
		{
			try
			{
				if (client == RegistrationType.Company)
				{
					_driver.Navigate().GoToUrl(_url + "/corp-reg");
				}
				else
				{
					_driver.Navigate().GoToUrl(_url + "/freelance-reg");
				}
			}
			catch
			{
				_driver.Navigate().Refresh();

				// Закрываем Modal Dialog
				AcceptModalDialog();

				// Пробуем перейти на страницу еще раз
				GoToRegistrationPage(client);
			}
		}

		protected struct UserInfo
		{
			public string login;
			public string password;

			public bool activated;
			public UserInfo(string l, string p, bool s)
			{
				login = l;
				password = p;
				activated = s;
			}
		}
	}
}
