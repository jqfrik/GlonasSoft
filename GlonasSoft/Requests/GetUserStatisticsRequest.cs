namespace GlonasSoft.Requests;

public class GetUserStatisticsRequest
{
    public Guid UserId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime FinishDate { get; set; }
}