using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessClub.DomainService.Models
{
    [DataContract]
    [Table("credentials")]
    public class Credential
    {
        [DataMember]
        [Key]
        [Column("login")]
        public string Login { get; set; }

        [DataMember]
        [Column("password")]
        public string Password { get; set; }

        [DataMember]
        [Column("telephone")]
        public string Telephone { get; set; }

        [DataMember]
        [Column("email")]
        public string Email { get; set; }
    }
}