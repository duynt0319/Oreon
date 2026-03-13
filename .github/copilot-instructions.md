# .github/copilot-instructions.md

Tài liệu này là **repository-wide custom instructions** cho GitHub Copilot nhằm buộc các gợi ý sinh mã/đề xuất giải pháp **tuân thủ Domain-Driven Design (DDD), Clean Architecture, CQRS, MediatR và .NET 8**. Tệp đặt tại `.github/copilot-instructions.md` và có thể được kết hợp với các tệp `.github/instructions/*.instructions.md` (path-specific) và `AGENTS.md` (agent instructions). citeturn5view0turn5view1turn5view2turn28view1

Lưu ý về ảnh hưởng: **Copilot code review chỉ đọc 4.000 ký tự đầu của mỗi instruction file**; Copilot Chat và Copilot coding agent không bị giới hạn 4.000 ký tự nhưng vẫn chịu ràng buộc context window và khả năng “bỏ sót” nếu file quá dài. Vì vậy, các quy tắc **không thể thương lượng** được đặt sớm. citeturn28view0turn28view1

## Mục tiêu và phạm vi

Copilot PHẢI ưu tiên theo thứ tự:

- **Đúng mô hình domain và bất biến (invariants)** trước tiên (không “CRUD hóa” domain).
- **Giữ ranh giới kiến trúc** (Clean Architecture dependency rule) trong mọi đề xuất/sinh mã.
- **Dễ bảo trì, dễ mở rộng, dễ test**: domain độc lập framework; orchestration rõ; mapping tường minh; side-effect có kiểm soát.

Copilot TUYỆT ĐỐI KHÔNG:

- Đưa nghiệp vụ lõi vào Infrastructure hoặc Presentation.
- Để Domain phụ thuộc Entity Framework Core/ASP.NET Core/MediatR/ILogger/HttpContext/DI container.
- Tạo “generic CRUD scaffolding” khi domain có quy tắc nghiệp vụ.

Copilot PHẢI hỏi làm rõ thay vì đoán bừa nếu yêu cầu thiếu dữ liệu domain hoặc có thể phá kiến trúc (ví dụ: “thêm field”, “đổi status”, “sync với hệ khác” nhưng không có rule/invariant/ownership).

## Kiến trúc Clean Architecture

### Dependency rule và nguyên tắc bất biến

Áp dụng dependency rule: **phụ thuộc code chỉ được hướng vào trong**; lớp trong không biết gì về lớp ngoài (kể cả tên class/namespace của lớp ngoài). citeturn4view0

Hệ quả bắt buộc:

- Domain không tham chiếu: `Microsoft.*`, `System.Data.*`, `EntityFrameworkCore`, `AspNetCore`, `MediatR`, `FluentValidation`, `Serilog`, `MassTransit`, `HttpClient`, v.v.
- Application không tham chiếu trực tiếp EF Core (DbContext/DbSet/IQueryable), framework web, message broker client. Application chỉ làm việc qua **abstraction** do Application định nghĩa (ports).
- Infrastructure và Presentation phụ thuộc Application + Domain (vì là outer layers).
- DTO/contract của API không đi vào Domain. Không truyền request model trực tiếp vào Aggregate.

Mục tiêu tách lớp là để business rules **độc lập frameworks/UI/DB** và **testable**. citeturn4view0

### Cấu trúc solution và project chuẩn

Cấu trúc khuyến nghị cho .NET 8:

```text
/
├─ src/
│  ├─ MyProduct.MyContext.Domain/
│  ├─ MyProduct.MyContext.Application/
│  ├─ MyProduct.MyContext.Infrastructure/
│  └─ MyProduct.MyContext.WebApi/           (hoặc Presentation)
└─ tests/
   ├─ MyProduct.MyContext.Domain.Tests/
   ├─ MyProduct.MyContext.Application.Tests/
   ├─ MyProduct.MyContext.Infrastructure.Tests/   (integration)
   └─ MyProduct.MyContext.WebApi.Tests/           (integration/contract)
```

