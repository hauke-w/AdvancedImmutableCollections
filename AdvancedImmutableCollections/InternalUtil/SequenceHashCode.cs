namespace AdvancedImmutableCollections.InternalUtil;

internal static class SequenceHashCode
{
    // https://de.wikibooks.org/wiki/Primzahlen:_Tabelle_der_Primzahlen_(2_-_100.000)
    // do not include 2 because it likely produces equal hash codes (e.g. 2 * -1.GetHashCode() = -1 and 2 * int.MaxValue.GetHashCode() = -2!!!)
    /// <summary>
    /// 112 prime number starting with 3
    /// </summary>
    private static readonly int[] PrimeNumbers =
    [
      /* 2 */  3,     5,     7,    11,    13,    17,    19,    23,    29,    31,    37,    41,    43,
       47,    53,    59,    61,    67,    71,    73,    79,    83,    89,    97,   101,   103,   107,
      109,   113,   127,   131,   137,   139,   149,   151,   157,   163,   167,   173,   179,   181,
      191,   193,   197,   199,   211,   223,   227,   229,   233,   239,   241,   251,   257,   263,
      269,   271,   277,   281,   283,   293,   307,   311,   313,   317,   331,   337,   347,   349,
      353,   359,   367,   373,   379,   383,   389,   397,   401,   409,   419,   421,   431,   433,
      439,   443,   449,   457,   461,   463,   467,   479,   487,   491,   499,   503,   509,   521,
      523,   541,   547,   557,   563,   569,   571,   577,   587,   593,   599,   601,   607,   613,
      617
    ];

    /// <summary>
    /// Computes the hash code for a sequence of items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sequence"></param>
    /// <returns></returns>
    public static int GetSequenceHashCode<T>(this IReadOnlyList<T> sequence)
    {
        unchecked
        {
            int hash = 2903 * sequence.Count; // 2903 is a prime number, which is not included in the PrimeNumbers list above
            int primeIndex = 0;
            for (var i = 0; i < sequence.Count; i++, primeIndex++)
            {
                var item = sequence[i];
                if (primeIndex >= PrimeNumbers.Length)
                {
                    int primeIndex2 = (i / PrimeNumbers.Length) % PrimeNumbers.Length;
                    hash ^= PrimeNumbers[primeIndex2];
                    primeIndex = 0;
                }
                if (item is not null)
                {
                    hash += PrimeNumbers[primeIndex] * item.GetHashCode();
                }
            }

            return hash;
        }
    }
}
