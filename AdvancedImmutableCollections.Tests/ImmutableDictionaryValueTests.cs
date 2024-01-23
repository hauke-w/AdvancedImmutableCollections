using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests;

[TestClass]
public class ImmutableDictionaryValueTests
{
    [TestMethod]
    public void WithValueSemanticsTest()
    {
        WithValueSemanticsTest(ImmutableDictionary<int, GenericParameterHelper>.Empty);
        WithValueSemanticsTest(ImmutableDictionary.Create<string, int>().Add("a", 1));

        void WithValueSemanticsTest<TKey, TValue>(ImmutableDictionary<TKey, TValue> value)
            where TKey : notnull
        {
            var actual = ImmutableDictionaryValue.WithValueSemantics(value);
            Assert.IsNotNull(actual);
            Assert.AreSame(value, actual.Value);
        }
    }
}
