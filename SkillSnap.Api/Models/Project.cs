using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Api.Models
{
    public class Project
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        // This now stores the ApplicationUser.Id (string GUID)
        public string? PortfolioUserId { get; set; }
    }
}
