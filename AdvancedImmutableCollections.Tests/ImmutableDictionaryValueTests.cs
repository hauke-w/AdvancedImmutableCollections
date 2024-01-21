using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests;

[TestClass]
public class ImmutableDictionaryValueTests
{
    [TestMethod]
    public void CtorTest()
    {
        CtorTest(ImmutableDictionary.Create<string, GenericParameterHelper>());
        CtorTest(ImmutableDictionary.Create<string, int>().Add("1", 1).Add("2", 2));


        static void CtorTest<TKey, TValue>(ImmutableDictionary<TKey, TValue> value)
            where TKey : notnull
        {
            var actual = new ImmutableDictionaryValue<TKey, TValue>(value);
            Assert.AreSame(value, actual.Value);
            Assert.AreEqual(value.Count, actual.Count);
        }
    }
}
