using System.Reflection;
using System.Runtime.CompilerServices;

namespace AdvancedImmutableCollections.Tests.Util;

public static class DynamicData
{
    public static DynamicDataTestCaseBuilder TestCase(ITestCaseDisplayNameBuilder? displayNameBuilder = null, string? inconclusiveMessage = null, [CallerFilePath] string sourceFile = "?", [CallerLineNumber] int line = 0)
        => new DynamicDataTestCaseBuilder(displayNameBuilder, inconclusiveMessage, sourceFile, line);

    public static DynamicDataTestCaseBuilder TestCase(string displayName, string? inconclusiveMessage = null, [CallerFilePath] string sourceFile = "?", [CallerLineNumber] int line = 0)
    {
        if (displayName is null)
        {
            throw new ArgumentNullException(nameof(displayName));
        }

        return TestCase(new DisplayName(displayName), inconclusiveMessage, sourceFile, line);
    }

    /// <summary>
    /// Gets the display name of a test case using the last <see cref="TestCaseInfo"/> found in <paramref name="data"/> 
    /// and formatting it using <see cref="TestCaseInfo.GetShortString()"/>.
    /// 
    /// Use this method for <see cref="DynamicDataAttribute.DynamicDataDisplayName"/>.
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string GetDynamicDataDisplayName(MethodInfo methodInfo, object?[] data)
    {
        for (int i = data.Length - 1; i >= 0; i--)
        {
            if (data[i] is TestCaseInfo info)
            {
                return info.GetShortString();
            }
        }
        return $"{methodInfo.Name} ({string.Join(", ", data)})";
    }

    /// <summary>
    /// Gets the display name of a test case using the last <see cref="TestCaseInfo"/> found in <paramref name="data"/> 
    /// and formatting it using <see cref="TestCaseInfo.ToString()"/>.
    /// 
    /// Use this method for <see cref="DynamicDataAttribute.DynamicDataDisplayName"/>.
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string GetFullDynamicDataDisplayName(MethodInfo methodInfo, object?[] data)
    {
        for (int i = data.Length - 1; i >= 0; i--)
        {
            if (data[i] is TestCaseInfo info)
            {
                return info.ToString();
            }
        }
        return $"{methodInfo.Name} ({string.Join(", ", data)})";
    }
}
