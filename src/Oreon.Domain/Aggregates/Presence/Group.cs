namespace Oreon.Domain.Aggregates.Presence;

public sealed class Group
{
    private readonly List<Connection> _connections = new();

    public string Name { get; private set; }
    public IReadOnlyCollection<Connection> Connections => _connections.AsReadOnly();

    private Group() { }

    public Group(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Group name is required.", nameof(name));
        Name = name;
    }

    public void AddConnection(Connection connection)
    {
        if (connection is null)
            throw new ArgumentNullException(nameof(connection));
        _connections.Add(connection);
    }

    public bool RemoveConnection(string connectionId)
    {
        var connection = _connections.FirstOrDefault(c => c.ConnectionId == connectionId);
        if (connection is null)
            return false;
        _connections.Remove(connection);
        return true;
    }
}
