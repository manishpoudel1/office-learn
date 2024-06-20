using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curdoperation.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("FirstName")]
        [Required]
        public string? FirstName { get; set; }

        [DisplayName("LastName")]
        [Required]
        public string? LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public double Salary { get; set; }
        [Required]

        [NotMapped]
        public string FullName
        {
            get { return FirstName + " " + LastName; }

        }
    }
}
