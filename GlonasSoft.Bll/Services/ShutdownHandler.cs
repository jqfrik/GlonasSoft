using System.Collections;
using System.Reflection;
using GlonasSoft.Bll.Domains;
using GlonasSoft.Dal;
using GlonasSoft.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace GlonasSoft.Bll.Services;

public class ShutdownHandler
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IMemoryCache _memoryCache;
        private readonly IDbContextFactory<GlonasContext> _dbContextFactory;

        public ShutdownHandler(
            IHostApplicationLifetime hostApplicationLifetime,
            IMemoryCache memoryCache,
            IDbContextFactory<GlonasContext> dbContextFactory)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _memoryCache = memoryCache;
            _dbContextFactory = dbContextFactory;
            _hostApplicationLifetime.ApplicationStopping.Register(async (c) =>
            {
                var keys = GetRequestIds();

                await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

                foreach (var key in keys)
                {
                    if (_memoryCache.TryGetValue(key, out UserRequest value))
                    {
                        dbContext.UserRequests.Update(new UserRequestDal()
                        {
                            Percent = value.Percent,
                            Query = value.Query,
                            Result = value.Result != null ? new ResultDal()
                            {
                                User_Id = value.Result.User_Id,
                                Count_Sign_In = value.Result.Count_Sign_In
                            } : new()
                        });
                    }
                }

                await dbContext.SaveChangesAsync();
            }, false);
        }

        private Guid[] GetRequestIds()
        {
            var coherent =
                _memoryCache.GetType().GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .GetValue(_memoryCache);
            var field = coherent.GetType()
                .GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var collection = field.GetValue(coherent) as ICollection;
            var items = new List<Guid>();
            if (collection != null)
                foreach (var item in collection)
                {
                    var methodInfo = item.GetType().GetProperty("Key");
                    var val = methodInfo.GetValue(item);
                    items.Add((Guid)val);
                }

            return items.ToArray();
        }
    }