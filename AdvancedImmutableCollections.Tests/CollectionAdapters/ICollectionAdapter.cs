using System.Collections;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public interface ICollectionAdapter
{
    IEnumerable Collection { get; }
}
