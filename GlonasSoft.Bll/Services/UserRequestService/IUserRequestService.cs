using GlonasSoft.Bll.Dtos;

namespace GlonasSoft.Bll.Services.UserRequestService;

public interface IUserRequestService
{
    Task<Guid> GetUserInfo(GetUserInfoRequestDto data,CancellationToken token);

    UserRequestDtoResponse GetUserRequest(GetUserRequestDto data);
}