using System;
using NConfiguration;

namespace AbbyyLS.SmartCAT.Selenium.Tests
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
