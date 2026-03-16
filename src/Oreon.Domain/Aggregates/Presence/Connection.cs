namespace Oreon.Domain.Aggregates.Presence;

public sealed class Connection
{
    public string ConnectionId { get; private set; }
    public string Username { get; private set; }

    private Connection() { }

    public Connection(string connectionId, string username)
    {
        if (string.IsNullOrWhiteSpace(connectionId))
            throw new ArgumentException("ConnectionId is required.", nameof(connectionId));
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.", nameof(username));

        ConnectionId = connectionId;
        Username = username;
    }
}
