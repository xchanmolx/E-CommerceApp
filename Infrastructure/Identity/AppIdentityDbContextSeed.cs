using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Chan",
                    Email = "cmanigos@gmail.com",
                    UserName = "cmanigos@gmail.com",
                    Address = new Address
                    {
                        FirstName = "Chiantine",
                        LastName = "Manigos",
                        Street = "Purok 7, Tagaytay Bojo",
                        City = "Aloguinsan",
                        State = "Alog",
                        Zipcode = "6040"
                    }
                };

                await userManager.CreateAsync(user, "Free232469*");
            }
        }
    }
}