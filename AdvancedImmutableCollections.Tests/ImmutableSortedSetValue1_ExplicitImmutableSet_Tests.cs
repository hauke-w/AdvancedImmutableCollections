using System.Collections.Immutable;
using AdvancedImmutableCollections.Tests.CollectionAdapters;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableSortedSetValue{T}"/> using <see cref="IImmutableSet{T}"/> interface explicitly
/// </summary>
[TestClass]
public class ImmutableSortedSetValue1_ExplicitImmutableSet_Tests : ImmutableSortedSetTestsBase<ExplicitImmutableSortedSetValueAdapterFactory>
{
    protected override Type GetTestObjectType<TItem>() => typeof(ImmutableSortedSetValue<TItem>);
}
