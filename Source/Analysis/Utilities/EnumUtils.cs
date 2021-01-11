﻿using System;

namespace Krypton.Analysis.Utilities
{
    public static class EnumUtils
    {
        public static T[] GetValues<T>()
            where T : struct, Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
