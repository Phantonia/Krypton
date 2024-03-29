﻿using System.Collections;
using System.Collections.Generic;

namespace Krypton.Utilities
{
    public interface IIndexedEnumerable<T> : IEnumerable<T>
    {
        T this[int index] { get; }

        int Count { get; }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
