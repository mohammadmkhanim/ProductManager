using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ProductDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is requierd.")]
        [MaxLength(40, ErrorMessage = "Max length of {0} should be {1}.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is requierd.")]
        public DateTime ProduceDate { get; set; }

        [Required(ErrorMessage = "{0} is requierd.")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "Enter the phone correctly.")]
        public string ManufacturePhone { get; set; }

        [Required(ErrorMessage = "{0} is requierd.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Enter the email correctly.")]
        public string ManufactureEmail { get; set; }

        [Required(ErrorMessage = "{0} is requierd.")]
        public bool IsAvailable { get; set; }
    }
}