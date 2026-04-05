using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FitnessClub.DomainService.Models;

namespace FitnessClub.DomainService
{
    public class DatabaseService : IDatabaseService
    {
        // Вспомогательный метод для сокращения кода (проверка на null и сохранение)
        private bool SaveChanges(FitnessDbContext db)
        {
            try
            {
                return db.SaveChanges() > 0;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                // Ошибки валидации (например, слишком длинная строка или null в обязательном поле)
                string errorMsg = string.Join("; ", dbEx.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                throw new Exception("Ошибка валидации: " + errorMsg);
            }
            catch (Exception ex)
            {
                // Ошибки самой базы данных (внешние ключи, уникальность и т.д.)
                throw new Exception("Ошибка БД: " + (ex.InnerException?.InnerException?.Message ?? ex.Message));
            }
        }

        #region Credentials (Учетные данные)
        public bool AddCredential(Credential cred)
        {
            using (var db = new FitnessDbContext())
            {
                db.Credentials.Add(cred);
                return SaveChanges(db);
            }
        }

        public Credential GetCredential(string login)
        {
            using (var db = new FitnessDbContext())
                return db.Credentials.Find(login);
        }

        public bool UpdateCredential(Credential cred)
        {
            using (var db = new FitnessDbContext())
            {
                db.Entry(cred).State = EntityState.Modified;
                return SaveChanges(db);
            }
        }

        public bool DeleteCredential(string login)
        {
            using (var db = new FitnessDbContext())
            {
                var item = db.Credentials.Find(login);
                if (item == null) return false;
                db.Credentials.Remove(item);
                return SaveChanges(db);
            }
        }
        #endregion

        #region Users (Клиенты)
        public bool AddUser(User user)
        {
            using (var db = new FitnessDbContext())
            {
                db.Users.Add(user);
                return SaveChanges(db);
            }
        }

        public User GetUser(string username)
        {
            using (var db = new FitnessDbContext())
                return db.Users.Find(username);
        }

        public List<User> GetAllUsers()
        {
            using (var db = new FitnessDbContext())
                return db.Users.ToList();
        }

        public bool UpdateUser(User user)
        {
            using (var db = new FitnessDbContext())
            {
                db.Entry(user).State = EntityState.Modified;
                return SaveChanges(db);
            }
        }

        public bool DeleteUser(string username)
        {
            using (var db = new FitnessDbContext())
            {
                var item = db.Users.Find(username);
                if (item == null) return false;
                db.Users.Remove(item);
                return SaveChanges(db);
            }
        }
        #endregion

        #region Admins (Администраторы)
        public bool AddAdmin(Admin admin)
        {
            using (var db = new FitnessDbContext())
            {
                db.Admins.Add(admin);
                return SaveChanges(db);
            }
        }

        public Admin GetAdmin(int adminId)
        {
            using (var db = new FitnessDbContext())
                return db.Admins.Find(adminId);
        }

        public List<Admin> GetAllAdmins()
        {
            using (var db = new FitnessDbContext())
                return db.Admins.ToList();
        }

        public bool UpdateAdmin(Admin admin)
        {
            using (var db = new FitnessDbContext())
            {
                db.Entry(admin).State = EntityState.Modified;
                return SaveChanges(db);
            }
        }

        public bool DeleteAdmin(int adminId)
        {
            using (var db = new FitnessDbContext())
            {
                var item = db.Admins.Find(adminId);
                if (item == null) return false;
                db.Admins.Remove(item);
                return SaveChanges(db);
            }
        }
        #endregion

        #region Trainers (Тренеры)
        public bool AddTrainer(Trainer trainer)
        {
            using (var db = new FitnessDbContext())
            {
                db.Trainers.Add(trainer);
                return SaveChanges(db);
            }
        }

        public Trainer GetTrainer(int trainerId)
        {
            using (var db = new FitnessDbContext())
                return db.Trainers.Find(trainerId);
        }

        public List<Trainer> GetAllTrainers()
        {
            using (var db = new FitnessDbContext())
                return db.Trainers.ToList();
        }

        public bool UpdateTrainer(Trainer trainer)
        {
            using (var db = new FitnessDbContext())
            {
                db.Entry(trainer).State = EntityState.Modified;
                return SaveChanges(db);
            }
        }

        public bool DeleteTrainer(int trainerId)
        {
            using (var db = new FitnessDbContext())
            {
                var item = db.Trainers.Find(trainerId);
                if (item == null) return false;
                db.Trainers.Remove(item);
                return SaveChanges(db);
            }
        }
        #endregion

        #region TrainerSchedules (Расписания)
        public bool AddSchedule(TrainerSchedule schedule)
        {
            using (var db = new FitnessDbContext())
            {
                db.TrainerSchedules.Add(schedule);
                return SaveChanges(db);
            }
        }

        public List<TrainerSchedule> GetSchedulesByTrainer(int trainerId)
        {
            using (var db = new FitnessDbContext())
                return db.TrainerSchedules.Where(s => s.TrainerId == trainerId).ToList();
        }

        public bool UpdateSchedule(TrainerSchedule schedule)
        {
            using (var db = new FitnessDbContext())
            {
                db.Entry(schedule).State = EntityState.Modified;
                return SaveChanges(db);
            }
        }

        public bool DeleteSchedule(int scheduleId)
        {
            using (var db = new FitnessDbContext())
            {
                var item = db.TrainerSchedules.Find(scheduleId);
                if (item == null) return false;
                db.TrainerSchedules.Remove(item);
                return SaveChanges(db);
            }
        }
        #endregion

        #region Services (Услуги)
        public bool AddService(Service service)
        {
            using (var db = new FitnessDbContext())
            {
                db.Services.Add(service);
                return SaveChanges(db);
            }
        }

        public Service GetService(string name)
        {
            using (var db = new FitnessDbContext())
                return db.Services.Find(name);
        }

        public List<Service> GetAllServices()
        {
            using (var db = new FitnessDbContext())
                return db.Services.ToList();
        }

        public bool UpdateService(Service service)
        {
            using (var db = new FitnessDbContext())
            {
                db.Entry(service).State = EntityState.Modified;
                return SaveChanges(db);
            }
        }

        public bool DeleteService(string name)
        {
            using (var db = new FitnessDbContext())
            {
                var item = db.Services.Find(name);
                if (item == null) return false;
                db.Services.Remove(item);
                return SaveChanges(db);
            }
        }
        #endregion

        #region Events (Занятия)
        public bool AddEvent(Event ev)
        {
            using (var db = new FitnessDbContext())
            {
                db.Events.Add(ev);
                return SaveChanges(db);
            }
        }

        public Event GetEvent(int eventId)
        {
            using (var db = new FitnessDbContext())
                return db.Events.Find(eventId);
        }

        public List<Event> GetAllEvents()
        {
            using (var db = new FitnessDbContext())
                return db.Events.ToList();
        }

        public List<Event> GetEventsByDate(DateTime date)
        {
            using (var db = new FitnessDbContext())
            {
                // Фильтруем события по дате (без учета времени)
                return db.Events.Where(e => DbFunctions.TruncateTime(e.FromDate) == date.Date).ToList();
            }
        }

        public bool UpdateEvent(Event ev)
        {
            using (var db = new FitnessDbContext())
            {
                db.Entry(ev).State = EntityState.Modified;
                return SaveChanges(db);
            }
        }

        public bool DeleteEvent(int eventId)
        {
            using (var db = new FitnessDbContext())
            {
                var item = db.Events.Find(eventId);
                if (item == null) return false;
                db.Events.Remove(item);
                return SaveChanges(db);
            }
        }
        #endregion

        #region Bookings (Записи)
        public bool AddBooking(Booking booking)
        {
            using (var db = new FitnessDbContext())
            {
                db.Bookings.Add(booking);
                return SaveChanges(db);
            }
        }

        public Booking GetBooking(int bookingId)
        {
            using (var db = new FitnessDbContext())
                return db.Bookings.Find(bookingId);
        }

        public List<Booking> GetBookingsByUser(string username)
        {
            using (var db = new FitnessDbContext())
                return db.Bookings.Where(b => b.Username == username).ToList();
        }

        public List<Booking> GetBookingsByEvent(int eventId)
        {
            using (var db = new FitnessDbContext())
                return db.Bookings.Where(b => b.EventId == eventId).ToList();
        }

        public bool UpdateBooking(Booking booking)
        {
            using (var db = new FitnessDbContext())
            {
                db.Entry(booking).State = EntityState.Modified;
                return SaveChanges(db);
            }
        }

        public bool DeleteBooking(int bookingId)
        {
            using (var db = new FitnessDbContext())
            {
                var item = db.Bookings.Find(bookingId);
                if (item == null) return false;
                db.Bookings.Remove(item);
                return SaveChanges(db);
            }
        }
        #endregion
    }
}