using FitnessClub;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FitnessClubClient
{
    /// <summary>
    /// Логика взаимодействия для WorkWindow.xaml
    /// </summary>
    public partial class WorkWindow : Window
    {
        // Коллекция данных для расписания. Использование ObservableCollection 
        // позволяет DataGrid обновляться автоматически при добавлении новых элементов.
        private ObservableCollection<FitnessClass> _scheduleList;

        // Изменяем конструктор так, чтобы он принимал логин авторизованного пользователя
        public WorkWindow(string userLogin = "Администратор")
        {
            InitializeComponent();

            // Вывод логина в информационной панели
            lblUserInfo.Content = $"Текущий пользователь: {userLogin}";

            // Инициализация статических данных для DataGrid
            InitializeStaticData();
        }

        private void InitializeStaticData()
        {
            _scheduleList = new ObservableCollection<FitnessClass>
            {
                new FitnessClass { ServiceName = "Йога", TrainerName = "Иванова А.И.", Date = DateTime.Now, Time = "10:00", AvailableSpots = 15 },
                new FitnessClass { ServiceName = "Бокс", TrainerName = "Смирнов П.В.", Date = DateTime.Now.AddDays(1), Time = "18:00", AvailableSpots = 8 },
                new FitnessClass { ServiceName = "Кроссфит", TrainerName = "Петров В.С.", Date = DateTime.Now, Time = "19:00", AvailableSpots = 10 }
            };

            // Привязка коллекции к таблице
            dgSchedule.ItemsSource = _scheduleList;
        }

        // Обработчик события изменения текста/выбора в полях формы добавления
        private void AddForm_Changed(object sender, RoutedEventArgs e)
        {
            // Валидация: проверяем заполнение всех необходимых полей
            bool isServiceSelected = cbService.SelectedItem != null;
            bool isTrainerFilled = !string.IsNullOrWhiteSpace(tbTrainer.Text);
            bool isDateSelected = dpAddDate.SelectedDate.HasValue;
            bool isTimeFilled = !string.IsNullOrWhiteSpace(tbTime.Text);
            bool isSpotsFilled = !string.IsNullOrWhiteSpace(tbSpots.Text) && int.TryParse(tbSpots.Text, out _);

            // Кнопка становится активной, только если все поля заполнены корректно
            if (bAddRecord != null)
            {
                bAddRecord.IsEnabled = isServiceSelected && isTrainerFilled && isDateSelected && isTimeFilled && isSpotsFilled;
            }
        }

        // Обработчик кнопки "Добавить запись"
        private void bAddRecord_Click(object sender, RoutedEventArgs e)
        {
            // Формирование нового объекта и добавление в коллекцию
            FitnessClass newClass = new FitnessClass
            {
                ServiceName = ((ComboBoxItem)cbService.SelectedItem).Content.ToString(),
                TrainerName = tbTrainer.Text,
                Date = dpAddDate.SelectedDate.Value,
                Time = tbTime.Text,
                AvailableSpots = int.Parse(tbSpots.Text)
            };

            _scheduleList.Add(newClass);

            // Очистка полей ввода после добавления
            cbService.SelectedIndex = -1;
            tbTrainer.Clear();
            dpAddDate.SelectedDate = null;
            tbTime.Clear();
            tbSpots.Clear();

            MessageBox.Show("Новая запись успешно добавлена в расписание!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Обработчик кнопки "Напоминания" (Фильтрация по дате)
        private void bReminders_Click(object sender, RoutedEventArgs e)
        {
            if (dpFilterDate.SelectedDate.HasValue)
            {
                DateTime selectedDate = dpFilterDate.SelectedDate.Value.Date;

                // Получаем представление данных DataGrid для применения фильтра
                ICollectionView cv = CollectionViewSource.GetDefaultView(dgSchedule.ItemsSource);

                // Устанавливаем фильтр: показывать только те записи, дата которых совпадает с выбранной
                cv.Filter = o =>
                {
                    FitnessClass fc = o as FitnessClass;
                    return fc != null && fc.Date.Date == selectedDate;
                };
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дату для поиска.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Вспомогательная кнопка для сброса фильтра
        private void bResetFilter_Click(object sender, RoutedEventArgs e)
        {
            dpFilterDate.SelectedDate = null;
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgSchedule.ItemsSource);
            cv.Filter = null; // Сброс фильтра
        }

        // Обработчик кнопки "Выход"
        private void bLogout_Click(object sender, RoutedEventArgs e)
        {
            // Возврат к окну авторизации
            MainWindow authWindow = new MainWindow();
            authWindow.Show();
            this.Close();
        }
    }
}

namespace FitnessClub
{
    // Модель данных для расписания
    public class FitnessClass
    {
        public string ServiceName { get; set; }
        public string TrainerName { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public int AvailableSpots { get; set; }
    }
}