using AdvancedImmutableCollections.Tests.CollectionAdapters;
using AdvancedImmutableCollections.Tests.Util;
using Castle.Components.DictionaryAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedImmutableCollections;

[TestClass]
public class ImmutableSortedSetValue1Tests : ImmutableSortedSetTestsBase<ImmutableSortedSetValueAdapterFactory>
{
    protected override Type GetTestObjectType<TItem>() => typeof(ImmutableSortedSetValue<TItem>);

    [TestMethod]
    public void IsDefaultTest()
    {
        Assert.IsTrue(default(ImmutableSortedSetValue<GenericParameterHelper>).IsDefault);
        Assert.IsTrue(new ImmutableSortedSetValue<GenericParameterHelper>().IsDefault);
        Assert.IsFalse(new ImmutableSortedSetValue<GenericParameterHelper>(ImmutableArray<GenericParameterHelper>.Empty).IsDefault);
        GenericParameterHelper[] items = [new()];
        Assert.IsFalse(new ImmutableSortedSetValue<GenericParameterHelper>(items).IsDefault);
    }

    [TestMethod]
    public void IsDefaultOrEmptyTest()
    {
        Assert.IsTrue(default(ImmutableSortedSetValue<GenericParameterHelper>).IsDefaultOrEmpty);
        Assert.IsTrue(new ImmutableSortedSetValue<GenericParameterHelper>().IsDefaultOrEmpty);
        Assert.IsTrue(new ImmutableSortedSetValue<GenericParameterHelper>(ImmutableArray<GenericParameterHelper>.Empty).IsDefaultOrEmpty);
        var items = new GenericParameterHelper[] { new GenericParameterHelper() };
        Assert.IsFalse(new ImmutableSortedSetValue<GenericParameterHelper>(items).IsDefaultOrEmpty);
    }

    /// <summary>
    /// Verifies constructor <see cref="ImmutableSortedSetValue{T}.ImmutableSortedSetValue(ImmutableSortedSet{T})"/>
    /// </summary>
    [TestMethod]
    public void Ctor_ImmutableSortedSet_Test()
    {
        ImmutableSortedSet<GenericParameterHelper> set = null!;
        Assert.ThrowsException<ArgumentNullException>(() => new ImmutableSortedSetValue<GenericParameterHelper>(set));

        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        set = ImmutableSortedSet.Create(item1, item2);
        var actual = new ImmutableSortedSetValue<GenericParameterHelper>(set);
        AssertCollectionsAreEqual(set, actual);
        Assert.AreSame(set, actual.Value);
        Assert.IsFalse(actual.IsDefault);
    }

    /// <summary>
    /// Verifies constructor <see cref="ImmutableSortedSetValue{T}.ImmutableSortedSetValue(IEnumerable{T})"/>
    /// </summary>
    [TestMethod]
    public void Ctor_IEnumerable_Test()
    {
        IEnumerable<GenericParameterHelper> items = null!;
        Assert.ThrowsException<ArgumentNullException>(() => new ImmutableSortedSetValue<GenericParameterHelper>(items));

        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        items = [item1, item2, item1];
        var set1 = new ImmutableSortedSetValue<GenericParameterHelper>(items);
        AssertCollectionsAreEqual(new SortedSet<GenericParameterHelper>(items), set1);
        Assert.IsFalse(set1.IsDefault);

        items = ImmutableSortedSet.Create(item1);
        set1 = new ImmutableSortedSetValue<GenericParameterHelper>(items);
        AssertCollectionsAreEqual(items, set1);
        Assert.AreSame(items, set1.Value);
        Assert.IsFalse(set1.IsDefault);

        items = set1;
        var set2 = new ImmutableSortedSetValue<GenericParameterHelper>(items);
        AssertCollectionsAreEqual(set1.Value, set2);
        Assert.AreSame(set1.Value, set2.Value);
        Assert.IsFalse(set2.IsDefault);

        items = new SortedSet<GenericParameterHelper>([item1, item2]);
        set1 = new ImmutableSortedSetValue<GenericParameterHelper>(items);
        AssertCollectionsAreEqual(items, set1);
        Assert.IsFalse(set1.IsDefault);
    }

