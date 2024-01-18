using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests;

public abstract class ImmutableSetTestsBase<TTestObject> : ImmutableCollectionTestsBase<TTestObject, HashSet<GenericParameterHelper>>
    where TTestObject : IImmutableSet<GenericParameterHelper>
{
    protected sealed override HashSet<GenericParameterHelper> GetMutableCollection(params GenericParameterHelper[] initialItems) => new(initialItems);

    protected sealed override void AssertCollectionsAreEqual(ICollection expected, ICollection actual)
        => CollectionAssert.AreEquivalent(expected, actual);
}
