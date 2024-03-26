using AdvancedImmutableCollections.Tests.CollectionAdapters;
using AdvancedImmutableCollections.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableArrayValue{T}"/>
/// </summary>
[TestClass]
public sealed class ImmutableArrayValue1Tests : ImmutableListTestsBase<ImmutableArrayValueAdapterFactory>
{
    protected override Type GetTestObjectType<TItem>() => typeof(ImmutableArrayValue<TItem>);

    /// <summary>
    /// Verifies <see cref="ImmutableArrayValue{T}.Equals(object?)"/>
    /// </summary>
    [TestMethod]
    public void Equals_object_Test()
    {
        ImmutableArrayValue<int> testObject1 = ImmutableArrayValue.Create(1, 2, 3);
        ImmutableArrayValue<int> testObject1b = ImmutableArrayValue.Create(1, 2, 3);
        ImmutableArrayValue<int> testObject2 = ImmutableArrayValue.Create(1, 2, 3, 4);
        ImmutableArrayValue<int> testObject3 = ImmutableArrayValue.Create(1, 5, 3);
        ImmutableArrayValue<long> testObject4 = ImmutableArrayValue.Create(1L, 2L, 3L);
        ImmutableArrayValue<int> testObject0 = ImmutableArrayValue.Create<int>();

        EqualsTest(testObject1, testObject1, true);
        EqualsTest(testObject1, testObject1b, true);
        EqualsTest(testObject1, testObject2, false);
        EqualsTest(testObject2, testObject1, false);
        EqualsTest(testObject1, testObject3, false);
        EqualsTest(testObject3, testObject1, false);
        EqualsTest(testObject1, testObject4, false);
        EqualsTest(testObject1, new int[] { 1, 2, 3 }, false);
        EqualsTest(testObject1, new object(), false);
        EqualsTest(testObject1, null, false);
        EqualsTest(testObject1, default(ImmutableArrayValue<int>), false);
        EqualsTest<int>(default, default(ImmutableArrayValue<int>), true);
        EqualsTest<int>(default, testObject0, true);
        EqualsTest(testObject0, default(ImmutableArrayValue<int>), true);

        void EqualsTest<T>(ImmutableArrayValue<T> testObject, object? other, bool expected)
        {
            var actual = testObject.Equals(other);
            Assert.AreEqual(expected, actual);
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableArrayValue{T}.Equals(ImmutableArrayValue{T})"/>"/>
    /// </summary>
    [TestMethod]
    public void Equals_ImmutableArrayValue_Test()
    {
        ImmutableArrayValue<int> testObject1 = ImmutableArrayValue.Create(1);
        ImmutableArrayValue<int> testObject1b = ImmutableArrayValue.Create([1]);
        ImmutableArrayValue<int> testObject2 = ImmutableArrayValue.Create([1, 2]);
        ImmutableArrayValue<int> testObject3 = ImmutableArrayValue.Create([2]);
        ImmutableArrayValue<int> testObject0 = ImmutableArrayValue.Create<int>();

        EqualsTest(testObject1, testObject1, true);
        EqualsTest(testObject1, testObject1b, true);
        EqualsTest(testObject1, testObject2, false);
        EqualsTest(testObject2, testObject1, false);
        EqualsTest(testObject1, testObject3, false);
        EqualsTest(testObject3, testObject1, false);
        EqualsTest(testObject1, null, false);
        EqualsTest(testObject1, default(ImmutableArrayValue<int>), false);
        EqualsTest<int>(default, default(ImmutableArrayValue<int>), true);
        EqualsTest<int>(default, testObject0, true);
        EqualsTest(testObject0, default(ImmutableArrayValue<int>), true);

        void EqualsTest<T>(ImmutableArrayValue<T> testObject, ImmutableArrayValue<T>? other, bool expected)
        {
            var actual = testObject.Equals(other);
            Assert.AreEqual(expected, actual);
        }
    }

    [TestMethod]
    public void ReplaceTest2()
    {
        ReplaceTest(default, 0, 1, default);
        ReplaceTest(ImmutableArrayValue.Create<int>(), 2, -1, ImmutableArrayValue.Create<int>(), EqualityComparer<int>.Default);

        void ReplaceTest<T>(ImmutableArrayValue<T> testObject, T oldValue, T newValue, ImmutableArrayValue<T> expected, EqualityComparer<T>? equalityComparer = null)
        {
            ImmutableArrayValue<T> actual = testObject.Replace(oldValue, newValue, equalityComparer);
            AssertCollectionsAreEqual(expected, actual);
        }
    }

    [TestMethod]
    public void SetItemTest2()
    {
        SetItemTestIndexOutOfRange(default, 0, new GenericParameterHelper(1), default);
        SetItemTestIndexOutOfRange(ImmutableArrayValue.Create<int>(), 0, 666, ImmutableArrayValue.Create<int>());

        void SetItemTestIndexOutOfRange<T>(ImmutableArrayValue<T> testObject, int index, T value, ImmutableArrayValue<T> expected)
        {
            var itemsBefore = testObject.ToList();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => testObject.SetItem(index, value));
            AssertCollectionsAreEqual(itemsBefore, testObject);
        }
    }

    protected override IEqualityTestStrategy EqualityTestStrategy => ListValueEqualityTestStrategy.Default;

    protected override void AssertCollectionsAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? itemComparer = null)
    {
        if (expected is ImmutableArrayValue<T> value1
            && actual is ImmutableArrayValue<T> value2)
        {
            AssertCollectionsAreEqual(value1, value2, itemComparer);
        }
        else
        {
            base.AssertCollectionsAreEqual(expected, actual, itemComparer);
        }
    }

