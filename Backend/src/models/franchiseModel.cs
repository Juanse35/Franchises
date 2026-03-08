using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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