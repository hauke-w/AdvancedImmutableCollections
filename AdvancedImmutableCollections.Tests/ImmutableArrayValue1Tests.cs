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
    protected override IImmutableList<GenericParameterHelper> RemoveRange(ImmutableArrayValue<GenericParameterHelper> collection, int start, int count) => collection.RemoveRange(start, count);
    protected override IImmutableList<GenericParameterHelper> SetItem(ImmutableArrayValue<GenericParameterHelper> collection, int index, GenericParameterHelper item) => collection.SetItem(index, item);
}
