namespace AdvancedImmutableCollections;

[TestClass]
public class ImmutableSortedSetValueTests
{
    [TestMethod]
    public void EmptyTest()
    {
        var actual = ImmutableSortedSetValue.Empty<GenericParameterHelper>();
        Assert.IsFalse(actual.IsDefault);
        Assert.AreEqual(0, actual.Count);
        Assert.AreEqual(0, actual.Value.Count);
    }
}