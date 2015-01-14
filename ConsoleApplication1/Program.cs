using System;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;
using YouTrackSharp.Projects;

namespace ConsoleApplication1
{


	internal class Program
	{
		private static void Main(string[] args)
		{
			String Username = "i.khaptanova@abbyy-ls.com";
			String Password = "434161tat";
			String Site = "blackhole.perevedem.ru";

			Connection Connection;
			IssueManagement IssueManager;

			Connection = new Connection(Site, 80, false, "youtrack");
			Connection.Authenticate(Username, Password);

			IssueManager = new IssueManagement(Connection);
			dynamic issue = IssueManager.GetIssue("PRX-6513");
			string[] state = issue.State;
			Console.WriteLine("issueId = " +state[0]);
			Console.ReadLine();

		}
	}
}
