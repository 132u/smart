using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

namespace AbbyyLS.CAT.Function.Selenium.Tests.CheckRights
{
	public class CheckCreateProjectRightHelper : CommonHelper
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		/// <param name="driver">Драйвер</param>
		/// <param name="wait">Таймаут</param>
		public CheckCreateProjectRightHelper(IWebDriver driver, WebDriverWait wait) :
			base(driver, wait)
		{
		}

		/// <summary>
		/// Метод проверяет, что ссылка для открытия проекта отсутствует.
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public bool LinkProjectExists(string projectName)
		{
			return GetIsElementDisplay(By.XPath("//a[text()='" + projectName + "']"));
		}

		/// <summary>
		/// Метод проверяет, что кнопки "Sign In to Connector" нет.
		/// </summary>
		public bool GetIsNotExistConnectorButton()
		{
			return GetIsElementDisplay(By.XPath(SIGN_TO_CONNECTOR_BUTTON));
		}

		/// <summary>
		/// Метод проверяет, в открытой свёртке проекта есть файл fileName.
		/// </summary>
		/// <param name="fileName">имя проекта</param>
		public bool GetIsFileInProjectExist(string filePath)
		{
			var fileName = GetFileNameWithoutExtension(filePath);
			return GetIsElementDisplay(By.XPath(FILE_IN_PROJECT + fileName + "']"));
		}

		/// <summary>
		/// Метод кликает по кнопке Download в открытой свёртке проекта.
		/// </summary>
		public void DownloadInProjectClick()
		{
			ClickElement(By.XPath(DOWNLOAD_BUTTON_IN_PROJECT));
		}

		/// <summary>
		/// Метод кликает по кнопке Download в главном меню.
		/// </summary>
		public void DownloadInMenuClick()
		{
			ClickElement(By.XPath(DOWNLOAD_BUTTON_IN_MENU));
		}

		/// <summary>
		/// Метод кликает по пункту menuItem во всплывающем меню Download открытой свёртки проекта.
		/// </summary>
		/// <param name="menuItem">Текст содержащийся в пункте меню</param>
		public void ExportClickMenuItemInProject(string menuItem)
		{
			ClickElement(By.XPath(MENU_ITEM_IN_PROJECT + menuItem + "']"));
		}

		/// <summary>
		/// Метод кликает по пункту menuItem в меню Download главного меню.
		/// </summary>
		/// <param name="menuItem">Текст содержащийся в пункте меню</param>
		public void ExportClickMenuItemInMenu(string menuItem)
		{
			ClickElement(By.XPath(MENU_ITEM_IN_MENU + menuItem + "']"));
		}

		/// <summary>
		/// Метод кликает по кнопке Add Files в открытой свёртки проекта.
		/// </summary>
		public void AddDocumentClick()
		{
			ClickElement(By.XPath(ADD_FILES_BUTTON));
		}

		/// <summary>
		/// Загрузка документа на странице workspace
		/// </summary>
		/// <param name="fileName">название документа</param>
		public void UploadFileOnProjectPage(string fileName)
		{
			UploadDocument(fileName, ADD_FILE_ON_WORKSPACE_PAGE);
		}

		/// <summary>
		/// Метод кликает по чекбоксу рядом с документов в свёртке проекта.
		/// </summary>
		/// <param name="docName">путь до документа</param>
		public void CheckBoxDocumentClick(string filePath)
		{
			var fileName = GetFileNameWithoutExtension(filePath);
			ClickElement(By.XPath(DOCUMENT_CHECKBOX + fileName + "']]]]//td[1]/input"));
		}

		/// <summary>
		/// Метод кликает по чекбоксу рядом с проектом.
		/// </summary>
		/// <param name="docName">имя проекта</param>
		public void CheckBoxProjectClick(string projectName)
		{
			ClickElement(By.XPath(PROJECT_CHECKBOX + projectName + "']]]]//td[1]/input"));
		}
		
		/// <summary>
		/// Метод принимает на вход путь до файла и возвращает имя файла без расширения.
		/// </summary>
		/// <param name="docName">путь до документа</param>
		public string GetFileNameWithoutExtension(string filePath)
		{
			return Path.GetFileNameWithoutExtension(filePath);
		}

		/// <summary>
		/// Метод принимает на вход путь до файла и возвращает имя файла.
		/// </summary>
		/// <param name="docName">путь до документа</param>
		public string GetFileName(string filePath)
		{
			return Path.GetFileName(filePath);
		}

		/// <summary>
		/// Метод (в зависимости от параметра выбирает удалить все файлы или проект целиком).
		/// </summary>
		/// <param name="mode">что удалять: все файлы или проект целиком</param>
		public void ClickConfirmDeleteProjectOrFiles(DELETE_MODE mode)
		{
			if (mode == DELETE_MODE.Project)
			{
				ClickElement(By.XPath(DELETE_PROJECT_BUTTON));
			}
			else 
			{
				ClickElement(By.XPath(DELETE_ALL_FILES_BUTON));
			}
		}

