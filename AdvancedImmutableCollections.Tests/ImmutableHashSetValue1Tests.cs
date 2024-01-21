using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableHashSetValue{T}"/>
/// </summary>
[TestClass]
public sealed class ImmutableHashSetValue1Tests : ImmutableSetTestsBase<ImmutableHashSetValue<GenericParameterHelper>>
{
#if NET6_0_OR_GREATER
    protected override ISetEqualityWithEqualityComparerTestStrategy EqualityTestStrategy
#else
    protected override ISetEqualityWithEqualityComparerTestStrategy SetEqualityTestStrategy
#endif
        => SetValueEqualityTestStrategy.Default;

    internal protected override ImmutableHashSetValue<GenericParameterHelper> CreateInstance() => new ImmutableHashSetValue<GenericParameterHelper>();

    internal protected override ImmutableHashSetValue<GenericParameterHelper> CreateInstance(params GenericParameterHelper[] initialItems)
        => new ImmutableHashSetValue<GenericParameterHelper>(initialItems.ToImmutableArray());
    protected override IReadOnlyCollection<T> CreateInstance<T>(params T[] initialItems) => new ImmutableHashSetValue<T>(initialItems.ToImmutableArray());
    protected override ImmutableHashSetValue<GenericParameterHelper> CreateInstance(HashSet<GenericParameterHelper> source)
        => new ImmutableHashSetValue<GenericParameterHelper>(source);

    protected override IReadOnlyCollection<T>? GetDefaultValue<T>() => default(ImmutableHashSetValue<T>);

    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(ImmutableHashSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Add(item);

    protected override IReadOnlyCollection<GenericParameterHelper> AddRange(ImmutableHashSetValue<GenericParameterHelper> collection, params GenericParameterHelper[] newItems) => collection.Union(newItems);

    protected override IReadOnlyCollection<GenericParameterHelper> Remove(ImmutableHashSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Remove(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Clear(ImmutableHashSetValue<GenericParameterHelper> collection) => collection.Clear();

    protected override IImmutableSet<GenericParameterHelper> Except(ImmutableHashSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.Except(other);
    protected override IImmutableSet<GenericParameterHelper> Union(ImmutableHashSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.Union(other);
    protected override IImmutableSet<GenericParameterHelper> Intersect(ImmutableHashSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.Intersect(other);
    protected override IImmutableSet<GenericParameterHelper> SymmetricExcept(ImmutableHashSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.SymmetricExcept(other);

    protected override bool Contains(ImmutableHashSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Contains(item);

    protected override IEnumerator<GenericParameterHelper> GetEnumerator(ImmutableHashSetValue<GenericParameterHelper> collection) => collection.GetEnumerator();

    public override bool VerifyIntersectWithReferenceEquality => false; // do not check reference equality because the underlying ImmutableHashSet takes elements from the other collection

    [TestMethod]
    public void Ctor_IEnumerable_Test()
    {
        Ctor_ImmutableHashSet_Test(ImmutableHashSet<GenericParameterHelper>.Empty);
        Ctor_ImmutableHashSet_Test(ImmutableHashSet.Create(1, 2, 3));

        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        Ctor_ImmutableHashSet_Test(new HashSet<GenericParameterHelper>([item0, item1, item0b, item2, item1b], ReferenceEqualityComparer.Instance));
        Ctor_ImmutableHashSet_Test(new List<GenericParameterHelper> { item0, item1, item0b, item2, item1b });
        Ctor_ImmutableHashSet_Test(new List<GenericParameterHelper> { item2, item1, item0 });
        Assert.ThrowsException<ArgumentNullException>(() => new ImmutableHashSetValue<GenericParameterHelper>((IEnumerable<GenericParameterHelper>)null!));

        void Ctor_ImmutableHashSet_Test<T>(IEnumerable<T> items)
        {
            var actual = new ImmutableHashSetValue<T>(items);

            List<T> uniqueItems;
            if (items is HashSet<T> set)
            {
                Assert.AreSame(set.Comparer, actual.Value.KeyComparer);
                uniqueItems = set.ToList();
            }
            else
            {
                uniqueItems = items.Distinct().ToList();
            }
            CollectionAssert.AreEquivalent(uniqueItems, actual.ToList());
        }
    }

    [TestMethod]
    public void Ctor_ImmutableHashSet_Test()
    {
        Ctor_ImmutableHashSet_Test(ImmutableHashSet<GenericParameterHelper>.Empty);
        Ctor_ImmutableHashSet_Test(ImmutableHashSet.Create(1, 2, 3));
        Assert.ThrowsException<ArgumentNullException>(() => new ImmutableHashSetValue<GenericParameterHelper>((ImmutableHashSet<GenericParameterHelper>)null!));

        void Ctor_ImmutableHashSet_Test<T>(ImmutableHashSet<T> set)
        {
            var actual = new ImmutableHashSetValue<T>(set);
            Assert.AreSame(set, actual.Value);
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableHashSetValue{T}.op_Implicit(ImmutableHashSetValue{T})"/>
    /// </summary>
    [TestMethod]
    public void OpToImmutableHashSetTest()
    {
        var source = ImmutableHashSet.Create(1, 2, 3);
        var value = new ImmutableHashSetValue<int>(source);
        ImmutableHashSet<int> actual = value; // do the conversion ImmutableHashSetValue<int> -> ImmutableHashSet<int>
        Assert.IsTrue(source.SetEquals(actual));
        Assert.AreSame(source, actual);
    }

    /// <summary>
    /// Verifies <see cref="ImmutableHashSetValue{T}.op_Implicit(ImmutableHashSet{T})"/>
    /// </summary>
    [TestMethod]
    public void OpToImmutableHashSetValueTest()
    {
        var value = ImmutableHashSet.Create(1, 2, 3);
        ImmutableHashSetValue<int> actual = value; // do the conversion ImmutableHashSet<int> -> ImmutableHashSetValue<int>
        var expected = new ImmutableHashSetValue<int>(value);
        Assert.AreEqual(expected, actual);
    }
}
