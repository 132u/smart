using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using AbbyyLs.CAT.Editor.Selenium.Tests;


namespace AbbyyLS.CAT.Selenium.StartPoint
{
    class Program
    {
        public static string FileEnRu = @"\\alyaska\a.kurenkova\TestFiles\English.docx";
        public static string FileRuEn = @"\\alyaska\a.kurenkova\TestFiles\Russian.docx";

        static void Main(string[] args)
        {

            WizardTest t = new WizardTest();
            t.SetupTest();
            t.CreateResultFile();
            t.AutorizationTest();

            //t.CreateDocumentSetupAllTest();
            //t.SettingsTest();

            //t.CreateDocumentDublicateNameTest();
            //t.CreateDocumentBigNameTest();
            t.CreateDocumentLimitFileNameTest();
            t.CreateDocumentWrongSymbolsTest();
            t.CreateDocumentEqualLanguagesTest();
            t.CreateDocumentEmptyNameTest();
            t.CreateDocumentEmptyPathTest();
            t.CreateDocumentSpaceNameTest();
            t.CreateDocumentCancelTest();
            //автоматическое создание ТМ - проверить, что ТМ выбрана для записи в настройках
            t.ChangeDocumentNameDuringCreationTest();
            t.AddTMDublicateNameTest();

            t.AutoCreateTMTest();
            t.SettingsTest();

            t.AddTMEmptyNameTest();
            t.AddTMNameWithSpaceTest();
            t.AddTMNoTMXFileTest();

            t.CreateDocumentWithDeletedDocNameTest();
            t.DeleteDocumentTest();

            // t.UploadFileForEditorTest(FileEnRu, "Russian", "English");
            // t.UploadFileForEditorTest(FileRuEn, "English", "Russian");
            t.CloseTest();




        }
    }
}