Nếu có nhiều bounded contexts: tách theo context ở mức solution folder hoặc repo; tối thiểu là tách theo namespace + project để tránh “shared model sai ngữ cảnh”.

### Trách nhiệm theo tầng

Domain (core business):
- Được chứa: Entity, Value Object, Aggregate Root, Domain Event, Domain Service (thuần), Specification (thuần), Domain Rules, Domain Errors/Exceptions, Policies.
- Không được chứa: DTO API, EF Core mapping, Http, Json serialization, logging, caching, message bus.

Application (use cases):
- Được chứa: CQRS (Commands/Queries), Handlers, DTO cho đọc/ghi (application DTO), Interfaces (Repository ports, UnitOfWork ports, Clock, CurrentUser, Outbox writer), Validators (input), Mapper explicit, Transaction boundary policy (thông qua pipeline/abstraction).
- Không được chứa: business rules lõi (invariants) vốn phải nằm trong aggregate/entity; không dùng DbContext/DbSet/IQueryable xuyên tầng.

Infrastructure (details):
- Được chứa: EF Core DbContext + mapping Fluent API, migrations, repository implementations, external service clients, message bus implementations, cache/file storage, outbox implementation, integration event publishing.
- Không được chứa: quy tắc nghiệp vụ lõi; không tự ý thay đổi invariant mà không đi qua Domain.

WebApi/Presentation:
- Được chứa: controllers/minimal endpoints, request/response contracts, authn/authz, API versioning, exception-to-ProblemDetails mapping, OpenAPI/Swagger, HTTP concerns.
- Không được chứa: domain orchestration phức tạp; không gọi DbContext trực tiếp; không viết nghiệp vụ.

## Quy tắc DDD bắt buộc

### Domain là trung tâm và ubiquitous language

Copilot PHẢI đặt tên lớp/hàm/biến theo **ubiquitous language** trong bounded context; thay đổi ngôn ngữ nghĩa là thay đổi model và phải refactor code tương ứng. citeturn3view2turn11view1

Copilot KHÔNG dùng:
- Tên chung chung như `Processor`, `Manager`, `Helper`, `Util`, `CommonService` cho nghiệp vụ.
- Tên kỹ thuật thay cho thuật ngữ domain (ví dụ `StatusHandler` thay vì `ApproveInvoice`, `CloseCase`, v.v.)

### Entity và Value Object

Quy tắc phân loại:

- Dùng **Entity** khi đối tượng được phân biệt bởi **identity xuyên thời gian/lifecycle**, không phải bởi thuộc tính. Model phải định nghĩa rõ “thế nào là cùng một thứ”. citeturn24view0turn3view0
- Dùng **Value Object** khi chỉ quan tâm **giá trị + logic** và không cần identity; value object phải **immutable** và các operation phải **side-effect-free**. citeturn24view1turn15view1

Quy tắc mã hóa bắt buộc:

- Entity:
  - Không public setter bừa bãi. Ưu tiên private setters + hành vi (methods) thể hiện state transitions.
  - Không expose collection mutable trực tiếp; dùng read-only view.
- Value Object:
  - Immutable (record/class với init-only hoặc private setters).
  - Equality dựa trên giá trị.

