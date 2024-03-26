using System.Diagnostics;
using AdvancedImmutableCollections.Tests.CollectionAdapters;

namespace AdvancedImmutableCollections;

partial class ImmutableCollectionTestsBase<TFactory>
{
    protected class EqualityTestStrategyImpl : IEqualityTestStrategy
    {
        protected EqualityTestStrategyImpl()
        {
            Debug.Assert(!DefaultValueIsEqualEmpty || IsValueEquality);
        }

        public static readonly EqualityTestStrategyImpl Default = new EqualityTestStrategyImpl();

        protected bool DefaultValueIsEqualEmpty { get; init; }
        protected virtual bool IsValueEquality => false;

        public virtual void EqualsTest(TFactory factory)
        {
            var item0 = new GenericParameterHelper(0);

            var empty1 = factory.Create<GenericParameterHelper>([]);
            var empty2 = factory.Create<GenericParameterHelper>([]);
            var notEmpty1 = factory.Create<GenericParameterHelper>([item0]);
            var notEmpty2 = factory.Create<GenericParameterHelper>([item0]);

            VerifyEquals(empty1, empty1, true);
            VerifyEquals(empty1, empty2, IsValueEquality); // different instances
            VerifyEquals(notEmpty1, empty1, false);
            VerifyEquals(empty1, notEmpty1, false);
            VerifyEquals(notEmpty1, notEmpty2, IsValueEquality);

            if (factory.GetDefaultValue<GenericParameterHelper>() is not null)
            {
                // it's a value type!
                EqualsDefaultValueTest(factory);
            }

            // objects of different type are never equal
            VerifyEquals(empty1, factory.Create<int>(), false);
            VerifyEquals(empty1, new object(), false);
            VerifyEquals(empty1, null, false);
        }

        /// <summary>
        /// Implements verification of <see cref="Equals(object)"/> for the default value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj0"></param>
        protected virtual void EqualsDefaultValueTest(TFactory factory)
        {
            var @default = factory.GetDefaultValue<GenericParameterHelper>();
            Debug.Assert(@default is not null);
            var obj = factory.Create<GenericParameterHelper>([]);

            // left is not default
            VerifyEquals(obj, @default, DefaultValueIsEqualEmpty);
            VerifyEquals(obj, factory.GetDefaultValue<int>(), false);

            // left is default
            VerifyEquals(@default!, @default, true); // same as default
            VerifyEquals(@default, obj, DefaultValueIsEqualEmpty);
            VerifyEquals(@default, new object(), false);
            VerifyEquals(@default, null, false);
            VerifyEquals(@default, factory.GetDefaultValue<int>(), false); // different types
            VerifyEquals(@default, factory.Create([new GenericParameterHelper(0)]), false);
        }

        protected void VerifyEquals<T>([NotNull]IImmutableCollectionAdapter<T> testObjectAdapter, object? other, bool expected)
        {
            if (other is ICollectionAdapter otherAdapter)
            {
                other = otherAdapter.Collection;
            }
            var actual = testObjectAdapter.Collection.Equals(other);
            Assert.AreEqual(expected, actual);
        }

        public virtual void GetHashCodeTest(TFactory factory)
        {
            var hashcodes = new Dictionary<int, IImmutableCollectionAdapter<GenericParameterHelper>>();
            var testObjectAdapters = GetValuesWithUniqueHashcode(factory);
            // use for because it is easier to debug
            for (int i = 0; i < testObjectAdapters.Count; i++)
            {
                var testObjectAdapter = testObjectAdapters[i];
                var actual = testObjectAdapter.Collection.GetHashCode();
                Assert.IsFalse(hashcodes.TryGetValue(actual, out var objectWithSameHashCode), "hash code is not unique");
                hashcodes.Add(actual, testObjectAdapter);
            }

            if (factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
            {
                var hashcodeOfEmpty = factory.Create<GenericParameterHelper>([]).GetHashCode();
                var hashcodeOfDefault = @default.GetHashCode();
                Assert.AreEqual(DefaultValueIsEqualEmpty, hashcodeOfEmpty == hashcodeOfDefault);
            }
        }

        protected virtual List<IImmutableCollectionAdapter<GenericParameterHelper>> GetValuesWithUniqueHashcode(TFactory factory)
        {
            var item0 = new GenericParameterHelper(0);
            var item1 = new GenericParameterHelper(1);
            var item2 = new GenericParameterHelper(2);

            var result = new List<IImmutableCollectionAdapter<GenericParameterHelper>>()
            {
                factory.Create<GenericParameterHelper>([]),
                factory.Create<GenericParameterHelper>([item0]),
                factory.Create<GenericParameterHelper>([item1]),
                factory.Create<GenericParameterHelper>([item0, item1]),
                factory.Create<GenericParameterHelper>([item0, item1, item2])
            };
            if (!DefaultValueIsEqualEmpty && factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
            {
                result.Add(@default);
            }
            return result;
        }
    }
}
