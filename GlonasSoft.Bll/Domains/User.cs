namespace GlonasSoft.Bll.Domains;

public class User
{
    public Guid Id { get; set; }
    
    public uint Sign_In_Count { get; set; }
    
    public ICollection<UserRequest> Requests { get; set; }
}