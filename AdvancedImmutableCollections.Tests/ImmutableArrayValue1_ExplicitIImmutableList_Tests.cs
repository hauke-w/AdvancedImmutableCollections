using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableArrayValue{T}"/> using <see cref="IImmutableList{T}"/> interface explicitly
/// </summary>
[TestClass]
public sealed class ImmutableArrayValue1_ExplicitIImmutableList_Tests : ExplicitImmutableListTestsBase<IImmutableList<GenericParameterHelper>>
{
    protected override IImmutableList<GenericParameterHelper> GetTestObject() => new ImmutableArrayValue<GenericParameterHelper>();
    protected override IImmutableList<GenericParameterHelper> GetTestObject(params GenericParameterHelper[] initialItems) => new ImmutableArrayValue<GenericParameterHelper>(initialItems.ToImmutableArray());

    protected override IImmutableList<GenericParameterHelper> DefaultValue => default(ImmutableArrayValue<GenericParameterHelper>);
}
