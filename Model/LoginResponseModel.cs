namespace APIAgroCoreOrquestradora.Model
{
    public class LoginResponseModel
    {
        public string Token { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
    }
}
