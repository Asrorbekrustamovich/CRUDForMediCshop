using Microsoft.AspNetCore.Identity;
namespace CrudforMedicshop.Domain.Entities
{
    public class ApplicationUser1
    {  public int Id { get; set; } 
        public string username { get; set; }
        public string password { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
