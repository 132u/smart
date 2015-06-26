using System.Collections.Generic;

namespace AbbyyLS.SmartCAT.Selenium.Tests.DataStructures
{
	public struct DataDictionaries
	{
		public static Dictionary<string, RevisionType> RevisionDictionary = new Dictionary<string, RevisionType>
		{
			{ "Manual input", RevisionType.ManualInput },
			{ "Confirmation", RevisionType.Confirmed },
			{ "MT insertion", RevisionType.InsertMT },
			{ "TM insertion", RevisionType.InsertTM },
			{ "TB insertion", RevisionType.InsertTb },
			{ "Restored", RevisionType.Restored },
			{ "Pretranslation", RevisionType.Pretranslation }
		};
	}
}
