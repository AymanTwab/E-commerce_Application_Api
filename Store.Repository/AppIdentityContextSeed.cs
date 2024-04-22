using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;

namespace Store.Repository
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Ahmed Samy",
                    Email = "ahmedsamy@gmail.com",
                    UserName = "ahmedsamy",
                    Address = new Address
                    {
                        FirstName = "ahmed",
                        LastName = "samy",
                        Street="1",
                        City = "Dokki",
                        State = "Cairo",
                        ZipCode = "123456"
                    }
                };

                await userManager.CreateAsync(user,"AdminPass123!");

            }
        }
    }
}
