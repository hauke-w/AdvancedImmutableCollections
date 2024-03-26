using AdvancedImmutableCollections.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
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

        Assert.ThrowsException<ArgumentNullException>(() => new ImmutableDictionaryValue<string, GenericParameterHelper>(null!));

        static void CtorTest<TKey, TValue>(ImmutableDictionary<TKey, TValue> value)
            where TKey : notnull
        {
            var actual = new ImmutableDictionaryValue<TKey, TValue>(value);
            Assert.AreSame(value, actual.Value);
            Assert.AreEqual(value.Count, actual.Count);
        }
    }

    [TestMethod]
    public void IsDefaultTest()
    {
        IsDefaultTest(default, true);
        // do not use collection expression here because it could produced default value!
        IsDefaultTest(ImmutableDictionaryValue.Create<int, GenericParameterHelper>(), false);
        IsDefaultTest(ImmutableDictionaryValue.Create(KeyValuePair.Create(0, new GenericParameterHelper(0))), false);

        static void IsDefaultTest(ImmutableDictionaryValue<int, GenericParameterHelper> testObject, bool expected)
        {
            Assert.AreEqual(expected, testObject.IsDefault);
        }
    }

    [TestMethod]
    public void IsDefaultOrEmptyTest()
    {
        IsDefaultTest(default, true);
        // do not use collection expression here because it could produced default value!
        IsDefaultTest(ImmutableDictionaryValue.Create<int, GenericParameterHelper>(), true);
        IsDefaultTest(ImmutableDictionaryValue.Create(KeyValuePair.Create(0, new GenericParameterHelper(0))), false);

        static void IsDefaultTest(ImmutableDictionaryValue<int, GenericParameterHelper> testObject, bool expected)
        {
            Assert.AreEqual(expected, testObject.IsDefaultOrEmpty);
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

    /// <summary>
    /// Verifies the get-indexer
    /// </summary>
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
    public void SetItemTest()
    {
        var expected = ImmutableDictionary.Create<string, string>();
        ImmutableDictionaryValue<string, string> testObject = expected.WithValueSemantics();
        SetItemTest("a", "val A");

        expected = ImmutableDictionary.Create<string, string>();
        testObject = default;
        SetItemTest("a", "val A");

        expected = ImmutableDictionary.Create<string, string>().AddRange([new("a", "value a"), new("b", "value b"), new("B", "value B")]);
        testObject = expected.WithValueSemantics();
        SetItemTest("a", "value a v2");
        SetItemTest("A", "value A");
        SetItemTest("B", "value B v2");

        expected = ImmutableDictionary.Create<string, string>(StringComparer.OrdinalIgnoreCase).AddRange([new("a", "value a"), new("b", "value b")]);
        testObject = expected.WithValueSemantics();
        SetItemTest("A", "value A");
        SetItemTest("b", "value B");

        expected = ImmutableDictionary.Create<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase).AddRange([new("a", "value a"), new("b", "value b")]);
        testObject = expected.WithValueSemantics();
        SetItemTest("A", "value A");
        SetItemTest("b", "value B");

        void SetItemTest(string key, string value)
        {
            var before = testObject.ToList();
            expected = expected.SetItem(key, value);
            var actual = testObject.SetItem(key, value);
            Assert.AreEqual(expected[key], actual[key]);
            var actualAsList = actual.ToList();
            CollectionAssert.AreEquivalent(expected, actualAsList);
            CollectionAssert.AreEquivalent(before, testObject.ToList());
            Assert.IsFalse(actual.IsDefault);
            testObject = actual;
        }
    }

    [TestMethod]
    public void SetItemsTest()
    {
        var expected = ImmutableDictionary.Create<string, string>();
        ImmutableDictionaryValue<string, string> testObject = expected.WithValueSemantics();
        SetItemsTest([]);
        SetItemsTest([new("a", "val A")]);

        expected = ImmutableDictionary.Create<string, string>();
        testObject = default;
        SetItemsTest([]);
        SetItemsTest([new("a", "val A")]);

        expected = ImmutableDictionary.Create<string, string>().AddRange([new("a", "value a"), new("b", "value b"), new("B", "value B")]);
        testObject = expected.WithValueSemantics();
        SetItemsTest([new("a", "value a v2"), new("A", "value A")]);
        SetItemsTest([new("a", "value a v3"), new("B", "value B v2")]);
        SetItemsTest([new("a", "value a v4"), new("a", "value a v5")]);
        SetItemsTest([new("a", "value a v6")]);
        SetItemsTest([]);

        expected = ImmutableDictionary.Create<string, string>(StringComparer.OrdinalIgnoreCase).AddRange([new("a", "value a"), new("b", "value b")]);
        testObject = expected.WithValueSemantics();
        SetItemsTest([new("a", "value A"), new("b", "value B")]);

        expected = ImmutableDictionary.Create<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase).AddRange([new("a", "value a"), new("b", "value b")]);
        testObject = expected.WithValueSemantics();
        SetItemsTest([new("A", "value A"), new("b", "value B")]);

        void SetItemsTest(IEnumerable<KeyValuePair<string, string>> items)
        {
            var before = testObject.ToList();
            expected = expected.SetItems(items);
            var actual = testObject.SetItems(items);
            var actualAsList = actual.ToList();
            CollectionAssert.AreEquivalent(expected, actualAsList);
            CollectionAssert.AreEquivalent(before, testObject.ToList());
            testObject = actual;
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

    [TestMethod]
    public void AddTest()
    {
        var expected = ImmutableDictionary.Create<string, string>();
        ImmutableDictionaryValue<string, string> testObject = expected.WithValueSemantics();
        Add("a", "a");
        AddThrowsException("a", "A"); // same key, different value
        Add("a", "a");
        Add("b", "b");

        expected = ImmutableDictionary.Create<string, string>();
        testObject = default;
        Add("a", "a");

        expected = ImmutableDictionary.Create<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal);
        testObject = expected.WithValueSemantics();
        Add("a", "a");
        Add("A", "a"); // equal key
        AddThrowsException("A", "A"); // equal key, different value

        expected = ImmutableDictionary.Create<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);
        testObject = expected.WithValueSemantics();
        Add("a", "a");
        Add("a", "A"); // same key, equal value according to comparer

        expected = ImmutableDictionary.Create<string, string>(StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase);
        testObject = expected.WithValueSemantics();
        Add("a", "a");
        Add("a", "A"); // equal value
        Add("A", "A");
        Add("A", "a"); // equal value

        void Add(string key, string value)
        {
            var actual = testObject.Add(key, value);
            expected = expected.Add(key, value);
            var actualAsList = actual.ToList();
            CollectionAssert.AreEquivalent(expected, actualAsList);
            Assert.IsFalse(actual.IsDefault);
            testObject = actual;
        }

        void AddThrowsException(string key, string value)
        {
            Assert.ThrowsException<ArgumentException>(() => testObject.Add(key, value));
            var actualAsList = testObject.ToList();
            CollectionAssert.AreEquivalent(expected, actualAsList);
        }
    }

    [TestMethod]
    public void AddRangeTest()
    {
        var expected = ImmutableDictionary.Create<string, string>();
        ImmutableDictionaryValue<string, string> testObject = expected.WithValueSemantics();
        AddRange([]);
        AddRange([new("a", "a"), new("b", "b")]);
        AddRange([new("a", "a")]);
        AddRange([new("c", "c")]);
        AddRange([]);
        AddRangeThrowsException([new("d", "d"), new("a", "A")]); // ("a", "A"): same key, different value

        expected = ImmutableDictionary.Create<string, string>();
        testObject = default;
        AddRange([]);

        expected = ImmutableDictionary.Create<string, string>();
        testObject = default;
        AddRange([new("a", "a"), new("A", "A"), new("b", "b")]);


        expected = ImmutableDictionary.Create<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal);
        testObject = expected.WithValueSemantics();
        AddRange([new("a", "a")]);
        AddRange([new("A", "a")]); // equal key
        AddRangeThrowsException([new("A", "A")]); // equal key, different value
        AddRangeThrowsException([new("b", "b"), new("A", "A"), new("c", "c")]); // ("A", "A"): equal key, different value

        expected = ImmutableDictionary.Create<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);
        testObject = expected.WithValueSemantics();
        AddRange([new("a", "a"), new("a", "A")]); // same key, equal value according to comparer

        expected = ImmutableDictionary.Create<string, string>(StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase);
        testObject = expected.WithValueSemantics();
        AddRange([new("a", "a")]);
        AddRange([new("a", "A")]); // equal value
        AddRange([new("A", "A"), new("A", "a")]); // equal value

        void AddRange(IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var actual = testObject.AddRange(pairs);
            expected = expected.AddRange(pairs);
            var actualAsList = actual.ToList();
            CollectionAssert.AreEquivalent(expected, actualAsList);
            Assert.IsFalse(actual.IsDefault);
            testObject = actual;
        }

        void AddRangeThrowsException(IEnumerable<KeyValuePair<string, string>> pairs)
        {
            Assert.ThrowsException<ArgumentException>(() => testObject.AddRange(pairs));
            var actualAsList = testObject.ToList();
            CollectionAssert.AreEquivalent(expected, actualAsList);
        }
    }

    [TestMethod]
    public void RemoveTest()
    {
        var expected = ImmutableDictionary
            .Create<string, string>()
            .AddRange([new("a", "a"), new("b", "b"), new("c", "c"), new("A", "A")]);
        ImmutableDictionaryValue<string, string> testObject = expected.WithValueSemantics();

        Remove("d"); // no change
        Remove("c"); // -> a, b, A
        Remove("A"); // no change
        Remove("a"); // -> b

        expected = ImmutableDictionary
            .Create<string, string>(StringComparer.OrdinalIgnoreCase)
            .AddRange([new("a", "a"), new("b", "b"), new("c", "c"), new("d", "d"), new("E", "E")]);
        testObject = expected.WithValueSemantics();
        Remove("D"); // a, b, c, E
        Remove("e"); // a, b, c
        Remove("b"); // a, c
        Remove("A"); // c
        Remove("C"); // empty
        Remove("C"); // no change

        expected = ImmutableDictionary.Create<string, string>();
        testObject = default;
        Remove("a");

        void Remove(string key)
        {
            bool wasDefault = testObject.IsDefault;
            var actual = testObject.Remove(key);
            expected = expected.Remove(key);
            var actualAsList = actual.ToList();
            CollectionAssert.AreEquivalent(expected, actualAsList);
            Assert.AreEqual(wasDefault, actual.IsDefault);
            testObject = actual;
        }
    }

    [TestMethod]
    public void RemoveRangeTest()
    {
        var expected = ImmutableDictionary
            .Create<string, string>()
            .AddRange([new("a", "a"), new("b", "b"), new("c", "c"), new("A", "A")]);
        ImmutableDictionaryValue<string, string> testObject = expected.WithValueSemantics();

        RemoveRange([]); // no change
        RemoveRange(["d", "e"]); // no change
        RemoveRange(["c", "A"]); // -> a, b, c
        RemoveRange(["b", "c"]); // -> a
        RemoveRange(["a", "b"]); // -> empty
        RemoveRange(["a", "b"]); // no change
        RemoveRange([]); // no change

        expected = ImmutableDictionary
            .Create<string, string>(StringComparer.OrdinalIgnoreCase)
            .AddRange([new("a", "a"), new("b", "b"), new("c", "c"), new("d", "d"), new("E", "E")]);
        testObject = expected.WithValueSemantics();
        RemoveRange(["c", "A"]); // -> b, d, e
        RemoveRange(["B", "D", "e"]); // -> empty

        expected = ImmutableDictionary.Create<string, string>();
        testObject = default;
        RemoveRange([]);
        RemoveRange(["a"]);

        void RemoveRange(IEnumerable<string> keys)
        {
            bool wasDefault = testObject.IsDefault;
            var actual = testObject.RemoveRange(keys);
            expected = expected.RemoveRange(keys);
            var actualAsList = actual.ToList();
            CollectionAssert.AreEquivalent(expected, actualAsList);
            Assert.AreEqual(wasDefault, actual.IsDefault);
            testObject = actual;
        }
    }

    #region tests for equality

