using Oreon.Application.Common.Pagination;

namespace Oreon.Application.DTOs;

public class MessageParams : PaginationParams
{
    public string Username { get; set; }
    public string Container { get; set; } = "Unread";
}
