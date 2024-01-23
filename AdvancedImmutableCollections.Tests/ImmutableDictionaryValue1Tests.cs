using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
#if !NET6_0_OR_GREATER
using KeyValuePair = System.KeyValuePairExtensions;
#endif
namespace AdvancedImmutableCollections;

[TestClass]
public class ImmutableDictionaryValue1Tests
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

    [TestMethod]
    public void Value_OfDefault_Test()
    {
        ImmutableDictionaryValue<string, GenericParameterHelper> value = default;
        var actual = value.Value;
        Assert.IsNotNull(actual);
        Assert.AreEqual(0, actual.Count);
        Assert.AreEqual(EqualityComparer<string>.Default, actual.KeyComparer);
        Assert.AreEqual(EqualityComparer<GenericParameterHelper>.Default, actual.ValueComparer);
    }

    [TestMethod]
    public void CountTest()
    {
        var item0 = KeyValuePair.Create(new GenericParameterHelper(0), "a");
        var item1 = KeyValuePair.Create(new GenericParameterHelper(1), "b");

        CountTest<GenericParameterHelper, int>([], 0);
        CountTest([item0], 1);
        CountTest([item0, item1], 2);
        Assert.AreEqual(0, default(ImmutableDictionaryValue<GenericParameterHelper, int>).Count);

        static void CountTest<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> value, int expected, IEqualityComparer<TKey>? keyComparer = null, IEqualityComparer<TValue>? valueComparer = null)
            where TKey : notnull
        {
            var testObject = value.ToImmutableDictionary(keyComparer, valueComparer).WithValueSemantics();
            var actual = testObject.Count;
            Assert.AreEqual(expected, actual);
        }
    }

    [TestMethod]
    public void GetItemTest()
    {
        var item0 = KeyValuePair.Create(new GenericParameterHelper(0), "a");
        var item0b = KeyValuePair.Create(new GenericParameterHelper(0), "A");
        var item1 = KeyValuePair.Create(new GenericParameterHelper(1), "b");
        var item1b = KeyValuePair.Create(new GenericParameterHelper(1), "B");

        GetItemTest([item0], item0.Key, item0.Value);
        GetItemTest([item0, item1], item1b.Key, item1.Value);
        GetItemTest([item0, item0b, item1], item0.Key, item0.Value, keyComparer: ReferenceEqualityComparer.Instance);
        GetItemTest([item0, item0b, item1], item0b.Key, item0b.Value, keyComparer: ReferenceEqualityComparer.Instance);
        GetItemTest([item0, item0b, item1], item1.Key, item1.Value, keyComparer: ReferenceEqualityComparer.Instance);
        GetItem_KeyNotFoundException_Test([item0b], item0.Key, keyComparer: ReferenceEqualityComparer.Instance);
        GetItem_KeyNotFoundException_Test<GenericParameterHelper, int>([], item0.Key, keyComparer: ReferenceEqualityComparer.Instance);

        Assert.ThrowsException<KeyNotFoundException>(() => default(ImmutableDictionaryValue<string, GenericParameterHelper>)["key"]);

        static void GetItemTest<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> value, TKey key, TValue expected, IEqualityComparer<TKey>? keyComparer = null, IEqualityComparer<TValue>? valueComparer = null)
            where TKey : notnull
        {
            var testObject = value.ToImmutableDictionary(keyComparer, valueComparer).WithValueSemantics();
            var actual = testObject[key];
            Assert.AreEqual(expected, actual);
        }

        static void GetItem_KeyNotFoundException_Test<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> value, TKey key, IEqualityComparer<TKey>? keyComparer = null, IEqualityComparer<TValue>? valueComparer = null)
            where TKey : notnull
        {
            var testObject = value.ToImmutableDictionary(keyComparer, valueComparer).WithValueSemantics();
            Assert.ThrowsException<KeyNotFoundException>(() => testObject[key]);
        }
    }

    [TestMethod]
    public void ContainsKeyTest()
    {
        var item0 = KeyValuePair.Create(new GenericParameterHelper(0), "a");
        var item0b = KeyValuePair.Create(new GenericParameterHelper(0), "A");
        var item1 = KeyValuePair.Create(new GenericParameterHelper(1), "b");
        var item1b = KeyValuePair.Create(new GenericParameterHelper(1), "B");

        ContainsKeyTest([item0], item0.Key, true);
        ContainsKeyTest([item0], item0b.Key, true);
        ContainsKeyTest([item0], item0.Key, true, keyComparer: ReferenceEqualityComparer.Instance);
        ContainsKeyTest([item0], item0b.Key, false, keyComparer: ReferenceEqualityComparer.Instance);
        ContainsKeyTest([item0, item1], item1b.Key, true);
        ContainsKeyTest([item0, item0b, item1], item0.Key, true, keyComparer: ReferenceEqualityComparer.Instance);
        ContainsKeyTest([item0, item0b, item1], item0b.Key, true, keyComparer: ReferenceEqualityComparer.Instance);
        ContainsKeyTest([item0, item0b, item1], item1.Key, true, keyComparer: ReferenceEqualityComparer.Instance);
        ContainsKeyTest([item0b], item0.Key, false, keyComparer: ReferenceEqualityComparer.Instance);
        ContainsKeyTest<GenericParameterHelper, int>([], item0.Key, false, keyComparer: ReferenceEqualityComparer.Instance);

        Assert.IsFalse(default(ImmutableDictionaryValue<string, GenericParameterHelper>).ContainsKey("key"));

        static void ContainsKeyTest<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> value, TKey key, bool expected, IEqualityComparer<TKey>? keyComparer = null, IEqualityComparer<TValue>? valueComparer = null)
            where TKey : notnull
        {
            var testObject = value.ToImmutableDictionary(it => it.Key, it => it.Value, keyComparer, valueComparer).WithValueSemantics();
            var actual = testObject.ContainsKey(key);
            Assert.AreEqual(expected, actual);
        }
    }

    [TestMethod]
    public void TryGetValueTest()
    {
        var item0 = KeyValuePair.Create(new GenericParameterHelper(0), "a");
        var item0b = KeyValuePair.Create(new GenericParameterHelper(0), "A");
        var item1 = KeyValuePair.Create(new GenericParameterHelper(1), "b");
        var item1b = KeyValuePair.Create(new GenericParameterHelper(1), "B");

        TryGetValueTest([item0], item0.Key, true, item0.Value);
        TryGetValueTest([item0], item0b.Key, true, item0.Value);
        TryGetValueTest([item0], item0.Key, true, item0.Value, keyComparer: ReferenceEqualityComparer.Instance);
        TryGetValueTest([item0], item0b.Key, false, default, keyComparer: ReferenceEqualityComparer.Instance);
        TryGetValueTest([item0, item1], item1b.Key, true, item1.Value);
        TryGetValueTest([item0, item1, item1b], item1.Key, true, item1.Value, keyComparer: ReferenceEqualityComparer.Instance);
        TryGetValueTest([item0, item1, item1b], item1b.Key, true, item1b.Value, keyComparer: ReferenceEqualityComparer.Instance);
        TryGetValueTest([item0b], item0.Key, false, default, keyComparer: ReferenceEqualityComparer.Instance);
        TryGetValueTest<GenericParameterHelper, int>([], item0.Key, false, default, keyComparer: ReferenceEqualityComparer.Instance);
        TryGetValueTestCore<string, int>(default, "key", false, default);

        static void TryGetValueTest<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> value, TKey key, bool expected, TValue expectedValue,
            IEqualityComparer<TKey>? keyComparer = null, IEqualityComparer<TValue>? valueComparer = null)
            where TKey : notnull
        {
            var testObject = value.ToImmutableDictionary(it => it.Key, it => it.Value, keyComparer, valueComparer).WithValueSemantics();
            TryGetValueTestCore(testObject, key, expected, expectedValue);
        }

        static void TryGetValueTestCore<TKey, TValue>(ImmutableDictionaryValue<TKey, TValue> testObject, TKey key, bool expected, TValue expectedValue)
            where TKey : notnull
        {
            var actual = testObject.TryGetValue(key, out var actualValue);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }
    }

    [TestMethod]
    public void ClearTest()
    {
        var item0 = KeyValuePair.Create(new GenericParameterHelper(0), "a");
        var item0b = KeyValuePair.Create(new GenericParameterHelper(0), "A");
        var item1 = KeyValuePair.Create(new GenericParameterHelper(1), "b");
        var item1b = KeyValuePair.Create(new GenericParameterHelper(1), "B");

        ClearTest([item0]);
        ClearTest([item0, item1]);
        ClearTest<GenericParameterHelper, string>([]);
        ClearTestCore<GenericParameterHelper, string>(default);

        static void ClearTest<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> value, IEqualityComparer<TKey>? keyComparer = null, IEqualityComparer<TValue>? valueComparer = null)
            where TKey : notnull
        {
            var testObject = value.ToImmutableDictionary(it => it.Key, it => it.Value, keyComparer, valueComparer).WithValueSemantics();
            ClearTestCore(testObject);
        }

        static void ClearTestCore<TKey, TValue>(ImmutableDictionaryValue<TKey, TValue> testObject)
            where TKey : notnull
        {
            bool expectChange = testObject.Count > 0;
            var itemsBefore = testObject.ToList();
            var actual = testObject.Clear();
            Assert.AreEqual(0, actual.Count);
            Assert.AreEqual(0, actual.Keys.ToList().Count);
            Assert.AreEqual(0, actual.Values.ToList().Count);
            Assert.AreEqual(0, actual.ToList().Count);

            CollectionAssert.AreEquivalent(itemsBefore, testObject.ToList());

            if (expectChange)
            {
                Assert.IsFalse(actual.IsDefault);
            }
            else
            {
                Assert.AreEqual(testObject.IsDefault, actual.IsDefault);
            }
        }
    }
}
