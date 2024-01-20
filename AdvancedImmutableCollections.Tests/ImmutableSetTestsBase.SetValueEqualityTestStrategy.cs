using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

public abstract partial class ImmutableSetTestsBase<TTestObject, TMutable>
{
    protected class SetValueEqualityTestStrategy : ValueEqualityTestStrategy, ISetEqualityWithEqualityComparerTestStrategy
    {
        public new static readonly SetValueEqualityTestStrategy Default = new SetValueEqualityTestStrategy() { DefaultValueIsEqualEmpty = true };

        protected override List<TTestObject> GetValuesWithUniqueHashcode(ImmutableCollectionTestsBase<TTestObject, TMutable> context)
        {
            var item0 = new GenericParameterHelper(0);
            var item1 = new GenericParameterHelper(1);
            var item2 = new GenericParameterHelper(2);
            var item3 = new GenericParameterHelper(3);
            var item4 = new GenericParameterHelper(4);

            var result = base.GetValuesWithUniqueHashcode(context);
            result.AddRange(
                [
                    context.CreateInstance([item0, item1, item2, item3]),
                    context.CreateInstance([item0, item1, item2, item3, item4])
                ]);
            return result;
        }

        public override void GetHashCode_ReferenceEqualityOfItems_Test(IImmutableSetWithEqualityComparerTests<TTestObject> context)
        {
            var item0 = new GenericParameterHelper(0);
            var item0b = new GenericParameterHelper(0);
            var item1 = new GenericParameterHelper(1);
            var item1b = new GenericParameterHelper(1);
            var item2 = new GenericParameterHelper(2);
            var item2b = new GenericParameterHelper(2);

            var items0 = CreateInstance(item0);
            var items01 = CreateInstance(item0, item1);

            AssertEqualHashCode(items0, CreateInstance(item0b));
            AssertEqualHashCode(items01, CreateInstance(item0b, item1));
            AssertEqualHashCode(items01, CreateInstance(item0, item1b));
            AssertEqualHashCode(items01, CreateInstance(item0b, item1b));

            var hashcodes = new HashSet<int>();
            TTestObject[] valuesWithUniqueHashcode =
                [
                    CreateInstance(),
                    items0,
                    items01,
                    CreateInstance(item0, item1, item2),
                    CreateInstance(item0, item0b),
                    CreateInstance(item0, item0b, item1),
                    CreateInstance(item0, item0b, item1, item1b),
                    CreateInstance(item0, item0b, item1, item1b, item2),
                    CreateInstance(item0, item0b, item1, item1b, item2, item2b),
                ];

            foreach (var testObject in valuesWithUniqueHashcode)
            {
                var actual = testObject.GetHashCode();
                Assert.IsTrue(hashcodes.Add(actual), "hash code is not unique");
            }

            TTestObject CreateInstance(params GenericParameterHelper[] items)
                => context.CreateInstance(items, ReferenceEqualityComparer.Instance);

            void AssertEqualHashCode(TTestObject items1, TTestObject items2)
            {
                var hashcode1 = items1.GetHashCode();
                var hashcode2 = items2.GetHashCode();
                Assert.AreEqual(hashcode1, hashcode2);
            }
        }
    }
}
