// Подключаем пространство имен вашей WCF службы (замените DbServiceRef на ваше имя, если нужно)
using FitnessClubClient.DbServiceRef;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FitnessClubClient
{
    /// <summary>
    /// Логика взаимодействия для WorkWindow.xaml
    /// </summary>
    public partial class WorkWindow : Window
    {
        private string _currentUserLogin;

        // Объявляем WCF TCP-клиента для связи с сервером
        private DatabaseServiceClient _dbClient;

        // Конструктор теперь принимает полное имя для отображения и логин для работы с БД
        public WorkWindow(string displayName = "Администратор", string login = "admin")
        {
            InitializeComponent();
            _currentUserLogin = login;

            // Вывод информации о пользователе
            lblUserInfo.Content = $"Текущий пользователь: {displayName}";

            // Инициализация подключения к серверу
            _dbClient = new DatabaseServiceClient();

            // Загрузка реальных данных из базы MySQL при открытии окна
            RefreshDataGrid();
        }



// МЕТОД: Загрузка всех событий из БД
private void RefreshDataGrid()
    {
        try
        {
            // 1. Получаем сырые данные с сервера (объекты Event из БД)
            var eventsFromDb = _dbClient.GetAllEvents();

            // 2. Преобразуем их так, чтобы названия свойств совпадали с Binding в XAML
            var displayList = eventsFromDb.Select(ev => new
            {
                // Оставляем как есть
                ServiceName = ev.ServiceName,

                // В БД только ID тренера. Превращаем в строку. 
                // (В идеале на сервере нужен JOIN с таблицей Trainers, но для отображения хватит так)
                TrainerName = "Тренер ID: " + ev.TrainerId,

                // Вытаскиваем только дату
                Date = ev.FromDate.Date,

                // Вытаскиваем время в формате "Часы:Минуты"
                Time = ev.FromDate.ToString("HH:mm"),

                // Подменяем название для свободных мест
                AvailableSpots = ev.MaxParticipants
            }).ToList();

            // 3. Привязка адаптированных данных к таблице
            dgSchedule.ItemsSource = displayList;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Нет связи с сервером БД: " + ex.Message, "Ошибка сети", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // ОБРАБОТЧИК: Валидация полей ввода (остается клиентской для мгновенного отклика UI)
    private void AddForm_Changed(object sender, RoutedEventArgs e)
        {
            bool isServiceSelected = cbService.SelectedItem != null;
            bool isTrainerFilled = !string.IsNullOrWhiteSpace(tbTrainer.Text);
            bool isDateSelected = dpAddDate.SelectedDate.HasValue;
            bool isTimeFilled = !string.IsNullOrWhiteSpace(tbTime.Text);
            bool isSpotsFilled = !string.IsNullOrWhiteSpace(tbSpots.Text) && int.TryParse(tbSpots.Text, out _);

            if (bAddRecord != null)
            {
                bAddRecord.IsEnabled = isServiceSelected && isTrainerFilled && isDateSelected && isTimeFilled && isSpotsFilled;
            }
        }

        // ОБРАБОТЧИК: Добавление новой записи через сервер
        private void bAddRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Считываем данные с формы и подготавливаем дату/время
                DateTime selectedDate = dpAddDate.SelectedDate.Value;
                TimeSpan time = TimeSpan.Parse(tbTime.Text); // Ожидается ввод в формате "14:00"
                DateTime fullDateTime = selectedDate.Add(time);
                
                // 2. Формируем объект события (сущность из БД)
                Event newEvent = new Event
                {
                    Id = 1,
                    ServiceName = "Yoga",
                    // Примечание: в вашей БД TrainerId - это int. Для простоты симулируем парсинг или берем ID = 1.
                    // В идеале в ComboBox должны выбираться тренеры из БД.
                    TrainerId = 1,
                    FromDate = fullDateTime,
                    Duration = 60, // Стандартная длительность 60 минут
                    MaxParticipants = int.Parse(tbSpots.Text),
                    Status = "new",
                    Repeatable = false
                };

                // 3. Отправляем на сервер
                bool isSuccess = _dbClient.AddEvent(newEvent);

                if (isSuccess)
                {
                    MessageBox.Show("Новая запись успешно добавлена в расписание БД!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Очистка формы
                    cbService.SelectedIndex = -1;
                    tbTrainer.Clear();
                    dpAddDate.SelectedDate = null;
                    tbTime.Clear();
                    tbSpots.Clear();

                    // 4. АВТОМАТИЧЕСКОЕ ОБНОВЛЕНИЕ ТАБЛИЦЫ напрямую с сервера
                    RefreshDataGrid();
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении в базу данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Проверьте формат введенного времени (ЧЧ:ММ). Подробности: " + ex.Message, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // ОБРАБОТЧИК: Напоминания (Строгая фильтрация ТОЛЬКО НА КЛИЕНТЕ)
        private void bReminders_Click(object sender, RoutedEventArgs e)
        {
            if (dpFilterDate.SelectedDate.HasValue)
            {
                DateTime selectedDate = dpFilterDate.SelectedDate.Value.Date;

                // Берем текущее представление данных, которое уже висит в DataGrid
                ICollectionView cv = CollectionViewSource.GetDefaultView(dgSchedule.ItemsSource);

                if (cv != null)
                {
                    // Применяем фильтр локально к строкам таблицы
                    cv.Filter = o =>
                    {
                        // Используем dynamic, так как данные загружены как анонимные типы
                        dynamic row = o;
                        return row.Date == selectedDate;
                    };

                    // Если после фильтрации строк не осталось, выводим сообщение
                    if (cv.IsEmpty)
                    {
                        MessageBox.Show("На выбранную дату тренировок не найдено.", "Напоминания", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дату для поиска.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // ОБРАБОТЧИК: Сброс фильтра (ТОЛЬКО НА КЛИЕНТЕ)
        private void bResetFilter_Click(object sender, RoutedEventArgs e)
        {
            dpFilterDate.SelectedDate = null;

            // Получаем представление таблицы и просто сбрасываем фильтр (возвращаем все строки)
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgSchedule.ItemsSource);
            if (cv != null)
            {
                cv.Filter = null;
            }
        }

        // ОБРАБОТЧИК: Выход из системы
        private void bLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Закрываем TCP соединение с сервером
                if (_dbClient != null && _dbClient.State == System.ServiceModel.CommunicationState.Opened)
                {
                    _dbClient.Close();
                }
            }
            catch { _dbClient.Abort(); }

            // Возврат к окну авторизации
            MainWindow authWindow = new MainWindow();
            authWindow.Show();
            this.Close();
        }

        // ====================================================================================
        // ДОПОЛНИТЕЛЬНО: Метод создания бронирования (записи клиента на тренировку)
        // Согласно тексту документа: "формирует объект бронирования и отправляет его на сервис"
        // Вы можете привязать этот метод к новой кнопке "Записаться" в вашем интерфейсе.
        // ====================================================================================
        private void bBookEvent_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedule.SelectedItem is Event selectedEvent)
            {
                Booking newBooking = new Booking
                {
                    EventId = selectedEvent.Id,
                    Username = _currentUserLogin, // Используем логин, переданный из окна авторизации
                    Status = "подтверждена",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                bool success = _dbClient.AddBooking(newBooking);
                if (success) MessageBox.Show("Вы успешно записались на тренировку!", "Успех");
            }
            else
            {
                MessageBox.Show("Сначала выберите тренировку из таблицы расписания.");
            }
        }
    }
}