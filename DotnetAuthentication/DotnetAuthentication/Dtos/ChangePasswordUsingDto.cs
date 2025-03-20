namespace DotnetAuthentication.Dtos
{
    public sealed record class ChangePasswordUsingDto(
        string Email,
        string NewPassword,
        string Token
        
        )
    {
    }
}
