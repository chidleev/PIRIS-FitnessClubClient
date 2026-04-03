using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessClub.DomainService.Models
{
    [DataContract]
    [Table("events")]
    public class Event
    {
        [DataMember]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [DataMember]
        [Column("service")]
        public string ServiceName { get; set; }

        [DataMember]
        [Column("trainer_id")]
        public int TrainerId { get; set; }

        [DataMember]
        [Column("max_participants")]
        public int MaxParticipants { get; set; }

        [DataMember]
        [Column("from")]
        public DateTime FromDate { get; set; }

        [DataMember]
        [Column("duration")]
        public int Duration { get; set; }

        [DataMember]
        [Column("status")]
        public string Status { get; set; }

        [DataMember]
        [Column("repeatable")]
        public bool Repeatable { get; set; }

        [DataMember]
        [Column("period")]
        public int? Period { get; set; }
    }
}