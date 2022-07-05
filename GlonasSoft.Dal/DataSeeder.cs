using GlonasSoft.Dal.Entities;

namespace GlonasSoft.Dal;

public static class DataSeeder
{
    public static async Task Seed(GlonasContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.Add(new UserDal()
            {
                Id = new Guid("a7a5c571-e5a4-4c6a-9ede-6cfc24d85efc"),
                Sign_In_Count = 0
            });
            await context.SaveChangesAsync(CancellationToken.None);
        }
    }
}