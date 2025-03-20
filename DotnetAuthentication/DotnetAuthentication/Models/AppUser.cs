
using Microsoft.AspNetCore.Identity;

namespace DotnetAuthentication.Models
{
    public sealed class AppUser :IdentityUser<Guid>
    {
        public string FirstName { get; set; }=string.Empty;
        public string Lastname { get; set; }= string.Empty;

        public string FullName => string.Join(" ", FirstName, Lastname);


    }
}
