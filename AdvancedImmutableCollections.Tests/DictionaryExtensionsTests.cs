#if NETFRAMEWORK || NETSTANDARD2_0
namespace System.Collections.Generic;

[TestClass]
public class DictionaryExtensionsTests
{
    [TestMethod]
    public void TryAddTest()
    {
        var key1 = new GenericParameterHelper(1);
        var key1b = new GenericParameterHelper(1);
        var key2 = new GenericParameterHelper(2);
        var key3 = new GenericParameterHelper(3);

        var dictionary = new Dictionary<GenericParameterHelper, string>();
        List<KeyValuePair<GenericParameterHelper, string>> expectedItems = [];
        TryAdd(key1, "a", true);
        TryAdd(key1, "a", false);
        TryAdd(key1, "a", false);
        TryAdd(key2, "a", true);
        TryAdd(key3, "c", true);

        dictionary = new Dictionary<GenericParameterHelper, string>(ReferenceEqualityComparer.Instance);
        expectedItems = [];
        TryAdd(key1, "a", true);
        TryAdd(key1, "a", false);
        TryAdd(key1b, "A", true);
        TryAdd(key1b, "a", false);


        void TryAdd(GenericParameterHelper key, string value, bool expected)
        {
            if (expected)
            {
                expectedItems.Add(new(key, value));
            }

            var actual = DictionaryExtensions.TryAdd(dictionary, key, value);
            Assert.AreEqual(expected, actual);
            CollectionAssert.AreEquivalent(expectedItems, dictionary);
        }
    }
}

#endif