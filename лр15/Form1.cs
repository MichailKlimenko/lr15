using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лр15
{
    public partial class Form1 : Form
    {
        // Список для хранения заметок
        private List<Note> notes = new List<Note>();
        // Путь к файлу для сохранения заметок
        private string filePath = "notes.json";

        // Конструктор формы
        public Form1()
        {
            InitializeComponent();
            // Привязка обработчика события выбора даты в календаре
            monthCalendar1.DateSelected += monthCalendar1_DateSelected;
            // Привязка обработчика события нажатия на кнопку сохранения заметки
            button2.Click += button2_Click;
            // Привязка обработчика события нажатия на кнопку удаления заметки
            button1.Click += button1_Click;

            // Загрузка заметок при запуске приложения
            LoadNotes();
        }

        // Обработчик нажатия на кнопку удаления заметки
        private void button1_Click(object sender, EventArgs e)
        {
            // Получение выбранной даты
            DateTime selectedDate = monthCalendar1.SelectionStart;
            // Поиск заметки по выбранной дате
            Note note = FindNoteByDate(selectedDate);
            // Если заметка найдена
            if (note != null)
            {
                // Удаление заметки из списка
                notes.Remove(note);
                // Сохранение обновленного списка заметок
                SaveNotes();
                // Очистка текстового поля
                textBox1.Text = "";
                // Изменение текста кнопки на "Сохранить"
                button2.Text = "Сохранить";
                // Отключение кнопки удаления заметки
                button1.Enabled = false;
                // Вывод сообщения об успешном удалении заметки
                MessageBox.Show("Заметка удалена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Обработчик нажатия на кнопку сохранения заметки
        private void button2_Click(object sender, EventArgs e)
        {
            // Получение выбранной даты
            DateTime selectedDate = monthCalendar1.SelectionStart;
            // Получение текста заметки
            string noteText = textBox1.Text.Trim();

            // Проверка наличия текста в заметке
            if (!string.IsNullOrEmpty(noteText))
            {
                // Поиск существующей заметки по выбранной дате
                Note existingNote = FindNoteByDate(selectedDate);
                // Если заметка существует
                if (existingNote != null)
                {
                    // Обновление текста существующей заметки
                    existingNote.Text = noteText;
                }
                else
                {
                    // Добавление новой заметки в список
                    notes.Add(new Note { Date = selectedDate, Text = noteText });
                }
                // Сохранение обновленного списка заметок
                SaveNotes();
                // Вывод сообщения об успешном сохранении заметки
                MessageBox.Show("Заметка сохранена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Вывод сообщения об ошибке в случае отсутствия текста в заметке
                MessageBox.Show("Введите текст заметки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик события выбора даты в календаре
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            // Получение выбранной даты
            DateTime selectedDate = monthCalendar1.SelectionStart;
            // Поиск заметки по выбранной дате
            Note note = FindNoteByDate(selectedDate);

            // Если заметка найдена
            if (note != null)
            {
                // Отображение текста заметки
                textBox1.Text = note.Text;
                // Изменение текста кнопки на "Изменить"
                button2.Text = "Изменить";
                // Включение кнопки удаления заметки
                button1.Enabled = true;
            }
            else
            {
                // Очистка текстового поля
                textBox1.Text = "";
                // Изменение текста кнопки на "Сохранить"
                button2.Text = "Сохранить";
                // Отключение кнопки удаления заметки
                button1.Enabled = false;
            }
        }

        // Метод для загрузки заметок из файла
        private void LoadNotes()
        {
            // Проверка существования файла с заметками
            if (File.Exists(filePath))
            {
                // Чтение JSON из файла
                string json = File.ReadAllText(filePath);
                // Десериализация JSON в список заметок
                notes = JsonConvert.DeserializeObject<List<Note>>(json);
            }
        }

        // Метод для сохранения заметок в файл
        private void SaveNotes()
        {
            // Сериализация списка заметок в JSON
            string json = JsonConvert.SerializeObject(notes);
            // Запись JSON в файл
            File.WriteAllText(filePath, json);
        }

        // Метод для поиска заметки по дате
        private Note FindNoteByDate(DateTime date)
        {
            // Поиск заметки в списке по заданной дате
            return notes.Find(note => note.Date.Date == date.Date);
        }
    }

    // Класс для представления заметки
    public class Note
    {
        // Дата заметки
        public DateTime Date { get; set; }
        // Текст заметки
        public string Text { get; set; }
    }
}