#if !NET6_0_OR_GREATER
    [TestMethod]
    public void Equals_Object_TestOld()
    {
        foreach (var testCase in Equals_Object_TestCases().Concat(Equals_ImmutableDictionaryValue_TestCases()))
        {
            Equals_Object_Test((ImmutableDictionaryValue<string, string>)testCase[0], testCase[1], (EqualityRelation)testCase[2], (TestCaseInfo)testCase[3]);
        }
    }
#else
    [DynamicData(nameof(Equals_Object_TestCases), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(DynamicData.GetDynamicDataDisplayName), DynamicDataDisplayNameDeclaringType = typeof(DynamicData))]
    [DynamicData(nameof(Equals_ImmutableDictionaryValue_TestCases), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(DynamicData.GetDynamicDataDisplayName), DynamicDataDisplayNameDeclaringType = typeof(DynamicData))]
    [TestMethod]
#endif
    public void Equals_Object_Test(ImmutableDictionaryValue<string, string> testObject, object? obj, EqualityRelation expectedRelation, TestCaseInfo testCase)
    {
        bool expected = expectedRelation.ToBoolForEquals();
        testCase.Execute(() =>
        {
            var actual = testObject.Equals(obj);
            Assert.AreEqual(expected, actual);
        });
    }

#if !NET6_0_OR_GREATER
    [TestMethod]
    public void Equals_ImmutableDictionaryValue_TestOld()
    {
        foreach (var testCase in Equals_ImmutableDictionaryValue_TestCases())
        {
            Equals_ImmutableDictionaryValue_Test((ImmutableDictionaryValue<string, string>)testCase[0], (ImmutableDictionaryValue<string, string>)testCase[1], (EqualityRelation)testCase[2], (TestCaseInfo)testCase[3]);
        }
    }
