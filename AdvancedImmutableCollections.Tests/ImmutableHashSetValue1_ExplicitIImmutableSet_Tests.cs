using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableHashSetValue{T}"/> using <see cref="IImmutableList{T}"/> interface explicitly
/// </summary>
[TestClass]
public sealed class ImmutableHashSetValue1_ExplicitIImmutableSet_Tests : ExplicitImmutableSetTestsBase<IImmutableSet<GenericParameterHelper>>
{
#if NET6_0_OR_GREATER
    protected override ISetEqualityWithEqualityComparerTestStrategy EqualityTestStrategy
#else
    protected override ISetEqualityWithEqualityComparerTestStrategy SetEqualityTestStrategy
#endif
        => SetValueEqualityTestStrategy.Default;

    protected override IImmutableSet<GenericParameterHelper> DefaultValue => default(ImmutableHashSetValue<GenericParameterHelper>);

    protected override IReadOnlyCollection<T>? GetDefaultValue<T>() => default(ImmutableHashSetValue<T>);

    internal protected override IImmutableSet<GenericParameterHelper> CreateInstance() => new ImmutableHashSetValue<GenericParameterHelper>();
    internal protected override IImmutableSet<GenericParameterHelper> CreateInstance(params GenericParameterHelper[] initialItems) => new ImmutableHashSetValue<GenericParameterHelper>(initialItems.ToImmutableArray());
    protected override IReadOnlyCollection<T> CreateInstance<T>(params T[] initialItems) => new ImmutableHashSetValue<T>(initialItems.ToImmutableArray());
    protected override IImmutableSet<GenericParameterHelper> CreateInstance(HashSet<GenericParameterHelper> source) => new ImmutableHashSetValue<GenericParameterHelper>(source);

    protected override IImmutableSet<GenericParameterHelper> Except(IImmutableSet<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other) => collection.Except(other);
}
