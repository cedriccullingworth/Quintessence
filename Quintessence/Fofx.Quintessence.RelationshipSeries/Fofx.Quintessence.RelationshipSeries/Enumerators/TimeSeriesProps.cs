using System;

namespace Fofx
{
    [Flags]
    public enum TimeSeriesProps
    {
        None = 0,
        Snapshot = 1,
        Characteristic = 2,
        Array = 4
    }
}
