﻿using System;

namespace AbbyyLS.SmartCAT.Selenium.Tests
{
	internal static class RelativeUrlProvider
	{
		public static string SignIn
		{
			get
			{
				return "/sign-in";
			}
		}

		public static string Workspace
		{
			get
			{
				return "/workspace";
			}
		}

		public static string Registratioin
		{
			get
			{
				return "/registration?email=";
			}
		}
	}
}