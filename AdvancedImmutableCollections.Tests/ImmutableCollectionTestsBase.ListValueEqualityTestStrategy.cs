using AdvancedImmutableCollections.Tests.CollectionAdapters;

namespace AdvancedImmutableCollections;

partial class ImmutableCollectionTestsBase<TFactory>
{
    protected class ListValueEqualityTestStrategy : ValueEqualityTestStrategy
    {
        public new static readonly ListValueEqualityTestStrategy Default = new ListValueEqualityTestStrategy() { DefaultValueIsEqualEmpty = true };

        protected override List<IImmutableCollectionAdapter<GenericParameterHelper>> GetValuesWithUniqueHashcode(TFactory factory)
        {
            var item0 = new GenericParameterHelper(0);
            var item1 = new GenericParameterHelper(1);
            var item2 = new GenericParameterHelper(2);
            var item3 = new GenericParameterHelper(3);

            var result = base.GetValuesWithUniqueHashcode(factory);
            result.AddRange(
                [
                    factory.Create([item0, item1, item2, item3]),
                    // add with different order
                    factory.Create([item1, item0]),
                    factory.Create([item1, item0, item2]),
                    factory.Create([item2, item1, item0]),
                    factory.Create([item2, item1, item3, item0]),
                ]);
            return result;
        }
    }
}
