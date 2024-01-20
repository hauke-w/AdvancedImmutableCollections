﻿#if NET462
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System;
internal static class ArrayExtensions
{
    public static void Fill<T>(this T[] array, T value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = value;
        }
    }
}
#endif