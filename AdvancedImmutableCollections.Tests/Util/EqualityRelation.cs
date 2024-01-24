using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedImmutableCollections.Tests.Util;
public enum EqualityRelation
{
    NotEqual,
    SetEqual,
    InterchangeablyEqual,
}

internal static class EqualityRelationExtensions
{
    public static bool ToBoolForEquals(this EqualityRelation eq) => eq != EqualityRelation.NotEqual;

    public static bool ToBoolForOpEqual(this EqualityRelation eq) => eq == EqualityRelation.InterchangeablyEqual;

    public static bool ToBoolForOpNotEqual(this EqualityRelation eq) => eq != EqualityRelation.InterchangeablyEqual;
}