using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Api.Models;

    public class PortfolioUser
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }

        public List<Project> Projects { get; set; }

        public List<Skill> Skills { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
