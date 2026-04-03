using System;
using System.Linq;
using FitnessClub.DomainService.Models;

namespace FitnessClub.DomainService
{
    public class AuthService : IAuthService
    {
        // МЕТОД АВТОРИЗАЦИИ
        public bool Login(string login, string password)
        {
            using (var db = new FitnessDbContext())
            {
                // Ищем учетную запись с таким логином и паролем
                // ВАЖНО: В реальных системах здесь должно быть сравнение хэшей паролей
                var account = db.Credentials.FirstOrDefault(c => c.Login == login && c.Password == password);

                return account != null; // Возвращаем true, если нашли совпадение
            }
        }

        // МЕТОД РЕГИСТРАЦИИ
        public bool Register(string login, string password, string username, string name, string lastname, string phone, string email)
        {
            using (var db = new FitnessDbContext())
            {
                // Используем транзакцию, так как нужно обновить две таблицы одновременно
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Проверяем, не занят ли логин
                        if (db.Credentials.Any(c => c.Login == login))
                            return false;

                        // 2. Создаем запись в таблице credentials
                        var newCred = new Credential
                        {
                            Login = login,
                            Password = password,
                            Telephone = phone,
                            Email = email
                        };
                        db.Credentials.Add(newCred);

                        // 3. Создаем запись в таблице users (профиль клиента)
                        var newUser = new User
                        {
                            Username = username,
                            Login = login,
                            Name = name,
                            Lastname = lastname
                        };
                        db.Users.Add(newUser);

                        // Сохраняем изменения в БД
                        db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        // МЕТОД ПОЛУЧЕНИЯ ДАННЫХ АКТИВНОГО ПОЛЬЗОВАТЕЛЯ
        public User GetCurrentUser(string login)
        {
            using (var db = new FitnessDbContext())
            {
                // Находим профиль пользователя, привязанный к данному логину
                var user = db.Users.FirstOrDefault(u => u.Login == login);
                return user;
            }
        }
    }
}