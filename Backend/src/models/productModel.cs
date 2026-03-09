// This file defines the Product model which represents the structure of the product data in the database
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// The Product class represents a product entity in the application, 
//it contains properties that correspond to the columns in the tbl_product table in the database
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

        [Column("imag_url")]
        public string Imag_url { get; set; }

        [Required]
        [Column("branch_id")]
        public int BranchId { get; set; }
    }
}
