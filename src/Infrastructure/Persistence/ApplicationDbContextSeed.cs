namespace Carpool.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{

    public static async Task SeedDefaultDatabaseAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        await SeedDefaultUserAsync(userManager);
        await SeedSampleEventAsync(context);
    }

    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var users = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        UserName = "bob",
                        Email = "bob@test.com",
                        EmailConfirmed = true,
                    },
                    new ApplicationUser
                    {
                        UserName = "tom",
                        Email = "tom@test.com",
                        EmailConfirmed = true,
                    },
                };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$word13");
            }
        }
    }


    public static async Task SeedSampleEventAsync(ApplicationDbContext context)
    {
        //        var Photos =
        if (!context.Events.Any())
        {
            var events = new List<Event>{
                new Event{
                Name="Christmas Party",
                Date = new DateTime(2021,12,09,18,00,00),
                Photo= new Photo{
                    Id="kr4bl7jjsgomqgvctyiz",
                    Url="https://res.cloudinary.com/reactivities404found/image/upload/v1637676997/kr4bl7jjsgomqgvctyiz.jpg"
                    }
                },
                new Event{
                Name="Poker",
                Date = new DateTime(2021,12,09,18,00,00),
                Photo= new Photo{
                    Id="rqxy0dorqjsgeoxiixos",
                    Url="https://res.cloudinary.com/reactivities404found/image/upload/v1637678895/rqxy0dorqjsgeoxiixos.jpg"
                    }
                },
                new Event{
                Name="Party",
                Date = new DateTime(2021,12,09,18,00,00),
                Photo= new Photo{
                    Id="aafyl3bybgheacc8nuan	",
                    Url="https://res.cloudinary.com/reactivities404found/image/upload/v1637678974/aafyl3bybgheacc8nuan.jpg"
                    }
                },
                new Event{
                Name="Easter Egg Party",
                Date = new DateTime(2021,12,09,18,00,00),
                Photo= new Photo{
                    Id="rznc2xcxezem1jf902sq",
                    Url="https://res.cloudinary.com/reactivities404found/image/upload/v1637679021/rznc2xcxezem1jf902sq.jpg"
                    }
                },
            };
            await context.Events.AddRangeAsync(events);
            await context.SaveChangesAsync();
        }
    }
}