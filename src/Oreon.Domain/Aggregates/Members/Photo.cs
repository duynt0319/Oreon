using Oreon.Domain.Shared;

namespace Oreon.Domain.Aggregates.Members;

public sealed class Photo : Entity<PhotoId>
{
    public string Url { get; private set; }
    public bool IsMain { get; private set; }
    public string PublicId { get; private set; }
    public Guid AppUserId { get; private set; }

    private Photo() { }

    internal Photo(string url, string publicId, bool isMain, Guid memberId)
    {
        Url = url;
        PublicId = publicId;
        IsMain = isMain;
        AppUserId = memberId;
    }

    internal void SetIsMain(bool value) => IsMain = value;
}
