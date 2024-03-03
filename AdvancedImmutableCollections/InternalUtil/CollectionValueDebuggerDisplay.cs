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
                return $"{typeName}<{typeof(T).Name}> Count=0 Value=[]";
            case { Count: 1 }:
                return $"{typeName}<{typeof(T).Name}> Count=1 Value=[{value.First()}]";
            default:
                break;
        }

        var sb = new StringBuilder();
        sb.Append($"{typeName}<{typeof(T).Name}> Count={value.Count} Value=[");
        var enumerator = value.GetEnumerator();
        enumerator.MoveNext();
        sb.Append(enumerator.Current);

        int count = 1;
        while (sb.Length < 100 && enumerator.MoveNext())
        {
            count++;
            sb.Append(", ");
            sb.Append(enumerator.Current);
        }
        if (count < value.Count)
        {
            sb.Append(", ...");
        }
        return sb.ToString();
    }
}
