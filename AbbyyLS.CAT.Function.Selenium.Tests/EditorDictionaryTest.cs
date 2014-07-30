﻿using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using OpenQA.Selenium.Interactions;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	public class EditorDictionaryTest : BaseTest
	{
		public EditorDictionaryTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
            // 1. Авторизация
            Authorization();

			// 2. Создание проекта с 1 документом внутри
            CreateProject(ProjectName);

            // 3. Открытие настроек проекта
			ImportDocumentProjectSettings(DocumentFile, ProjectName);

            // 4. Назначение задачи на пользователя
            AssignTask();

            // 5. Открытие документа
            OpenDocument();

			// Подготавливаем словарь к работе
			DeleteAllWords();
        }

		/// <summary>
		/// Тест: проверка добавления слова в словарь
		/// </summary>
		[Test]
		public void AddNewWord()
		{
			string word = "Ккапучино";
			
			// Добавляем новое слово в словарь
			AddWord(word);
			
			// Снова открываем форму словаря
			OpenEditorDictionary();

			// Проверяем, что слово добавлено в словарь
			Assert.IsTrue(GetIsWordPresentInDictionary(word),
				"Ошибка: Слово отсутствует в словаре.");

			// Закрываем форму словаря
			CloseEditorDictionary();

			// Закрываем редактор
			EditorClickBackBtn();

			// Снова открываем редактор
			OpenDocument();

			// Снова открываем форму словаря
			OpenEditorDictionary();

			// Проверяем, что слово добавлено в словарь
			Assert.IsTrue(GetIsWordPresentInDictionary(word),
				"Ошибка: Слово отсутствует в словаре.");
		}

		/// <summary>
		/// Тест: проверка удаления слова из словаря
		/// </summary>
		[Test]
		public void DeleteWord()
		{
			string word = "Ллатте";

			// Добавляем новое слово в словарь
			AddWord(word);

			// Удаляем слово
			DeleteWord(word);

			// Снова открываем форму словаря
			OpenEditorDictionary();

			// Проверяем, что слово отсутствует в словарь
			Assert.IsFalse(GetIsWordPresentInDictionary(word),
				"Ошибка: Слово не удалилось после повторного открытия окна словаря.");

			// Закрываем форму словаря
			CloseEditorDictionary();

			// Закрываем редактор
			EditorClickBackBtn();

			// Снова открываем редактор
			OpenDocument();

			// Снова открываем форму словаря
			OpenEditorDictionary();

			// Проверяем, что слово отсутствует в словарь
			Assert.IsFalse(GetIsWordPresentInDictionary(word),
				"Ошибка: Слово не удалилось после повторного открытия редактора.");
		}

		/// <summary>
		/// Тест: проверка, что новое слово подчеркнуто в сегменте
		/// </summary>
		[Test]
		public void UnderlineBeforeAddWord()
		{
			string word = "Ээспрессо";

			// Добавляем слово в target сегмента
			EditorPage.AddTextTarget(1, word);

			// Дождаться автосохранения
			AutoSave();

			// Проверяем, что слово подчеркнуто
			Assert.IsTrue(EditorPage.GetWordListSpellcheck(1).Contains(word),
				"Ошибка: Нужное слово не обнаружено в списке подчеркнутых.");
		}

		/// <summary>
		/// Тест: проверка, что добавленное в словарь слово в сегменте не подчеркивается
		/// </summary>
		[Test]
		public void UnderlineAfterAddWord()
		{
			string word = "Аамерикано";

			// Добавляем новое слово
			AddWord(word);

			// Добавляем слово в target сегмента
			EditorPage.AddTextTarget(1, word);

			// Дождаться автосохранения
			AutoSave();

			// Проверяем, что слово не подчеркнуто
			Assert.IsFalse(EditorPage.GetWordListSpellcheck(1).Contains(word),
				"Ошибка: Нужное слово обнаружено в списке подчеркнутых.");
		}

		/// <summary>
		/// Тест: проверка подчеркивания после удаления слова из словаря
		/// </summary>
		[Test]
		public void UnderlineAfterDeleteWord()
		{
			string word = "Ммокка";

			// Добавляем новое слово
			AddWord(word);

			// Удаляем слово
			DeleteWord(word);

			// Добавляем слово в target сегмента
			EditorPage.AddTextTarget(1, word);

			// Дождаться автосохранения
			AutoSave();

			// Проверяем, что слово подчеркнуто
			Assert.IsTrue(EditorPage.GetWordListSpellcheck(1).Contains(word),
				"Ошибка: Нужное слово не обнаружено в списке подчеркнутых.");
		}

		const string Word1 = "Планета";
		const string Word2 = "Чуть-чуть";
		/// <summary>
		/// Тест: проверка подчеркивания слова с дефисом
		/// </summary>
		[Test]
		[TestCase(Word1)]
		[TestCase(Word2)]
		public void UnderlineWord(string word)
		{
			string wrongWord = "Ы" + word;

			// Проверяем, что слово не подчеркнуто
			Assert.IsFalse(GetIsWordUnderlined(word), 
				"Ошибка: Слово должно быть не подчеркнуто");

			// Проверяем, что слово подчеркнуто
			Assert.IsTrue(GetIsWordUnderlined(wrongWord),
				"Ошибка: Слово должно быть подчеркнуто");
		}

		/// <summary>
		/// Тест: проверка повторного добавления слова в словарь
		/// </summary>
		[Test]
		public void AddSameWord()
		{
			string word = "Ббариста";

			// Добавляем новое слово
			AddWord(word);

			// Открываем словарь
			OpenEditorDictionary();

			// Добавляем это же слово еще раз
			AddWordDictionaryOpened(word);

			// Проверяем, что открылось сообщение об ошибке
			Assert.IsTrue(EditorPage.WaitAlreadyExistInDictionaryMessageDisplay(),
				"Ошибка: Сообщение об ошибке не открылось.");
		}

		/// <summary>
		/// Тест: проверка редактирования слова в словаре
		/// </summary>
		[Test]
		public void EditWord()
		{
			string wordFirst = "Рристретто";
			string wordSecond = "Ррристретто";

			// Добавляем новое слово
			AddWord(wordFirst);

			// Открываем словарь
			OpenEditorDictionary();

			// Дважды кликаем на созданном слове
			EditorPage.DoubleClickWordDictionary(wordFirst);
			
			// Добавляем в поле новое слово
			EditorPage.AddWordDictionary(wordSecond);
			Thread.Sleep(1000);

			// Проверяем, что слово добавлено в словарь
			Assert.IsTrue(GetIsWordPresentInDictionary(wordSecond),
				"Ошибка: Нужное слово отсутствует в словаре.");

			// Закрываем форму словаря
			CloseEditorDictionary();
			
			// Снова открываем форму словаря
			OpenEditorDictionary();

			// Проверяем, что слово добавлено в словарь
			Assert.IsTrue(GetIsWordPresentInDictionary(wordSecond),
				"Ошибка: Нужное слово отсутствует в словаре.");
		}

		

		/// <summary>
		/// Добавление нового слова в открытом словаре
		/// </summary>
		/// <param name="word">Слово</param>
		protected void AddWordDictionaryOpened(string word)
		{
			// Нажать кнопку добавления нового слова в словарь
			EditorPage.ClickAddWordDictionaryBtn();

			// Добавляем в поле новое слово
			EditorPage.AddWordDictionary(word);
			Thread.Sleep(1000);
		}
		
		/// <summary>
		/// Закрывает форму словаря в редакторе
		/// </summary>
		protected void CloseEditorDictionary()
		{
			// Закрываем форму словаря
			EditorPage.ClickCloseDictionaryBtn();

			// Проверка, что форма закрылась
			Assert.IsTrue(EditorPage.WaitDictionaryFormDisappear(),
				"Ошибка: Форма со словарем закрылась.");
		}

		/// <summary>
		/// Добавление нового слова в словарь
		/// </summary>
		/// <param name="word">Слово</param>
		protected void AddWord(string word)
		{
			// Открываем словарь
			OpenEditorDictionary();

			// Добавляем новое слово
			AddWordDictionaryOpened(word);

			// Проверяем, что слово добавлено в словарь
			Assert.IsTrue(GetIsWordPresentInDictionary(word),
				"Ошибка: Нужное слово отсутствует в словаре.");

			// Закрываем форму словаря
			CloseEditorDictionary();
		}

		/// <summary>
		/// Удаление слова из словаря
		/// </summary>
		/// <param name="word">Слово</param>
		protected void DeleteWord(string word)
		{
			// Открываем форму словаря
			OpenEditorDictionary();

			// Удаляем слово
			EditorPage.ClickDeleteWordDictionaryBtn(word);

			// Проверяем, что слово отсутствует в словарь
			Assert.IsFalse(GetIsWordPresentInDictionary(word),
				"Ошибка: Слово не удалилось.");

			// Закрываем форму словаря
			CloseEditorDictionary();
		}

		/// <summary>
		/// Удаление всех слов из словаря
		/// </summary>
		protected void DeleteAllWords()
		{
			// Открываем словарь
			OpenEditorDictionary();

			List<string> wordsList = EditorPage.GetWordListDictionary();

			// Проверяем, что в словаре есть слова и удаляем их
			if (wordsList.Count != 0)
				foreach (string word in wordsList)
					EditorPage.ClickDeleteWordDictionaryBtn(word);

			// Закрываем форму словаря
			CloseEditorDictionary();
		}

		/// <summary>
		/// Проверка присутствия в словаре заданного слова
		/// </summary>
		/// <param name="word">Слово для проверки</param>
		/// <returns>Слово присутствует</returns>
		protected bool GetIsWordPresentInDictionary(string word)
		{
			List<string> wordsList = EditorPage.GetWordListDictionary();
			bool isPresent = false;

			// Проверяем, в словаре есть слова
			if ((wordsList.Count != 0) && (wordsList.Contains(word)))
				isPresent = true;

			return isPresent;
		}

		/// <summary>
		/// Добавляет слово в сегмент и возвращает подчеркнуто оно или нет
		/// </summary>
		/// <param name="word">Заданное слово</param>
		/// <returns>Слово подчеркнуто</returns>
		protected bool GetIsWordUnderlined(string word)
		{
			// Добавляем слово в target сегмента
			EditorPage.AddTextTarget(1, word);

			// Дождаться автосохранения
			AutoSave();

			// Проверяем, что слово подчеркнуто
			if (EditorPage.GetWordListSpellcheck(1).Contains(word))
				return true;
			else
				return false;
		}
	}
}
