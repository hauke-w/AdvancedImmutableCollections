using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedImmutableCollections.Tests.Util;
internal class GenericParameterHelperComparer : IComparer<GenericParameterHelper>
{
    public static readonly GenericParameterHelperComparer Instance = new GenericParameterHelperComparer();
    public int Compare(GenericParameterHelper? x, GenericParameterHelper? y)
    {
        if (x is null)
        {
            return y is null ? 0 : -1;
        }
        else if (y is null)
        {
            return 1;
        }
        return x.Data.CompareTo(y.Data);
    }
}
