using Oreon.Domain.DomainEvents;
using Oreon.Domain.Exceptions;
using Oreon.Domain.Shared;

namespace Oreon.Domain.Aggregates.Members;

public sealed class Member : AggregateRoot<MemberId>
{
    private readonly List<Photo> _photos = new();

    private Member() { }

    public string Username { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public string KnownAs { get; private set; }
    public DateTime Created { get; private set; }
    public DateTime LastActive { get; private set; }
    public string Gender { get; private set; }
    public string Introduction { get; private set; }
    public string LookingFor { get; private set; }
    public string Interests { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }

    public IReadOnlyCollection<Photo> Photos => _photos.AsReadOnly();

    /// <summary>Creates a new Member (validates business rules).</summary>
    public static Member Create(
        string username,
        DateOnly dateOfBirth,
        string knownAs,
        string gender,
        string city,
        string country)
    {
        Guard.AgainstNullOrWhiteSpace(username, nameof(username));
        Guard.AgainstNullOrWhiteSpace(knownAs, nameof(knownAs));
        Guard.AgainstNullOrWhiteSpace(gender, nameof(gender));

        var age = DateHelper.CalculateAge(dateOfBirth);
        if (age < 18) throw new InvalidAgeException();

        return new Member
        {
            Username = username.ToLowerInvariant(),
            DateOfBirth = dateOfBirth,
            KnownAs = knownAs,
            Gender = gender,
            City = city,
            Country = country,
            Created = DateTime.UtcNow,
            LastActive = DateTime.UtcNow
        };
    }

    /// <summary>Reconstitutes a Member from persisted data (no validation).</summary>
    public static Member FromPersistence(
        Guid id,
        string username,
        DateOnly dateOfBirth,
        string knownAs,
        DateTime created,
        DateTime lastActive,
        string gender,
        string introduction,
        string lookingFor,
        string interests,
        string city,
        string country)
    {
        return new Member
        {
            Id = new MemberId(id),
            Username = username,
            DateOfBirth = dateOfBirth,
            KnownAs = knownAs,
            Created = created,
            LastActive = lastActive,
            Gender = gender,
            Introduction = introduction,
            LookingFor = lookingFor,
            Interests = interests,
            City = city,
            Country = country
        };
    }

    public void UpdateLastActive() => LastActive = DateTime.UtcNow;

    public void UpdateProfile(
        string introduction,
        string lookingFor,
        string interests,
        string city,
        string country)
    {
        Introduction = introduction;
        LookingFor = lookingFor;
        Interests = interests;
        City = city;
        Country = country;
    }

    public Photo AddPhoto(string url, string publicId)
    {
        Guard.AgainstNullOrWhiteSpace(url, nameof(url));

        var isMain = !_photos.Any();
        var photo = new Photo(url, publicId, isMain, Id.Value);
        _photos.Add(photo);

        AddDomainEvent(new PhotoUploadedDomainEvent(Id, DateTimeOffset.UtcNow));
        return photo;
    }

    public void DeletePhoto(Guid photoId)
    {
        var photo = _photos.FirstOrDefault(p => p.Id != null && p.Id.Value == photoId)
            ?? throw new InvalidOperationException($"Photo {photoId} not found.");

        Guard.AgainstCondition(photo.IsMain, "Cannot delete the main photo.");

        _photos.Remove(photo);
    }

    public void SetMainPhoto(Guid photoId)
    {
        var photo = _photos.FirstOrDefault(p => p.Id != null && p.Id.Value == photoId)
            ?? throw new InvalidOperationException($"Photo {photoId} not found.");

        if (photo.IsMain) return;

        foreach (var p in _photos)
            p.SetIsMain(p.Id != null && p.Id.Value == photoId);
    }

    /// <summary>Used by repository when reconstituting from persistence.</summary>
    internal void AddExistingPhoto(Photo photo) => _photos.Add(photo);
}
