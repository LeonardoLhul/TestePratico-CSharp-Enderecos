using System.Text.Json;
using TesteEndereco.Web.Models;

namespace TesteEndereco.Web.Services
{
    public class ViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ViaCepResponse?> BuscarEnderecoPorCepAsync(string cep)
        {
            cep = new string(cep.Where(char.IsDigit).ToArray());

            if (cep.Length != 8)
            {
                return null;
            }

            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ViaCepResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
