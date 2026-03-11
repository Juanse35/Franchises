// This file defines the Branch model which represents the structure of the branch table in the database
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace branch.Models
{
    [Table("tbl_branch")]
    public class Branch
    {
        [Key]
        [Column("id_branch")]
        public int Id_branch { get; set; }

        [Required]
        [Column("name_branch")]
        public string Name_branch { get; set; }

        [Required]
        [Column("franchise_id")]
        public int FranchiseId { get; set; }

        [Column("registration_date")]
        public DateTime RegistrationDate { get; set; }

        // Campo que viene del JOIN con tbl_franchise
        [NotMapped]
        public string FranchiseName { get; set; }
    }
}