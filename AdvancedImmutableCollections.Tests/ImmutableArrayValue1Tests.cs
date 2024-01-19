using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableArrayValue{T}"/>
/// </summary>
[TestClass]
public sealed class ImmutableArrayValue1Tests : ImmutableListTestsBase<ImmutableArrayValue<GenericParameterHelper>>
{
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

    protected override ImmutableArrayValue<GenericParameterHelper> GetTestObject() => new ImmutableArrayValue<GenericParameterHelper>();
    protected override ImmutableArrayValue<GenericParameterHelper> GetTestObject(params GenericParameterHelper[] initialItems) => new ImmutableArrayValue<GenericParameterHelper>(initialItems.ToImmutableArray());

    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(ImmutableArrayValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Add(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Remove(ImmutableArrayValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Remove(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Clear(ImmutableArrayValue<GenericParameterHelper> collection) => collection.Clear();
    protected override IReadOnlyCollection<GenericParameterHelper> AddRange(ImmutableArrayValue<GenericParameterHelper> testObject, params GenericParameterHelper[] newItems)
        => testObject.AddRange(newItems);
    protected override bool Contains(ImmutableArrayValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Contains(item);

    protected override int IndexOf(ImmutableArrayValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.IndexOf(item);

    protected override int LastIndexOf(ImmutableArrayValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.LastIndexOf(item);
    protected override IImmutableList<GenericParameterHelper> Insert(ImmutableArrayValue<GenericParameterHelper> collection, int index, GenericParameterHelper item) => collection.Insert(index, item);
    protected override IImmutableList<GenericParameterHelper> InsertRange(ImmutableArrayValue<GenericParameterHelper> collection, int index, params GenericParameterHelper[] items) => collection.InsertRange(index, items);
    protected override IImmutableList<GenericParameterHelper> RemoveAt(ImmutableArrayValue<GenericParameterHelper> collection, int index) => collection.RemoveAt(index);
    protected override IImmutableList<GenericParameterHelper> RemoveRange(ImmutableArrayValue<GenericParameterHelper> collection, int start, int count)
        => collection.RemoveRange(start, count);
    protected override IImmutableList<GenericParameterHelper> RemoveRange(ImmutableArrayValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> items, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        => collection.RemoveRange(items, equalityComparer);
    protected override IImmutableList<GenericParameterHelper> RemoveAll(ImmutableArrayValue<GenericParameterHelper> collection, Predicate<GenericParameterHelper> predicate)
        => collection.RemoveAll(predicate);
    protected override IImmutableList<GenericParameterHelper> SetItem(ImmutableArrayValue<GenericParameterHelper> collection, int index, GenericParameterHelper item) => collection.SetItem(index, item);
    protected override IImmutableList<GenericParameterHelper> Replace(ImmutableArrayValue<GenericParameterHelper> collection, GenericParameterHelper oldValue, GenericParameterHelper newValue, IEqualityComparer<GenericParameterHelper>? equalityComparer) => collection.Replace(oldValue, newValue, equalityComparer);

    protected override void AssertCollectionsAreEqual(IEnumerable<GenericParameterHelper> expected, IEnumerable<GenericParameterHelper> actual)
    {
        base.AssertCollectionsAreEqual(expected, actual);
        if (expected is ImmutableArrayValue<GenericParameterHelper> value1
            && actual is ImmutableArrayValue<GenericParameterHelper> value2)
        {
            bool areEqual = value1.Equals(value2);
            Assert.IsTrue(areEqual);
        }
    }
}
