using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests;

public abstract class ExplicitImmutableSetTestsBase<TTestObject> : ImmutableSetTestsBase<TTestObject>
    where TTestObject : IImmutableSet<GenericParameterHelper>
{
    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(TTestObject collection, GenericParameterHelper item) => collection.Add(item);
    protected override IReadOnlyCollection<GenericParameterHelper> Remove(TTestObject collection, GenericParameterHelper item) => collection.Remove(item);
    protected override IReadOnlyCollection<GenericParameterHelper> Clear(TTestObject collection) => collection.Clear();

    protected override bool Contains(TTestObject collection, GenericParameterHelper item) => collection.Contains(item);

    protected override IReadOnlyCollection<GenericParameterHelper> AddRange(TTestObject collection, params GenericParameterHelper[] newItems)
    {
        IImmutableSet<GenericParameterHelper> result = collection;
        foreach (var item in newItems)
        {
            result = result.Add(item);
        }
        return result;
    }
}
