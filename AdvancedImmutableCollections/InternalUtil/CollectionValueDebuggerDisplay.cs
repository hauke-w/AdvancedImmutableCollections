using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedImmutableCollections.InternalUtil;
internal static class CollectionValueDebuggerDisplay
{
    public static string GetDebuggerDisplay<T>(this ICollection<T>? value, string typeName)
    {
        switch (value)
        {
            case null:
            case { Count: 0 }:
                return $"{typeName}<{typeof(T).Name}> Count=0, Value=[]";
            case { Count: 1 }:
                var first = value.First() switch
                {
                    null => "null",
                    string s => $"\"{s}\"",
                    var x => x.ToString()
                };
                return $"""{typeName}<{typeof(T).Name}> Count=1, Value=[{first}]""";
            default:
                break;
        }

        var sb = new StringBuilder();
        sb.Append($"{typeName}<{typeof(T).Name}> Count={value.Count}, Value=[");
        var enumerator = value.GetEnumerator();
        enumerator.MoveNext();
        var current = enumerator.Current switch
        {
            null => "null",
            string s => $"\"{s}\"",
            var x => x.ToString()
        };
        sb.Append(current);

        int count = 1;
        while (sb.Length < 100 && enumerator.MoveNext())
        {
            count++;
            sb.Append(", ");

            current = enumerator.Current switch
            {
                null => "null",
                string s => $"\"{s}\"",
                var x => x.ToString()
            };
            sb.Append(current);
        }
        if (count < value.Count)
        {
            sb.Append(", ...");
        }

        sb.Append(']');
        return sb.ToString();
    }
}
