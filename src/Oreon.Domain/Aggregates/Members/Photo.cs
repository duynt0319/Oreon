using Oreon.Domain.Shared;

namespace Oreon.Domain.Aggregates.Members;

public sealed class Photo : Entity<PhotoId>
{
    public string Url { get; private set; }
    public bool IsMain { get; private set; }
    public string PublicId { get; private set; }
    public MemberId MemberId { get; private set; }

    private Photo() { }

    internal Photo(string url, string publicId, bool isMain, MemberId memberId)
    {
        Url = url;
        PublicId = publicId;
        IsMain = isMain;
        MemberId = memberId;
    }

    internal void SetIsMain(bool value) => IsMain = value;
}
