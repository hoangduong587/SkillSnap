using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Client.Dtos
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
