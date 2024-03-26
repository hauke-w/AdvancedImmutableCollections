using System.Diagnostics;

namespace AdvancedImmutableCollections;

partial class ImmutableCollectionTestsBase<TFactory>
{
    protected abstract class ValueEqualityTestStrategy : EqualityTestStrategyImpl
    {
        protected sealed override bool IsValueEquality => true;

        public override void EqualsTest(TFactory factory)
        {
            base.EqualsTest(factory);

            var item0 = new GenericParameterHelper(0);
            var item1 = new GenericParameterHelper(1);

            VerifyEquals(factory.Create([item0]), factory.Create([item1]), false);
            VerifyEquals(factory.Create([item0, item1]), factory.Create([item0, item1]), true);
            VerifyEquals(factory.Create([item0, item1]), factory.Create([item1]), false);
            VerifyEquals(factory.Create([item0]), factory.Create([item0, item1]), false);
        }

        /// <summary>
        /// Implements verification of <see cref="Equals(object)"/> for the default value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj0"></param>
        protected override void EqualsDefaultValueTest(TFactory factory)
        {
            var @default = factory.GetDefaultValue<GenericParameterHelper>();
            Debug.Assert(@default is not null);
            var empty = factory.Create<GenericParameterHelper>([]);
            var notEmpty = factory.Create([new GenericParameterHelper(0)]);

            // left is not default
            VerifyEquals(empty, @default, true); // both are empty
            VerifyEquals(notEmpty, @default, false);

            // left is default
            VerifyEquals(@default, @default, true); // same as default
            VerifyEquals(@default, empty, true);
            VerifyEquals(@default, notEmpty, false);
            VerifyEquals(@default, new object(), false);
            VerifyEquals(@default, null, false);
            VerifyEquals(@default, factory.GetDefaultValue<int>(), false); // different types
            VerifyEquals(@default, factory.Create([new GenericParameterHelper(0)]), false);
        }
    }
}
