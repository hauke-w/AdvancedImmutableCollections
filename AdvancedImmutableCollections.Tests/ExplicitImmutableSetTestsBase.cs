using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests;

public abstract class ExplicitImmutableSetTestsBase<TTestObject> : ImmutableSetTestsBase<TTestObject>
    where TTestObject : IImmutableSet<GenericParameterHelper>
{
    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(TTestObject collection, GenericParameterHelper item) => ((IImmutableSet<GenericParameterHelper>)collection).Add(item);
    protected override IReadOnlyCollection<GenericParameterHelper> Remove(TTestObject collection, GenericParameterHelper item) => ((IImmutableSet<GenericParameterHelper>)collection).Remove(item);
    protected override IReadOnlyCollection<GenericParameterHelper> Clear(TTestObject collection) => collection.Clear();
}
