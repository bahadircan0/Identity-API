namespace DotnetAuthentication.Dtos
{
    public sealed record class LoginDto(
        string UserNameOrEmail,
        string Password
      
        )
    {
    }
}