#else
    [TestMethod]
    [DynamicData(nameof(Equals_ImmutableDictionaryValue_TestCases), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(DynamicData.GetDynamicDataDisplayName), DynamicDataDisplayNameDeclaringType = typeof(DynamicData))]
#endif
    public void Equals_ImmutableDictionaryValue_Test(ImmutableDictionaryValue<string, string> testObject, ImmutableDictionaryValue<string, string> other, EqualityRelation expectedRelation, TestCaseInfo testCase)
    {
        bool expected = expectedRelation.ToBoolForEquals();
        testCase.Execute(() =>
        {
            var actual = testObject.Equals(other);
            var comparer = new KeyValuePairComparer<string, string>(testObject.Value.KeyComparer, testObject.Value.ValueComparer);
            Assert.AreEqual(expected, new HashSet<KeyValuePair<string, string>>(testObject, comparer).SetEquals(new HashSet<KeyValuePair<string, string>>(other, comparer)), "incorrect expected value");
            Assert.AreEqual(expected, actual);
        });
    }

    private class KeyValuePairComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
    {
        public KeyValuePairComparer(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            KeyComparer = keyComparer ?? throw new ArgumentNullException(nameof(keyComparer));
            ValueComparer = valueComparer ?? throw new ArgumentNullException(nameof(valueComparer));
        }

        public IEqualityComparer<TKey> KeyComparer { get; }
        public IEqualityComparer<TValue> ValueComparer { get; }

        public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            return KeyComparer.Equals(x.Key, y.Key) && ValueComparer.Equals(x.Value, y.Value);
        }

        public int GetHashCode([DisallowNull] KeyValuePair<TKey, TValue> obj)
        {
#if NETCOREAPP
            return HashCode.Combine(KeyComparer.GetHashCode(obj.Key!), ValueComparer.GetHashCode(obj.Value!));
#else
            unchecked
            {
                return KeyComparer.GetHashCode(obj.Key) * 5 + ValueComparer.GetHashCode(obj.Value) * 7;
            }
#endif
        }
    }

    private static IEnumerable<object[]> Equals_Object_TestCases()
    {
        return
            [
                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a"), new("b", "b")]))
                    .With<object?>(null)
                    .With(EqualityRelation.NotEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")]))
                    .With(ImmutableDictionary.Create<string, string>().Add("a", "a"))
                    .With(EqualityRelation.NotEqual)
            ];
    }

    private static IEnumerable<object[]> Equals_ImmutableDictionaryValue_TestCases()
    {
        return
            [
                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a"), new("b", "b")]))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a"), new("b", "b")]))
                    .With(EqualityRelation.InterchangeablyEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")]))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "A")]))
                    .With(EqualityRelation.NotEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")]))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("A", "a")]))
                    .With(EqualityRelation.NotEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.OrdinalIgnoreCase, null))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("A", "a")]))
                    .With(EqualityRelation.SetEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("A", "a")]))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.OrdinalIgnoreCase, null))
                    .With(EqualityRelation.NotEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("A", "A")]))
                    .With(EqualityRelation.NotEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("A", "A")]))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase))
                    .With(EqualityRelation.NotEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a"), new("A", "A")]))
                    .With(EqualityRelation.NotEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a"), new("A", "A")]))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal))
                    .With(EqualityRelation.NotEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a"), new("A", "A")]))
                    .With(EqualityRelation.SetEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a"), new("b", "b")]))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")]))
                    .With(EqualityRelation.NotEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")]))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a"), new("b", "b")]))
                    .With(EqualityRelation.NotEqual),

                DynamicData.TestCase()
                    .With<ImmutableDictionaryValue<string, string>>([])
                    .With<ImmutableDictionaryValue<string, string>>([])
                    .With(EqualityRelation.InterchangeablyEqual),

                DynamicData.TestCase()
                    .With<ImmutableDictionaryValue<string, string>>(default)
                    .With<ImmutableDictionaryValue<string, string>>(default)
                    .With(EqualityRelation.InterchangeablyEqual),

                DynamicData.TestCase()
                    .With<ImmutableDictionaryValue<string, string>>([])
                    .With<ImmutableDictionaryValue<string, string>>(default)
                    .With(EqualityRelation.InterchangeablyEqual),

                DynamicData.TestCase()
                    .With<ImmutableDictionaryValue<string, string>>(default)
                    .With<ImmutableDictionaryValue<string, string>>([])
                    .With(EqualityRelation.InterchangeablyEqual),

                DynamicData.TestCase()
                    .With<ImmutableDictionaryValue<string, string>>(default)
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")]))
                    .With(EqualityRelation.NotEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase))
                    .With(ImmutableDictionaryValue.Create<string, string>([], StringComparer.Ordinal, StringComparer.Ordinal))
                    .With(EqualityRelation.SetEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([], StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase))
                    .With(ImmutableDictionaryValue.Create<string, string>([], StringComparer.Ordinal, StringComparer.Ordinal))
                    .With(EqualityRelation.SetEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([], StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal))
                    .With(ImmutableDictionaryValue.Create<string, string>([], StringComparer.Ordinal, StringComparer.Ordinal))
                    .With(EqualityRelation.SetEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.Ordinal, StringComparer.Ordinal))
                    .With(EqualityRelation.SetEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.Ordinal, StringComparer.Ordinal))
                    .With(EqualityRelation.SetEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.Ordinal, StringComparer.Ordinal))
                    .With(EqualityRelation.SetEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase))
                    .With(EqualityRelation.InterchangeablyEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "A")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase))
                    .With(EqualityRelation.SetEqual),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<string, string>([new("a", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase))
                    .With(ImmutableDictionaryValue.Create<string, string>([new("A", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase))
                    .With(EqualityRelation.SetEqual),
            ];
    }
    #endregion

    [TestMethod]
    public void GetHashCodeTest()
    {
        ImmutableDictionaryValue<string, string?> testObject = default;
        Assert.AreEqual(0, testObject.GetHashCode());

        testObject = ImmutableDictionaryValue.Create<string, string?>();
        Assert.AreEqual(0, testObject.GetHashCode());
        var hashCodes = new HashSet<int>() { 0 };
        VerifyHashCodeWithNewItem("a", "a");
        VerifyHashCodeWithNewItem("A", "A");
        VerifyHashCodeWithNewItem("b", "b");
        VerifyHashCodeWithNewItem("B", "B");
        VerifyHashCodeWithNewItem("c", "c");
        VerifyHashCodeWithNewItem("C", "C");
        VerifyHashCodeWithNewItem("d", null);
        VerifyHashCodeWithNewItem("D", null);

        testObject = ImmutableDictionaryValue.Create<string, string?>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);
        Assert.AreEqual(0, testObject.GetHashCode());
        hashCodes = new HashSet<int>() { 0 };
        VerifyHashCodeWithNewItem("a", "a");
        VerifyHashCodeWithNewItem("b", "B");
        VerifyHashCodeWithNewItem("C", "c");
        VerifyHashCodeWithNewItem("D", "D");
        VerifyHashCodeWithNewItem("e", null);

        hashCodes = new HashSet<int>();
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("a", "a")], StringComparer.Ordinal, StringComparer.Ordinal));
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("a", "A")], StringComparer.Ordinal, StringComparer.Ordinal));
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("A", "a")], StringComparer.Ordinal, StringComparer.Ordinal));
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("A", "A")], StringComparer.Ordinal, StringComparer.Ordinal));

        // following dictionaries are equal to the above ones
