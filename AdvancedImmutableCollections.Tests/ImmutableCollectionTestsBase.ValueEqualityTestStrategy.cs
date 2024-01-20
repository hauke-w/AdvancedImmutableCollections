using System.Diagnostics;

namespace AdvancedImmutableCollections;

public abstract partial class ImmutableCollectionTestsBase<TTestObject, TMutable>
    where TTestObject : IReadOnlyCollection<GenericParameterHelper>
    where TMutable : ICollection<GenericParameterHelper>
{
    protected abstract class ValueEqualityTestStrategy : EqualityTestStrategyImpl
    {
        protected sealed override bool IsValueEquality => true;

        public override void EqualsTest(ImmutableCollectionTestsBase<TTestObject, TMutable> context)
        {
            base.EqualsTest(context);

            var item0 = new GenericParameterHelper(0);
            var item1 = new GenericParameterHelper(1);

            VerifyEquals(context.CreateInstance([item0]), context.CreateInstance([item1]), false);
            VerifyEquals(context.CreateInstance([item0, item1]), context.CreateInstance([item0, item1]), true);
            VerifyEquals(context.CreateInstance([item0, item1]), context.CreateInstance([item1]), false);
            VerifyEquals(context.CreateInstance([item0]), context.CreateInstance([item0, item1]), false);
        }

        /// <summary>
        /// Implements verification of <see cref="Equals(object)"/> for the default value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj0"></param>
        protected override void EqualsDefaultValueTest(ImmutableCollectionTestsBase<TTestObject, TMutable> context)
        {
            Debug.Assert(context.DefaultValue is not null);
            var empty = context.CreateInstance([]);
            var notEmpty = context.CreateInstance([new GenericParameterHelper(0)]);

            // left is not default
            VerifyEquals(empty, context.DefaultValue, true); // both are empty
            VerifyEquals(notEmpty, context.DefaultValue, false);

            // left is default
            VerifyEquals(context.DefaultValue, context.DefaultValue, true); // same as default
            VerifyEquals(context.DefaultValue, empty, true);
            VerifyEquals(context.DefaultValue, notEmpty, false);
            VerifyEquals(context.DefaultValue, new object(), false);
            VerifyEquals(context.DefaultValue, null, false);
            VerifyEquals(context.DefaultValue, context.GetDefaultValue<int>(), false); // different types
            VerifyEquals(context.DefaultValue, context.CreateInstance([new GenericParameterHelper(0)]), false);
        }
    }
}
