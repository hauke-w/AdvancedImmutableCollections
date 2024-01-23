#if !NET6_0_OR_GREATER
using System.Collections.Generic;

namespace System;

internal static class KeyValuePairExtensions
{
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) => new(key, value);

#if !NETCOREAPP
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value)
    {
        key = pair.Key;
        value = pair.Value;
    } 
#endif
}
#endif