#if !NET462 // in .net 4.6 hashcodes are computed differently
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("a", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase), expectIsUnique: false);
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("a", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal), expectIsUnique: false);
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("a", "a")], StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase), expectIsUnique: false);

        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("a", "A")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase), expectIsUnique: false);
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("a", "A")], StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal), expectIsUnique: false);
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("a", "A")], StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase), expectIsUnique: false);

        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("A", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase), expectIsUnique: false);
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("A", "a")], StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal), expectIsUnique: false);
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("A", "a")], StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase), expectIsUnique: false);

        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("A", "A")], StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase), expectIsUnique: false);
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("A", "A")], StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal), expectIsUnique: false);
        VerifyHashCodeIsUnique(ImmutableDictionaryValue.Create([new("A", "A")], StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase), expectIsUnique: false);
#endif

        void VerifyHashCodeWithNewItem(string key, string? value)
        {
            testObject = testObject.Add(key, value);
            VerifyHashCodeIsUnique(testObject);
        }

        void VerifyHashCodeIsUnique(ImmutableDictionaryValue<string, string?> testObject, bool expectIsUnique = true)
        {
            var actual = testObject.GetHashCode();
            Assert.AreEqual(expectIsUnique, hashCodes.Add(actual));
        }
    }

