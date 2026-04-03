using System.Data.Entity;
using FitnessClub.DomainService.Models;

namespace FitnessClub.DomainService
{
    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class FitnessDbContext : DbContext
    {
        public FitnessDbContext() : base("name=MySqlConnectionString") { }

        public DbSet<Credential> Credentials { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainerSchedule> TrainerSchedules { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}