# Identity Architecture - Clean Architecture + DDD

## 📐 Architecture Overview

This project follows **Clean Architecture** and **Domain-Driven Design** principles for ASP.NET Core Identity integration.

### Core Principle
**Domain Layer is framework-agnostic and does not know about ASP.NET Core Identity.**

## 🏗️ Layer Responsibilities

### 1. Domain Layer (`Oreon.Domain`)
**Location**: `src/Oreon.Domain/`

**Responsibilities**:
- Define **domain concepts** (Member aggregate, not ApplicationUser)
- Define **abstractions** for infrastructure services
- **ZERO dependency** on ASP.NET Core, Entity Framework Core, or Identity framework

**Files**:
```
Oreon.Domain/
├── Abstractions/
│   └── Services/
│       └── ICurrentUserService.cs      // ✅ Abstraction to get current user info
├── Aggregates/
│   └── Members/
│       ├── Member.cs                   // ✅ Domain aggregate (business entity)
│       └── MemberId.cs                 // ✅ Strongly-typed ID (Guid)
└── Oreon.Domain.csproj                 // ✅ No framework references!
```

**Key Point**: Domain **uses** `ICurrentUserService` but doesn't know it's implemented via HttpContext.

---

### 2. Infrastructure Layer (`Oreon.Infrastructure`)
**Location**: `src/Oreon.Infrastructure/`

**Responsibilities**:
- Implement **technical details** (ASP.NET Core Identity, JWT, database)
- Implement **abstractions defined in Domain**
- Configure Identity, Authentication, Authorization

**Files**:
```
Oreon.Infrastructure/
├── Identity/
│   ├── ApplicationUser.cs              // ✅ Inherits IdentityUser<Guid>
│   ├── ApplicationRole.cs              // ✅ Inherits IdentityRole<Guid>
│   └── Services/
│       └── CurrentUserService.cs       // ✅ Implements ICurrentUserService
├── Persistence/
│   ├── DataContext.cs                  // ✅ IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
│   └── Configurations/
│       ├── ApplicationUserConfiguration.cs
│       └── ApplicationRoleConfiguration.cs
└── DependencyInjection.cs              // ✅ Register Identity services
```

**Key Point**: Infrastructure **knows** about both Domain (ICurrentUserService) and Identity framework.

---

### 3. Application Layer (`Oreon.Application`)
**Location**: `src/Oreon.Application/`

**Responsibilities**:
- Define **use cases** (Commands/Queries via MediatR)
- Orchestrate domain logic + infrastructure services
- **Use abstractions** (ICurrentUserService, IUserRepository) without knowing implementation

**Example**:
```csharp
// ✅ Application handler uses ICurrentUserService (abstraction from Domain)
public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, Result>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result> Handle(UpdateMemberCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();  // ✅ No HttpContext dependency!
        var member = await _unitOfWork.Members.GetByIdAsync(userId, ct);
        // ... business logic
    }
}
```

---

### 4. WebApi Layer (`Oreon.WebApi`)
**Location**: `src/Oreon.WebApi/`

**Responsibilities**:
- HTTP entry point (Controllers, Middleware)
- **Configure** Dependency Injection
- Apply Authentication/Authorization middleware

**Configuration in `Program.cs`**:
```csharp
// ✅ All Identity configuration happens in Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// ✅ Middleware pipeline (order matters!)
app.UseAuthentication();  // Must come before Authorization
app.UseAuthorization();
```

---

## 🔑 Identity Design Decisions

### Decision 1: Guid for User IDs
**Choice**: `IdentityUser<Guid>` instead of default `IdentityUser<string>` or `IdentityUser<int>`

**Rationale**:
- Consistency with `MemberId` (strongly-typed ID using Guid)
- Better for distributed systems (no database auto-increment dependency)
- Easier to sync between Identity (ApplicationUser) and Domain (Member)

**Implementation**:
```csharp
public sealed class ApplicationUser : IdentityUser<Guid>
{
    // No domain logic here - this is purely technical identity
}
```

---

### Decision 2: Separation of Concerns
**ApplicationUser vs Member**:

| Aspect           | ApplicationUser (Identity)         | Member (Domain)                  |
|------------------|-----------------------------------|----------------------------------|
| **Location**     | `Infrastructure.Identity`         | `Domain.Aggregates.Members`      |
| **Purpose**      | Authentication & Authorization    | Business logic & domain rules    |
| **Persistence**  | AspNetUsers table                 | Members table                    |
| **Properties**   | UserName, Email, PasswordHash     | KnownAs, DateOfBirth, Photos     |
| **Lifecycle**    | Managed by UserManager            | Managed by Repository + UnitOfWork |

