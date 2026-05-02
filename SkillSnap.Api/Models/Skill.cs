using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Api.Models
{
    public class Skill
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; }

        public string Level { get; set; }

        // This now stores the ApplicationUser.Id (string GUID)
        public string? PortfolioUserId { get; set; }
    }
}
