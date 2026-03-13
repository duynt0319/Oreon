namespace Oreon.Domain.Shared;

public static class Guard
{
    public static string AgainstNullOrWhiteSpace(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"'{paramName}' cannot be null or whitespace.", paramName);
        return value;
    }

    public static T AgainstNull<T>(T value, string paramName) where T : class
    {
        if (value is null) throw new ArgumentNullException(paramName);
        return value;
    }

    public static int AgainstNonPositive(int value, string paramName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(paramName, $"'{paramName}' must be positive.");
        return value;
    }

    public static void AgainstCondition(bool condition, string message)
    {
        if (condition) throw new InvalidOperationException(message);
    }
}
