namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

[TestClass]
public class ImmutableHashSetValueAdapterTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var collection = ImmutableHashSetValue.Create<GenericParameterHelper>();
        var actual = new ImmutableHashSetValueAdapter<GenericParameterHelper>(collection);
        Assert.AreEqual(collection, actual.Collection);
        Assert.AreEqual(EqualityComparer<GenericParameterHelper>.Default, actual.Collection.Value.KeyComparer);
    }

    [TestMethod]
    public void Constructor_Array_Test()
    {
        GenericParameterHelper[] items = [new GenericParameterHelper(1), new GenericParameterHelper(2)];
        var actual = new ImmutableHashSetValueAdapter<GenericParameterHelper>(items);
        CollectionAssert.AreEquivalent(items, actual.ToList());
        Assert.AreEqual(EqualityComparer<GenericParameterHelper>.Default, actual.Collection.Value.KeyComparer);
    }

    [TestMethod]
    public void Constructor_IEqualityComparer_Array_Test()
    {
        Constructor_IEqualityComparer_Array_Test(["a", "b", "C"], StringComparer.Ordinal);
        Constructor_IEqualityComparer_Array_Test(["a", "b", "C"], null);

        static void Constructor_IEqualityComparer_Array_Test(string[] items, IEqualityComparer<string>? comparer)
        {
            var actual = new ImmutableHashSetValueAdapter<string>(comparer, items);
            CollectionAssert.AreEquivalent(items, actual.ToList());
            var expectedComparer = comparer ?? EqualityComparer<string>.Default;
            Assert.AreEqual(expectedComparer, actual.Collection.Value.KeyComparer);
        }
    }
}