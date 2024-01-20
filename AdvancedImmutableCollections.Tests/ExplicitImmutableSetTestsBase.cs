using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

public abstract class ExplicitImmutableSetTestsBase<TTestObject> : ImmutableSetTestsBase<TTestObject>
    where TTestObject : IImmutableSet<GenericParameterHelper>
{
    protected abstract override TTestObject? DefaultValue { get; }

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

    protected override IEnumerator<GenericParameterHelper> GetEnumerator(TTestObject collection) => collection.GetEnumerator();
}
