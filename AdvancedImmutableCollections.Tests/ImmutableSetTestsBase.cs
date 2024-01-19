using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

public abstract class ImmutableSetTestsBase<TTestObject> : ImmutableCollectionTestsBase<TTestObject, HashSet<GenericParameterHelper>>
    where TTestObject : IImmutableSet<GenericParameterHelper>
{
    protected sealed override HashSet<GenericParameterHelper> GetMutableCollection(params GenericParameterHelper[] initialItems) => new(initialItems);
}
