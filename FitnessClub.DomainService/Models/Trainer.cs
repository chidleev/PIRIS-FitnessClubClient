using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessClub.DomainService.Models
{
    [DataContract]
    [Table("trainers")]
    public class Trainer
    {
        [DataMember]
        [Key]
        [Column("trainer_id")]
        public int TrainerId { get; set; }

        [DataMember]
        [Column("login")]
        public string Login { get; set; }

        [DataMember]
        [Column("created_by")]
        public int CreatedBy { get; set; }

        [DataMember]
        [Column("name")]
        public string Name { get; set; }

        [DataMember]
        [Column("lastname")]
        public string Lastname { get; set; }
    }
}