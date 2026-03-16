using Oreon.Domain.DomainEvents;
using Oreon.Domain.Exceptions;
using Oreon.Domain.Shared;

namespace Oreon.Domain.Aggregates.Members;

public sealed class Member : AggregateRoot<MemberId>
{
    private readonly List<Photo> _photos = new();

    private Member() { }

    public Guid AppUserId { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    public string KnownAs { get; private set; } = string.Empty;

    public DateTime Created { get; private set; }

    public DateTime LastActive { get; private set; }

    public string Gender { get; private set; } = string.Empty;

    public string Introduction { get; private set; } = string.Empty;

    public string LookingFor { get; private set; } = string.Empty;

    public string Interests { get; private set; } = string.Empty;

    public string City { get; private set; } = string.Empty;

    public string Country { get; private set; } = string.Empty;

    public IReadOnlyCollection<Photo> Photos => _photos.AsReadOnly();

    /// <summary>
    /// Creates a new Member and binds it to an AppUser.
    /// </summary>
    public static Member Create(
        Guid appUserId,
        DateOnly dateOfBirth,
        string knownAs,
        string gender,
        string city,
        string country
    )
    {
        Guard.AgainstCondition(appUserId == Guid.Empty, "AppUserId is required.");
        Guard.AgainstNullOrWhiteSpace(knownAs, nameof(knownAs));
        Guard.AgainstNullOrWhiteSpace(gender, nameof(gender));
        Guard.AgainstNullOrWhiteSpace(city, nameof(city));
        Guard.AgainstNullOrWhiteSpace(country, nameof(country));

        var age = DateHelper.CalculateAge(dateOfBirth);
        if (age < 18)
        {
            throw new InvalidAgeException();
        }

        var utcNow = DateTime.UtcNow;

        return new Member
        {
            AppUserId = appUserId,
            DateOfBirth = dateOfBirth,
            KnownAs = knownAs.Trim(),
            Gender = gender.Trim(),
            City = city.Trim(),
            Country = country.Trim(),
            Created = utcNow,
            LastActive = utcNow,
            Introduction = string.Empty,
            LookingFor = string.Empty,
            Interests = string.Empty,
        };
    }

    /// <summary>
    /// Reconstitutes a Member from persistence.
    /// </summary>
    public static Member FromPersistence(
        Guid id,
        Guid appUserId,
        DateOnly dateOfBirth,
        string knownAs,
        DateTime created,
        DateTime lastActive,
        string gender,
        string introduction,
        string lookingFor,
        string interests,
        string city,
        string country
    )
    {
        return new Member
        {
            Id = MemberId.Of(id),
            AppUserId = appUserId,
            DateOfBirth = dateOfBirth,
            KnownAs = knownAs,
            Created = created,
            LastActive = lastActive,
            Gender = gender,
            Introduction = introduction,
            LookingFor = lookingFor,
            Interests = interests,
            City = city,
            Country = country,
        };
    }

    public void UpdateLastActive()
    {
        LastActive = DateTime.UtcNow;
    }

    public void UpdateProfile(
        string introduction,
        string lookingFor,
        string interests,
        string city,
        string country
    )
    {
        Introduction = introduction?.Trim() ?? string.Empty;
        LookingFor = lookingFor?.Trim() ?? string.Empty;
        Interests = interests?.Trim() ?? string.Empty;
        City = city?.Trim() ?? string.Empty;
        Country = country?.Trim() ?? string.Empty;
    }

    public Photo AddPhoto(string url, string publicId)
    {
        Guard.AgainstNullOrWhiteSpace(url, nameof(url));

        var isMain = !_photos.Any();
        var photo = new Photo(url, publicId, isMain, Id);
        _photos.Add(photo);

        AddDomainEvent(new PhotoUploadedDomainEvent(Id, DateTimeOffset.UtcNow));

        return photo;
    }

    public void DeletePhoto(Guid photoId)
    {
        var photo =
            _photos.FirstOrDefault(p => p.Id != null && p.Id.Value == photoId)
            ?? throw new InvalidOperationException($"Photo {photoId} not found.");

        Guard.AgainstCondition(photo.IsMain, "Cannot delete the main photo.");

        _photos.Remove(photo);
    }

    public void SetMainPhoto(Guid photoId)
    {
        var photo =
            _photos.FirstOrDefault(p => p.Id != null && p.Id.Value == photoId)
            ?? throw new InvalidOperationException($"Photo {photoId} not found.");

        if (photo.IsMain)
        {
            return;
        }

        foreach (var p in _photos)
        {
            p.SetIsMain(p.Id != null && p.Id.Value == photoId);
        }
    }

    internal void AddExistingPhoto(Photo photo)
    {
        _photos.Add(photo);
    }
}
