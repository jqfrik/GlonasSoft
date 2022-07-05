using GlonasSoft.Bll.Domains;
using GlonasSoft.Bll.Dtos;
using GlonasSoft.Bll.Services;
using GlonasSoft.Bll.Services.UserRequestService;
using GlonasSoft.Dal;
using GlonasSoft.Dal.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace GlonasSoft.Bll;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBll(this IServiceCollection collection, string connectionString)
    {
        collection.AddScoped<IUserRequestService, UserRequestService>();
        collection.AddSingleton<ProcessorQueue>();
        collection.AddHostedService<ProcessorRequests>();
        collection.AddSingleton<ShutdownHandler>();
        collection.AddDal(connectionString);

        return collection;
    }

    public static UserRequestDal Convert(this UserRequest request)
    {
        return new()
        {
            Percent = request.Percent,
            Query = request.Query,
            Result = request.Result != null ? new()
            {
                User_Id =  request.Result.User_Id,
                Count_Sign_In = request.Result.Count_Sign_In,
            } : new()
        };
    }

    // public static UserRequestDtoResponse ConvertToDto(this UserRequest request)
    // {
    //     return new()
    //     {
    //         Percent = request.Percent.ToString("F"),
    //         Query = request.Query,
    //         Result = new()
    //         {
    //             User_Id = request.Result.User_Id,
    //             Count_Sign_In = request.Result.Count_Sign_In
    //         }
    //     };
    // }

    public static UserRequest Convert(this UserRequestDal request)
    {
        return new()
        {
            Percent = request.Percent,
            Query = request.Query,
            Result = new Result()
            {
                User_Id = request.Result.User_Id,
                Count_Sign_In = request.Result.Count_Sign_In
            }
        };
    }

    public static UserRequestDtoResponse ConvertToDto(this UserRequestDal request)
    {
        return new()
        {
            Percent = request.Percent.ToString("F"),
            Query = request.Query,
            Result = request.Result != null
                ? new ResultDto()
                {
                    User_Id = request.Result.User_Id,
                    Count_Sign_In = request.Result.Count_Sign_In
                }
                : null
        };
    }

    public static UserRequestDtoResponse ConvertToDto(this UserRequest request)
    {
        return new()
        {
            Percent = request.Percent.ToString("F"),
            Query = request.Query,
            Result = request.Result != null
                ? new ResultDto()
                {
                    User_Id = request.Result.User_Id,
                    Count_Sign_In = request.Result.Count_Sign_In
                } : null
        };
    }

    public static void WithoutWarnings(this Task task)
    {
        ;
    }
}