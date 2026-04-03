using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessClub.DomainService.Models
{
    [DataContract]
    [Table("services")]
    public class Service
    {
        [DataMember]
        [Key]
        [Column("name")]
        public string Name { get; set; }

        [DataMember]
        [Column("price")]
        public int Price { get; set; }

        [DataMember]
        [Column("description")]
        public string Description { get; set; }
    }
}