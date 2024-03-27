namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

[TestClass]
public class ImmutableArrayValueAdapterFactoryTests
{
    [TestMethod]
    public void IImmutableListAdapterFactory_GetDefaultTest()
    {
        IImmutableListAdapterFactory testObject = new ImmutableArrayValueAdapterFactory();
        var actual = testObject.GetDefaultValue<GenericParameterHelper>();
        VerifyIsDefault(actual);
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_GetDefaultTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableArrayValueAdapterFactory();
        var actual = testObject.GetDefaultValue<GenericParameterHelper>();
        VerifyIsDefault(actual);
    }

    private static void VerifyIsDefault(IImmutableCollectionAdapter<GenericParameterHelper>? actual)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(ImmutableArrayValueAdapter<GenericParameterHelper>));
        Assert.AreEqual(default(ImmutableArrayValue<GenericParameterHelper>), actual.Collection);
        Assert.IsTrue(((ImmutableArrayValue<GenericParameterHelper>)actual.Collection).IsDefault);
    }

    [TestMethod]
    public void IImmutableListAdapterFactory_CastTest()
    {
        IImmutableListAdapterFactory testObject = new ImmutableArrayValueAdapterFactory();
        var collection = ImmutableArrayValue.Create(new GenericParameterHelper(1), new GenericParameterHelper(2));
        var actual = testObject.Cast(collection);
        VerifyCast(actual, collection);
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_CastTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableArrayValueAdapterFactory();
        var collection = ImmutableArrayValue.Create(new GenericParameterHelper(1), new GenericParameterHelper(2));
        var actual = testObject.Cast(collection);
        VerifyCast(actual, collection);
    }

    private void VerifyCast(IImmutableCollectionAdapter<GenericParameterHelper> actual, ImmutableArrayValue<GenericParameterHelper> expected)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(ImmutableArrayValueAdapter<GenericParameterHelper>));
        Assert.IsInstanceOfType(actual.Collection, typeof(ImmutableArrayValue<GenericParameterHelper>));
        CollectionAssert.AreEqual(expected.ToList(), actual.Collection.ToList());
    }

    [TestMethod]
    public void IImmutableListAdapterFactory_CreateTest()
    {
        IImmutableListAdapterFactory testObject = new ImmutableArrayValueAdapterFactory();
        GenericParameterHelper[] items = [new GenericParameterHelper(1), new GenericParameterHelper(3), new GenericParameterHelper(2)];
        var actual = testObject.Create(items);
        VerifyCreate(items, actual);
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_CreateTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableArrayValueAdapterFactory();
        GenericParameterHelper[] items = [new GenericParameterHelper(1), new GenericParameterHelper(3), new GenericParameterHelper(2)];
        var actual = testObject.Create(items);
        VerifyCreate(items, actual);
    }

    private static void VerifyCreate(GenericParameterHelper[] items, IImmutableCollectionAdapter<GenericParameterHelper> actual)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(ImmutableArrayValueAdapter<GenericParameterHelper>));
        Assert.IsInstanceOfType(actual.Collection, typeof(ImmutableArrayValue<GenericParameterHelper>));
        CollectionAssert.AreEqual(items, actual.Collection.ToList());
    }

    [TestMethod]
    public void IImmutableCollectionAdapterFactory_CreateMutableTest()
    {
        IImmutableCollectionAdapterFactory testObject = new ImmutableArrayValueAdapterFactory();
        GenericParameterHelper[] items = [new GenericParameterHelper(1), new GenericParameterHelper(3), new GenericParameterHelper(2)];
        var actual = testObject.CreateMutable(items);
        VerifyCreateMutable(items, actual);
    }

    private static void VerifyCreateMutable(GenericParameterHelper[] items, ICollection<GenericParameterHelper> actual)
    {
        Assert.IsNotNull(actual);
        Assert.IsInstanceOfType(actual, typeof(List<GenericParameterHelper>));
        CollectionAssert.AreEqual(items, (List<GenericParameterHelper>)actual);
    }
}