using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessClub.DomainService.Models
{
    [DataContract]
    [Table("bookings")]
    public class Booking
    {
        [DataMember]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [DataMember]
        [Column("event")]
        public int EventId { get; set; }

        [DataMember]
        [Column("user")]
        public string Username { get; set; }

        [DataMember]
        [Column("status")]
        public string Status { get; set; }

        [DataMember]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}