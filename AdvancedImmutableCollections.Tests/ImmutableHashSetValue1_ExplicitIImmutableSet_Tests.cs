using System.Collections.Immutable;
using AdvancedImmutableCollections.Tests.CollectionAdapters;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableHashSetValue{T}"/> using <see cref="IImmutableSet{T}"/> interface explicitly
/// </summary>
[TestClass]
public sealed class ImmutableHashSetValue1_ExplicitIImmutableSet_Tests : ImmutableHashSetTestsBase<ExplicitImmutableHashSetValueAdapterFactory>
{
    protected override Type GetTestObjectType<TItem>() => typeof(ImmutableHashSetValue<TItem>);

    public override bool VerifyIntersectWithReferenceEquality => false; // do not check reference equality because the underlying ImmutableHashSet takes elements from the other collection
}