**Why Separate?**
- Domain should not be polluted with authentication concerns
- Different bounded contexts: Identity (technical) vs Dating (business)
- Easier to test: mock `ICurrentUserService`, no need for UserManager in domain tests

---

### Decision 3: ICurrentUserService Abstraction
**Interface** (in Domain):
```csharp
public interface ICurrentUserService
{
    Guid GetUserId();
    string? GetUsername();
    bool IsAuthenticated();
    bool IsInRole(string role);
}
```

**Implementation** (in Infrastructure):
```csharp
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public Guid GetUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }
}
```

**Benefits**:
- Domain code can access current user **without** depending on ASP.NET Core
- Easy to test: mock `ICurrentUserService.GetUserId()` returns `Guid.NewGuid()`
- Swappable implementation (could use gRPC context, message headers, etc.)

---

## 🔐 Authentication Flow

### JWT Token Flow (Current Implementation)

1. **User Login** → `POST /api/account/login`
   - Controller calls `ITokenService.CreateTokenAsync(user)`
   - TokenService generates JWT with claims:
     ```json
     {
       "nameid": "guid-user-id",
       "unique_name": "username",
       "email": "user@example.com",
       "role": ["Member", "Admin"]
     }
     ```

2. **Authenticated Request** → `GET /api/members` (with `Authorization: Bearer <token>`)
   - Middleware validates JWT
   - Claims are added to `HttpContext.User`
   - `CurrentUserService` extracts claims
   - Application handlers use `ICurrentUserService.GetUserId()`

3. **SignalR Hub Connection** → `ws://localhost/hubs/presence?access_token=<token>`
   - JWT passed via query string
   - `JwtBearerEvents.OnMessageReceived` extracts token
   - Hub methods can access `Context.User` (or use `ICurrentUserService`)

---

## 🧪 Testing Strategy

### Unit Tests (Domain)
```csharp
[Fact]
public void Member_Create_ShouldThrowException_WhenUserUnder18()
{
    // ✅ No Identity dependencies!
    var act = () => Member.Create(
        username: "john",
        dateOfBirth: DateOnly.FromDateTime(DateTime.Today.AddYears(-17)),
        knownAs: "John",
        gender: "male",
        city: "NYC",
        country: "USA"
    );
    
    act.Should().Throw<InvalidAgeException>();
}
```

### Integration Tests (Application)
```csharp
[Fact]
public async Task UpdateMember_ShouldSucceed_WhenUserAuthenticated()
{
    // Arrange
    var mockCurrentUser = new Mock<ICurrentUserService>();
    mockCurrentUser.Setup(x => x.GetUserId()).Returns(Guid.NewGuid());
    
    var handler = new UpdateMemberCommandHandler(mockCurrentUser.Object, ...);
    
    // Act & Assert
    var result = await handler.Handle(command, CancellationToken.None);
    result.IsSuccess.Should().BeTrue();
}
```

---

## 🚀 Migration from Old Structure

### Old (Incorrect) Structure ❌
```
Domain/
├── Users/
│   ├── AppUser.cs : IdentityUser<int>     // ❌ Identity in Domain!
│   └── AppRole.cs : IdentityRole<int>

API/
├── Data/
│   └── DataContext.cs : IdentityDbContext  // ❌ DbContext in API!
```

### New (Correct) Structure ✅
```
Domain/
├── Abstractions/Services/
│   └── ICurrentUserService.cs              // ✅ Only abstraction
├── Aggregates/Members/
│   └── Member.cs                          // ✅ Business entity

Infrastructure/
├── Identity/
│   ├── ApplicationUser.cs : IdentityUser<Guid>
│   └── Services/CurrentUserService.cs
├── Persistence/
│   └── DataContext.cs : IdentityDbContext
```

---

## 📚 References
- [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [Clean Architecture by Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design by Eric Evans](https://domainlanguage.com/ddd/)
- [.NET Microservices: Architecture for Containerized .NET Applications](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/)

---

## ⚠️ Common Mistakes to Avoid

1. **❌ Putting IdentityUser in Domain**
   - Domain entities should NOT inherit from `IdentityUser`
   - Keep authentication separate from business logic

2. **❌ Using UserManager directly in Domain/Application**
   - Wrap Identity operations in abstraction (`IIdentityService`)
   - Application layer calls `IIdentityService`, not `UserManager<T>`

3. **❌ Mixing Member and ApplicationUser**
   - ApplicationUser: authentication credentials
   - Member: business profile data
   - They reference each other by `Guid Id`, but are separate entities

4. **❌ Testing with real Identity**
   - Mock `ICurrentUserService` in tests
   - Don't spin up Identity database for unit tests

---

## 📞 Contact
For questions about this architecture, see:
- `.github/copilot-instructions.md` (repository-wide DDD rules)
- `AGENTS.md` (AI agent instructions)
