using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class OAuthRepo : IAuthRepo
    {

        DataContext context;
         public OAuthRepo(DataContext context)
        {
            this.context = context;
        }

        public async Task<User> Login(string username, string password)
        {
           var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);

           if(user == null)
           {
                return null;
           }

           if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
           {
               return user;
           }

           return null;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //Verify users hashed passwords matched.
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < ComputeHash.Length; i++)
                {
                    if(ComputeHash[i] != passwordHash[i]) return false;
                }

                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await context.Users.AddAsync(user);//Create new user
            await context.SaveChangesAsync();//Save to DB

            return user;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            //Encryupt users password
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExsist(string username)
        {

            //Check if user already exists with same username
            if(await context.Users.AnyAsync(x => x.Username == username))
            {
                return true;
            }

            return false;
        }
    }
}