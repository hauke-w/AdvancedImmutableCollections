# AdvancedImmutableCollections
Advanced immutable collections on top of of System.Collections.Immutable. This includes particularly types and features that enable value semantics for collections.
These collections can be useful when creating source generators or record types.

## Example usage
```csharp
using AdvancedImmutableCollections;

ImmutableArrayValue<int> numbers1 = [1, 2, 3]; // use collection builder (.net 8 and later)
ImmutableArrayValue<int> numbers2 = ImmutableArrayValue.Create(1, 2, 3); // use overload with params
Console.WriteLine(number1.Equals(numbers2)); // => true
Console.WriteLine(number1.Equals(numbers1.Add(4))); // => false

// explicitly convert to value using extension method
var empty = ImmutableArray<int>.Empty.WithValueSemantics();

// implicit conversion to ImmutableArray
empty = ImmutableArray<int>.Empty;

// default values are considered equal to empty collections
Console.WriteLine(Iempty.Equals(default(ImmutableArrayValue<int>))); // => true

// record with ImmutableArrayValue property. Equals will compare the items of the array.
public record MyRecord(ImmutableArrayValue<int> Items);
```

ImmutableHashSetValue, ImmutableSortedSetValue and ImmutableDictionaryValue can be used in the same way.