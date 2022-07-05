using GlonasSoft.Bll.Domains;
using GlonasSoft.Bll.Dtos;
using GlonasSoft.Dal;
using GlonasSoft.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GlonasSoft.Bll.Services.UserRequestService;

public class UserRequestService : IUserRequestService
{
    private readonly ProcessorQueue _queue;
    private GlonasContext _context { get; set; }
    private IConfiguration _configuration { get; }
    private IMemoryCache _cache { get; }

    private IHostApplicationLifetime _hostLifetime { get; }

    private UserRequest _currentUserRequest { get; set; }

    public UserRequestService(IConfiguration configuration, IMemoryCache cache, IHostApplicationLifetime hostLifetime,
        GlonasContext context, ProcessorQueue queue, IDbContextFactory<GlonasContext> contextFactory)
    {
        _queue = queue;
        _context = context;
        _configuration = configuration;
        _cache = cache;
        _hostLifetime = hostLifetime;
    }

    public async Task<Guid> GetUserInfo(GetUserInfoRequestDto data, CancellationToken token)
    {
        var userRequest = new UserRequest
        {
            Percent = 0
        };
        _currentUserRequest = userRequest;
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == data.UserId,token);
        if (user == null)
        {
            throw new ArgumentException("Incorrect User Id");
        }

        ++user.Sign_In_Count;
        var func = ProcessUserRequest("60", 0, userRequest, user);
        _queue.Tasks.Enqueue(func);
        await _context.SaveChangesAsync(CancellationToken.None);
        
        return userRequest.Query;
    }

    private Func<GlonasContext,Task> ProcessUserRequest(string countTimeProcessing, int count,
        UserRequest userRequest, UserDal user)
    {
        return context =>
        {
            Task.Run(async () =>
            {
                if (Int32.TryParse(countTimeProcessing, out var countTimeInteger))
                {
                    var incrementForPercent = (double)Decimal.Divide((decimal)100, (decimal)countTimeInteger);
                    var dbRequest =
                        context.UserRequests.FirstOrDefault(req => req.Query == _currentUserRequest.Query);
                    while (count <= countTimeInteger)
                    {
                        if (count == countTimeInteger)
                        {
                            if (dbRequest != null)
                            {
                                dbRequest.Percent = 100;
                                dbRequest.Result = new ResultDal()
                                {
                                    User_Id = user.Id,
                                    Count_Sign_In = user.Sign_In_Count
                                };
                            }
                            else
                            {
                                var newUserRequest = new UserRequestDal()
                                {
                                    Percent = 100,
                                    Query = userRequest.Query,
                                    Result = new ResultDal()
                                    {
                                        User_Id = user.Id,
                                        Count_Sign_In = user.Sign_In_Count
                                    }
                                };
                                context.UserRequests.Add(newUserRequest);
                            }

                            _cache.Remove(_currentUserRequest.Query);
                            await context.SaveChangesAsync(CancellationToken.None);


                            break;
                        }

                        //Upd in cache
                        var cacheGetRequestResult = _cache.TryGetValue(userRequest.Query, out UserRequest request);
                        if (!cacheGetRequestResult)
                        {
                            _cache.Set(userRequest.Query, userRequest);
                        }
                        else
                        {
                            request.Percent += incrementForPercent;
                            _cache.Set(userRequest.Query, request);
                        }

                        await Task.Delay(1 * 1000);
                        ++count;
                    }
                }
            });
            return Task.CompletedTask;
        };
    }

    public UserRequestDtoResponse GetUserRequest(GetUserRequestDto data)
    {
        var requestId = data.requestId;
        var request = _cache.Get(requestId) as UserRequest;
        var value = request?.ConvertToDto();
        if (value == null)
        {
            var requestDal = _context.UserRequests.Include(x => x.Result).FirstOrDefault(req => req.Query == requestId);
            if (requestDal != null && requestDal.Result != null)
            {
                var currentUser = _context.Users.FirstOrDefault(user => user.Id == requestDal.Result.User_Id);
                value = new UserRequestDtoResponse()
                {
                    Percent = requestDal.Percent.ToString("F"),
                    Query = requestDal.Query,
                    Result = requestDal.Percent == 100 ? new ResultDto()
                    {
                        User_Id = currentUser.Id,
                        Count_Sign_In = currentUser.Sign_In_Count
                    } : null
                };
            }
            if (value == null)
            {
                throw new ArgumentException("Incorrect requestId");
            }
        }

        return value;
    }
}