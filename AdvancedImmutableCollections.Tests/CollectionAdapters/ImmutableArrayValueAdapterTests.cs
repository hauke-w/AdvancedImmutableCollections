namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

[TestClass]
public class ImmutableArrayValueAdapterTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var collection = ImmutableArrayValue.Create<GenericParameterHelper>();
        var actual = new ImmutableArrayValueAdapter<GenericParameterHelper>(collection);
        Assert.AreEqual(collection, actual.Collection);
    }

    [TestMethod]
    public void Constructor_Array_Test()
    {
        GenericParameterHelper[] items = [ new GenericParameterHelper(1), new GenericParameterHelper(2)];
        var actual = new ImmutableArrayValueAdapter<GenericParameterHelper>(items);        
        CollectionAssert.AreEqual(items, actual.ToList());
    }
}