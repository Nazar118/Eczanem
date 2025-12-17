namespace Eczanem.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty; // Kullanıcı Adı
        public string Email { get; set; } = string.Empty; 
        public string Password { get; set; } = string.Empty; // Şifre
        public string Name { get; set; } = string.Empty;     // Görünecek İsim 
        public string Role { get; set; } = string.Empty;     // Rolü 
    }
}