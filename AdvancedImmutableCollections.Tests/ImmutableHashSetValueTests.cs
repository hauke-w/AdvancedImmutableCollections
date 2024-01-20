using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdvancedImmutableCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

[TestClass()]
public class ImmutableHashSetValueTests
{
    [TestMethod]
    [DataRow(new int[] { 1, 2, 3 })]
    [DataRow(new int[] { })]
    public void WithValueSemanticsTest(int[] values)
    {
        ImmutableHashSet<int> hashSet = values.ToImmutableHashSet();
        ImmutableHashSetValue<int> actual = ImmutableHashSetValue.WithValueSemantics(hashSet);
        Assert.AreSame(hashSet, actual.Value);
    }

    [TestMethod()]
    public void CreateTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        CreateTest([item0, item1, item0b, item2, item1b], [item0, item1, item2], null);
        CreateTest([item0b, item1, item0, item1b, item2, item0], [item0b, item1, item2], EqualityComparer<GenericParameterHelper>.Default);
        CreateTest([item0, item0, item0], [item0], ReferenceEqualityComparer.Instance);
        CreateTest([item0, item1, item0b, item2, item1b], [item0, item1, item0b, item2, item1b], ReferenceEqualityComparer.Instance);
        CreateTest([], [], null);
        CreateTest([], [], ReferenceEqualityComparer.Instance);

        void CreateTest(GenericParameterHelper[] sourceItems, GenericParameterHelper[] expectedItems, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var source = expectedItems.ToImmutableArray();
            var actual = ImmutableHashSetValue.Create(source, equalityComparer);
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Value);
            var expectedComparer = equalityComparer ?? EqualityComparer<GenericParameterHelper>.Default;
            Assert.AreSame(expectedComparer, actual.Value.KeyComparer);
            CollectionAssert.AreEquivalent(expectedItems, actual.ToList());
        }
    }
}