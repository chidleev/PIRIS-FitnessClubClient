using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessClub.DomainService.Models
{
    [DataContract]
    [Table("trainer_schedules")]
    public class TrainerSchedule
    {
        [DataMember]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [DataMember]
        [Column("trainer_id")]
        public int TrainerId { get; set; }

        [DataMember]
        [Column("day")]
        public int Day { get; set; }

        [DataMember]
        [Column("from")]
        public TimeSpan FromTime { get; set; }

        [DataMember]
        [Column("to")]
        public TimeSpan ToTime { get; set; }
    }
}