    private void AssertCollectionsAreEqual<T>(ImmutableArrayValue<T> expected, ImmutableArrayValue<T> actual, IEqualityComparer<T>? itemComparer = null)
    {
        CollectionAssert.That.AreEqual(expected.ToList(), actual.ToList(), itemComparer);
        bool areEqual = expected.Equals(actual);
        Assert.IsTrue(areEqual);
    }

    /// <summary>
    /// Verifies <see cref="ImmutableArrayValue{T}.GetEnumerator"/> while the test methods <see cref="GetEnumeratorTest"/> and <see cref="IEnumerable_GetEnumeratorTest"/> verify the <see cref="IEnumerable{T}.GetEnumerator"/> and <see cref="IEnumerable.GetEnumerator"/> methods, respectively.
    /// </summary>
    [TestMethod]
    public void GetEnumeratorTest2()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);

        GetEnumeratorTest([]);
        GetEnumeratorTest([item0]);
        GetEnumeratorTest([item0, item1]);
        GetEnumeratorTest([item0, item1, item2, item3]);
        GetEnumeratorTest([item0, item1, item1, item0, item1]);

        void GetEnumeratorTest(GenericParameterHelper[] items)
        {
            var testObject = ImmutableArrayValue.Create(items);
            ImmutableArray<GenericParameterHelper>.Enumerator actual = testObject.GetEnumerator();
            Assert.IsNotNull(actual);
            IEnumerator<GenericParameterHelper> expected = items.AsEnumerable().GetEnumerator();
            while (expected.MoveNext())
            {
                Assert.IsTrue(actual.MoveNext());
                Assert.AreEqual(expected.Current, actual.Current);
            }
            Assert.IsFalse(actual.MoveNext());
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableArray{T}.GetHashCode"/> <i>typically</i> produces unique hash codes
    /// </summary>
    [TestMethod]
    public void GetHashCodeTest2()
    {
        var hashCodes = new HashSet<int>();
        AddHashCode();
        for (int i = 0; i < 100; i++)
        {
            AddHashCode(i);
        }
        for (int i = -1; i >= -100; i--)
        {
            AddHashCode(i);
        }
        AddHashCode(int.MaxValue);
        AddHashCode(int.MinValue);
        AddHashCode(0, 1);
        AddHashCode(1, 0);

        // add arrays where all items are equal [0, 0], [0, 0, 0], ..., [1, 1], [1, 1, 1], ..., [2, 2], ...
        for (int num = 0; num < 3; num++)
        {
            for (int i = 2; i < 200; i++)
            {
                var items = new int[i];
#if NET462
                ArrayExtensions.Fill(items, num);
#else
                Array.Fill(items, num);
#endif
                AddHashCode(items);
            }
        }

        {
            var items = new List<int>() { 1 };
            for (int i = 2; i < 200; i++)
            {
                items.Add(i);
                AddHashCode(items.ToArray());
            }
        }

        void AddHashCode(params int[] items)
        {
            var testObject = ImmutableArrayValue.Create(items);
            int hashCode = testObject.GetHashCode();
            var isUnique = hashCodes.Add(hashCode);
            Assert.IsTrue(isUnique);
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableArrayValue{T}.op_Implicit(ImmutableArrayValue{T})"/>
    /// </summary>
    [TestMethod]
    public void OpToImmutableArrayTest()
    {
        var value = ImmutableArrayValue.Create(1, 2, 3);
        ImmutableArray<int> actual = value; // do the conversion ImmutableArrayValue<int> -> ImmutableArray<int>
        var expected = value.Value;
        Assert.IsTrue(ImmutableArrayValue.SequenceEqual(expected, actual));

        value = default;
        actual = value; // do the conversion ImmutableArrayValue<int> -> ImmutableArray<int>
        Assert.AreEqual(default, actual);
    }

    /// <summary>
    /// Verifies <see cref="ImmutableArrayValue{T}.op_Implicit(ImmutableArray{T})"/>
    /// </summary>
    [TestMethod]
    public void OpToImmutableArrayValueTest()
    {
        var value = ImmutableArray.Create(1, 2, 3);
        ImmutableArrayValue<int> actual = value; // do the conversion ImmutableArray<int> -> ImmutableArrayValue<int>
        var expected = new ImmutableArrayValue<int>(value);
        Assert.AreEqual(expected, actual);

        value = default;
        actual = value;  // do the conversion ImmutableArray<int> -> ImmutableArrayValue<int>
        Assert.AreEqual(default, actual);
        Assert.IsTrue(actual.IsDefault);
        Assert.AreEqual(0, actual.Length);
    }

    [TestMethod]
    public void IsDefaultTest()
    {
        Assert.IsTrue(default(ImmutableArrayValue<GenericParameterHelper>).IsDefault);
        Assert.IsTrue(new ImmutableArrayValue<GenericParameterHelper>().IsDefault);
        Assert.IsFalse(new ImmutableArrayValue<GenericParameterHelper>(ImmutableArray<GenericParameterHelper>.Empty).IsDefault);
        var items = ImmutableArray.Create(new GenericParameterHelper());
        Assert.IsFalse(new ImmutableArrayValue<GenericParameterHelper>(items).IsDefault);
    }

    [TestMethod]
    public void IsDefaultOrEmptyTest()
    {
        Assert.IsTrue(default(ImmutableArrayValue<GenericParameterHelper>).IsDefaultOrEmpty);
        Assert.IsTrue(new ImmutableArrayValue<GenericParameterHelper>().IsDefaultOrEmpty);
        Assert.IsTrue(new ImmutableArrayValue<GenericParameterHelper>(ImmutableArray<GenericParameterHelper>.Empty).IsDefaultOrEmpty);
        var items = ImmutableArray.Create(new GenericParameterHelper());
        Assert.IsFalse(new ImmutableArrayValue<GenericParameterHelper>(items).IsDefaultOrEmpty);
    }

    [TestMethod]
    public void ValueTest()
    {
        var item1 = new GenericParameterHelper(1);
        ValueTest(default, []);
        ValueTest(new ImmutableArrayValue<GenericParameterHelper>(), []);
#pragma warning disable IDE0303 // Simplify collection initialization // cannot use collection initialization with .net versions before 8.0
        ValueTest(new ImmutableArrayValue<GenericParameterHelper>(ImmutableArray.Create<GenericParameterHelper>()), []);
        ValueTest(new ImmutableArrayValue<GenericParameterHelper>(ImmutableArray.Create<GenericParameterHelper>(item1)), [item1]);
#pragma warning restore IDE0303 // Simplify collection initialization

        void ValueTest(ImmutableArrayValue<GenericParameterHelper> testObject, GenericParameterHelper[] expectedItems)
        {
            var actual = testObject.Value;
            Assert.IsFalse(actual.IsDefault);
            CollectionAssert.AreEqual(expectedItems, actual);
        }
    }
}
