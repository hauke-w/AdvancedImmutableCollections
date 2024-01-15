using System.Collections;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdvancedImmutableCollections.Tests.Util;

public interface ITestCaseDisplayNameBuilder
{
#if NETCOREAPP
    internal protected
#endif
    string GetDisplayName();

#if NETCOREAPP
    internal protected
#endif
    void AppendCollectionParameter<T>(IEnumerable<T> parameter);

#if NETCOREAPP
    internal protected
#endif
    void AppendCollectionParameter(IEnumerable parameter);

#if NETCOREAPP
    internal protected
# endif
    void AppendAllToString(object?[] parameters);

#if NETCOREAPP
    internal protected
# endif
    void AppendAll(string? displayNameFragment, object?[] parameters);

#if NETCOREAPP
    internal protected
#endif
    void Append<T>(T? parameter, string? displayNameFragment);
}

public class TestCaseDisplayNameBuilder : ITestCaseDisplayNameBuilder
{
    private readonly StringBuilder DisplayName = new StringBuilder();

    public string GetDisplayName() => DisplayName.ToString();

    public override string ToString() => GetDisplayName();

    void ITestCaseDisplayNameBuilder.AppendCollectionParameter<T>(IEnumerable<T> parameter)
    {
        AppendSeparatorIfNotEmpty();
        DisplayName.Append('[');
        AppendValues(parameter);
        DisplayName.Append(']');
    }

    void ITestCaseDisplayNameBuilder.AppendCollectionParameter(IEnumerable parameter)
    {
        AppendSeparatorIfNotEmpty();
        DisplayName.Append('[');
        AppendValues(parameter);
        DisplayName.Append(']');
    }

    void ITestCaseDisplayNameBuilder.AppendAllToString(object?[] parameters)
    {
        AppendSeparatorIfNotEmpty();
        AppendValues(parameters);
    }

    void ITestCaseDisplayNameBuilder.AppendAll(string? displayNameFragment, object?[] parameters)
    {
        AppendSeparatorIfNotEmpty();
        if (displayNameFragment is null)
        {
            AppendValues(parameters);
        }
        else
        {
            DisplayName.Append(displayNameFragment);
        }
    }

    void ITestCaseDisplayNameBuilder.Append<T>(T? parameter, string? displayNameFragment) where T : default
    {
        AppendSeparatorIfNotEmpty();
        if (displayNameFragment is null)
        {
            AppendValue(parameter);
        }
        else
        {
            DisplayName.Append(displayNameFragment);
        }
    }

    private void AppendSeparatorIfNotEmpty()
    {
        if (DisplayName.Length > 0)
        {
            DisplayName.Append(", ");
        }
    }

    private void AppendValues<T>(IEnumerable<T> parameters)
    {
        if (parameters is IReadOnlyList<T> list)
        {
            AppendValues(list);
        }
        else
        {
            var enumerator = parameters.GetEnumerator();
            if (enumerator.MoveNext())
            {
                AppendValue(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    DisplayName.Append(", ");
                    AppendValue(enumerator.Current);
                }
            }
        }
    }

    private void AppendValues(IEnumerable parameters)
    {
        var enumerator = parameters.GetEnumerator();
        if (enumerator.MoveNext())
        {
            AppendValue(enumerator.Current);
            while (enumerator.MoveNext())
            {
                DisplayName.Append(", ");
                AppendValue(enumerator.Current);
            }
        }
    }

    private void AppendValues<T>(IReadOnlyList<T> parameters)
    {
        if (parameters.Count > 0)
        {
            AppendValue(parameters[0]);

            for (int i = 0; i < parameters.Count; i++)
            {
                DisplayName.Append(", ");
                AppendValue(parameters[i]);
            }
        }
    }

    private void AppendValue(object? parameter)
    {
        string? value = parameter switch
        {
            null => "null",
            string s => $"\"{s}\"",
            _ => parameter.ToString()
        };
        DisplayName.Append(value);
    }
}

public sealed class DynamicDataTestCaseBuilder
{
    private readonly ITestCaseDisplayNameBuilder DisplayNameBuilder;
    private readonly string? InconclusiveMessage;
    private readonly string SourceFile;
    private readonly int Line;

    private readonly List<object?> Data = new List<object?>();

    internal DynamicDataTestCaseBuilder(ITestCaseDisplayNameBuilder? displayNameBuilder, string? inconclusiveMessage, string sourceFile, int line)
    {
        DisplayNameBuilder = displayNameBuilder ?? new TestCaseDisplayNameBuilder();
        InconclusiveMessage = inconclusiveMessage;
        SourceFile = sourceFile;
        Line = line;
    }

    public static implicit operator object?[](DynamicDataTestCaseBuilder builder) => builder.Build();

    public object?[] Build()
    {
        string displayName = DisplayNameBuilder.GetDisplayName();
        var testCaseInfo = new TestCaseInfo(displayName, Line, InconclusiveMessage, SourceFile);
        object?[] result = new object?[Data.Count + 1];
        Data.CopyTo(result, 0);
        result[^1] = testCaseInfo;
        return result;
    }

    /// <summary>
    /// Adds an atomic <paramref name="parameter"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="displayNameFragment"></param>
    /// <returns>The current instance (<see langword="this"/>)</returns>
    public DynamicDataTestCaseBuilder With<T>(T parameter, [CallerArgumentExpression(nameof(parameter))] string? displayNameFragment = null)
    {
        Data.Add(parameter);
        DisplayNameBuilder.Append(parameter, displayNameFragment);
        return this;
    }

    public DynamicDataTestCaseBuilder WithArray<T>(params T[] parameter)
    {
        Data.Add(parameter);
        DisplayNameBuilder.AppendCollectionParameter(parameter);
        return this;
    }
    //public DynamicDataTestCaseBuilder WithArray<T>(ImmutableArray<T> parameter) => WithCollection(parameter.ToTestDataContainer());

    /// <summary>
    /// Adds a list parameter. The display name fragment is constructed from all elements in the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    public DynamicDataTestCaseBuilder WithCollection<T>(TestDataContainer<T> parameter)
        where T : IEnumerable
    {
        Data.Add(parameter);
        DisplayNameBuilder.AppendCollectionParameter(parameter.Value);
        return this;
    }


    /// <summary>
    /// Adds the specified test <paramref name="parameters"/> and <paramref name="displayNameFragment"/>
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns>The current instance (<see langword="this"/>)</returns>
    public DynamicDataTestCaseBuilder WithAll(string? displayNameFragment, params object?[] parameters)
    {
        Data.AddRange(parameters);
        DisplayNameBuilder.AppendAll(displayNameFragment, parameters);
        return this;
    }

    /// <summary>
    /// Adds the specified test <paramref name="parameters"/> and uses <see cref="object.ToString"/> for formatting their displayname fragments.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns>The current instance (<see langword="this"/>)</returns>
    public DynamicDataTestCaseBuilder WithAll(params object?[] parameters)
    {
        Data.AddRange(parameters);
        DisplayNameBuilder.AppendAllToString(parameters);
        return this;
    }
}