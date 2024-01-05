using System.ComponentModel.DataAnnotations;

namespace Test_API.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string gender { get; set; }
        public bool IsActive { get; set; }
    }
}
