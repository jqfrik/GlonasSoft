namespace GlonasSoft.Bll.Domains;

public class UserRequest
{
    public Guid Id { get; set; }
    public Guid Query { get; set; } = Guid.NewGuid();

    public double Percent { get; set; }
    
    public Result Result { get; set; }
}

public class Result
{
    public Guid Id { get; set; }
    public Guid User_Id { get; set; }
    
    public uint Count_Sign_In { get; set; }
}