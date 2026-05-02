using System.Net.Http.Json;
using SkillSnap.Shared;
using SkillSnap.Client.Services.State;

namespace SkillSnap.Client.Services
{
    public class ProjectService
    {
        private readonly HttpClient _http;
        private readonly AppState _state;

        public ProjectService(HttpClient http, AppState state)
        {
            _http = http;
            _state = state;
        }

        // GET: api/projects (with client-side state)
        public async Task<List<ProjectDto>> GetProjectsAsync()
        {
            // If already loaded, return from AppState (client-side cache)
            if (_state.Projects != null)
                return _state.Projects;

            // Otherwise fetch from API
            var result = await _http.GetFromJsonAsync<List<ProjectDto>>("api/projects");

            // Store in AppState
            _state.SetProjects(result ?? new List<ProjectDto>());

            return _state.Projects!;
        }

        // POST: api/projects (auto-refresh state)
        public async Task<ProjectDto?> AddProjectAsync(ProjectDto newProject)
        {
            var response = await _http.PostAsJsonAsync("api/projects", newProject);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to add project: {error}");
            }

            var created = await response.Content.ReadFromJsonAsync<ProjectDto>();

            // Refresh project list from API
            var updatedList = await _http.GetFromJsonAsync<List<ProjectDto>>("api/projects");

            // Update AppState
            _state.SetProjects(updatedList ?? new List<ProjectDto>());

            return created;
        }
    }
}
