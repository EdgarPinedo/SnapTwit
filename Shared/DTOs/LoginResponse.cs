namespace Snaptwit.Shared.DTOs;

public class LoginResponse
{
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public DateTime ExpiryTimeStamp { get; set; }
}