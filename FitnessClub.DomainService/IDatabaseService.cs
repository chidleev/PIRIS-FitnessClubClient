using System;
using System.Collections.Generic;
using System.ServiceModel;
using FitnessClub.DomainService.Models;

namespace FitnessClub.DomainService
{
    [ServiceContract]
    public interface IDatabaseService
    {
        #region Credentials (Учетные данные)
        [OperationContract]
        bool AddCredential(Credential cred);

        [OperationContract]
        Credential GetCredential(string login);

        [OperationContract]
        bool UpdateCredential(Credential cred);

        [OperationContract]
        bool DeleteCredential(string login);
        #endregion

        #region Users (Клиенты)
        [OperationContract]
        bool AddUser(User user);

        [OperationContract]
        User GetUser(string username);

        [OperationContract]
        List<User> GetAllUsers();

        [OperationContract]
        bool UpdateUser(User user);

        [OperationContract]
        bool DeleteUser(string username);
        #endregion

        #region Admins (Администраторы)
        [OperationContract]
        bool AddAdmin(Admin admin);

        [OperationContract]
        Admin GetAdmin(int adminId);

        [OperationContract]
        List<Admin> GetAllAdmins();

        [OperationContract]
        bool UpdateAdmin(Admin admin);

        [OperationContract]
        bool DeleteAdmin(int adminId);
        #endregion

        #region Trainers (Тренеры)
        [OperationContract]
        bool AddTrainer(Trainer trainer);

        [OperationContract]
        Trainer GetTrainer(int trainerId);

        [OperationContract]
        List<Trainer> GetAllTrainers();

        [OperationContract]
        bool UpdateTrainer(Trainer trainer);

        [OperationContract]
        bool DeleteTrainer(int trainerId);
        #endregion

        #region TrainerSchedules (Расписания тренеров)
        [OperationContract]
        bool AddSchedule(TrainerSchedule schedule);

        [OperationContract]
        List<TrainerSchedule> GetSchedulesByTrainer(int trainerId);

        [OperationContract]
        bool UpdateSchedule(TrainerSchedule schedule);

        [OperationContract]
        bool DeleteSchedule(int scheduleId);
        #endregion

        #region Services (Услуги)
        [OperationContract]
        bool AddService(Service service);

        [OperationContract]
        Service GetService(string name);

        [OperationContract]
        List<Service> GetAllServices();

        [OperationContract]
        bool UpdateService(Service service);

        [OperationContract]
        bool DeleteService(string name);
        #endregion

        #region Events (Занятия/События)
        [OperationContract]
        bool AddEvent(Event ev);

        [OperationContract]
        Event GetEvent(int eventId);

        [OperationContract]
        List<Event> GetAllEvents();

        [OperationContract]
        List<Event> GetEventsByDate(DateTime date);

        [OperationContract]
        bool UpdateEvent(Event ev);

        [OperationContract]
        bool DeleteEvent(int eventId);
        #endregion

        #region Bookings (Бронирования)
        [OperationContract]
        bool AddBooking(Booking booking);

        [OperationContract]
        Booking GetBooking(int bookingId);

        [OperationContract]
        List<Booking> GetBookingsByUser(string username);

        [OperationContract]
        List<Booking> GetBookingsByEvent(int eventId);

        [OperationContract]
        bool UpdateBooking(Booking booking);

        [OperationContract]
        bool DeleteBooking(int bookingId);
        #endregion
    }
}