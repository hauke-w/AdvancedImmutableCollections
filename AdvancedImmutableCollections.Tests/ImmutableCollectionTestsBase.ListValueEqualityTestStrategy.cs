namespace AdvancedImmutableCollections;

public abstract partial class ImmutableCollectionTestsBase<TTestObject, TMutable>
    where TTestObject : IReadOnlyCollection<GenericParameterHelper>
    where TMutable : ICollection<GenericParameterHelper>
{
    protected class ListValueEqualityTestStrategy : ValueEqualityTestStrategy
    {
        public new static readonly ListValueEqualityTestStrategy Default = new ListValueEqualityTestStrategy() { DefaultValueIsEqualEmpty = true };

        protected override List<TTestObject> GetValuesWithUniqueHashcode(ImmutableCollectionTestsBase<TTestObject, TMutable> context)
        {
            var item0 = new GenericParameterHelper(0);
            var item1 = new GenericParameterHelper(1);
            var item2 = new GenericParameterHelper(2);
            var item3 = new GenericParameterHelper(3);

            var result = base.GetValuesWithUniqueHashcode(context);
            result.AddRange(
                [
                    context.CreateInstance([item0, item1, item2, item3]),
                    // add with different order
                    context.CreateInstance([item1, item0]),
                    context.CreateInstance([item1, item0, item2]),
                    context.CreateInstance([item2, item1, item0]),
                    context.CreateInstance([item2, item1, item3, item0]),
                ]);
            return result;
        }
    }
}
