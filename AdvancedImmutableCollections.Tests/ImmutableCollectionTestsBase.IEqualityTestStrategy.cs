using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdvancedImmutableCollections;

partial class ImmutableCollectionTestsBase<TFactory>
{
    protected interface IEqualityTestStrategy
    {
        void EqualsTest(TFactory factory);
        void GetHashCodeTest(TFactory factory);
    }
}
