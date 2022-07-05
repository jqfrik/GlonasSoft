using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GlonasSoft.Dal;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDal(this IServiceCollection collection, string connectionString)
    {
        collection.AddDbContextFactory<GlonasContext>();
        return collection;
    }
}