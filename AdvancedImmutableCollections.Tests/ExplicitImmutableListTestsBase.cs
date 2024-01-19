using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

public abstract class ExplicitImmutableListTestsBase<TTestObject> : ImmutableListTestsBase<TTestObject>
    where TTestObject : IImmutableList<GenericParameterHelper>
{
    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(TTestObject collection, GenericParameterHelper item) => collection.Add(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Remove(TTestObject collection, GenericParameterHelper item) => collection.Remove(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Clear(TTestObject collection) => collection.Clear();

    protected override IReadOnlyCollection<GenericParameterHelper> AddRange(TTestObject collection, params GenericParameterHelper[] newItems) => collection.AddRange(newItems);

    protected override bool Contains(TTestObject collection, GenericParameterHelper item) => collection.Contains(item);

    protected override int IndexOf(TTestObject collection, GenericParameterHelper item) => collection.IndexOf(item);

    protected override int LastIndexOf(TTestObject collection, GenericParameterHelper item) => collection.LastIndexOf(item);
    protected override IImmutableList<GenericParameterHelper> Insert(TTestObject collection, int index, GenericParameterHelper item) => collection.Insert(index, item);
    protected override IImmutableList<GenericParameterHelper> InsertRange(TTestObject collection, int index, params GenericParameterHelper[] items) => collection.InsertRange(index, items);
    protected override IImmutableList<GenericParameterHelper> RemoveAt(TTestObject collection, int index) => collection.RemoveAt(index);
    protected override IImmutableList<GenericParameterHelper> RemoveRange(TTestObject collection, int start, int count) => collection.RemoveRange(start, count);
    protected override IImmutableList<GenericParameterHelper> RemoveRange(TTestObject collection, IEnumerable<GenericParameterHelper> items, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        => collection.RemoveRange(items, equalityComparer);
    protected override IImmutableList<GenericParameterHelper> RemoveAll(TTestObject collection, Predicate<GenericParameterHelper> predicate)
        => collection.RemoveAll(predicate);
    protected override IImmutableList<GenericParameterHelper> SetItem(TTestObject collection, int index, GenericParameterHelper item) => collection.SetItem(index, item);
}
