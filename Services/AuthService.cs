using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Rentbook.DTOs;
using Rentbook.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace Rentbook.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;

        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, ILogger<AuthService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _logger = logger;
            _configuration = configuration;
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            _logger.LogInformation($"API Base Address from configuration: {apiBaseUrl}");
            _logger.LogInformation($"API Base Address from HttpClient: {_httpClient.BaseAddress}");
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            try
            {
                string apiBaseUrl = _configuration["ApiBaseUrl"];
                string loginUrl = $"{apiBaseUrl}api/auth/login";

                var response = await _httpClient.PostAsJsonAsync(loginUrl, loginModel);
                if (response.IsSuccessStatusCode)
                {
                    var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
                    if (loginResult != null && !string.IsNullOrEmpty(loginResult.Token))
                    {
                        await _localStorage.SetItemAsync("authToken", loginResult.Token);
                        ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(loginResult.Token);
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);
                        return loginResult;
                    }
                }
                return new LoginResult { Token = null, Roles = null };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login");
                return new LoginResult { Token = null, Roles = null };
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity.IsAuthenticated;
        }

        public async Task<RegisterResult> Register(RegisterModel registerModel)
        {
            try
            {
                string apiBaseUrl = _configuration["ApiBaseUrl"];
                if (string.IsNullOrEmpty(apiBaseUrl))
                {
                    _logger.LogError("API Base URL is not configured");
                    return new RegisterResult { Success = false, Message = "Server configuration error" };
                }

                string registerUrl = $"{apiBaseUrl}api/auth/register";
                var response = await _httpClient.PostAsJsonAsync(registerUrl, registerModel);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("User registered successfully");
                    return new RegisterResult { Success = true, Message = "Registration successful" };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Registration failed. Status: {response.StatusCode}, Content: {errorContent}");
                    return new RegisterResult
                    {
                        Success = false,
                        Message = "Registration failed. Please try again.",
                        Errors = new List<string> { errorContent }
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration");
                return new RegisterResult
                {
                    Success = false,
                    Message = "An error occurred during registration. Please try again.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<VerifyOtpResult> VerifyOtp(VerifyOtpModel model)
        {
            try
            {
                string apiBaseUrl = _configuration["ApiBaseUrl"];
                string verifyOtpUrl = $"{apiBaseUrl}api/auth/verify-email";

                var response = await _httpClient.PostAsJsonAsync(verifyOtpUrl, model);
                if (response.IsSuccessStatusCode)
                {
                    var verifyOtpResult = await response.Content.ReadFromJsonAsync<VerifyOtpResult>();
                    if (verifyOtpResult != null && !string.IsNullOrEmpty(verifyOtpResult.Token))
                    {
                        // Store the token and roles in local storage
                        await _localStorage.SetItemAsync("authToken", verifyOtpResult.Token);
                        await _localStorage.SetItemAsync("userRoles", verifyOtpResult.Roles);

                        // Update the authentication state
                        ((ApiAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(verifyOtpResult.Token);
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", verifyOtpResult.Token);

                        return verifyOtpResult;
                    }
                }
                return new VerifyOtpResult { Token = null, Roles = null };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during OTP verification");
                return new VerifyOtpResult { Token = null, Roles = null };
            }
        }

        public async Task<bool> ResendOtp(string email)
        {
            try
            {
                string apiBaseUrl = _configuration["ApiBaseUrl"];
                string resendOtpUrl = $"{apiBaseUrl}api/auth/request-new-verification-code";

                var resendModel = new { Email = email };
                var response = await _httpClient.PostAsJsonAsync(resendOtpUrl, resendModel);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"OTP resent successfully to {email}");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Failed to resend OTP. Status: {response.StatusCode}, Content: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while resending OTP to {email}");
                return false;
            }
        }



    }
}