using System.Text;
using System.Text.Json;
using APIAgroCoreOrquestradora.Model;

namespace APIAgroCoreOrquestradora.Service
{
    public interface IUserService
    {
        Task<bool> Authenticate(BasicUserRequestModel request);
        Task<bool> CreateUser(BasicUserRequestModel request);
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Authenticate(BasicUserRequestModel request)
        {
            try
            {
                var url = "http://agrocorelogin/api/User/login";
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

        public async Task<bool> CreateUser(BasicUserRequestModel request)
        {
            try
            {
                var url = "http://agrocorelogin/api/User/create";
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
                Console.WriteLine($"Erro na criação de usuário: {ex.Message}");
                return false;
            }
            finally
            {
                Console.WriteLine("Criação de usuário finalizada.");
            }
        }
    }
}
