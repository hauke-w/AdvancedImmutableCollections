namespace AdvancedImmutableCollections.Tests.Util;

public interface ITestDataContainer
{
    object Value { get; }
}

/// <summary>
/// Interface for a container of any kind of object.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ITestDataContainer<out T> : ITestDataContainer
    where T : notnull
{
    /// <summary>
    /// The value that is serialized / deserialized by this container
    /// </summary>
    new T Value { get; }
}