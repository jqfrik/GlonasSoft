namespace GlonasSoft.Bll.Dtos;

public record GetUserInfoRequestDto(Guid UserId, DateTime StartDate, DateTime FinishDate);