#if !NET6_0_OR_GREATER
    [TestMethod]
    public void GetEnumeratorTestOld()
    {
        foreach (var testCase in GetEnumeratorTestCases())
        {
            GetEnumeratorTest((ImmutableDictionaryValue<GenericParameterHelper, int>)testCase[0], (IEnumerable<KeyValuePair<GenericParameterHelper, int>>)testCase[1], (TestCaseInfo)testCase[2]);
        }
    }
#else
    [TestMethod]
    [DynamicData(nameof(GetEnumeratorTestCases), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(DynamicData.GetDynamicDataDisplayName), DynamicDataDisplayNameDeclaringType = typeof(DynamicData))]
#endif
    public void GetEnumeratorTest(ImmutableDictionaryValue<GenericParameterHelper, int> testObject, IEnumerable<KeyValuePair<GenericParameterHelper, int>> expected, TestCaseInfo testCase)
    {
        testCase.Execute(() =>
        {
            var expectedAsList = expected.ToList();
            var actualItems = new List<KeyValuePair<GenericParameterHelper, int>>();
            var actual = testObject.GetEnumerator();
            while (actual.MoveNext())
            {
                actualItems.Add(actual.Current);
            }
            CollectionAssert.AreEquivalent(expectedAsList, actualItems);
        });
    }

