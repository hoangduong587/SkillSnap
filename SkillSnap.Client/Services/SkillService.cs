using System.Net.Http.Json;
using SkillSnap.Shared;
using SkillSnap.Client.Services.State;

namespace SkillSnap.Client.Services
{
    public class SkillService
    {
        private readonly HttpClient _http;
        private readonly AppState _state;

        public SkillService(HttpClient http, AppState state)
        {
            _http = http;
            _state = state;
        }

        // GET: api/skills (with client-side state)
        public async Task<List<SkillDto>> GetSkillsAsync()
        {
            // If already loaded, return from AppState (client-side cache)
            if (_state.Skills != null)
                return _state.Skills;

            // Otherwise fetch from API
            var result = await _http.GetFromJsonAsync<List<SkillDto>>("api/skills");

            // Store in AppState
            _state.SetSkills(result ?? new List<SkillDto>());

            return _state.Skills!;
        }

        // POST: api/skills (auto-refresh state)
        public async Task<SkillDto?> AddSkillAsync(SkillDto newSkill)
        {
            var response = await _http.PostAsJsonAsync("api/skills", newSkill);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to add skill: {error}");
            }

            var created = await response.Content.ReadFromJsonAsync<SkillDto>();

            // Refresh skill list from API
            var updatedList = await _http.GetFromJsonAsync<List<SkillDto>>("api/skills");

            // Update AppState
            _state.SetSkills(updatedList ?? new List<SkillDto>());

            return created;
        }
    }
}
