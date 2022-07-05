using GlonasSoft.Bll.Domains;

namespace GlonasSoft.Bll.Dtos;

public class UserRequestDtoResponse
{
    public string Percent { get; set; }
    
    public Guid Query { get; set; }
    
    public ResultDto Result { get; set; }
}

public class ResultDto
{
    public Guid User_Id { get; set; }
    
    public uint Count_Sign_In { get; set; }
} 