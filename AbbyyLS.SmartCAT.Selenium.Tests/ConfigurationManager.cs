using System.Collections.Generic;

using NConfiguration;

using AbbyyLS.SmartCAT.Selenium.Tests.Configs;

namespace AbbyyLS.SmartCAT.Selenium.Tests
{
	public class ConfigurationManager
	{
		public static void InitializeRelatedToUserFields()
		{
			var config = TestSettingDefinition.Instance.Get<UserInfoConfig>();

			Login = config.Login;
			Password = config.Password;
			UserName = config.Name ?? string.Empty;
			UserSurname = config.Surname ?? string.Empty;
			NickName = UserName;

			if (!string.IsNullOrEmpty(UserSurname))
			{
				NickName += " " + UserSurname;
			}

			Login2 = config.Login2;
			Password2 = config.Password2;
			NickName2 = config.NickName2;
			UserName2 = NickName2.Substring(0, NickName2.IndexOf(' '));
			UserSurname2 = NickName2.Substring(NickName2.IndexOf(' ') + 1);

			RightsTestLogin = config.TestRightsLogin;
			RightsTestPassword = config.TestRightsPassword;
			RightsTestNickName = config.TestRightsNickName;
			RightsTestUserName = RightsTestNickName.Substring(0, RightsTestNickName.IndexOf(' '));
			RightsTestSurname = RightsTestNickName.Substring(RightsTestNickName.IndexOf(' ') + 1);
		}

		public static void InitializeUsersAndCompanyList()
		{
			var cfgTestUser = TestSettingDefinition.Instance.TryGet<TestUsersConfig>();

			if (cfgTestUser != null)
			{
				TestUserList = cfgTestUser.Users;
				TestCompanyList = cfgTestUser.Companies;
				CourseraUserList = cfgTestUser.CourseraUsers;
				AolUserList = cfgTestUser.AolUsers;
				SocialNetworksUserList = cfgTestUser.SocialNetworksUsers;
			}
			else
			{
				TestUserList = new List<TestUser>();
				TestCompanyList = new List<TestUser>();
				CourseraUserList = new List<TestUser>();
				AolUserList = new List<TestUser>();
				SocialNetworksUserList = new List<TestUser>();
			}
		}

		public static void InitializeRelatedToServerFields()
		{
			var config = TestSettingDefinition.Instance.Get<CatServerConfig>();

			var prefix = config.IsHttpsEnabled ? "https://" : "http://";
			Standalone = config.Standalone;

			if (Standalone)
			{
				// доменная авторизация в ОР
				var domainName = Login.Contains("@") ? Login.Substring(0, Login.IndexOf("@")) : Login;
				Url = string.Format("{0}{1}:{2}@{3}", prefix, domainName, Password, config.Url);
			}
			else
			{
				Url = prefix + config.Url;
				AdminUrl = "http://" + config.Url + ":81";
			}

			WorkspaceUrl = string.IsNullOrWhiteSpace(config.Workspace) ? Url + RelativeUrlProvider.Workspace : config.Workspace;
		}

		public static string Login { get; private set; }
		public static string Password { get; private set; }
		public static string NickName { get; private set; }
		public static string UserName { get; private set; }
		public static string UserSurname { get; private set; }

		public static string Login2 { get; private set; }
		public static string Password2 { get; private set; }
		public static string NickName2 { get; private set; }
		public static string UserName2 { get; private set; }
		public static string UserSurname2 { get; private set; }

		public static string RightsTestLogin { get; private set; }
		public static string RightsTestPassword { get; private set; }
		public static string RightsTestNickName { get; private set; }
		public static string RightsTestUserName { get; private set; }
		public static string RightsTestSurname { get; private set; }

		public static List<TestUser> TestUserList { get; private set; }
		public static List<TestUser> TestCompanyList { get; private set; }
		public static List<TestUser> CourseraUserList { get; private set; }
		public static List<TestUser> AolUserList { get; private set; }
		public static List<TestUser> SocialNetworksUserList { get; private set; }

		public static bool Standalone { get; private set; }
		public static string Url { get; private set; }
		public static string WorkspaceUrl { get; private set; }
		public static string AdminUrl { get; private set; }
	}
}
