using SkillSnap.Shared;


namespace SkillSnap.Client.Services.State;

public class AppState
{
    public List<ProjectDto>? Projects { get; private set; }
    public List<SkillDto>? Skills { get; private set; }
    public PortfolioUserDto? Profile { get; private set; }

    public event Action? OnChange;

    public void SetProjects(List<ProjectDto> projects)
    {
        Projects = projects;
        NotifyStateChanged();
    }

    public void SetSkills(List<SkillDto> skills)
    {
        Skills = skills;
        NotifyStateChanged();
    }

    public void SetProfile(PortfolioUserDto profile)
    {
        Profile = profile;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
