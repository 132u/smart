using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using AbbyyLs.CAT.Editor.Selenium.Tests;
using AbbyyLs.CAT.Projects.Selenium.Tests;



namespace AbbyyLs.CAT.Selenium.StartPoint
{
    class Program
    {
        public static string FileEnRu = @"\\alyaska\a.kurenkova\TestFiles\English.docx";
        public static string FileRuEn = @"\\alyaska\a.kurenkova\TestFiles\Russian.docx";

        public static string DocxFile1 = @"\\cat-dev\Share\CAT\TestFiles\TextEng2.docx";
        public static string DocxFile2 = @"\\cat-dev\Share\CAT\TestFiles\TextEng1.docx";
        public static string TtxFile1 = @"\\cat-dev\Share\CAT\TestFiles\test.txt_l.ttx";
        public static string TtxFile2 = @"\\cat-dev\Share\CAT\TestFiles\test.txt_l1.ttx";
        public static string WrongFormatFile = @"\\cat-dev\Share\CAT\TestFiles\doc98.doc";

        static void Main(string[] args)
        {
            
            WizardTest t = new WizardTest();
            ProjectTest p = new ProjectTest();
            p.SetupTest();
            p.CreateResultFile();
            p.AutorizationTest();
            //p.CreateProjectTest();
            //p.DeleteProjectTest();
            //p.CreateProjectDeletedNameTest();
            //p.CreateProjectLimitNameTest();
            //p.CreateProjectBigNameTest();
            //p.CreateProjectEqualLanguagesTest();
            //p.CreateProjectForbiddenSymbolsTest();
            //p.CreateProjectForbiddenSymbolsTest2();
            //p.CreateProjectEmptyNameTest();
            //p.CreateProjectSpaceNameTest();
            //p.CancelFirstTest();
            //p.CancelYesTest();
            //p.CancelNoTest();
            p.ChangeProjectNameOnNew();
            //Thread.Sleep(6000);

            //Тестирование импорта файлов
            //Импорт одного файла docx
            //p.WriteFileConsoleResults("Import Docx File Test", 2);
            //bool resdocx = p.ImportSomeFilesTest(DocxFile1, 1);
            //if (resdocx)
            //{
            //    p.WriteFileConsoleResults("Test Pass", 1);
            //}
            //else
            //{
            //    p.WriteFileConsoleResults("Test Fail", 1);
            //}
            ////Импорт нескольких файлов docx
            //p.WriteFileConsoleResults("Import 2 Docx Files Test", 2);
            //for (int i = 0; i < 2; i++)
            //{
            //    bool resdocx1 = p.ImportSomeFilesTest(DocxFile1, 1);
            //    bool resdocx2 = p.ImportSomeFilesTest(DocxFile2, 1);
            //    if (resdocx1 && resdocx2)
            //    {
            //        p.WriteFileConsoleResults("Test Pass", 1);
            //    }
            //    else
            //    {
            //        p.WriteFileConsoleResults("Test Fail", 0);
            //    }
            //}
            ////Импорт одного файл ttx
            //p.WriteFileConsoleResults("Import Ttx File Test", 2);
            //bool resttx = p.ImportSomeFilesTest(TtxFile1, 1);
            //if (resttx)
            //{
            //    p.WriteFileConsoleResults("Test Pass", 1);
            //}
            //else
            //{
            //    p.WriteFileConsoleResults("Test Fail", 0);
            //}
            ////Импорт 2 файлов ttx
            //p.WriteFileConsoleResults("Import 2 Ttx Files Test", 2);
            //for (int i = 0; i < 2; i++)
            //{
            //    bool resttx1 = p.ImportSomeFilesTest(TtxFile1, 1);
            //    bool resttx2 = p.ImportSomeFilesTest(TtxFile2, 1);
            //    if (resttx1 && resttx2)
            //    {
            //        p.WriteFileConsoleResults("Test Pass", 1);
            //    }
            //    else
            //    {
            //        p.WriteFileConsoleResults("Test Fail", 0);
            //    }
            //}
            ////Импорт Docx+Ttx
            //p.WriteFileConsoleResults("Import Ttx+Docx Files Test", 2);
            //for (int i = 0; i < 2; i++)
            //{
            //    bool restd1 = p.ImportSomeFilesTest(DocxFile1, 1);
            //    bool restd2 = p.ImportSomeFilesTest(TtxFile1, 1);
            //    if (restd1 && restd2)
            //    {
            //        p.WriteFileConsoleResults("Test Pass", 1);
            //    }
            //    else
            //    {
            //        p.WriteFileConsoleResults("Test Fail", 0);
            //    }
            //}
            ////Импорт неправильного формата
            //p.WriteFileConsoleResults("Import Wrong Format File", 2);
            //bool wrongres = p.ImportSomeFilesTest(WrongFormatFile, 0);
            //if (wrongres)
            //{
            //    p.WriteFileConsoleResults("Test Pass", 1);
            //}
            //else
            //{
            //    p.WriteFileConsoleResults("Test Fail", 0);
            //}

            

 
  
            
            //p.CreateProjectDuplicateNameTest();
            //p.DeleteProjectTest();
            //p.CreateTMTest();
            //p.CloseTest();

            /*t.SetupTest();
            t.CreateResultFile();
            t.AutorizationTest();

            t.CreateDocumentSetupAllTest();
            t.SettingsTest();

            t.CreateDocumentDublicateNameTest();
            t.CreateDocumentBigNameTest();
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
            t.CloseTest();*/




        }
    }
}
