namespace Rentbook.Models
{
    public class LoginResult
    {
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
