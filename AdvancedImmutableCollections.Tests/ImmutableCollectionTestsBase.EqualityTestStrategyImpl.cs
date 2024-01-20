using System.Diagnostics;

namespace AdvancedImmutableCollections;

public abstract partial class ImmutableCollectionTestsBase<TTestObject, TMutable>
    where TTestObject : IReadOnlyCollection<GenericParameterHelper>
    where TMutable : ICollection<GenericParameterHelper>
{
    protected class EqualityTestStrategyImpl : IEqualityTestStrategy, ISetEqualityWithEqualityComparerTestStrategy
    {
        protected EqualityTestStrategyImpl()
        {
            Debug.Assert(!DefaultValueIsEqualEmpty || IsValueEquality);
        }

        public static readonly EqualityTestStrategyImpl Default = new EqualityTestStrategyImpl();

        protected bool DefaultValueIsEqualEmpty { get; init; }
        protected virtual bool IsValueEquality => false;

        public virtual void EqualsTest(ImmutableCollectionTestsBase<TTestObject, TMutable> context)
        {
            var item0 = new GenericParameterHelper(0);

            var empty1 = context.CreateInstance([]);
            var empty2 = context.CreateInstance([]);
            var notEmpty1 = context.CreateInstance([item0]);
            var notEmpty2 = context.CreateInstance([item0]);

            VerifyEquals(empty1, empty1, true);
            VerifyEquals(empty1, empty2, IsValueEquality); // different instances
            VerifyEquals(notEmpty1, empty1, false);
            VerifyEquals(empty1, notEmpty1, false);
            VerifyEquals(notEmpty1, notEmpty2, IsValueEquality);

            if (context.DefaultValue is not null)
            {
                // it's a value type!
                EqualsDefaultValueTest(context);
            }

            // objects of different type are never equal
            VerifyEquals(empty1, context.CreateInstance<int>(), false);
            VerifyEquals(empty1, new object(), false);
            VerifyEquals(empty1, null, false);
        }

        /// <summary>
        /// Implements verification of <see cref="Equals(object)"/> for the default value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj0"></param>
        protected virtual void EqualsDefaultValueTest(ImmutableCollectionTestsBase<TTestObject, TMutable> context)
        {
            Debug.Assert(context.DefaultValue is not null);
            var obj = context.CreateInstance([]);

            // left is not default
            VerifyEquals(obj, context.DefaultValue, DefaultValueIsEqualEmpty);
            VerifyEquals(obj, context.GetDefaultValue<int>(), false);

            // left is default
            VerifyEquals(context.DefaultValue, context.DefaultValue, true); // same as default
            VerifyEquals(context.DefaultValue, obj, DefaultValueIsEqualEmpty);
            VerifyEquals(context.DefaultValue, new object(), false);
            VerifyEquals(context.DefaultValue, null, false);
            VerifyEquals(context.DefaultValue, context.GetDefaultValue<int>(), false); // different types
            VerifyEquals(context.DefaultValue, context.CreateInstance([new GenericParameterHelper(0)]), false);
        }

        protected void VerifyEquals(TTestObject testObject, object? other, bool expected)
        {
            var actual = testObject.Equals(other);
            Assert.AreEqual(expected, actual);
        }

        public virtual void GetHashCodeTest(ImmutableCollectionTestsBase<TTestObject, TMutable> context)
        {
            var hashcodes = new Dictionary<int, TTestObject>();
            var testObjects = GetValuesWithUniqueHashcode(context);
            // use for because it is easier to debug
            for (int i = 0; i < testObjects.Count; i++)
            {
                var testObject = testObjects[i];
                var actual = testObject.GetHashCode();
                Assert.IsFalse(hashcodes.TryGetValue(actual, out var objectWithSameHashCode), "hash code is not unique");
                hashcodes.Add(actual, testObject);
            }

            if (context.DefaultValue is not null)
            {
                var hashcodeOfEmpty = context.CreateInstance([]).GetHashCode();
                var hashcodeOfDefault = context.DefaultValue.GetHashCode();
                Assert.AreEqual(DefaultValueIsEqualEmpty, hashcodeOfEmpty == hashcodeOfDefault);
            }
        }

        protected virtual List<TTestObject> GetValuesWithUniqueHashcode(ImmutableCollectionTestsBase<TTestObject, TMutable> context)
        {
            var item0 = new GenericParameterHelper(0);
            var item1 = new GenericParameterHelper(1);
            var item2 = new GenericParameterHelper(2);

            var result = new List<TTestObject>()
            {
                context.CreateInstance([]),
                context.CreateInstance([item0]),
                context.CreateInstance([item1]),
                context.CreateInstance([item0, item1]),
                context.CreateInstance([item0, item1, item2])
            };
            if (!DefaultValueIsEqualEmpty && context.DefaultValue is not null)
            {
                result.Add(context.DefaultValue);
            }
            return result;
        }

        public virtual void GetHashCode_ReferenceEqualityOfItems_Test(IImmutableSetWithEqualityComparerTests<TTestObject> context)
        {
            var item0 = new GenericParameterHelper(0);
            var item0b = new GenericParameterHelper(0);
            var item1 = new GenericParameterHelper(1);

            var hashcodes = new HashSet<int>();
            TTestObject[] valuesWithUniqueHashcode =
                [
                    CreateInstance(),
                    CreateInstance(item0),
                    CreateInstance(item0b),
                    CreateInstance(item0, item1),
                    CreateInstance(item0, item0b),
                ];

            foreach (var testObject in valuesWithUniqueHashcode)
            {
                var actual = testObject.GetHashCode();
                Assert.IsTrue(hashcodes.Add(actual), "hash code is not unique");
            }

            TTestObject CreateInstance(params GenericParameterHelper[] items)
                => context.CreateInstance(items, ReferenceEqualityComparer.Instance);
        }
    }
}
