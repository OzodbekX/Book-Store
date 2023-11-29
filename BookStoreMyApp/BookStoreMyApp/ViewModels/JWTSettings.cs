namespace BookStoreMyApp.Models
{
    public class JWTSettings
    {
        public string SecretKey { get; set; } 
        public string ValidIssuer { get; set; } 
        public string Audience { get; set; } 
    }
}
