using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessClub.DomainService.Models
{
    [DataContract]
    [Table("admins")]
    public class Admin
    {
        [DataMember]
        [Key]
        [Column("idmin_id")]
        public int AdminId { get; set; }

        [DataMember]
        [Column("login")]
        public string Login { get; set; }

        [DataMember]
        [Column("name")]
        public string Name { get; set; }

        [DataMember]
        [Column("lastname")]
        public string Lastname { get; set; }
    }
}