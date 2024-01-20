﻿using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableHashSetValue{T}"/> using <see cref="IImmutableList{T}"/> interface explicitly
/// </summary>
[TestClass]
public sealed class ImmutableHashSetValue1_ExplicitIImmutableList_Tests : ExplicitImmutableSetTestsBase<IImmutableSet<GenericParameterHelper>>
{
    protected override IImmutableSet<GenericParameterHelper> DefaultValue => default(ImmutableHashSetValue<GenericParameterHelper>);
    protected override IImmutableSet<GenericParameterHelper> GetTestObject() => new ImmutableHashSetValue<GenericParameterHelper>();
    protected override IImmutableSet<GenericParameterHelper> GetTestObject(params GenericParameterHelper[] initialItems) => new ImmutableHashSetValue<GenericParameterHelper>(initialItems.ToImmutableArray());
    protected override IImmutableSet<GenericParameterHelper> GetTestObject(HashSet<GenericParameterHelper> source) => new ImmutableHashSetValue<GenericParameterHelper>(source);
}
