using System;
using System.IO;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class ExportFileHelper : WorkspaceHelper
	{
		public ExportFileHelper(WebDriver driver) : base(driver)
		{
			_exportMenu = new ExportMenu(Driver);
			_exportNotification = new ExportNotification(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_projectsPage = new ProjectsPage(Driver);
		}

		public ExportFileHelper SelectExportType<T>(ExportType exportType) where T : class, IAbstractPage<T>
		{
			BaseObject.InitPage(_exportMenu, Driver);
			_exportMenu.ClickExportType<T>(exportType, Driver);

			return this;
		}

		public ExportFileHelper ClickDownloadNotifier<T>() where T: class, IAbstractPage<T>
		{
			BaseObject.InitPage(_exportNotification, Driver);
			_exportNotification.ClickDownloadNotifier<T>(Driver);

			return this;
		}

		public ExportFileHelper AssertPreparingDownloadMessageDisappeared()
		{
			BaseObject.InitPage(_projectsPage, Driver);
			_projectsPage.AssertPreparingDownloadMessageDisappeared();

			return new ExportFileHelper(Driver);
		}


		public ExportFileHelper CancelAllNotifiers<T>() where T: class, IAbstractPage<T>
		{
			BaseObject.InitPage(_workspacePage, Driver);
			var countNotifiers = _workspacePage.GetCountExportNotifiers();

			for (int i = 0; i < countNotifiers; i++)
			{
				//sleep стоит,чтобы закрываемые уведомления успевали пропадать
				Thread.Sleep(1000);
				BaseObject.InitPage(_exportNotification, Driver);
				_exportNotification.ClickCancelNotifier<T>(Driver);
			}

			return this;
		}

		public int GetCountExportNotifiers()
		{
			BaseObject.InitPage(_workspacePage, Driver);

			return _workspacePage.GetCountExportNotifiers();
		}

		public ExportFileHelper ClickCancelInDowloadNotifier<T>() where T : class, IAbstractPage<T>
		{
			BaseObject.InitPage(_exportNotification, Driver);
			_exportNotification.ClickCancelNotifier<T>(Driver);

			return this;
		}

		public ExportFileHelper AssertCountExportNotifiers(int expectedCount)
		{
			BaseObject.InitPage(_exportNotification, Driver);
			_exportNotification.AssertCountExportNotifiers(expectedCount);

			return this;
		}

		public ExportFileHelper AssertNotificationNotExist()
		{
			BaseObject.InitPage(_workspacePage, Driver);
			
			Assert.IsTrue(_workspacePage.GetCountExportNotifiers() == 0,
				"Произошла ошибка:\n остались открытые уведомления.");

			return this;
		}

		public ExportFileHelper AssertContainsText(string text)
		{
			BaseObject.InitPage(_exportNotification, Driver);
			_exportNotification.AssertContainsText(text);

			return this;
		}

		public ExportFileHelper AssertContainsCurrentDate()
		{
			BaseObject.InitPage(_exportNotification, Driver);
			_exportNotification.AssertContainsCurrentDate();

			return this;
		}

		/// <summary>
		/// Перелючиться на уведомление по его номеру
		/// </summary>
		/// <remarks>
		/// Уведомления считаются по оси z
		/// 1-е - дальнее, скрытое
		/// Последнее - ближнее, видимое целиком
		/// </remarks>
		/// <param name="notificationNumber">Номер уведомления</param>
		/// <returns>Текст верхнего сообщения уведомления</returns>
		public string GetTextNotificationByNumber(int notificationNumber)
		{
			BaseObject.InitPage(_exportNotification, Driver);

			return _exportNotification
				.SwitchToNotificationByNumber(notificationNumber)
				.GetTextUpperNotification();	
		}

		/// <summary>
		/// Возвращает маску имени экспортиремого файла для поиска на жёстком диске
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		/// <param name="filePath">путь до файла</param>
		/// <returns></returns>
		public string GetExportFileNameMask(ExportType exportType, string filePath)
		{
			return exportType == ExportType.Tmx 
				? Path.GetFileNameWithoutExtension(filePath) + "*.tmx"
				: Path.GetFileNameWithoutExtension(filePath) + "*" + Path.GetExtension(filePath);
		}

		public ExportFileHelper AssertFileDownloaded(string fileMask)
		{
			var files = getDownloadedFiles(fileMask, 15, PathProvider.ExportFiles);

			Assert.IsTrue(files.Length > 0,
				"Ошибка: файл не загрузился за отведённое время (15 секунд)");

			var directoryInfo = Directory.CreateDirectory(Path.Combine(
				new string[]{ 
					PathProvider.ResultsFolderPath,
					TestContext.CurrentContext.Test.Name,
					DateTime.Now.Ticks.ToString()
				})).FullName;

			var pathToMove = Path.Combine(
				new string[]{
					directoryInfo, 
					Path.GetFileNameWithoutExtension(files[0]) + DateTime.Now.Ticks + Path.GetExtension(files[0])
				});

			File.Move(files[0], pathToMove);

			return this;
		}

		private string[] getDownloadedFiles(string mask, int waitTime, string dirName)
		{
			var files = new string[0];

			for (int i = 0; i < waitTime; i++)
			{
				files = Directory.GetFiles(dirName, mask, SearchOption.TopDirectoryOnly);
				if (files.Length > 0)
				{
					break;
				}
				Thread.Sleep(1000);//Ждём секунду
			}

			return files;
		}

		private readonly ProjectsPage _projectsPage;
		private readonly ExportMenu _exportMenu;
		private readonly ExportNotification _exportNotification;
		private readonly WorkspacePage _workspacePage;
	}
}
