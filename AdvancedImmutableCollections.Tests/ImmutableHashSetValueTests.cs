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

    /// <summary>
    /// Verifies <see cref="ImmutableHashSetValue.Create{T}(ImmutableArray{T}, IEqualityComparer{T}?)"/>
    /// </summary>
    [TestMethod]
    public void Create_ImmutableArray_IEqualityComparer_Test()
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

        static void CreateTest(GenericParameterHelper[] sourceItems, GenericParameterHelper[] expectedItems, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var source = sourceItems.ToImmutableArray();
            var actual = ImmutableHashSetValue.Create(source, equalityComparer);
            VerifyCreateResult(actual, expectedItems, equalityComparer);
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableHashSetValue.Create{T}(IEqualityComparer{T}?, T[])"/>  
    /// </summary>
    [TestMethod]
    public void Create_IEqualityComparer_Array_Test()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        CreateTest([item0, item1, item1b, item2, item0b], [item0, item1, item2], null);
        CreateTest([item0b, item1, item1b, item0, item2, item0], [item0b, item1, item2], EqualityComparer<GenericParameterHelper>.Default);
        CreateTest([item0, item0, item0], [item0], ReferenceEqualityComparer.Instance);
        CreateTest([item0, item1, item0b, item1b, item2], [item0, item1, item0b, item2, item1b], ReferenceEqualityComparer.Instance);
        CreateTest([], [], null);
        CreateTest([], [], ReferenceEqualityComparer.Instance);

        static void CreateTest(GenericParameterHelper[] sourceItems, GenericParameterHelper[] expectedItems, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var actual = ImmutableHashSetValue.Create(equalityComparer, sourceItems);
            VerifyCreateResult(actual, expectedItems, equalityComparer);
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableHashSetValue.Create{T}(T[])"/>
    /// </summary>
    [TestMethod]
    public void Create_Array_Test()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        CreateTest([], []);
        CreateTest([item0], [item0]);
        CreateTest([item0, item0b], [item0]);
        CreateTest([item0, item1, item2, item0b, item1b], [item0, item1, item2]);

        static void CreateTest(GenericParameterHelper[] sourceItems, GenericParameterHelper[] expectedItems)
        {
            var actual = ImmutableHashSetValue.Create(sourceItems);
            VerifyCreateResult(actual, expectedItems, EqualityComparer<GenericParameterHelper>.Default);
        }
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Verifies <see cref="ImmutableHashSetValue.Create{T}(ReadOnlySpan{T})"/>
    /// </summary>
    [TestMethod]
    public void Create_ReadOnlySpan_Test()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        CreateTest([], []);
        CreateTest([item0], [item0]);
        CreateTest([item0, item0b], [item0]);
        CreateTest([item0, item2, item1, item0b, item1b, item2], [item0, item1, item2]);

        static void CreateTest(ReadOnlySpan<GenericParameterHelper> sourceItems, GenericParameterHelper[] expectedItems)
        {
            var actual = ImmutableHashSetValue.Create(sourceItems);
            VerifyCreateResult(actual, expectedItems, EqualityComparer<GenericParameterHelper>.Default);
        }
    } 
#endif

    private static void VerifyCreateResult(
        ImmutableHashSetValue<GenericParameterHelper> actual,
        GenericParameterHelper[] expectedItems,
        IEqualityComparer<GenericParameterHelper>? equalityComparer)
    {
        Assert.IsNotNull(actual);
        Assert.IsNotNull(actual.Value);
        var expectedComparer = equalityComparer ?? EqualityComparer<GenericParameterHelper>.Default;
        Assert.AreSame(expectedComparer, actual.Value.KeyComparer);

        Assert.AreEqual(expectedItems.Length, actual.Count);
        foreach (var item in expectedItems)
        {
            Assert.IsTrue(actual.TryGetValue(item, out var actualItem));
            Assert.AreSame(item, actualItem);
        }
    }

    [TestMethod]
    public void EmptyTest()
    {
        var actual = ImmutableHashSetValue.Empty<GenericParameterHelper>();
        Assert.IsFalse(actual.IsDefault);
        Assert.AreEqual(0, actual.Count);
        Assert.AreEqual(0, actual.Value.Count);
    }
}