    /// <summary>
    /// Verifies constructor <see cref="ImmutableSortedSetValue{T}.ImmutableSortedSetValue(IComparer{T}?, IEnumerable{T})"/>
    /// </summary>
    [TestMethod]
    public void Ctor_IComparer_IEnumerable_Test()
    {
        IComparer<string>? comparer = null;
        IEnumerable<string> items = null!;
        Assert.ThrowsException<ArgumentNullException>(() => new ImmutableSortedSetValue<string>(comparer, items));
        comparer = Comparer<string>.Default;
        Assert.ThrowsException<ArgumentNullException>(() => new ImmutableSortedSetValue<string>(comparer, items));

        items = ["a", "b", "a", "B"];
        comparer = null;
        var actual = new ImmutableSortedSetValue<string>(comparer, items);
        AssertCollectionsAreEqual(["a", "b", "B"], actual);
        Assert.IsFalse(actual.IsDefault);
        Assert.AreEqual(Comparer<string>.Default, actual.Value.KeyComparer);

        comparer = StringComparer.InvariantCultureIgnoreCase;
        actual = new ImmutableSortedSetValue<string>(comparer, items);
        AssertCollectionsAreEqual(["a", "b"], actual);
        Assert.IsFalse(actual.IsDefault);
        Assert.AreSame(comparer, actual.Value.KeyComparer);
    }

    /// <summary>
    /// Verifies constructor <see cref="ImmutableSortedSetValue{T}.ImmutableSortedSetValue(IComparer{T}?)"/>
    /// </summary>
    [TestMethod]
    public void Ctor_IComparer_Test()
    {
        IComparer<string>? comparer = null;
        var actual = new ImmutableSortedSetValue<string>(comparer);
        Assert.AreEqual(0, actual.Count);
        Assert.IsFalse(actual.IsDefault);
        Assert.AreEqual(Comparer<string>.Default, actual.Value.KeyComparer);

        comparer = StringComparer.InvariantCultureIgnoreCase;
        actual = new ImmutableSortedSetValue<string>(comparer);
        Assert.AreEqual(0, actual.Count);
        Assert.IsFalse(actual.IsDefault);
        Assert.AreSame(comparer, actual.Value.KeyComparer);
    }

