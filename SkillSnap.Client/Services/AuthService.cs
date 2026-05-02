using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using SkillSnap.Shared;
using SkillSnap.Client.Dtos;
using SkillSnap.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;


namespace SkillSnap.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        private const string TokenKey = "authToken";

        public AuthService(
            HttpClient http,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        // ----------------------------------------------------
        // REGISTER (returns token)
        // ----------------------------------------------------
        public async Task<string?> Register(RegisterDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", dto);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            await SaveToken(result.Token);

            return result.Token;
        }

        // ----------------------------------------------------
        // LOGIN (returns token)
        // ----------------------------------------------------
        public async Task<string?> Login(LoginDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            await SaveToken(result.Token);

            return result.Token;
        }

        // ----------------------------------------------------
        // LOGOUT
        // ----------------------------------------------------
        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(TokenKey);
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
            _http.DefaultRequestHeaders.Authorization = null;
        }

        // ----------------------------------------------------
        // SAVE TOKEN + UPDATE AUTH STATE
        // ----------------------------------------------------
        private async Task SaveToken(string token)
        {
            await _localStorage.SetItemAsync(TokenKey, token);

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(token);
        }

        // ----------------------------------------------------
        // GET TOKEN
        // ----------------------------------------------------
        public async Task<string?> GetToken()
        {
            return await _localStorage.GetItemAsync<string>(TokenKey);
        }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
