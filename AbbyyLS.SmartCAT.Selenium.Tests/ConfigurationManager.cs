using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using NConfiguration;

using AbbyyLS.SmartCAT.Selenium.Tests.Configs;

namespace AbbyyLS.SmartCAT.Selenium.Tests
{
	public class ConfigurationManager
	{
		public static void InitializeRelatedToUserFields()
		{
			var threadUserConfig = TestSettingDefinition.Instance.TryGet<ThreadUserConfig>();
			var serverConfig = TestSettingDefinition.Instance.TryGet<CatServerConfig>();
			var testUsersConfig = TestSettingDefinition.Instance.TryGet<TestUsersConfig>();
			var testUsersInfoConfig = TestSettingDefinition.Instance.TryGet<UserInfoConfig>();

			TestUserList = testUsersConfig.Users;
			TestCompanyList = testUsersConfig.Companies;
			CourseraReviewerUserList = testUsersConfig.CourseraUsers.Where(x=>x.IsCrowdsource == false).ToList();
			CourseraCrowdsourceUserList = testUsersConfig.CourseraUsers.Where(x => x.IsCrowdsource).ToList();
			AolUserList = testUsersConfig.AolUsers;
			SocialNetworksUserList = testUsersConfig.SocialNetworksUsers;

			var prefix = serverConfig.IsHttpsEnabled ? "https://" : "http://";

			UsersInfoList = testUsersInfoConfig.UsersInfo;

			foreach (TestUser user in UsersInfoList)
			{
				var domainName = string.Concat(user.Login.TakeWhile(c => c != '@'));
				user.StandaloneUrl = string.Format("{0}{1}:{2}@{3}", prefix, domainName, user.Password, serverConfig.Url);
			}

			ThreadUsersList = threadUserConfig.Users;

			foreach (TestUser user in ThreadUsersList)
			{
				var domainName = string.Concat(user.Login.TakeWhile(c => c != '@'));
				user.StandaloneUrl = string.Format("{0}{1}:{2}@{3}", prefix, domainName, user.Password, serverConfig.Url);
			}

			AdditionalUsersList = testUsersConfig.RightsTestUsers;

			foreach (TestUser user in AdditionalUsersList)
			{
				var domainName = string.Concat(user.Login.TakeWhile(c => c != '@'));
				user.StandaloneUrl = string.Format("{0}{1}:{2}@{3}", prefix, domainName, user.Password, serverConfig.Url);
			}
			
			foreach (TestUser user in CourseraReviewerUserList)
			{
				var domainName = string.Concat(user.Login.TakeWhile(c => c != '@'));
				user.StandaloneUrl = string.Format("{0}{1}:{2}@{3}", prefix, domainName, user.Password, serverConfig.Url);
			}
			
			foreach (TestUser user in CourseraCrowdsourceUserList)
			{
				var domainName = string.Concat(user.Login.TakeWhile(c => c != '@'));
				user.StandaloneUrl = string.Format("{0}{1}:{2}@{3}", prefix, domainName, user.Password, serverConfig.Url);
			}
			
			Users = new ConcurrentBag<TestUser>(ThreadUsersList);
			AdditionalUsers = new ConcurrentBag<TestUser>(AdditionalUsersList);
			CourseraReviewerUsers = new ConcurrentBag<TestUser>(CourseraReviewerUserList);
			CourseraCrowdsourceUsers = new ConcurrentBag<TestUser>(CourseraCrowdsourceUserList);
			UsersInfoList = new List<TestUser>(UsersInfoList);
		}

		public static void InitializeRelatedToServerFields()
		{
			var config = TestSettingDefinition.Instance.Get<CatServerConfig>();
			var courseraConfig = TestSettingDefinition.Instance.Get<CourseraServerConfig>();
			var prefix = config.IsHttpsEnabled ? "https://" : "http://";
			Standalone = config.Standalone;
			Url = prefix + config.Url;
			CourseraUrl = courseraConfig.Url;
			AdminUrl = "http://" + config.AdminUrl;
		}

		public static ConcurrentBag<TestUser> Users { get; set; }
		public static ConcurrentBag<TestUser> AdditionalUsers { get; set; }
		public static ConcurrentBag<TestUser> CourseraReviewerUsers { get; set; }
		public static ConcurrentBag<TestUser> CourseraCrowdsourceUsers { get; set; }
		public static List<TestUser> TestUserList { get; private set; }
		public static List<TestUser> TestCompanyList { get; private set; }
		public static List<TestUser> CourseraReviewerUserList { get; private set; }
		public static List<TestUser> CourseraCrowdsourceUserList { get; private set; }
		public static List<TestUser> AolUserList { get; private set; }
		public static List<TestUser> SocialNetworksUserList { get; private set; }
		public static List<TestUser> ThreadUsersList { get; private set; }
		public static List<TestUser> AdditionalUsersList { get; private set; }
		public static List<TestUser> UsersInfoList { get; private set; }
		public static bool Standalone { get; private set; }
		public static string Url { get; private set; }
		public static string CourseraUrl { get; private set; }
		public static string AdminUrl { get; private set; }
	}
}
