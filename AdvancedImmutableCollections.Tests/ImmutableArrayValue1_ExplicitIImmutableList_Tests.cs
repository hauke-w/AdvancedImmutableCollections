using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableArrayValue{T}"/> using <see cref="IImmutableList{T}"/> interface explicitly
/// </summary>
[TestClass]
public sealed class ImmutableArrayValue1_ExplicitIImmutableList_Tests : ExplicitImmutableListTestsBase<IImmutableList<GenericParameterHelper>>
{
    protected override IEqualityTestStrategy EqualityTestStrategy => ListValueEqualityTestStrategy.Default;
    protected override IImmutableList<GenericParameterHelper> DefaultValue => default(ImmutableArrayValue<GenericParameterHelper>);
    protected override IReadOnlyCollection<T>? GetDefaultValue<T>() => default(ImmutableArrayValue<T>);
    internal protected override IImmutableList<GenericParameterHelper> CreateInstance() => new ImmutableArrayValue<GenericParameterHelper>();
    internal protected override IImmutableList<GenericParameterHelper> CreateInstance(params GenericParameterHelper[] initialItems) => new ImmutableArrayValue<GenericParameterHelper>(initialItems.ToImmutableArray());
    protected override IReadOnlyCollection<T> CreateInstance<T>(params T[] initialItems) => new ImmutableArrayValue<T>(initialItems.ToImmutableArray());
}
