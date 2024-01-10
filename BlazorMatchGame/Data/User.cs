using System.ComponentModel.DataAnnotations;

namespace BlazorMatchGame.Data
{
    public class User
    {
        [Required]
        [MinLength(3, ErrorMessage = "Username must be atleast 3 characters long")]
        [MaxLength(16, ErrorMessage = "Username must not exceed 16 characters long")]
        [RegularExpression(@"([a-zA-Z0-9]+)+", ErrorMessage = "Username must be alpha-numeric")]
        public string Username { get; set; }

        public bool IsGuest { get; set; } = false;

        public int ID { get; set; }
        public int Score { get; set; }

    }
}
