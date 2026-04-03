using System.ServiceModel;
using FitnessClub.DomainService.Models;

namespace FitnessClub.DomainService
{
    [ServiceContract]
    public interface IAuthService
    {
        // Проверка логина и пароля
        [OperationContract]
        bool Login(string login, string password);

        // Регистрация нового клиента (создание записей в credentials и users)
        [OperationContract]
        bool Register(string login, string password, string username, string name, string lastname, string phone, string email);

        // Получение данных профиля текущего авторизованного пользователя
        [OperationContract]
        User GetCurrentUser(string login);
    }
}