#if !NET6_0_OR_GREATER
    [TestMethod]
    public void IEnumerable_T_GetEnumeratorTestOld()
    {
        foreach (var testCase in GetEnumeratorTestCases())
        {
            IEnumerable_T_GetEnumeratorTest((IEnumerable<KeyValuePair<GenericParameterHelper, int>>)testCase[0], (IEnumerable<KeyValuePair<GenericParameterHelper, int>>)testCase[1], (TestCaseInfo)testCase[2]);
        }
    }
#else
    [TestMethod]
    [DynamicData(nameof(GetEnumeratorTestCases), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(DynamicData.GetDynamicDataDisplayName), DynamicDataDisplayNameDeclaringType = typeof(DynamicData))]
#endif
    public void IEnumerable_T_GetEnumeratorTest(IEnumerable<KeyValuePair<GenericParameterHelper, int>> testObject, IEnumerable<KeyValuePair<GenericParameterHelper, int>> expected, TestCaseInfo testCase)
    {
        testCase.Execute(() =>
        {
            var expectedAsList = expected.ToList();
            var actualItems = new List<KeyValuePair<GenericParameterHelper, int>>();
            var actual = testObject.GetEnumerator();
            while (actual.MoveNext())
            {
                actualItems.Add(actual.Current);
            }
            CollectionAssert.AreEquivalent(expectedAsList, actualItems);
        });
    }

