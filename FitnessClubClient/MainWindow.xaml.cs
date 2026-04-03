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

        // Обработчик нажатия на кнопку "Зарегистрироваться"
        private void bRegister_Click(object sender, RoutedEventArgs e)
        {
            // Проверка идентичности введенных паролей
            if (tbPassword.Text != tbConfirmPassword.Text)
            {
                MessageBox.Show("Введенные пароли не совпадают. Пожалуйста, проверьте данные.",
                                "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Прерываем процесс регистрации
            }

            // Заглушка: имитация успешного ответа от сервера при регистрации
            MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            // Переход к основному рабочему окну
            OpenWorkWindow();
        }

        // Обработчик нажатия на кнопку "Войти"
        private void bLogin_Click(object sender, RoutedEventArgs e)
        {
            // Заглушка: имитация успешной авторизации
            // В реальном приложении здесь был бы запрос к БД

            // Переход к основному рабочему окну
            OpenWorkWindow();
        }

        // Вспомогательный метод для инициализации WorkWindow и закрытия текущего окна
        private void OpenWorkWindow()
        {
            // Инициализация объекта основного рабочего окна
            WorkWindow workWindow = new WorkWindow();

            // Отображение нового окна
            workWindow.Show();

            // Закрытие текущего окна авторизации
            this.Close();
        }
    }
}
