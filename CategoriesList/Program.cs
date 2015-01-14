using System;
using System.Linq;
using System.Reflection;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;
using System.Collections.Generic;
using System.IO;

namespace AbbyyLS.CAT.Function.Selenium.CategoriesList
{
	class Program
	{
		static void Main(string[] args)
		{
			Dictionary<string, string> issues = new Dictionary<string, string>();
			List<string> testName = new List<string>();

			//Загрузить сборку
			var assembly = Assembly.LoadFile(args[0]);

			//Получить список testfixture классов из сборки
			var testTypes = from t in assembly.GetTypes()
							let attributes = t.GetCustomAttributes(typeof(NUnit.Framework.TestFixtureAttribute), true)
							where attributes != null && attributes.Length > 0
							orderby t.Name
							select t;
			foreach (var type in testTypes)
			{
				var testMethods = from m in type.GetMethods()
								  let attributes = m.GetCustomAttributes(typeof(NUnit.Framework.CategoryAttribute), true)
								  where attributes != null && attributes.Length > 0
								  from ctgAttr in attributes.OfType<NUnit.Framework.CategoryAttribute>()
								  orderby m.Name
								  select new { MethodName = m.Name, CategoryName = ctgAttr.Name };
				foreach (var method in testMethods)
				{
					issues.Add(method.MethodName, method.CategoryName.Replace("_", "-"));
				}
			}

			foreach (KeyValuePair<string, string> keyValue in issues)
			{
				if (GetIssueState(keyValue.Value) != "Fixed")
					testName.Add(keyValue.Value);
			}

			var uniq = testName.Distinct();

			foreach (var t in uniq)
			{
				Console.WriteLine(t);
			}

			Console.ReadLine();
		}
		
		/// <summary>
		/// Получить статус тикета
		/// </summary>
		/// <param name="id"> issue id </param>
		public static string GetIssueState(string id)
		{
			var cfgUserInfo = TestSettingDefinition.Instance.Get<UserInfoConfig>("UserInfo");

			Connection Connection = new Connection(cfgUserInfo.Site, 80, false, "youtrack");
			Connection.Authenticate(cfgUserInfo.Login, cfgUserInfo.Password);

			IssueManagement IssueManager = new IssueManagement(Connection);
			dynamic issue = IssueManager.GetIssue(id);
			string[] state = issue.State;
			return state[0];
		}
	}
}
