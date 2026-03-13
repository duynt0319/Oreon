using Microsoft.AspNetCore.Identity;

namespace Oreon.Infrastructure.Identity;

public sealed class AppUser : IdentityUser<Guid>
{
    // Identity technical properties are inherited from IdentityUser<Guid>
    // Domain properties should be in Member aggregate, not here
}
