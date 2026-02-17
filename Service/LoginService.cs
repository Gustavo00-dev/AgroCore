using System.Text;
using System.Text.Json;
using APIAgroCoreOrquestradora.Model;

namespace APIAgroCoreOrquestradora.Service
{
    public interface ILoginService
    {
        Task<bool> Authenticate(LoginRequestModel request);
    }

    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Authenticate(LoginRequestModel request)
        {
            try
            {
                var url = "http://agrocorelogin/api/Login/login";
                //var url = "http://localhost/api/LoginUser/login";
                var json = JsonSerializer.Serialize(request);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na autenticação: {ex.Message}");
                return false;
            }
            finally
            {
                Console.WriteLine("Autenticação finalizada.");
            }
        }
    }
}
