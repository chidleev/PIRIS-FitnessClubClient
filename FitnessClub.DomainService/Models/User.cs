using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessClub.DomainService.Models
{
    [DataContract]
    [Table("users")]
    public class User
    {
        [DataMember]
        [Key]
        [Column("username")]
        public string Username { get; set; }

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