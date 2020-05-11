using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserLoginDTO
    {
        public int Id {get;set;}
        public string Username { get; set; }
        public string Password { get; set; }
    }
}