#if !NET6_0_OR_GREATER
    [TestMethod]
    public void IEnumerable_GetEnumeratorTestOld()
    {
        foreach (var testCase in GetEnumeratorTestCases())
        {
            IEnumerable_GetEnumeratorTest((IEnumerable)testCase[0], (IEnumerable<KeyValuePair<GenericParameterHelper, int>>)testCase[1], (TestCaseInfo)testCase[2]);
        }
    }
#else
    [TestMethod]
    [DynamicData(nameof(GetEnumeratorTestCases), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(DynamicData.GetDynamicDataDisplayName), DynamicDataDisplayNameDeclaringType = typeof(DynamicData))]
#endif
    public void IEnumerable_GetEnumeratorTest(IEnumerable testObject, IEnumerable<KeyValuePair<GenericParameterHelper, int>> expected, TestCaseInfo testCase)
    {
        testCase.Execute(() =>
        {
            var expectedAsList = expected.ToList();
            var actualItems = new List<object>();
            var actual = testObject.GetEnumerator();
            Assert.IsNotNull(actual);
            while (actual.MoveNext())
            {
                actualItems.Add(actual.Current);
            }
            CollectionAssert.AreEquivalent(expectedAsList, actualItems);
        });
    }

    private static IEnumerable<object[]> GetEnumeratorTestCases()
    {
        var item0 = new KeyValuePair<GenericParameterHelper, int>(new GenericParameterHelper(0), 0);
        var item0b = new KeyValuePair<GenericParameterHelper, int>(new GenericParameterHelper(0), 0);
        var item1 = new KeyValuePair<GenericParameterHelper, int>(new GenericParameterHelper(1), 1);
        return
            [
                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create<GenericParameterHelper, int>())
                    .With<IEnumerable<KeyValuePair<GenericParameterHelper, int>>>([]),

                DynamicData.TestCase()
                    .With<ImmutableDictionaryValue<GenericParameterHelper, int>>(default)
                    .With<IEnumerable<KeyValuePair<GenericParameterHelper, int>>>([]),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create([item0]))
                    .With<IEnumerable<KeyValuePair<GenericParameterHelper, int>>>([item0]),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create([item0, item1]))
                    .With<IEnumerable<KeyValuePair<GenericParameterHelper, int>>>([item0, item1]),

                DynamicData.TestCase()
                    .With(ImmutableDictionaryValue.Create([item0, item1, item0b], ReferenceEqualityComparer.Instance, null))
                    .With<IEnumerable<KeyValuePair<GenericParameterHelper, int>>>([item0, item1, item0b]),
            ];
    }

    /// <summary>
    /// Verifies <see cref="ImmutableDictionaryValue{TKey, TValue}.op_Implicit(ImmutableDictionaryValue{TKey, TValue})"/>
    /// </summary>
    [TestMethod]
    public void OpToImmutableHashSetTest()
    {
        var source = ImmutableDictionary.Create<int, string>().AddRange([KeyValuePair.Create(1, "a"), KeyValuePair.Create(2, "b")]);
        var value = new ImmutableDictionaryValue<int, string>(source);
        ImmutableDictionary<int, string> actual = value; // do the conversion ImmutableDictionaryValue<int, string> -> ImmutableDictionary<int, string>
        Assert.AreSame(source, actual);

        value = default;
        actual = value; // do the conversion ImmutableDictionaryValue<int, string> -> ImmutableDictionary<int, string>
        Assert.IsNotNull(actual);
        Assert.AreEqual(0, actual.Count);
    }

    /// <summary>
    /// Verifies <see cref="ImmutableDictionaryValue{TKey, TValue}.op_Implicit(ImmutableDictionary{TKey, TValue})"/>
    /// </summary>
    [TestMethod]
    public void OpToImmutableHashSetValueTest()
    {
        var value = ImmutableDictionary.Create<int, string>().AddRange([KeyValuePair.Create(1, "a"), KeyValuePair.Create(2, "b")]);
        ImmutableDictionaryValue<int, string> actual = value; // do the conversion ImmutableDictionary<int, string> -> ImmutableDictionaryValue<int, string>
        Assert.AreSame(value, actual.Value);

        value = null;
        Assert.ThrowsException<ArgumentNullException>(() => actual = value!); // do the conversion ImmutableDictionary<int, string> -> ImmutableDictionaryValue<int, string>
    }
}