		/// <summary>
		/// Метод сообщает, открылась ли форма удаления(проект или файлы).
		/// </summary>
		public bool WaitDeleteProjectOrFilesDisplayDialog()
		{
			return WaitUntilDisplayElement(By.XPath(DELETE_PROJECT_OR_FILES));
		}

		/// <summary>
		/// Метод сообщает, закрылась ли форма удаления(проект или файлы).
		/// </summary>
		public bool WaitDeleteProjectOrFilesDisappearDialog()
		{
			return WaitUntilDisappearElement(By.XPath(DELETE_PROJECT_OR_FILES));
		}
		
		/// <summary>
		/// Метод кликает по кнопке удаления в свёртке проекта.
		/// </summary>
		public void ClickDeleteButtonInProject()
		{
			ClickElement(By.XPath(DELETE_BUTTON_IN_PROJECT));
		}
		
		/// <summary>
		/// Метод проверяет открытие свёртки.
		/// </summary>
		///<param name="projectName">имя проекта</param>
		public bool CheckOpenProject(string projectName)
		{
			return WaitUntilDisplayElement(By.XPath(PROJECT_CHECKBOX + projectName + "']]]]//following-sibling::tr[1][@class='js-project-panel']"));
		}
		
		/// <summary>
		/// Метод проверяет отображение кнопки поиска ошибок.
		/// </summary>
		public bool QACheckButtonExist()
		{
			return WaitUntilDisplayElement(By.XPath(QA_CHECK_BUTTON));
		}
		
		/// <summary>
		/// Метод кликает по кнопке настроек в свёртке проекта.
		/// </summary>
		public void ClickProjectSettings()
		{
			ClickElement(By.XPath(SETTINGS_BUTTON_IN_PROJECT));
		}

		/// <summary>
		/// Метод проверяет отображение окна настроек.
		/// </summary>
		public bool SettingsFormExist()
		{
			return WaitUntilDisplayElement(By.XPath(SETTINGS_FORM));
		}

		/// <summary>
		/// Метод кликает по кнопке анализа в свёртке проекта.
		/// </summary>
		public void ClickProjectAnalysisButton()
		{
			ClickElement(By.XPath(ANALYSIS_BUTTON_IN_PROJECT));
		}

		/// <summary>
		/// Метод проверяет отображение окна анализа.
		/// </summary>
		public bool AnalysisFormExist()
		{
			return WaitUntilDisplayElement(By.XPath(ANALYSIS_FORM));
		}

		
		public enum DELETE_MODE { Files, Project };
		
		protected const string FILE_IN_PROJECT = "//a[contains(@class,'doc-link')][text()='";
		protected const string PROJECT_PANEL_XPATH = "//div[@class='js-panel-container']";
		protected const string DOWNLOAD_BUTTON_IN_PROJECT = PROJECT_PANEL_XPATH + "//li[@class='l-project-export-block']";
		protected const string DOWNLOAD_BUTTON_IN_MENU = "//div[contains(@class,'hd')]//span[contains(@class,'js-document-export')]";
		protected const string MENU_ITEM_IN_PROJECT = PROJECT_PANEL_XPATH + "//a[@class='l-project-export-link' and text()='";
		protected const string MENU_ITEM_IN_MENU = "//div[contains(@class,'hd')]//a[text()='";
		protected const string ADD_FILES_BUTTON = "//span[contains(@data-bind,'importJob')]";
		protected const string ADD_FILE_ON_WORKSPACE_PAGE = "//div[contains(@class,'js-popup-import-document')][2]//div[@class='js-files-uploader']//input";
		protected const string DOCUMENT_CHECKBOX = "//tr[td[div[a[contains(@class,'doc-link')][text()='";
		protected const string PROJECT_CHECKBOX = "//tr[td[div[span[contains(@class,'js-name')][text()='";
		protected const string SIGN_TO_CONNECTOR_BUTTON = "//a[text()='Sign In to Connector']";
		protected const string DELETE_PROJECT_OR_FILES = "//div[text()='Delete project(s) or document(s)?']";
		protected const string DELETE_ALL_FILES_BUTON = "//input[contains(@class,'js-delete-document-btn')]";
		protected const string DELETE_PROJECT_BUTTON = "//input[contains(@class,'js-delete-project-btn')]";
		protected const string DELETE_BUTTON_IN_PROJECT = "//span[contains(@class,'js-delete-project-btn')]";
		protected const string QA_CHECK_BUTTON = "//span[contains(@class,'js-qa-check-btn')]";
		protected const string SETTINGS_BUTTON_IN_PROJECT = "//span[contains(@class,'js-settings-btn')]";
		protected const string SETTINGS_FORM = "//div[contains(@class,'js-popup-edit')][2]";
		protected const string ANALYSIS_BUTTON_IN_PROJECT = "//span[contains(@class,'js-analysis-btn')]";
		protected const string ANALYSIS_FORM = "//div[contains(@class,'js-popup-analyse')][2]";

	}
}
