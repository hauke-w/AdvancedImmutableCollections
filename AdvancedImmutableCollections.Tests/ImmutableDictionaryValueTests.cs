using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
#if !NET6_0_OR_GREATER
using KeyValuePair = System.KeyValuePairExtensions;
#endif

namespace AdvancedImmutableCollections;

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

#if NET8_0_OR_GREATER
    [TestMethod]
    public void Create_ReadOnlySpan_Test()
    {
        KeyValuePair<int, GenericParameterHelper>[] array = [new(0, new(0)), new(1, new(1))];
        ReadOnlySpan<KeyValuePair<int, GenericParameterHelper>> items = array;
        var actual = ImmutableDictionaryValue.Create(items);
        Assert.IsNotNull(actual);
        Assert.AreEqual(2, actual.Value.Count);
        CollectionAssert.AreEquivalent(array, actual.ToArray());
    }
#endif

    [TestMethod]
    public void Create_IEnumerable_Test()
    {
        KeyValuePair<string, GenericParameterHelper>[] array = [new("a", new(0)), new("b", new(1)), new("c", new(2))];
        IEnumerable<KeyValuePair<string, GenericParameterHelper>> items = array;
        var actual = ImmutableDictionaryValue.Create(items);
        Assert.IsNotNull(actual);
        Assert.AreEqual(3, actual.Value.Count);
        CollectionAssert.AreEquivalent(array, actual.ToArray());
    }

    [TestMethod]
    public void Create_Array_Test()
    {
        KeyValuePair<string, GenericParameterHelper>[] array = [new("a", new(0)), new("b", new(1)), new("c", new(2))];
        var actual = ImmutableDictionaryValue.Create(array.ToArray());
        Assert.IsNotNull(actual);
        Assert.AreEqual(3, actual.Value.Count);
        CollectionAssert.AreEquivalent(array, actual.ToArray());
    }

    [TestMethod]
    public void Create_IEnumerable_IEqualityComparer_IEqualityComparer_Test()
    {
        var itemA1 = KeyValuePair.Create("a", "value=a");
        var itemA2 = KeyValuePair.Create("A", "value=A");
        var itemB1 = KeyValuePair.Create("b", "value=b");
        var itemB2 = KeyValuePair.Create("B", "value=B");
        var itemB3 = KeyValuePair.Create("b", "value=B");

        Create_IEnumerable_Test([itemA1, itemA2, itemB1, itemB2], [itemA1, itemB1], keyComparer: StringComparer.OrdinalIgnoreCase, valueComparer: StringComparer.OrdinalIgnoreCase);
        Create_IEnumerable_Test([itemA1, itemA2, itemA1, itemA2], [itemA1, itemA2]);
        Create_IEnumerable_Test([itemA1, itemA2, itemB1, itemB2], [itemA1, itemA2, itemB1, itemB2]);
        Create_IEnumerable_Test([itemA1, itemA2, itemB1, itemB2], [itemA1, itemA2, itemB1, itemB2], StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase);

        Assert.ThrowsException<ArgumentException>(() => ImmutableDictionaryValue.Create([itemA1, itemA2, itemB1, itemB2], StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal));
        Assert.ThrowsException<ArgumentException>(() => ImmutableDictionaryValue.Create([itemA1, itemA2, itemB1, itemB2, itemB3], StringComparer.Ordinal, StringComparer.Ordinal));

        static void Create_IEnumerable_Test(IEnumerable<KeyValuePair<string, string>> items, KeyValuePair<string, string>[] expected,
            IEqualityComparer<string>? keyComparer = null, IEqualityComparer<string>? valueComparer = null)
        {
            var actual = ImmutableDictionaryValue.Create(items, keyComparer, valueComparer);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Count);
            CollectionAssert.AreEquivalent(expected, actual.ToArray());
        }
    }
}
