using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests;

public abstract class ExplicitImmutableListTestsBase<TTestObject> : ImmutableListTestsBase<TTestObject>
    where TTestObject : IImmutableList<GenericParameterHelper>
{
    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(TTestObject collection, GenericParameterHelper item) => collection.Add(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Remove(TTestObject collection, GenericParameterHelper item) => collection.Remove(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Clear(TTestObject collection) => collection.Clear();

    protected override IReadOnlyCollection<GenericParameterHelper> AddRange(TTestObject collection, params GenericParameterHelper[] newItems) => collection.AddRange(newItems);

    protected override bool Contains(TTestObject collection, GenericParameterHelper item) => collection.Contains(item);
}
