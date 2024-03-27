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
}