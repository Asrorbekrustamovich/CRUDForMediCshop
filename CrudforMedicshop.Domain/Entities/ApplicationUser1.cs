using Microsoft.AspNetCore.Identity;
namespace CrudforMedicshop.Domain.Entities
{
    public class ApplicationUser1:IdentityUser
    {  public int Id { get; set; } 
       public string FirstName { get; set; }
       public string LastName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