    /// <summary>
    /// Verifies <see cref="ImmutableSortedSetValue{T}.SetEquals(IEnumerable{T})"/>
    /// with additional whitebox test cases
    /// </summary>
    [TestMethod]
    public void SetEqualsTest2()
    {
        VerifySetEquals(ImmutableSortedSetValue.Create("a"), ImmutableSortedSet.Create("a"), true);
        VerifySetEquals(default, ImmutableSortedSet.Create("a"), false);
        VerifySetEquals(default, default(ImmutableSortedSetValue<string>), true);
        VerifySetEquals(default, ImmutableSortedSetValue.Create<string>(), true);

        void VerifySetEquals(ImmutableSortedSetValue<string> testObject, IEnumerable<string> other, bool expected)
        {
            if (other is Tests.CollectionAdapters.ICollectionAdapter<string> otherAdapter)
            {
                other = otherAdapter.Collection;
            }
            var actual = testObject.SetEquals(other);
            Assert.AreEqual(expected, actual);
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableSortedSetValue{T}.SetEquals(ImmutableSortedSet{T})"/>
    /// with additional whitebox test cases
    /// </summary>
    [TestMethod]
    public void SetEqualsTest3()
    {
        VerifySetEquals(ImmutableSortedSetValue.Create<string>(), ImmutableSortedSet<string>.Empty, true);
        VerifySetEquals(ImmutableSortedSetValue.Create("a"), ImmutableSortedSet.Create("a"), true);
        VerifySetEquals(ImmutableSortedSetValue.Create("a"), ImmutableSortedSet.Create("a", "b"), false);
        VerifySetEquals(ImmutableSortedSetValue.Create("a"), ImmutableSortedSet.Create(StringComparer.Ordinal, "a"), true);
        VerifySetEquals(ImmutableSortedSetValue.Create("a"), ImmutableSortedSet.Create(StringComparer.Ordinal, "A"), false);
        VerifySetEquals(ImmutableSortedSetValue.Create("a"), ImmutableSortedSet.Create(StringComparer.OrdinalIgnoreCase, "A"), false);
        VerifySetEquals(ImmutableSortedSetValue.Create(StringComparer.OrdinalIgnoreCase, "a"), ImmutableSortedSet.Create(StringComparer.OrdinalIgnoreCase, "A"), true);
        VerifySetEquals(ImmutableSortedSetValue.Create(StringComparer.OrdinalIgnoreCase, "a"), ImmutableSortedSet.Create(StringComparer.Ordinal, "A"), true);
        VerifySetEquals(ImmutableSortedSetValue.Create(StringComparer.Ordinal, "a", "A"), ImmutableSortedSet.Create(StringComparer.Ordinal, "A"), false);
        VerifySetEquals(ImmutableSortedSetValue.Create(StringComparer.Ordinal, "a", "A"), ImmutableSortedSet.Create(StringComparer.OrdinalIgnoreCase, "A"), false);
        VerifySetEquals(default, ImmutableSortedSet.Create("a"), false);
        VerifySetEquals(default, ImmutableSortedSet<string>.Empty, true);

        void VerifySetEquals(ImmutableSortedSetValue<string> testObject, ImmutableSortedSet<string> other, bool expected)
        {
            var actual = testObject.SetEquals(other);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(testObject.Value.SetEquals(other), expected, "Inconsistent results");
        }
    }

    [TestMethod]
    public void WithComparerTest()
    {
        ImmutableSortedSetValue<string> testObject = default;
        var actual = testObject.WithComparer(null);
        Assert.IsTrue(actual.IsDefault);
        Assert.AreEqual(default, actual);
        Assert.AreEqual(Comparer<string>.Default, actual.Value.KeyComparer);

        testObject = default;
        actual = testObject.WithComparer(Comparer<string>.Default);
        Assert.IsFalse(actual.IsDefault);
        Assert.AreEqual(0, actual.Count);
        Assert.AreEqual(Comparer<string>.Default, actual.Value.KeyComparer);

        testObject = default;
        actual = testObject.WithComparer(StringComparer.OrdinalIgnoreCase);
        Assert.IsFalse(actual.IsDefault);
        Assert.AreEqual(StringComparer.OrdinalIgnoreCase, actual.Value.KeyComparer);
        Assert.AreEqual(0, actual.Count);

        testObject = actual;
        actual = testObject.WithComparer(StringComparer.InvariantCulture);
        Assert.IsFalse(actual.IsDefault);
        Assert.AreEqual(StringComparer.InvariantCulture, actual.Value.KeyComparer);
        Assert.AreEqual(0, actual.Count);

        testObject = ImmutableSortedSetValue.Create(StringComparer.Ordinal, "a", "b", "A");
        actual = testObject.WithComparer(null);
        Assert.IsFalse(actual.IsDefault);
        Assert.AreEqual(Comparer<string>.Default, actual.Value.KeyComparer);
        AssertCollectionsAreEqual(["a", "A", "b"], actual);

        testObject = actual;
        actual = testObject.WithComparer(StringComparer.OrdinalIgnoreCase);
        Assert.IsFalse(actual.IsDefault);
        Assert.AreEqual(StringComparer.OrdinalIgnoreCase, actual.Value.KeyComparer);
        AssertCollectionsAreEqual(["a", "b"], actual);

        testObject = actual;
        actual = testObject.WithComparer(StringComparer.OrdinalIgnoreCase);
        Assert.IsFalse(actual.IsDefault);
        Assert.AreEqual(StringComparer.OrdinalIgnoreCase, actual.Value.KeyComparer);
        AssertCollectionsAreEqual(["a", "b"], actual);
        Assert.AreSame(testObject.Value, actual.Value);
    }

    /// <summary>
    /// Additional test for <see cref="ImmutableSortedSetValue{T}.GetHashCode()"/>
    /// </summary>
    [TestMethod]
    public void GetHashCodeTest2()
    {
        var hashCodes = new HashSet<int>();

        var testObject = ImmutableSortedSetValue.Create<int>();

        for (int i = 0; i < 4 * 115; i++)
        {
            testObject = testObject.Add(i);
            var actual = testObject.GetHashCode();
            var isUnique = hashCodes.Add(actual);
            Assert.AreNotEqual(0, actual);
            Assert.IsTrue(isUnique);
        }
    }
}