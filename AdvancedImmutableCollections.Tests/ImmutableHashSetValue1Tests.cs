using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableHashSetValue{T}"/>
/// </summary>
[TestClass]
public sealed class ImmutableHashSetValue1Tests : ImmutableSetTestsBase<ImmutableHashSetValue<GenericParameterHelper>>
{
    protected override ImmutableHashSetValue<GenericParameterHelper> GetTestObject() => new ImmutableHashSetValue<GenericParameterHelper>();
    protected override ImmutableHashSetValue<GenericParameterHelper> GetTestObject(params GenericParameterHelper[] initialItems) => new ImmutableHashSetValue<GenericParameterHelper>(initialItems.ToImmutableArray());

    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(ImmutableHashSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Add(item);

    protected override IReadOnlyCollection<GenericParameterHelper> AddRange(ImmutableHashSetValue<GenericParameterHelper> collection, params GenericParameterHelper[] newItems) => collection.AddRange(newItems);

    protected override IReadOnlyCollection<GenericParameterHelper> Remove(ImmutableHashSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Remove(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Clear(ImmutableHashSetValue<GenericParameterHelper> collection) => collection.Clear();
    protected override bool Contains(ImmutableHashSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Contains(item);

    protected override IEnumerator<GenericParameterHelper> GetEnumerator(ImmutableHashSetValue<GenericParameterHelper> collection) => collection.GetEnumerator();
    protected override IEnumerator IEnumerable_GetEnumerator(ImmutableHashSetValue<GenericParameterHelper> collection) => ((IEnumerable)collection).GetEnumerator();
}
