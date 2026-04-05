using FitnessClubClient.AuthServiceRef;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FitnessClubClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик события изменения текста в любом из трех TextBox
        private void Fields_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Проверяем, заполнены ли поля (не пустые ли они)
            bool isLoginFilled = !string.IsNullOrWhiteSpace(tbLogin.Text);
            bool isPasswordFilled = !string.IsNullOrWhiteSpace(tbPassword.Text);
            bool isConfirmFilled = !string.IsNullOrWhiteSpace(tbConfirmPassword.Text);

            // Кнопка авторизации активна только при заполнении логина и пароля
            bLogin.IsEnabled = isLoginFilled && isPasswordFilled;

            // Кнопка регистрации активна при заполнении всех трех полей
            bRegister.IsEnabled = isLoginFilled && isPasswordFilled && isConfirmFilled;
        }

        // ОБНОВЛЕННЫЙ МЕТОД РЕГИСТРАЦИИ
        private void bRegister_Click(object sender, RoutedEventArgs e)
        {
            // 1. Локальная проверка идентичности введенных паролей
            if (tbPassword.Text != tbConfirmPassword.Text)
            {
                MessageBox.Show("Введенные пароли не совпадают. Пожалуйста, проверьте данные.",
                                "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Прерываем процесс регистрации
            }

            try
            {
                // 2. Инициализация TCP-клиента службы авторизации
                AuthServiceClient authClient = new AuthServiceClient();

                // Считываем данные с формы
                string login = tbLogin.Text;
                string password = tbPassword.Text;

                // ВАЖНО: Так как в текущем XAML нет полей для имени, телефона и email,
                // временно используем заглушки. Позже вы можете привязать сюда новые TextBox.
                string username = login; // Используем логин как уникальный юзернейм
                string name = "Новый";
                string lastname = "Клиент";
                string phone = "00000000000";
                string email = $"{login}@fitness.local";

                // 3. Отправляем данные на сервер для регистрации (создание записей в БД)
                bool isRegistered = authClient.Register(login, password, username, name, lastname, phone, email);

                if (isRegistered)
                {
                    // 4. Успешный сценарий
                    MessageBox.Show("Регистрация прошла успешно! Теперь вы можете работать в системе.",
                                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Формируем отображаемое имя
                    string displayName = $"{name} {lastname}";

                    // Инициализация объекта основного рабочего окна и передача данных
                    WorkWindow workWindow = new WorkWindow(displayName);
                    workWindow.Show();

                    // Закрытие текущего окна авторизации
                    this.Close();
                }
                else
                {
                    // 5. Ошибка со стороны сервера (например, логин уже занят)
                    MessageBox.Show("Пользователь с таким логином уже существует. Пожалуйста, придумайте другой логин.",
                                    "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

                // Закрываем сетевое соединение
                authClient.Close();
            }
            catch (System.Exception ex)
            {
                // Обработка ситуаций, когда сервер не запущен или нет сети
                MessageBox.Show("Ошибка соединения с сервером. Проверьте подключение к сети.\n\nДетали: " + ex.Message,
                                "Сбой сети", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик нажатия на кнопку "Войти"
        private void bLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Инициализация TCP-клиента службы авторизации
                AuthServiceClient authClient = new AuthServiceClient();

                // Обращение к серверу для верификации
                bool isAuthorized = authClient.Login(tbLogin.Text, tbPassword.Text);

                if (isAuthorized)
                {
                    // Получаем данные профиля (Имя и Фамилию) для рабочего окна
                    var userProfile = authClient.GetCurrentUser(tbLogin.Text);
                    string displayName = $"{userProfile.Name} {userProfile.Lastname}";

                    MessageBox.Show("Успешная авторизация в системе", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Передаем полное имя и логин в рабочее окно
                    WorkWindow workWindow = new WorkWindow(displayName);
                    workWindow.Show();
                    this.Close();
                }
                else
                {
                    // Сообщение о неверном пароле
                    MessageBox.Show("Неверный логин или пароль. Доступ к системе заблокирован.",
                                    "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                authClient.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Нет связи с сервером: " + ex.Message, "Ошибка сети", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
