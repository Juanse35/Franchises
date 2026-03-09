// This file defines the Franchise model which represents the franchise entity in the database
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// the namespace for the franchise model, it will be used to organize the code and avoid naming conflicts 
namespace franchise.Models
{
    [Table("tbl_franchise")]
    public class Franchise
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}