using AdvancedImmutableCollections.Tests.CollectionAdapters;
using AdvancedImmutableCollections.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

partial class ImmutableSetTestsBase<TFactory>
{
    protected class SetValueEqualityTestStrategy : ValueEqualityTestStrategy
    {
        public new static readonly SetValueEqualityTestStrategy Default = new SetValueEqualityTestStrategy() { DefaultValueIsEqualEmpty = true };

        protected override List<IImmutableCollectionAdapter<GenericParameterHelper>> GetValuesWithUniqueHashcode(TFactory factory)
        {
            var item0 = new GenericParameterHelper(0);
            var item1 = new GenericParameterHelper(1);
            var item2 = new GenericParameterHelper(2);
            var item3 = new GenericParameterHelper(3);
            var item4 = new GenericParameterHelper(4);

            var result = base.GetValuesWithUniqueHashcode(factory);
            result.AddRange(
                [
                    factory.Create([item0, item1, item2, item3]),
                    factory.Create([item0, item1, item2, item3, item4])
                ]);
            return result;
        }
    }
}
