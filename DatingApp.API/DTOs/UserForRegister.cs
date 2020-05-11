using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForRegister
    {
        [Required]
        //[StringLength(6, MinimumLength = 6, ErrorMessage = "Username can not be empty. Have to be 6 or more characters.")]
        public string Username{get;set;}

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage="Password mut be a min of 4 characters and max 8.")]
        public string Password{get;set;}
    }
}