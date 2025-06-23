using AgendaApp.BlazorWasm.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AgendaApp.BlazorWasm.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public ApiService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private async Task SetAuthHeaderAsync()
        {
            var token = await _authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // Categories
        public async Task<List<CategoryModel>> GetCategoriesAsync()
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.GetFromJsonAsync<List<CategoryModel>>("api/categories");
            return response ?? new List<CategoryModel>();
        }

        public async Task<CategoryModel?> CreateCategoryAsync(CategoryModel category)
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync("api/categories", category);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoryModel>();
            }
            return null;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryModel category)
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/categories/{category.Id}", category);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.DeleteAsync($"api/categories/{categoryId}");
            return response.IsSuccessStatusCode;
        }

        // Appointments
        public async Task<List<AppointmentModel>> GetAppointmentsAsync(DateTime startDate, DateTime endDate)
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.GetFromJsonAsync<List<AppointmentModel>>(
                $"api/appointments?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response ?? new List<AppointmentModel>();
        }

        public async Task<AppointmentModel?> CreateAppointmentAsync(AppointmentModel appointment)
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync("api/appointments", appointment);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AppointmentModel>();
            }
            return null;
        }

        public async Task<bool> UpdateAppointmentAsync(AppointmentModel appointment)
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/appointments/{appointment.Id}", appointment);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.DeleteAsync($"api/appointments/{appointmentId}");
            return response.IsSuccessStatusCode;
        }
    }
}
