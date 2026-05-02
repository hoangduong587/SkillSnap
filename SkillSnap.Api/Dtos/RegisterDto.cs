namespace SkillSnap.Api.Dtos
{
    public class RegisterDto
    {
        public string Name { get; set; }      // PortfolioUser.Name
        public string Email { get; set; }     // Identity Email + Username
        public string Password { get; set; }  // Identity password
    }
}
