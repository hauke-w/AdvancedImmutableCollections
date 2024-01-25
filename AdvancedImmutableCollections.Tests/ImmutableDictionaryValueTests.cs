using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.Diagnostics;
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

        CreateTest([itemA1, itemA2, itemB1, itemB2], [itemA1, itemB1], keyComparer: StringComparer.OrdinalIgnoreCase, valueComparer: StringComparer.OrdinalIgnoreCase);
        CreateTest([itemA1, itemA2, itemA1, itemA2], [itemA1, itemA2]);
        CreateTest([itemA1, itemA2, itemB1, itemB2], [itemA1, itemA2, itemB1, itemB2]);
        CreateTest([itemA1, itemA2, itemB1, itemB2], [itemA1, itemA2, itemB1, itemB2], StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase);

        Assert.ThrowsException<ArgumentException>(() => ImmutableDictionaryValue.Create([itemA1, itemA2, itemB1, itemB2], StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal));
        Assert.ThrowsException<ArgumentException>(() => ImmutableDictionaryValue.Create([itemA1, itemA2, itemB1, itemB2, itemB3], StringComparer.Ordinal, StringComparer.Ordinal));

        static void CreateTest(IEnumerable<KeyValuePair<string, string>> items, KeyValuePair<string, string>[] expected,
            IEqualityComparer<string>? keyComparer = null, IEqualityComparer<string>? valueComparer = null)
        {
            var actual = ImmutableDictionaryValue.Create(items, keyComparer, valueComparer);
            Assert.IsFalse(actual.IsDefault);
            Assert.AreEqual(expected.Length, actual.Count);
            CollectionAssert.AreEquivalent(expected, actual.ToArray());
        }
    }

    [TestMethod]
    public void Create_IEqualityComparer_IEqualityComparer_Test()
    {
        CreateTest<string, string?>(StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase);
        CreateTest<string, string?>(StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal);
        CreateTest<GenericParameterHelper, int>(null, null);

        static void CreateTest<TKey, TValue>(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            where TKey : notnull
        {
            var actual = ImmutableDictionaryValue.Create(keyComparer, valueComparer);
            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.IsDefault);
            Assert.AreSame(keyComparer ?? EqualityComparer<TKey>.Default, actual.Value.KeyComparer);
            Assert.AreSame(valueComparer ?? EqualityComparer<TValue>.Default, actual.Value.ValueComparer);
        }
    }

    [TestMethod]
    public void SetEquals_WithComparers_Test()
    {
        var empty = ImmutableDictionary<string, string>.Empty;
        SetEqualsTest(empty, empty, null, null, true);
        SetEqualsTest(empty, empty, StringComparer.OrdinalIgnoreCase, null, true);
        SetEqualsTest(empty, empty, null, StringComparer.OrdinalIgnoreCase, true);
        SetEqualsTest(empty, empty, StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, true);
        SetEqualsTest(empty, empty, StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase, true);
        SetEqualsTest(empty, empty, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase, true);


        SetEqualsTest(empty, empty, null, null, true);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "a"), null, null, true);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "A"), null, null, false);
        SetEqualsTest(empty.Add("A", "a"), empty.Add("a", "a"), null, null, false);

        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "A"), StringComparer.Ordinal, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "A"), StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase, true);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "A"), StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "A"), StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase, true);

        SetEqualsTest(empty.Add("a", "a"), empty.Add("A", "a"), StringComparer.Ordinal, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("A", "a"), StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("A", "a"), StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, true);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("A", "a"), StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase, true);

        SetEqualsTest(empty.Add("a", "a"), empty.Add("A", "A"), StringComparer.Ordinal, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("A", "A"), StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("A", "A"), StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("A", "A"), StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase, true);

        // different count
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "a").Add("A", "A"), StringComparer.Ordinal, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "a").Add("A", "A"), StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "a").Add("A", "A"), StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "a").Add("A", "A"), StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase, true);
        SetEqualsTest(empty.Add("a", "a").Add("A", "A"), empty.Add("a", "a"), StringComparer.Ordinal, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a").Add("A", "A"), empty.Add("a", "a"), StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a").Add("A", "A"), empty.Add("a", "a"), StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase, false);
        SetEqualsTest(empty.Add("a", "a").Add("A", "A"), empty.Add("a", "a"), StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase, true);

        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "a").Add("A", "a"), StringComparer.Ordinal, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "a").Add("A", "a"), StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, true);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "a").Add("A", "a"), StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase, false);
        SetEqualsTest(empty.Add("a", "a"), empty.Add("a", "a").Add("A", "a"), StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase, true);
        SetEqualsTest(empty.Add("a", "a").Add("A", "a"), empty.Add("a", "a"), StringComparer.Ordinal, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a").Add("A", "a"), empty.Add("a", "a"), StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, true);
        SetEqualsTest(empty.Add("a", "a").Add("A", "a"), empty.Add("a", "a"), StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase, false);
        SetEqualsTest(empty.Add("a", "a").Add("A", "a"), empty.Add("a", "a"), StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase, true);

        var key1a = new GenericParameterHelper(1);
        var key1b = new GenericParameterHelper(1);
        var empty2 = ImmutableDictionary<GenericParameterHelper, string>.Empty.WithComparers(ReferenceEqualityComparer.Instance);
        SetEqualsTest(empty2.Add(key1a, "a"), empty2.Add(key1a, "a").Add(key1b, "A"), ReferenceEqualityComparer.Instance, StringComparer.Ordinal, false); // right contains key not in left
        SetEqualsTest(empty2.Add(key1a, "a"), empty2.Add(key1a, "a").Add(key1b, "A"), null, StringComparer.Ordinal, false);
        SetEqualsTest(empty2.Add(key1a, "a"), empty2.Add(key1a, "a").Add(key1b, "A"), ReferenceEqualityComparer.Instance, StringComparer.OrdinalIgnoreCase, false); // right contains key not in left
        SetEqualsTest(empty2.Add(key1a, "a"), empty2.Add(key1a, "a").Add(key1b, "A"), null, StringComparer.OrdinalIgnoreCase, true);
        SetEqualsTest(empty2.Add(key1a, "a").Add(key1b, "A"), empty2.Add(key1a, "a"), ReferenceEqualityComparer.Instance, StringComparer.Ordinal, false); // left contains key not in right
        SetEqualsTest(empty2.Add(key1a, "a").Add(key1b, "A"), empty2.Add(key1a, "a"), null, StringComparer.Ordinal, false);
        SetEqualsTest(empty2.Add(key1a, "a").Add(key1b, "A"), empty2.Add(key1a, "a"), ReferenceEqualityComparer.Instance, StringComparer.OrdinalIgnoreCase, false); // left contains key not in right
        SetEqualsTest(empty2.Add(key1a, "a").Add(key1b, "A"), empty2.Add(key1a, "a"), null, StringComparer.OrdinalIgnoreCase, true);

        // same collisions
        SetEqualsTest(empty.Add("a", "a").Add("A", "A"), empty.Add("a", "a").Add("A", "A"), StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, true);
        SetEqualsTest(empty.Add("a", "a").Add("A", "A"), empty.Add("a", "a").Add("A", "A"), StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase, true);
        // equal collisions
        SetEqualsTest(empty.Add("a", "a").Add("A", "A"), empty.Add("a", "a").Add("A", "a"), StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase, true);

        // different collisions
        SetEqualsTest(empty.Add("a", "a").Add("A", "A"), empty.Add("a", "a").Add("A", "a"), StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, false);

        // same collisions with 3 items
        SetEqualsTest(empty.Add("a", "a").Add("A", "A").Add("ä", "ä"), empty.Add("a", "a").Add("A", "A").Add("ä", "ä"), StringComparerWithoutUmlauts.OrdinalIgnoreCase, StringComparer.Ordinal, true);
        SetEqualsTest(
            empty.Add("a", "a").Add("A", "A").Add("ä", "ä"),
            empty.Add("a", "a").Add("A", "A").Add("ä", "ä"),
            StringComparerWithoutUmlauts.OrdinalIgnoreCase, StringComparerWithoutUmlauts.Ordinal, true);
        SetEqualsTest(empty.Add("a", "a").Add("A", "A").Add("ä", "ä"), empty.Add("a", "a").Add("A", "A").Add("ä", "ä"), StringComparerWithoutUmlauts.OrdinalIgnoreCase, StringComparerWithoutUmlauts.OrdinalIgnoreCase, true);

        // equal collisions with 4 items
        SetEqualsTest(empty.Add("a", "a").Add("A", "a").Add("ä", "ä").Add("Ä", "Ä"), empty.Add("a", "a").Add("A", "A").Add("ä", "ä").Add("Ä", "Ä"), StringComparerWithoutUmlauts.OrdinalIgnoreCase, StringComparerWithoutUmlauts.OrdinalIgnoreCase, true);

        // different collisions with 3 items
        SetEqualsTest(empty.Add("a", "a").Add("A", "A").Add("ä", "ä"), empty.Add("a", "a").Add("A", "a").Add("ä", "ä"), StringComparerWithoutUmlauts.OrdinalIgnoreCase, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a").Add("A", "A").Add("ä", "ä"), empty.Add("a", "a").Add("A", "a").Add("ä", "ä"), StringComparerWithoutUmlauts.OrdinalIgnoreCase, StringComparerWithoutUmlauts.Ordinal, false);

        // further collisions
        SetEqualsTest(empty.Add("a", "a").Add("A", "A").Add("ä", "ä"), empty.Add("a", "a"), StringComparerWithoutUmlauts.OrdinalIgnoreCase, StringComparer.Ordinal, false);
        SetEqualsTest(empty.Add("a", "a").Add("A", "a").Add("ä", "a"), empty.Add("a", "a"), StringComparerWithoutUmlauts.OrdinalIgnoreCase, StringComparer.Ordinal, true);
        SetEqualsTest(empty.Add("a", "a").Add("A", "A").Add("ä", "a"), empty.Add("a", "a"), StringComparerWithoutUmlauts.OrdinalIgnoreCase, StringComparer.Ordinal, false);

        SetEqualsTest(
            empty.Add("a", "a").Add("A", "A").Add("ä", "a").Add("b", "b"),
            empty.Add("a", "a").Add("b", "b").Add("B", "B"),
            StringComparerWithoutUmlauts.OrdinalIgnoreCase, StringComparer.Ordinal, false);

        SetEqualsTest(
            empty.Add("a", "a").Add("A", "A").Add("b", "b").Add("B", "B"),
            empty.Add("a", "a").Add("A", "A").Add("b", "b").Add("B", "B"),
            StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, true);

        SetEqualsTest(
            empty.Add("a", "a").Add("A", "A").Add("b", "b").Add("B", "B"),
            empty.Add("a", "a").Add("A", "Ä").Add("b", "b").Add("B", "B"),
            StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, false);

        SetEqualsTest(
            empty.Add("a", "a").Add("A", "A").Add("b", "b").Add("B", "B").Add("c", "c"),
            empty.Add("a", "a").Add("A", "Ä").Add("b", "b").Add("c", "c").Add("C", "C"),
            StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal, false);

        void SetEqualsTest<TKey, TValue>(ImmutableDictionary<TKey, TValue> first, ImmutableDictionary<TKey, TValue> second,
            IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer, bool expected)
            where TKey : notnull
        {
            var actual = ImmutableDictionaryValue.SetEquals(first, second, keyComparer, valueComparer);
            Assert.AreEqual(expected, actual);
        }
    }

    private sealed class StringComparerWithoutUmlauts : IEqualityComparer<string>
    {
        private StringComparerWithoutUmlauts(StringComparer internalComparer)
        {
            InternalComparer = internalComparer;
        }

        private StringComparer InternalComparer;

        public static readonly StringComparerWithoutUmlauts Ordinal = new(StringComparer.Ordinal);
        public static readonly StringComparerWithoutUmlauts OrdinalIgnoreCase = new(StringComparer.OrdinalIgnoreCase);        

        public bool Equals(string? x, string? y)
        {
            ReplaceChars(ref x);
            ReplaceChars(ref y);
            return InternalComparer.Equals(x, y);
        }

        public int GetHashCode(string obj)
        {
            ReplaceChars(ref obj);
            return InternalComparer.GetHashCode(obj);
        }

        private static void ReplaceChars([NotNullIfNotNull(nameof(s))]ref string? s)
        {
            if (s is null)
            {
                return;
            }

            char[]? chars = null;

            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case 'ä':
                        chars ??= s.ToCharArray();
                        chars[i] = 'a';
                        break;
                    case 'ö':
                        chars ??= s.ToCharArray();
                        chars[i] = 'o';
                        break;
                    case 'ü':
                        chars ??= s.ToCharArray();
                        chars[i] = 'u';
                        break;
                    case 'Ä':
                        chars ??= s.ToCharArray();
                        chars[i] = 'A';
                        break;
                    case 'Ö':
                        chars ??= s.ToCharArray();
                        chars[i] = 'O';
                        break;
                    case 'Ü':
                        chars ??= s.ToCharArray();
                        chars[i] = 'U';
                        break;
                }
            }

            if (chars is not null)
            {
                s = new string(chars);
            }
        }
    }
}
