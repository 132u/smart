using NConfiguration;
using NConfiguration.Combination;
using NConfiguration.GenericView;
using NConfiguration.Joining;
using NConfiguration.Xml;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	public class SettingDefinition
	{
		private static Logger Log = LogManager.GetCurrentClassLogger();

		private static StringConverter _strConv;
		private static GenericDeserializer _deserializer;

		private static XmlFileSettingsLoader _xmlFileLoader;

		static SettingDefinition()
		{
			_strConv = new StringConverter();
			_deserializer = new GenericDeserializer();

			_xmlFileLoader = new XmlFileSettingsLoader(_deserializer, _strConv);

			Instance = Load();
		}

		public static ICombinableAppSettings Load()
		{
			var loader = new SettingsLoader(_xmlFileLoader);
			loader.Loaded += (s, e) =>
			{
				Log.Info("Loaded: {0} ({1})", e.Settings.GetType(), e.Settings.Identity);
			};

			loader.LoadSettings(new XmlSystemSettings("ExtConfigure", _strConv, _deserializer));

			return new CombinableAppSettings(loader.Settings);
		}

		public static ICombinableAppSettings LoadSettings(EventHandler changedHandler, out string configPath)
		{
			var xmlFileLoader = new XmlFileSettingsLoader(_deserializer, _strConv);

			xmlFileLoader.FindingSettings += (s, e) =>
			{
				var rpo = e.Source as IFilePathOwner;
				var baseDir = rpo != null ? rpo.Path : null;
				Log.Debug("Find settings '{0}' base directory: '{1}'", e.IncludeFile.Path, baseDir);
			};

			var loader = new SettingsLoader(xmlFileLoader);
			loader.Loaded += (s, e) =>
			{
				Log.Info("Loaded: {0} ({1})", e.Settings.GetType(), e.Settings.Identity);
			};

			var sysSetting = new XmlSystemSettings("ExtConfigure", _strConv, _deserializer);
			configPath = sysSetting.Path;
			loader.LoadSettings(sysSetting);

			var m = loader.Settings;
			m.Changed += changedHandler;

			var result = new CombinableAppSettings(loader.Settings);

			return result;
		}

		public static ICombinableAppSettings Instance { get; private set; }
	}
}
