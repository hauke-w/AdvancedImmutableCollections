using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AdvancedImmutableCollections.Tests.Util;
public static class TestDataContainer
{
    public static TestDataContainer<T> ToTestDataContainer<T>(this T value)
        where T : notnull => new TestDataContainer<T>(value);
}

/// <summary>
/// Container for any kind of serializable object that would not be correctly serialized by the test framework.
/// Do not use this class for other purposes than unit testing because it uses <see cref="BinaryFormatter"/> internally,
/// which is <a href="https://learn.microsoft.com/en-us/dotnet/standard/serialization/binaryformatter-security-guide"> inherently not secure</a>!
/// </summary>
/// <remarks>
/// For null values the container instance must be null.
/// </remarks>
/// <typeparam name="T">The type of serialized data</typeparam>
[Serializable]
public sealed class TestDataContainer<T> : ISerializable, IConvertible, ITestDataContainer<T>
    where T : notnull
{
    // TODO: we should add an analyzer that warns when this class is used outside of a unit test

    /// <summary>
    /// Initialízes a new container
    /// </summary>
    /// <param name="value">The value of this container. Must not be null and must be serializable.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null</exception>
    /// <exception cref="ArgumentException"><paramref name="value"/> is not an instance of a serializable type</exception>
    public TestDataContainer(T value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));

#pragma warning disable SYSLIB0050 // Type or member is obsolete
        if (!value.GetType().IsSerializable)
#pragma warning restore SYSLIB0050 // Type or member is obsolete
        {
            throw new ArgumentException($"The value's type {value.GetType().FullName} is not serializable");
        }
    }

    /// <summary>
    /// Initializes a deserialized instance. Called by the deserializer.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="streamingContext"></param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private TestDataContainer(SerializationInfo info, StreamingContext streamingContext)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
        var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        using var s = new MemoryStream();
        using var writer = new BinaryWriter(s);

        var data = (Array)info.GetValue(nameof(Value), typeof(Array))!;

        // when serialized by Visual studio the deserialized data is type of object[]
        // otherwise it is type of byte[]
        if (data is not byte[] byteArray)
        {
            byteArray = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                byteArray[i] = checked((byte)(int)data.GetValue(i)!);
            }
        }

        writer.Write(byteArray);
        s.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
        Value = (T)formatter.Deserialize(s);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
    }

    /// <summary>
    /// The value that is serialized / deserialized by this container
    /// </summary>
    public T Value { get; }

    object ITestDataContainer.Value => Value;

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
        var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        using var s = new MemoryStream();
#pragma warning disable SYSLIB0011 // Type or member is obsolete
        formatter.Serialize(s, Value);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        s.Position = 0;
        using var reader = new BinaryReader(s);
        var bytes = reader.ReadBytes(checked((int)s.Length));
        info.AddValue(nameof(Value), bytes, typeof(byte[]));
    }

    /// <summary>
    /// Gets the value of the container
    /// </summary>
    /// <param name="container"></param>
    [return: NotNullIfNotNull(nameof(container))]
    public static implicit operator T?(TestDataContainer<T>? container) => container is null ? default : container.Value;

    /// <summary>
    /// Creates a container for the specified <paramref name="value"/>
    /// </summary>
    /// <param name="value"></param>
    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator TestDataContainer<T>?(T? value) => value is null ? null : new TestDataContainer<T>(value);

    TypeCode IConvertible.GetTypeCode() => Type.GetTypeCode(typeof(T));

    private IConvertible ConvertibleValue => Value as IConvertible ?? throw new InvalidOperationException($"The {nameof(Value)} instance is null or does not implement IConvertible");
    bool IConvertible.ToBoolean(IFormatProvider? provider) => ConvertibleValue.ToBoolean(provider);

    byte IConvertible.ToByte(IFormatProvider? provider) => ConvertibleValue.ToByte(provider);

    char IConvertible.ToChar(IFormatProvider? provider) => ConvertibleValue.ToChar(provider);

    DateTime IConvertible.ToDateTime(IFormatProvider? provider) => ConvertibleValue.ToDateTime(provider);

    decimal IConvertible.ToDecimal(IFormatProvider? provider) => ConvertibleValue.ToDecimal(provider);

    double IConvertible.ToDouble(IFormatProvider? provider) => ConvertibleValue.ToDouble(provider);

    short IConvertible.ToInt16(IFormatProvider? provider) => ConvertibleValue.ToInt16(provider);

    int IConvertible.ToInt32(IFormatProvider? provider) => ConvertibleValue.ToInt32(provider);

    long IConvertible.ToInt64(IFormatProvider? provider) => ConvertibleValue.ToInt64(provider);

    sbyte IConvertible.ToSByte(IFormatProvider? provider) => ConvertibleValue.ToSByte(provider);

    float IConvertible.ToSingle(IFormatProvider? provider) => ConvertibleValue.ToSingle(provider);

    string IConvertible.ToString(IFormatProvider? provider) => ConvertibleValue.ToString(provider);

    object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
    {
        return conversionType.IsAssignableFrom(Value.GetType())
            ? Value
            : ConvertibleValue.ToType(conversionType, provider);
    }

    ushort IConvertible.ToUInt16(IFormatProvider? provider) => ConvertibleValue.ToUInt16(provider);

    uint IConvertible.ToUInt32(IFormatProvider? provider) => ConvertibleValue.ToUInt32(provider);

    ulong IConvertible.ToUInt64(IFormatProvider? provider) => ConvertibleValue.ToUInt64(provider);

    public override string? ToString() => Value.ToString();
}
