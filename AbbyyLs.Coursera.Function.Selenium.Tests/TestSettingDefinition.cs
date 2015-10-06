
using NConfiguration;
using NConfiguration.Combination;
using NConfiguration.GenericView;
using NConfiguration.Joining;
using NConfiguration.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbbyyLS.Coursera.Function.Selenium.Tests
{
	public static class TestSettingDefinition
	{
		private static Lazy<ICombinableAppSettings> _lazySettings;

		static TestSettingDefinition()
		{
			_lazySettings = new Lazy<ICombinableAppSettings>(Load, true);
		}

		private static void SettingsChanged()
		{
			_lazySettings = new Lazy<ICombinableAppSettings>(Load, true);
		}

		public static ICombinableAppSettings Load()
		{
			string path;
			return SettingDefinition.LoadSettings((s, e) => SettingsChanged(), out path);
		}

		public static ICombinableAppSettings Instance
		{
			get
			{
				return _lazySettings.Value;
			}
		}
	}
}
