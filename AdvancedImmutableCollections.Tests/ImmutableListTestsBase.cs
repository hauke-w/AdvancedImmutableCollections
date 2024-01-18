using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests;

public abstract class ImmutableListTestsBase<TTestObject> : ImmutableCollectionTestsBase<TTestObject, List<GenericParameterHelper>>
    where TTestObject : IImmutableList<GenericParameterHelper>
{
    protected sealed override List<GenericParameterHelper> GetMutableCollection(params GenericParameterHelper[] initialItems) => new(initialItems);

    protected sealed override void AssertCollectionsAreEqual(ICollection expected, ICollection actual) 
        => CollectionAssert.AreEqual(expected, actual);
}
