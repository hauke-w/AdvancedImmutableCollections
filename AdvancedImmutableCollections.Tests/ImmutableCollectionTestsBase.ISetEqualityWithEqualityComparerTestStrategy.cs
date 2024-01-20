namespace AdvancedImmutableCollections;

public abstract partial class ImmutableCollectionTestsBase<TTestObject, TMutable>
    where TTestObject : IReadOnlyCollection<GenericParameterHelper>
    where TMutable : ICollection<GenericParameterHelper>
{
    protected interface ISetEqualityWithEqualityComparerTestStrategy : IEqualityTestStrategy
    {
        void GetHashCode_ReferenceEqualityOfItems_Test(IImmutableSetWithEqualityComparerTests<TTestObject> context);
    }
}
