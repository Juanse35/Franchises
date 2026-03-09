// This file defines the Branch model which represents the structure of the branch table in the database
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// namespace for the branch model, it contains the properties that represent the columns of the tbl_branch table in the database
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
    }
}