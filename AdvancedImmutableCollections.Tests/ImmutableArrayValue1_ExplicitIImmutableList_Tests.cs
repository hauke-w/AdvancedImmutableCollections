using System.Collections.Immutable;
using AdvancedImmutableCollections.Tests.CollectionAdapters;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableArrayValue{T}"/> using <see cref="IImmutableList{T}"/> interface explicitly
/// </summary>
[TestClass]
public sealed class ImmutableArrayValue1_ExplicitIImmutableList_Tests : ImmutableListTestsBase<ExplicitImmutableArrayValueAdapterFactory>
{
    protected override Type GetTestObjectType<TItem>() => typeof(ImmutableArrayValue<TItem>);
    protected override IEqualityTestStrategy EqualityTestStrategy => ListValueEqualityTestStrategy.Default;
}
