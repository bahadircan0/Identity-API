namespace DotnetAuthentication.Dtos
{
    public sealed record class CahngePasswordDto(
        Guid Id,
        string CurrentPassword,
        string NewPassword
        
        )
    {
    }
}