Ví dụ skeleton (C#):

```csharp
public abstract class Entity<TId>
{
    public TId Id { get; protected set; } = default!;
}

public sealed record Money(decimal Amount, string Currency)
{
    public static Money Of(decimal amount, string currency)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required.", nameof(currency));
        return new Money(amount, currency.Trim().ToUpperInvariant());
    }
}
```

### Aggregate Root và consistency boundary

Copilot PHẢI coi **Aggregate là ranh giới bất biến + ranh giới giao dịch**:

- Chỉ cho phép bên ngoài giữ reference đến **Aggregate Root**; không cho phép giữ reference đến entity con để sửa trực tiếp.
- Mọi thay đổi state của entity con bắt buộc đi qua hành vi của root.
- Invariants được định nghĩa ở mức aggregate và root chịu trách nhiệm enforce.
- Dùng chính aggregate boundaries để govern transactions/distribution: trong boundary áp consistency đồng bộ; **qua boundary xử lý bất đồng bộ**. citeturn13view0

Đây là nguyên tắc nền để tránh lock rộng và để mở đường cho eventual consistency. citeturn13view0turn6view0

Anti-pattern bị cấm:
- Aggregate “to khổng lồ” vì cố nhét mọi thứ vào 1 transaction.
- Anemic aggregate: root chỉ có getters/setters + mọi rule nằm ở service.

Ví dụ aggregate root (C#):

```csharp
public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}

public abstract class AggregateRoot<TId> : Entity<TId>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);

    public void ClearDomainEvents() => _domainEvents.Clear();
}

public sealed record OrderPlaced(Guid OrderId, DateTimeOffset OccurredAt) : IDomainEvent;

public sealed class Order : AggregateRoot<Guid>
{
    private readonly List<OrderLine> _lines = new();

    public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();
    public string Status { get; private set; } = "Draft";

    public static Order Create(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id is required.", nameof(id));
        return new Order { Id = id };
    }

    public void AddLine(Guid productId, int quantity, Money unitPrice)
    {
        EnsureDraft();
        _lines.Add(new OrderLine(productId, quantity, unitPrice));
    }

    public void Place()
    {
        EnsureDraft();
        if (_lines.Count == 0) throw new DomainRuleViolationException("order.empty", "Cannot place an empty order.");

        Status = "Placed";
        AddDomainEvent(new OrderPlaced(Id, DateTimeOffset.UtcNow));
    }

    private void EnsureDraft()
    {
        if (!string.Equals(Status, "Draft", StringComparison.Ordinal))
            throw new DomainRuleViolationException("order.not_draft", "Operation allowed only in Draft status.");
    }
}

public sealed class OrderLine
{
    public Guid ProductId { get; }
    public int Quantity { get; }
    public Money UnitPrice { get; }

    public OrderLine(Guid productId, int quantity, Money unitPrice)
    {
        if (productId == Guid.Empty) throw new ArgumentException("ProductId is required.", nameof(productId));
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}

public sealed class DomainRuleViolationException : Exception
{
    public string Code { get; }

    public DomainRuleViolationException(string code, string message) : base(message) => Code = code;
}
```

### Domain Service, Factory, Specification

Domain Service:
- Dùng khi “một quá trình/biến đổi quan trọng” **không phải trách nhiệm tự nhiên** của entity/value object; service là interface standalone và contract phải được diễn đạt bằng ubiquitous language. citeturn15view0turn15view1
- Domain service trong Domain PHẢI thuần (không I/O). Nếu cần truy cập DB/external để ra quyết định, orchestration nằm ở Application; Domain chỉ giữ rule/policy thuần.

Factory:
- Dùng khi tạo một aggregate/large value object trở nên phức tạp hoặc làm lộ cấu trúc; factory chịu trách nhiệm tạo “một khối” và enforce invariants tại thời điểm tạo. citeturn13view2
- Không tạo factory cho trường hợp constructor đơn giản.

Specification:
- Dùng để tách “phát biểu điều kiện chọn/validate/đặt hàng” ra khỏi candidate object; giúp rules có tên, tái dùng, kết hợp, testable. citeturn18view0turn18view1
- Không lạm dụng specification để biến entity thành “frame system” hoặc nhầm specification với domain entity thật; nếu object bắt đầu “đại diện cho entity thật” thay vì “đặt ràng buộc lên entity khác”, cần dừng. citeturn18view1

### Domain Event, Integration Event và nhầm lẫn bị cấm

Domain Event:
- Là một phần của domain model: “có việc đã xảy ra mà domain experts quan tâm”; thường immutable. citeturn22view0
- Dùng để diễn đạt side effects rõ ràng và tách concerns trong cùng domain; có thể hỗ trợ eventual consistency giữa aggregates trong cùng domain. citeturn6view0turn13view0
- Trong hệ .NET, domain event handlers thường chạy in-process, gần như ngay lập tức; integration events thì luôn async. citeturn6view0

Integration Event:
- Dùng để giao tiếp giữa bounded contexts hoặc external systems; contract phải ổn định, versioned, và coi như boundary. (Ưu tiên published language/open-host service khi làm API công khai). citeturn20view2turn15view2

Nhầm lẫn bị cấm:
- Không publish integration event trực tiếp từ Domain.
- Không serialize domain event làm message contract public.

## CQRS + MediatR trong Application layer

### Quy tắc CQRS bắt buộc

- Query:
  - **Idempotent và side-effect-free**: không thay đổi state, không gọi `SaveChanges`. citeturn6view1
  - Trả về DTO/read model tối ưu cho đọc (có thể khác domain model).
- Command:
  - Gây thay đổi state (transaction).
  - **Không trả dữ liệu thừa**. Mặc định: trả `Result`/`Result<TId>` hoặc `Unit` + side effects cần thiết.
- Handler:
  - Mỗi handler xử lý **đúng 1 use case**.
  - Không gọi chồng chéo nhiều use case trong 1 handler trừ khi đó là orchestration hợp lệ và vẫn giữ transaction boundary rõ.

Microsoft hướng dẫn tách query khỏi command và dùng mediator pipeline để giảm coupling và áp cross-cutting concerns. citeturn6view2turn6view1

### Tổ chức thư mục Application theo feature

Ví dụ cấu trúc (khuyến nghị):

```text
MyProduct.MyContext.Application/
├─ Abstractions/
│  ├─ Persistence/
│  │  ├─ IUnitOfWork.cs
│  │  ├─ IOrderRepository.cs
│  │  └─ ...
│  ├─ Messaging/
│  │  ├─ IOutboxWriter.cs
│  │  └─ IIntegrationEventPublisher.cs
│  ├─ Time/
│  │  └─ IClock.cs
│  └─ Security/
│     └─ ICurrentUser.cs
├─ Features/
│  └─ Orders/
│     ├─ Commands/
│     │  └─ PlaceOrder/
│     │     ├─ PlaceOrderCommand.cs
│     │     ├─ PlaceOrderCommandValidator.cs
│     │     └─ PlaceOrderCommandHandler.cs
│     ├─ Queries/
│     │  └─ GetOrderById/
│     │     ├─ GetOrderByIdQuery.cs
│     │     └─ GetOrderByIdQueryHandler.cs
│     └─ Dtos/
│        └─ OrderDetailsDto.cs
└─ Behaviors/ (MediatR pipeline)
   ├─ ValidationBehavior.cs
   ├─ LoggingBehavior.cs
   ├─ TransactionBehavior.cs
   └─ IdempotencyBehavior.cs (nếu dùng)
```

### Validation bắt buộc theo lớp

- Input validation (technical/format/range) nằm ở Presentation hoặc Application (FluentValidation được phép).
- Business validation/invariant nằm trong Domain (aggregate/entity/specification) và không được thay bằng FluentValidation/DataAnnotations. citeturn15view1

Pipeline validation (mẫu):

```csharp
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var failures = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken))))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
            throw new AppValidationException(failures);

        return await next();
    }
}

public sealed class AppValidationException : Exception
{
    public IReadOnlyCollection<ValidationFailure> Failures { get; }

    public AppValidationException(IReadOnlyCollection<ValidationFailure> failures)
        : base("Validation failed.")
        => Failures = failures;
}
```

### Mapping phải tường minh

Copilot KHÔNG tự động đưa AutoMapper vào nếu chưa được chỉ định.

Quy tắc mapping:
- DTO -> Command: map tại Presentation (request contract) hoặc Application (mapper class).
- Domain -> DTO: map tại QueryHandler (read model) hoặc mapper class ở Application.
- Không map trực tiếp request model vào entity/aggregate.

### Transaction boundary và idempotency

Transaction boundary:
- Mỗi command handler tương ứng một transaction (nếu có thay đổi).
- Transaction được áp như cross-cutting concern, ưu tiên qua pipeline behavior đối với command marker interface.

Idempotency:
- Nếu command có thể bị gửi lặp (network retries, message broker at-least-once), phải có cơ chế xử lý “chỉ xử lý một lần”.
- eShopOnContainers minh họa cơ chế “IdentifiedCommand” wrapper + request id tracking để đảm bảo idempotent processing. citeturn8view0

MediatR pipeline được khuyến nghị để áp logging/validation/audit/security/transaction tập trung và giảm coupling. citeturn6view2

## Infrastructure và Presentation

### Repository rules bắt buộc

Repository trong DDD:
- Là “query access to aggregates expressed in ubiquitous language”.
- Chỉ tạo repository cho **aggregate roots thật sự cần global access**.
- Cảnh báo: unconstrained queries có thể phá encapsulation, bypass root, làm domain logic trôi sang query/application và biến entity/VO thành data containers. citeturn13view1

Quy tắc triển khai trong dự án này:

- Interface repository:
  - Đặt ở Application.Abstractions (mặc định).
  - Chỉ đặt ở Domain nếu thực sự muốn “Domain package tái sử dụng độc lập application” và vẫn đảm bảo Domain không lệ thuộc Infrastructure.
- Implementation repository:
  - Luôn ở Infrastructure.
- Read side:
  - Query tối ưu đọc có thể tách riêng (query service) và có thể dùng SQL/projection trực tiếp, miễn không làm ô nhiễm domain model.

Cấm:
- `IGenericRepository<T>` CRUD vô nghĩa.
- Expose `IQueryable<T>` ra khỏi Infrastructure.
- Viết query phức tạp phục vụ read model vào repository domain theo kiểu “kéo entity ra rồi select bừa”.

Ví dụ interface (Application):

```csharp
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct);
    void Add(Order order);
}
```

### EF Core và persistence

EF Core là **chi tiết hạ tầng**.

Quy tắc bắt buộc:

- Không dùng EF Core attributes để “nhuộm” Domain nếu tránh được; mapping cấu hình ở Infrastructure bằng Fluent API.
- Value object nên persist bằng owned entity types (EF Core 2.0+), đây là cách được Microsoft khuyến nghị trong bối cảnh DDD (dù có khác biệt nhỏ với VO canonical). citeturn9view0
- Owned types phải khai báo explicit (OwnsOne/OwnedAttribute) và thường được eager-loaded. citeturn9view0turn9view3
- Tránh lazy loading nếu tạo side effects khó kiểm soát.
- Không để DbContext/DbSet/IQueryable “rò” lên Application/Domain.

Ví dụ mapping VO trong Infrastructure:

```csharp
public sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(x => x.Id);

        // Không persist domain events
        builder.Ignore(x => x.DomainEvents);

        // Value Object mapping
        builder.OwnsOne(x => x.ShippingAddress, owned =>
        {
            owned.Property(p => p.Street).HasColumnName("shipping_street").HasMaxLength(200);
            owned.Property(p => p.City).HasColumnName("shipping_city").HasMaxLength(100);
        });

        // Optimistic concurrency token (ví dụ RowVersion)
        builder.Property<byte[]>("RowVersion").IsRowVersion();
    }
}
```

Concurrency:
- Dùng optimistic concurrency bằng concurrency token để phát hiện xung đột khi SaveChanges; EF Core so sánh token database với token đã read. citeturn25search2
- Khi conflict: map ra HTTP 409 (Conflict) ở API layer.

Soft delete (nếu dùng):
- Dùng global query filters (HasQueryFilter) để lọc `IsDeleted` mặc định. citeturn9view1
- Cấm “quên lọc IsDeleted” bằng cách để developer tự where ở mọi query.

### Domain Event publish, transaction và outbox

Trong cùng transaction:
- Domain tạo domain events.
- Application/Infrastructure collect domain events từ aggregates, rồi xử lý:
  - Domain handlers in-process (nếu mục tiêu là consistency nội bộ).
  - Integration event ghi outbox (nếu mục tiêu là giao tiếp giữa contexts/systems).

Transactional Outbox:
- Vấn đề: cần cập nhật DB và gửi message atomically; tránh 2PC.
- Giải pháp: ghi message vào outbox trong cùng transaction, một process khác relay message ra broker. citeturn10view0turn6view3

Quy tắc outbox bắt buộc:
- Không publish integration event “giữa transaction”.
- Publish sau commit bằng outbox relay.
- Consumer phải idempotent (at-least-once delivery là thực tế). citeturn10view0turn8view0

### Anti-corruption layer và tích hợp hệ ngoài

Khi tích hợp legacy/external system:
- Dùng Anti-Corruption Layer (ACL) để **dịch** giữa hai mô hình, tránh “làm bẩn” domain model mới. citeturn9view2turn20view1
- ACL có chi phí (latency/maintenance/scale) và phải được quản trị như một thành phần riêng. citeturn9view2

Khi upstream phục vụ nhiều downstream:
- Xem xét Open-host Service + Published Language để có protocol/contract rõ và ổn định. citeturn20view2turn15view2

Shared Kernel (chỉ khi thật sự cần):
- Shared kernel là interdependency rất “thân mật”; phải **giữ nhỏ**, có quy trình CI và không đổi nếu không tham vấn team khác. citeturn20view0
- Cấm biến Shared Kernel thành “project share” chứa DTO/enums/helpers/random.

## Quy tắc chất lượng, error handling, tests và Copilot Response Policy

### Error handling và ProblemDetails ở API layer

Nguyên tắc:
- Không lạm dụng exception cho flow control thường nhật.
- Domain errors phải rõ nghĩa (code + message), map ra response có cấu trúc.
- API trả lỗi theo Problem Details (RFC 7807) để máy đọc được và thống nhất format. citeturn25search0turn25search9

Mapping khuyến nghị:
- Validation (Application): 400 + ValidationProblemDetails.
- DomainRuleViolation: 409 (Conflict) hoặc 422 (Unprocessable Entity) tuỳ policy dự án (chọn 1 và nhất quán).
- NotFound: 404.
- Concurrency conflict: 409.
- Unexpected exception: 500 + trace id; không leak stacktrace ở production.

Ví dụ minimal exception mapping (ý tưởng):

```csharp
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var problem = new ProblemDetails
        {
            Instance = context.Request.Path,
            Extensions =
            {
                ["traceId"] = context.TraceIdentifier
            }
        };

        switch (exception)
        {
            case AppValidationException vex:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                problem.Title = "Validation failed";
                problem.Detail = "One or more validation errors occurred.";
                break;

            case DomainRuleViolationException dex:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                problem.Title = "Business rule violated";
                problem.Detail = dex.Message;
                problem.Extensions["code"] = dex.Code;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problem.Title = "Unexpected error";
                problem.Detail = "An unexpected error occurred.";
                break;
        }

        await context.Response.WriteAsJsonAsync(problem);
    });
});
```

### Test strategy bắt buộc theo layer

Domain.Tests:
- Unit test invariants, behavior methods của aggregate/entity/value objects.
- Test domain events phát sinh đúng khi state change.

Application.Tests:
- Test command/query handlers:
  - Validation path.
  - Orchestration đúng (load aggregate, gọi hành vi domain, save, outbox).
  - Không test EF mapping ở đây.

Infrastructure.Tests (integration):
- Test repository implementations, EF Core mapping, migrations, outbox persistence.
- Có thể dùng database thật/containers.

WebApi.Tests:
- Contract test endpoint: status codes, ProblemDetails schema, auth policies.
- Smoke test: request -> handler -> persistence.

DDD nhấn mạnh việc viết assertions/invariants và nếu không code assertion trực tiếp thì phải viết unit tests cho invariants đó. citeturn15view1

### Do / Don’t và smells cần tự động cảnh báo

Copilot PHẢI cảnh báo nếu thấy:

- **Anemic domain model**: entity/aggregate chỉ là data + setters; mọi rule ở service/handler.
  - Sửa: đẩy rule vào domain behavior, ẩn setters, expose methods theo ubiquitous language.
- **God service / transaction script trá hình**: Application service dài, if/else lớn, gọi 10 repos, tự enforce rules.
  - Sửa: xác định aggregate boundaries; tách use case; dùng domain behaviors + domain events.
- **Generic repository lạm dụng**: `Repository<T>.GetAll().Where(...)` khắp nơi.
  - Sửa: repository gắn aggregate root + criteria theo ubiquitous language; read side tách riêng.
- **Leaking EF entities**: controller trả thẳng entity; Application nhận DbSet/IQueryable.
  - Sửa: DTO mapping explicit; query service in infrastructure; domain không biết EF.
- **Fat controller**: controller validate nghiệp vụ, tự mở transaction, gọi DbContext trực tiếp.
  - Sửa: controller chỉ map request -> command/query và trả response.

Copilot TUYỆT ĐỐI KHÔNG tự tạo “Shared” project kiểu dump. Nếu cần chia sẻ, áp Shared Kernel rất nhỏ và có governance. citeturn20view0turn13view1

### Copilot Response Policy trong repo này

Khi được yêu cầu **tạo feature mới**, Copilot phải đề xuất theo thứ tự:

- Domain:
  - Thu thập thuật ngữ/invariants/events.
  - Thiết kế aggregate root và hành vi.
- Application:
  - Định nghĩa Command/Query, DTO, Validator, Handler; giải thích transaction boundary.
- Infrastructure:
  - Mapping EF Core, repository implementation, migrations, outbox/integration.
- WebAPI:
  - Endpoint/controller, auth, versioning, ProblemDetails mapping, OpenAPI.

Khi **sửa bug**, Copilot phải:
- Xác định bug thuộc tầng nào:
  - Bug invariant/domain behavior => sửa Domain + tests.
  - Bug orchestration/transaction/idempotency => sửa Application behaviors/handler.
  - Bug persistence mapping/query => sửa Infrastructure + integration tests.
  - Bug contract/status code => sửa WebAPI + contract tests.

Khi **thêm field dữ liệu**, Copilot phải phân tích tác động tối thiểu:
- Domain invariant nào bị ảnh hưởng?
- Mapping EF Core/owned types/concurrency tokens?
- DTO và API contract (versioning nếu breaking change)?
- Migration + backfill (nếu cần)?
- Tests cần bổ sung ở layer nào?

Khi yêu cầu **mơ hồ**, Copilot phải hỏi:
- Bounded context nào? Thuật ngữ domain chính xác là gì?
- Quy tắc nghiệp vụ/invariants liên quan?
- Đây là thay đổi transactional hay eventual consistency được chấp nhận?
- Có integration/external system nào bị ràng buộc contract không?

## Cơ sở tham chiếu ưu tiên

- entity["book","Domain-Driven Design: Tackling Complexity in the Heart of Software","eric evans 2003"] và entity["book","Domain-Driven Design Reference: Definitions and Pattern Summaries","eric evans 2015"] (định nghĩa ubiquitous language/bounded context, building blocks, aggregates, repositories, factories, shared kernel, ACL, v.v.). citeturn3view0turn3view2turn13view0turn13view1turn20view0turn20view1  
- entity["people","Robert C. Martin","software engineer"]: dependency rule và tính độc lập framework/UI/DB. citeturn4view0  
- entity["people","Martin Fowler","software author"] và entity["people","Eric Evans","software author"]: Specification pattern và khi không nên dùng. citeturn18view0turn18view1  
- Microsoft Learn (.NET microservices DDD/CQRS patterns): domain vs integration events, CQRS separation, mediator pipeline, idempotent commands, mapping value objects với owned entity types. citeturn6view0turn6view1turn6view2turn8view0turn9view0  
- entity["company","GitHub","developer platform"] Docs và VS Code docs: cấu trúc copilot instructions files, `.github/instructions/*.instructions.md`, `AGENTS.md`, giới hạn 4.000 ký tự cho code review và khuyến nghị cấu trúc. citeturn5view0turn5view1turn5view2turn28view0turn28view1