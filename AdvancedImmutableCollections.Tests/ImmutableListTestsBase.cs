﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

public abstract class ImmutableListTestsBase<TTestObject> : ImmutableCollectionTestsBase<TTestObject, List<GenericParameterHelper>>
    where TTestObject : IImmutableList<GenericParameterHelper>
{
    protected sealed override List<GenericParameterHelper> GetMutableCollection(params GenericParameterHelper[] initialItems) => new(initialItems);

    protected sealed override void AssertCollectionsAreEqual(ICollection expected, ICollection actual)
        => CollectionAssert.AreEqual(expected, actual);

    protected abstract int IndexOf(TTestObject collection, GenericParameterHelper item);

    protected abstract int LastIndexOf(TTestObject collection, GenericParameterHelper item);

    protected abstract IImmutableList<GenericParameterHelper> Insert(TTestObject collection, int index, GenericParameterHelper item);

    protected abstract IImmutableList<GenericParameterHelper> InsertRange(TTestObject collection, int index, params GenericParameterHelper[] items);

    protected abstract IImmutableList<GenericParameterHelper> RemoveAt(TTestObject collection, int index);

    protected abstract IImmutableList<GenericParameterHelper> RemoveRange(TTestObject collection, int start, int count);

    protected abstract IImmutableList<GenericParameterHelper> RemoveRange(TTestObject collection, IEnumerable<GenericParameterHelper> items, IEqualityComparer<GenericParameterHelper>? equalityComparer);

    protected abstract IImmutableList<GenericParameterHelper> RemoveAll(TTestObject collection, Predicate<GenericParameterHelper> predicate);

    protected abstract IImmutableList<GenericParameterHelper> SetItem(TTestObject collection, int index, GenericParameterHelper item);

    [TestMethod]
    public void IndexOfTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var testObject = GetTestObject(item0, item1, item2, item0, item1, item2);

        IndexOfTest(item0, 0);
        IndexOfTest(item2, 2);
        IndexOfTest(item1, 1);
        IndexOfTest(item3, -1);

        void IndexOfTest(GenericParameterHelper item, int expexted)
        {
            var actual = IndexOf(testObject, item);
            Assert.AreEqual(expexted, actual);
        }
    }

    [TestMethod]
    public void LastIndexOfTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var testObject = GetTestObject(item0, item1, item2, item0, item1, item2);

        LastIndexOfTest(item0, 3);
        LastIndexOfTest(item2, 5);
        LastIndexOfTest(item1, 4);
        LastIndexOfTest(item3, -1);

        void LastIndexOfTest(GenericParameterHelper item, int expexted)
        {
            var actual = LastIndexOf(testObject, item);
            Assert.AreEqual(expexted, actual);
        }
    }

    [TestMethod]
    public void InsertTest()
    {
        var testObject = GetTestObject();
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);

        InsertTest(item0, 0, [item0]);
        InsertTest(item1, 0, [item1, item0]);
        InsertTest(item2, 2, [item1, item0, item2]);
        InsertTest(item3, 1, [item1, item3, item0, item2]);

        void InsertTest(GenericParameterHelper item, int index, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = Insert(testObject, index, item);
            AssertCollectionsAreEqual(expected, actual);
            AssertCollectionsAreEqual(itemsBefore, testObject);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }

    [TestMethod]
    public void InsertRangeTest()
    {
        var testObject = GetTestObject();
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);
        var item8 = new GenericParameterHelper(8);
        var item9 = new GenericParameterHelper(9);

        InsertRangeTest(0, [item0, item1], [item0, item1]);
        InsertRangeTest(2, [item2, item3], [item0, item1, item2, item3]);
        InsertRangeTest(1, [item4, item5], [item0, item4, item5, item1, item2, item3]);
        InsertRangeTest(6, [item6], [item0, item4, item5, item1, item2, item3, item6]);
        InsertRangeTest(5, [item7, item8, item9], [item0, item4, item5, item1, item2, item7, item8, item9, item3, item6]);
        InsertRangeTest(5, [item4, item9], [item0, item4, item5, item1, item2, item4, item9, item7, item8, item9, item3, item6]);

        void InsertRangeTest(int index, GenericParameterHelper[] items, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = InsertRange(testObject, index, items);
            AssertCollectionsAreEqual(expected, actual);
            AssertCollectionsAreEqual(itemsBefore, testObject);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }

    [TestMethod]
    public void RemoveAtTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var testObject = GetTestObject(item0, item1, item2, item3, item4);

        RemoveAtTest(4, [item0, item1, item2, item3]);
        RemoveAtTest(2, [item0, item1, item3]);
        RemoveAtTest(0, [item1, item3]);
        RemoveAtTest(1, [item1]);
        RemoveAtTest(0, []);

        void RemoveAtTest(int index, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = RemoveAt(testObject, index);
            AssertCollectionsAreEqual(expected, actual);
            AssertCollectionsAreEqual(itemsBefore, testObject);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }

    /// <summary>
    /// Verifies removing a range using start index and count
    /// </summary>
    [TestMethod]
    public void RemoveRange_int_int_Test()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);
        var item8 = new GenericParameterHelper(8);
        var item9 = new GenericParameterHelper(9);
        var testObject = GetTestObject(item0, item1, item2, item3, item4, item5, item6, item7, item8, item9);

        RemoveRangeTest(0, 0, [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9]);
        RemoveRangeTest(0, 1, [item1, item2, item3, item4, item5, item6, item7, item8, item9]);
        RemoveRangeTest(7, 2, [item1, item2, item3, item4, item5, item6, item7]);
        RemoveRangeTest(2, 3, [item1, item2, item6, item7]);
        RemoveRangeTest(1, 1, [item1, item6, item7]);
        RemoveRangeTest(0, 3, []);

        void RemoveRangeTest(int start, int count, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = RemoveRange(testObject, start, count);
            AssertCollectionsAreEqual(expected, actual);
            AssertCollectionsAreEqual(itemsBefore, testObject);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }

    /// <summary>
    /// Verifies removing a range using a predicate: RemoveAll(Predicate&lt;T&gt; match)
    /// </summary>
    [TestMethod]
    public void RemoveAllTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);
        var item8 = new GenericParameterHelper(8);
        var item9 = new GenericParameterHelper(9);
        var item10 = new GenericParameterHelper(10);
        var testObject = GetTestObject(item10, item0, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, new GenericParameterHelper(10));

        RemoveAllTest(it => it.Data == 10, [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9]);
        RemoveAllTest(it => it.Data > 7, [item0, item1, item2, item3, item4, item5, item6, item7]);
        RemoveAllTest(it => it.Data <= 0, [item1, item2, item3, item4, item5, item6, item7]);
        RemoveAllTest(it => it.Data < 1, [item1, item2, item3, item4, item5, item6, item7]);
        RemoveAllTest(it => it.Data % 2 == 0, [item1, item3, item5, item7]);
        RemoveAllTest(it => it.Data % 2 == 0, [item1, item3, item5, item7]);
        RemoveAllTest(it => it.Data is 3 or 5, [item1, item7]);
        RemoveAllTest(it => it.Data is 1 or 7, []);
        RemoveAllTest(PredicateNotExpected, []);


        void RemoveAllTest(Predicate<GenericParameterHelper> predicate, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = RemoveAll(testObject, predicate);
            AssertCollectionsAreEqual(expected, actual);
            AssertCollectionsAreEqual(itemsBefore, testObject);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }

        bool PredicateNotExpected(GenericParameterHelper item)
        {
            Assert.Fail("Predicate should not be called");
            return false;
        }
    }

    /// <summary>
    /// Verifies removing a range using another collection and an equality comparer: RemoveRange(IEnumerable&lt;T&gt; items, IEqualityComparer&lt;T&gt;? equalityComparer)
    /// </summary>
    [TestMethod]
    public void RemoveRange_IEnumerable_IEqualityComparer_Test()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);
        var item8 = new GenericParameterHelper(8);
        var item9 = new GenericParameterHelper(9);
        var item10 = new GenericParameterHelper(10);
        var item10_2 = new GenericParameterHelper(10);
        var testObject = GetTestObject(item10, item0, item1, item2, item3, item10_2, item4, item10, item5, item6, item7, item8, item9, item10);

        RemoveRangeTest([item10, item10], [item0, item1, item2, item3, item4, item10, item5, item6, item7, item8, item9, item10]);
        RemoveRangeTest([item10], [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10], EqualityComparer<GenericParameterHelper>.Default);
        RemoveRangeTest([item10_2], [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9]);
        RemoveRangeTest([item8, item3, item4], [item0, item1, item2, item5, item6, item7, item9]);
        RemoveRangeTest([item8], [item0, item1, item2, item5, item6, item7, item9]);
        RemoveRangeTest([item4, item7, item5], [item0, item1, item2, item6, item9]);
        RemoveRangeTest([], [item0, item1, item2, item6, item9], EqualityComparer<GenericParameterHelper>.Default);
        RemoveRangeTest([item0, item9], [item1, item2, item6]);
        RemoveRangeTest([item1, item9], [item2, item6], EqualityComparer<GenericParameterHelper>.Default);
        RemoveRangeTest([item6, item2], []);
        RemoveRangeTest([item6, item2], []);
        RemoveRangeTest([], []);

        void RemoveRangeTest(IEnumerable<GenericParameterHelper> items, GenericParameterHelper[] expected, IEqualityComparer<GenericParameterHelper>? equalityComparer = null)
        {
            var itemsBefore = testObject.ToList();
            var actual = RemoveRange(testObject, items, equalityComparer);
            AssertCollectionsAreEqual(expected, actual);
            AssertCollectionsAreEqual(itemsBefore, testObject);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }

    /// <summary>
    /// Verifies removing a range using another collection and an equality comparer: RemoveRange(IEnumerable&lt;T&gt; items, IEqualityComparer&lt;T&gt;? equalityComparer)
    /// </summary>
    [TestMethod]
    public void RemoveRange_IEnumerable_IEqualityComparer_Test2()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);
        var item8 = new GenericParameterHelper(8);
        var item9 = new GenericParameterHelper(9);
        var item10 = new GenericParameterHelper(10);
        var item10_2 = new GenericParameterHelper(10);
        var testObject = GetTestObject(item10, item0, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10_2, item10, item10_2, item10);

        RemoveRangeTest([item10, item10], [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10_2, item10_2, item10]);
        RemoveRangeTest([item10_2, item10_2, item10_2], [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10]);
        RemoveRangeTest([item8, item10, new GenericParameterHelper(3), new GenericParameterHelper(4)], [item0, item1, item2, item3, item4, item5, item6, item7, item9]);
        RemoveRangeTest([item3, item4], [item0, item1, item2, item5, item6, item7, item9]);
        RemoveRangeTest([new GenericParameterHelper(0), new GenericParameterHelper(1), item1, item2, item5, item6, item7, new GenericParameterHelper(9)], [item0, item9]);
        RemoveRangeTest([item0, item9, new GenericParameterHelper(0)], []);
        RemoveRangeTest([item0, new GenericParameterHelper(0)], []);

        void RemoveRangeTest(IEnumerable<GenericParameterHelper> items, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = RemoveRange(testObject, items, ReferenceEqualityComparer.Instance);
            AssertCollectionsAreEqual(expected, actual);
            AssertCollectionsAreEqual(itemsBefore, testObject);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }

    [TestMethod]
    public void SetItemTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var testObject = GetTestObject(item0, item1, item2);

        SetItemTest(0, item3, [item3, item1, item2]);
        SetItemTest(2, item4, [item3, item1, item4]);
        SetItemTest(1, item4, [item3, item4, item4]);

        void SetItemTest(int index, GenericParameterHelper item, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = SetItem(testObject, index, item);
            AssertCollectionsAreEqual(expected, actual);
            AssertCollectionsAreEqual(itemsBefore, testObject);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }
}