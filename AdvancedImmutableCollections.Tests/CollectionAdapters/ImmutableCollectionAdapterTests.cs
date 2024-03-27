using Moq;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

[TestClass]
public class ImmutableCollectionAdapterTests
{
    [TestMethod]
    public void CollectionTest()
    {
        var collection = new Mock<IReadOnlyCollection<GenericParameterHelper>>().Object;
        var mock = new Mock<ImmutableCollectionAdapter<GenericParameterHelper, IReadOnlyCollection<GenericParameterHelper>>>(collection)
        {
            CallBase = true
        };
        var actual = mock.Object.Collection;
        Assert.AreSame(collection, actual);

        var actual_ICollectionAdapter = ((ICollectionAdapter) mock.Object).Collection;
        Assert.AreSame(collection, actual_ICollectionAdapter);

        var actual_ICollectionAdapter_1 = ((ICollectionAdapter<GenericParameterHelper>)mock.Object).Collection;
        Assert.AreSame(collection, actual_ICollectionAdapter_1);
    }

    [TestMethod]
    public void EqualsTest()
    {
        EqualsTest(CreateAdapter(out var collection), true, collection);
        EqualsTest(CreateAdapter(out collection), false, collection);
        EqualsTest(new GenericParameterHelper[] { }, true);
        EqualsTest("object of different type", false);

        static void EqualsTest(object? obj, bool expected, object? equalCollection=null)
        {
            equalCollection ??= obj;
            var collectionMock = new Mock<IReadOnlyCollection<GenericParameterHelper>>();
            collectionMock.Setup(it => it.Equals(It.Is<object?>(equalCollection, ReferenceEqualityComparer.Instance))).Returns(expected).Verifiable();
            var mock = new Mock<ImmutableCollectionAdapter<GenericParameterHelper, IReadOnlyCollection<GenericParameterHelper>>>(collectionMock.Object)
            {
                CallBase = true
            };
            var actual = mock.Object.Equals(obj);
            Assert.AreEqual(expected, actual);
            collectionMock.Verify();
        }

        static ICollectionAdapter<GenericParameterHelper> CreateAdapter(out IReadOnlyCollection<GenericParameterHelper> collection)
        {
            collection = new Mock<IReadOnlyCollection<GenericParameterHelper>>(MockBehavior.Strict).Object;
            var adapterMock = new Mock<ICollectionAdapter<GenericParameterHelper>>(MockBehavior.Strict);
            adapterMock.As<ICollectionAdapter>().SetupGet(it => it.Collection).Returns(collection);
            return adapterMock.Object;
        }
    }

    [TestMethod]
    public void GetHashCodeTest()
    {
        var collectionMock = new Mock<IReadOnlyCollection<GenericParameterHelper>>();
        int expected = new Random().Next();
        collectionMock.Setup(it => it.GetHashCode()).Returns(expected).Verifiable();
        var mock = new Mock<ImmutableCollectionAdapter<GenericParameterHelper, IReadOnlyCollection<GenericParameterHelper>>>(collectionMock.Object)
        {
            CallBase = true
        };
        var actual = mock.Object.GetHashCode();
        Assert.AreEqual(expected, actual);
        collectionMock.Verify();
    }
}