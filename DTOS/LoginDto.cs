using System.ComponentModel.DataAnnotations;

namespace CurrencyConvertorAPI.DTOS
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
