using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdvancedImmutableCollections;

public abstract partial class ImmutableCollectionTestsBase<TTestObject, TMutable>
    where TTestObject : IReadOnlyCollection<GenericParameterHelper>
    where TMutable : ICollection<GenericParameterHelper>
{
    protected interface IEqualityTestStrategy
    {
        void EqualsTest(ImmutableCollectionTestsBase<TTestObject, TMutable> context);
        void GetHashCodeTest(ImmutableCollectionTestsBase<TTestObject, TMutable> context);
    }
}
