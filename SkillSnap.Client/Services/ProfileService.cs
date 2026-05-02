using System.Net.Http.Json;
using SkillSnap.Shared;
using SkillSnap.Client.Services.State;

namespace SkillSnap.Client.Services
{
    public class ProfileService
    {
        private readonly HttpClient _http;
        private readonly AppState _state;

        public ProfileService(HttpClient http, AppState state)
        {
            _http = http;
            _state = state;
        }

        public async Task<PortfolioUserDto?> GetProfileAsync()
        {
            // If profile already loaded, return it
            if (_state.Profile != null)
                return _state.Profile;

            // Otherwise fetch from API
            var profile = await _http.GetFromJsonAsync<PortfolioUserDto>("api/profile");

            // Store in AppState
            _state.SetProfile(profile!);

            return profile;
        }
    }
}
