namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

[TestClass]
public class ImmutableSortedSetValueAdapterFactoryTests
{
    [TestMethod]
    public void IImmutableSetAdapterFactory_GetDefaultTest()
    {
        IImmutableSetAdapterFactory testObject = new ImmutableSortedSetValueAdapterFactory();
        var actual = testObject.GetDefaultValue<GenericParameterHelper>();
        VerifyIsDefault(actual);
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_GetDefaultTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableSortedSetValueAdapterFactory();
        var actual = testObject.GetDefaultValue<GenericParameterHelper>();
        VerifyIsDefault(actual);
    }

    private static void VerifyIsDefault(IImmutableCollectionAdapter<GenericParameterHelper>? actual)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(ImmutableSortedSetValueAdapter<GenericParameterHelper>));
        Assert.AreEqual(default(ImmutableSortedSetValue<GenericParameterHelper>), actual.Collection);
        Assert.IsTrue(((ImmutableSortedSetValue<GenericParameterHelper>)actual.Collection).IsDefault);
    }

    [TestMethod]
    public void IImmutableSetAdapterFactory_CastTest()
    {
        IImmutableSetAdapterFactory testObject = new ImmutableSortedSetValueAdapterFactory();
        var collection = ImmutableSortedSetValue.Create(new GenericParameterHelper(1), new GenericParameterHelper(2));
        var actual = testObject.Cast(collection);
        VerifyCast(actual, collection);
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_CastTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableSortedSetValueAdapterFactory();
        var collection = ImmutableSortedSetValue.Create(new GenericParameterHelper(1), new GenericParameterHelper(2));
        var actual = testObject.Cast(collection);
        VerifyCast(actual, collection);
    }

    private void VerifyCast(IImmutableCollectionAdapter<GenericParameterHelper> actual, ImmutableSortedSetValue<GenericParameterHelper> expected)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(ImmutableSortedSetValueAdapter<GenericParameterHelper>));
        Assert.IsInstanceOfType(actual.Collection, typeof(ImmutableSortedSetValue<GenericParameterHelper>));
        CollectionAssert.AreEquivalent(expected.ToList(), actual.Collection.ToList());
    }

    [TestMethod]
    public void IImmutableSetAdapterFactory_CreateTest()
    {
        IImmutableSetAdapterFactory testObject = new ImmutableSortedSetValueAdapterFactory();
        GenericParameterHelper[] items = [new GenericParameterHelper(1), new GenericParameterHelper(3), new GenericParameterHelper(2)];
        var actual = testObject.Create(items);
        VerifyCreate(items, actual);
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_CreateTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableSortedSetValueAdapterFactory();
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
        IImmutableSetAdapterFactory testObject = new ImmutableSortedSetValueAdapterFactory();
        GenericParameterHelper[] items = [new GenericParameterHelper(1), new GenericParameterHelper(3), new GenericParameterHelper(2)];
        var actual = testObject.Create((IEnumerable<GenericParameterHelper>)items);
        VerifyCreate(items, actual);
    }

    /// <summary>
    /// Verifies <see cref="IImmutableSortedSetAdapterFactory.Create{T}(IComparer{T}?, T[])"/> implementation.
    /// </summary>
    [TestMethod]
    public void IImmutableSortedSetAdapterFactory_Create_IComparer_Array_Test()
    {
        IImmutableSortedSetAdapterFactory testObject = new ImmutableSortedSetValueAdapterFactory();
        string[] items = ["a", "C", "b", "A"];
        var actual = testObject.Create(StringComparer.InvariantCulture, items);
        VerifyCreate(["a", "A", "b", "C"], actual, StringComparer.InvariantCulture);

        actual = testObject.Create(StringComparer.InvariantCultureIgnoreCase, items);
        VerifyCreate(["a", "b", "C"], actual, StringComparer.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Verifies <see cref="IImmutableSortedSetAdapterFactory.Create{T}(IComparer{T}?)"/> implementation.
    /// </summary>
    [TestMethod]
    public void IImmutableSetWithEqualityComparerAdapterFactory_Create_IComparer_Test()
    {
        IImmutableSortedSetAdapterFactory testObject = new ImmutableSortedSetValueAdapterFactory();
        var actual = testObject.Create(StringComparer.Ordinal);
        VerifyCreate([], actual, StringComparer.Ordinal);

        actual = testObject.Create(StringComparer.OrdinalIgnoreCase);
        VerifyCreate([], actual, StringComparer.OrdinalIgnoreCase);
    }

    private static void VerifyCreate<T>(T[] expected, IImmutableCollectionAdapter<T> actual, IComparer<T>? expectedComparer = null)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(ImmutableSortedSetValueAdapter<T>));
        Assert.IsInstanceOfType(actual.Collection, typeof(ImmutableSortedSetValue<T>));
        CollectionAssert.AreEquivalent(expected, actual.Collection.ToList());

        var actualCollection = (ImmutableSortedSetValue<T>)actual.Collection;
        expectedComparer ??= Comparer<T>.Default;
        Assert.AreEqual(expectedComparer, actualCollection.Value.KeyComparer);
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_CreateMutableTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableSortedSetValueAdapterFactory();
        GenericParameterHelper[] items = [new GenericParameterHelper(1), new GenericParameterHelper(3), new GenericParameterHelper(2)];
        var actual = testObject.CreateMutable(items);
        VerifyCreateMutable(items, actual);
    }

    private static void VerifyCreateMutable(GenericParameterHelper[] items, ICollection<GenericParameterHelper> actual)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(SortedSet<GenericParameterHelper>));
        CollectionAssert.AreEquivalent(items, actual.ToList());
    }
}