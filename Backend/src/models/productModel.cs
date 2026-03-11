using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace product.Models
{
    [Table("tbl_product")]
    public class Product
    {
        [Key]
        [Column("id_product")]
        public int Id_product { get; set; }

        [Required]
        [Column("name_product")]
        public string Name_product { get; set; }

        [Required]
        [Column("branch_id")]
        public int BranchId { get; set; }

        [Required]
        [Column("stock")]
        public int Stock { get; set; }

    
        [NotMapped]
        public string BranchName { get; set; }

        [NotMapped]
        public string FranchiseName { get; set; }
    }
}