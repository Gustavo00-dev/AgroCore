using APIAgroCoreOrquestradora.Model;

namespace APIAgroCoreOrquestradora.Service
{
    public interface ILoginService
    {
        bool Authenticate(LoginRequestModel request);
    }
    public class LoginService : ILoginService
    {
        public LoginService() { }

        public bool Authenticate(LoginRequestModel request)
        {
            try
            {

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
