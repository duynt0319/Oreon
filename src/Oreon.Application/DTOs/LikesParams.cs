using Oreon.Application.Common.Pagination;

namespace Oreon.Application.DTOs;

public class LikesParams : PaginationParams
{
    public int UserId { get; set; }
    public string Predicate { get; set; }
}
