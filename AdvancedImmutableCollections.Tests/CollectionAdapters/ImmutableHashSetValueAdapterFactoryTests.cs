namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

[TestClass]
public class ImmutableHashSetValueAdapterFactoryTests
{
    [TestMethod]
    public void IImmutableSetAdapterFactory_GetDefaultTest()
    {
        IImmutableSetAdapterFactory testObject = new ImmutableHashSetValueAdapterFactory();
        var actual = testObject.GetDefaultValue<GenericParameterHelper>();
        VerifyIsDefault(actual);
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_GetDefaultTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableHashSetValueAdapterFactory();
        var actual = testObject.GetDefaultValue<GenericParameterHelper>();
        VerifyIsDefault(actual);
    }

    private static void VerifyIsDefault(IImmutableCollectionAdapter<GenericParameterHelper>? actual)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(ImmutableHashSetValueAdapter<GenericParameterHelper>));
        Assert.AreEqual(default(ImmutableHashSetValue<GenericParameterHelper>), actual.Collection);
        Assert.IsTrue(((ImmutableHashSetValue<GenericParameterHelper>)actual.Collection).IsDefault);
    }

    [TestMethod]
    public void IImmutableSetAdapterFactory_CastTest()
    {
        IImmutableSetAdapterFactory testObject = new ImmutableHashSetValueAdapterFactory();
        var collection = ImmutableHashSetValue.Create(new GenericParameterHelper(1), new GenericParameterHelper(2));
        var actual = testObject.Cast(collection);
        VerifyCast(actual, collection);
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_CastTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableHashSetValueAdapterFactory();
        var collection = ImmutableHashSetValue.Create(new GenericParameterHelper(1), new GenericParameterHelper(2));
        var actual = testObject.Cast(collection);
        VerifyCast(actual, collection);
    }

    private void VerifyCast(IImmutableCollectionAdapter<GenericParameterHelper> actual, ImmutableHashSetValue<GenericParameterHelper> expected)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(ImmutableHashSetValueAdapter<GenericParameterHelper>));
        Assert.IsInstanceOfType(actual.Collection, typeof(ImmutableHashSetValue<GenericParameterHelper>));
        CollectionAssert.AreEquivalent(expected.ToList(), actual.Collection.ToList());
    }

    [TestMethod]
    public void IImmutableSetAdapterFactory_CreateTest()
    {
        IImmutableSetAdapterFactory testObject = new ImmutableHashSetValueAdapterFactory();
        GenericParameterHelper[] items = [new GenericParameterHelper(1), new GenericParameterHelper(3), new GenericParameterHelper(2)];
        var actual = testObject.Create(items);
        VerifyCreate(items, actual);
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_CreateTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableHashSetValueAdapterFactory();
        GenericParameterHelper[] items = [new GenericParameterHelper(1), new GenericParameterHelper(3), new GenericParameterHelper(2)];
        var actual = testObject.Create(items);
        VerifyCreate(items, actual);
    }

    /// <summary>
    /// Verifies <see cref="IImmutableSetAdapterFactory.Create{T}(IEnumerable{T})"/> implementation.
    /// </summary>
    [TestMethod]
    public void IImmutableSetAdapterFactory_Create_IEnumerable_Test()
    {
        IImmutableSetAdapterFactory testObject = new ImmutableHashSetValueAdapterFactory();
        GenericParameterHelper[] items = [new GenericParameterHelper(1), new GenericParameterHelper(3), new GenericParameterHelper(2)];
        var actual = testObject.Create((IEnumerable<GenericParameterHelper>)items);
        VerifyCreate(items, actual);
    }

    /// <summary>
    /// Verifies <see cref="IImmutableSetWithEqualityComparerAdapterFactory.Create{T}(IEqualityComparer{T}?, T[])"/> implementation.
    /// </summary>
    [TestMethod]
    public void IImmutableSetWithEqualityComparerAdapterFactory_Create_IEqualityComparer_Array_Test()
    {
        IImmutableSetWithEqualityComparerAdapterFactory testObject = new ImmutableHashSetValueAdapterFactory();
        string[] items = ["a", "b", "A", "C"];
        var actual = testObject.Create(StringComparer.Ordinal, items);
        VerifyCreate(items, actual, StringComparer.Ordinal);

        actual = testObject.Create(StringComparer.OrdinalIgnoreCase, items);
        VerifyCreate(["a", "b", "C"], actual, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Verifies <see cref="IImmutableSetWithEqualityComparerAdapterFactory.Create{T}(IEqualityComparer{T}?)"/> implementation.
    /// </summary>
    [TestMethod]
    public void IImmutableSetWithEqualityComparerAdapterFactory_Create_IEqualityComparer_Test()
    {
        IImmutableSetWithEqualityComparerAdapterFactory testObject = new ImmutableHashSetValueAdapterFactory();
        var actual = testObject.Create(StringComparer.Ordinal);
        VerifyCreate([], actual, StringComparer.Ordinal);

        actual = testObject.Create(StringComparer.OrdinalIgnoreCase);
        VerifyCreate([], actual, StringComparer.OrdinalIgnoreCase);
    }

    private static void VerifyCreate<T>(T[] expected, IImmutableCollectionAdapter<T> actual, IEqualityComparer<T>? expectedComparer = null)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(ImmutableHashSetValueAdapter<T>));
        Assert.IsInstanceOfType(actual.Collection, typeof(ImmutableHashSetValue<T>));
        CollectionAssert.AreEquivalent(expected, actual.Collection.ToList());

        var actualCollection = (ImmutableHashSetValue<T>)actual.Collection;
        expectedComparer ??= EqualityComparer<T>.Default;
        Assert.AreEqual(expectedComparer, actualCollection.Value.KeyComparer);
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_CreateMutableTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableHashSetValueAdapterFactory();
        GenericParameterHelper[] items = [new GenericParameterHelper(1), new GenericParameterHelper(3), new GenericParameterHelper(2)];
        var actual = testObject.CreateMutable(items);
        VerifyCreateMutable(items, actual);
    }

    private static void VerifyCreateMutable(GenericParameterHelper[] items, ICollection<GenericParameterHelper> actual)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(HashSet<GenericParameterHelper>));
        CollectionAssert.AreEquivalent(items, actual.ToList());
    }
}