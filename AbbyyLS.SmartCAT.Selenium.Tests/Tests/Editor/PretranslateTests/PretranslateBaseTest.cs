using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor.PretranslateTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Editor]
	class PretranslateBaseTest<TWebDriverProvider> :
		EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void SetupTest()
		{
			_projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			_file = PathProvider.PretranslateEarthFile;
		}

		protected string _file;
		protected const string TM_FIRST_TARGET_SEGMENT = "Земля - третья планета от Солнца, и первая по плотности и пятая по размеру среди восьми планет Солнечной системы.";
		protected const string TM_SECOND_TARGET_SEGMENT = "Она является также крупнейшей среди планет земной группы.";
		protected const string TM_THIRD_TARGET_SEGMENT = "Иногда упоминается как мир, Голубая Планета, или Латинское имя - Терра.";
		protected const string TM_FOURTH_TARGET_SEGMENT = "Земля образовалась примерно 4.54 миллиарда лет назад, и жизнь появилась в течение первого миллиарда лет.";
		protected const string MT_FIRST_TARGET_SEGMENT = "Земля – третья планета с солнца и самая ёмкая и пятая по величине из восьми планет в солнечной Системе.";
		protected const string MT_SECOND_TARGET_SEGMENT = "Это также наибольшая из четырёх земных планет солнечной Системы.";
		protected const string MT_THIRD_TARGET_SEGMENT = "Оно иногда называется миром, Голубой Планетой или с помощью своего латинского названия, Земли.";
		protected const string MT_FOURTH_TARGET_SEGMENT = "Земля образовалась примерно 4.54 миллиарда лет назад, и жизнь появилась на её поверхности в течение одного миллиарда лет.";
	}
}