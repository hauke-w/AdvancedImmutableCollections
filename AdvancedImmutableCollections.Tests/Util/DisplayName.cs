
using System.Collections;

namespace AdvancedImmutableCollections.Tests.Util;

internal readonly struct DisplayName(string Value) : ITestCaseDisplayNameBuilder
{
    string ITestCaseDisplayNameBuilder.GetDisplayName() => Value;

    public override string ToString() => Value;
    void ITestCaseDisplayNameBuilder.AppendCollectionParameter<T>(IEnumerable<T> parameter) { }
    void ITestCaseDisplayNameBuilder.AppendCollectionParameter(IEnumerable parameter) { }
    void ITestCaseDisplayNameBuilder.AppendAllToString(object?[] parameters) { }
    void ITestCaseDisplayNameBuilder.AppendAll(string? displayNameFragment, object?[] parameters) { }
    void ITestCaseDisplayNameBuilder.Append<T>(T? parameter, string? displayNameFragment) where T: default { }

}
