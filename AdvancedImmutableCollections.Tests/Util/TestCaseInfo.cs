using System.IO;

namespace AdvancedImmutableCollections.Tests.Util;

[Serializable]
public record TestCaseInfo(string DisplayName, int Line, string? InconclusiveMessage, string SourceFile)
{
    [MemberNotNullWhen(true, nameof(InconclusiveMessage))]
    public bool IsInconclusive => InconclusiveMessage is not null;

    public override string ToString() => $"{SourceFile}@{Line}: {DisplayName}";


    /// <summary>
    /// Constructs a string consisting of the <see cref="SourceFile"/>'s filename (except path), <see cref="Line"/> and <see cref="DisplayName"/>"/>
    /// </summary>
    /// <returns></returns>
    public string GetCompactString()
    {
        var filename = Path.GetFileName(SourceFile);
        return $"{filename}@{Line}: {DisplayName}";
    }

    /// <summary>
    /// Constructs a string consisting of the <see cref="Line"/> and <see cref="DisplayName"/>"/>
    /// </summary>
    /// <returns></returns>
    public string GetShortString() => $"@{Line}: {DisplayName}";

    /// <summary>
    /// Throws an <see cref="AssertInconclusiveException"/> by calling <see cref="Assert.Inconclusive(string)"/> if <see cref="IsInconclusive"/> is <see langword="true"/>.
    /// </summary>
    /// <exception cref="AssertInconclusiveException"></exception>
    public void ThrowOnInconclusive()
    {
        if (IsInconclusive)
        {
            Assert.Inconclusive(InconclusiveMessage);
        }
    }

    /// <summary>
    /// Execute <paramref name="testImpl"/> and throws a <see cref="TestCaseFailedException"/> if that action throws an exception.
    /// </summary>
    /// <remarks>
    /// This is basically a wrapper for the test implementation that will catch any exception 
    /// and create a stack trace that allows jumping to the test case source code.
    /// </remarks>
    /// <param name="testImpl">The method that implements the test</param>
    /// <exception cref="TestCaseFailedException"></exception>
    public void Execute(Action testImpl)
    {
        ThrowOnInconclusive();
        try
        {
            testImpl();
        }
        catch (TestCaseFailedException)
        {
            // do not wrap TestCaseFailedExceptions
            throw;
        }
        catch (Exception e)
        {
            throw new TestCaseFailedException(this, e);
        }
    }
}
