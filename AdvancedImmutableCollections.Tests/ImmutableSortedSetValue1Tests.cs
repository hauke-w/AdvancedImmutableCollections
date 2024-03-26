using AdvancedImmutableCollections.Tests.CollectionAdapters;
using AdvancedImmutableCollections.Tests.Util;
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

        void VerifySetEquals(ImmutableSortedSetValue<string> testObject, IEnumerable<string> other, bool expected)
        {
            var actual = testObject.SetEquals(other);
            Assert.AreEqual(expected, actual);
        }
    }
}