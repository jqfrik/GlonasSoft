namespace GlonasSoft.Dal.Entities;

public class UserRequestDal
{
    public Guid Id { get; set; }
    public Guid Query { get; set; } = Guid.NewGuid();

    public double Percent { get; set; } = 0;
    
    public ResultDal Result { get; set; }
}

public class ResultDal
{
    public Guid Id { get; set; }
    public Guid User_Id { get; set; }
    
    public uint Count_Sign_In { get; set; }
}