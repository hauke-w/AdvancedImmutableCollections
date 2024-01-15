using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedImmutableCollections.Tests.Util;

/// <summary>
/// Exception that is thrown when <see cref="TestCaseInfo.Execute(Action)">executing</see> <see cref="TestCaseInfo"/> fails.
/// </summary>
[Serializable]
public sealed class TestCaseFailedException : UnitTestAssertException
{
    internal TestCaseFailedException(TestCaseInfo testCase, Exception exception)
        : base(exception.Message, exception)
    {
        TestCase = testCase;
    }

    // lazy evaluated stack trace
    private string? _StackTrace;
    /// <inheritdoc/>
    public override string StackTrace
    {
        get
        {
            return _StackTrace
               ??= $"""
               {InnerException!.StackTrace}
                  at test case in {TestCase.SourceFile}:line {TestCase.Line}
               """;
        }
    }

    /// <summary>
    /// The test case that failed.
    /// </summary>
    public TestCaseInfo TestCase { get; }
}
