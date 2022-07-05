using GlonasSoft.Bll.Dtos;
using GlonasSoft.Bll.Services.UserRequestService;
using GlonasSoft.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GlonasSoft.Controllers;

[ApiController]
[Route("report")]
public class UserStatisticController : ControllerBase
{
    public IUserRequestService UserService { get; }

    public UserStatisticController(IUserRequestService userService)
    {
        UserService = userService;
    }

    [HttpPost("user_statistics")]
    public async Task<IActionResult> GetUserInfo(GetUserStatisticsRequest request,CancellationToken token)
    {
        if (ModelState.IsValid)
        {
            Guid? result = null;
            try
            {
                result = await UserService.GetUserInfo(new GetUserInfoRequestDto(request.UserId, request.StartDate,
                    request.FinishDate),token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }

        return BadRequest(new { error = "incorrect data" });
    }

    [HttpGet("info/{requestId}")]
    public IActionResult GetRequestInfo([FromRoute] Guid requestId)
    {
        if (ModelState.IsValid)
        {
            UserRequestDtoResponse userRequest = null;
            try
            {
                userRequest = UserService.GetUserRequest(new GetUserRequestDto(requestId));
                return Ok(userRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return BadRequest(new { error = "incorrectData" });
    }
}