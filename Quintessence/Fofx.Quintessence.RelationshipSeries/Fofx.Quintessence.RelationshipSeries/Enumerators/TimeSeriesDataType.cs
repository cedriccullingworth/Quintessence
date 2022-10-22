using System.ComponentModel;

namespace Fofx
{
    public enum TimeSeriesDataType
    {
        Numeric = 'N',
        Text = 'T',
        Enum = 'E',
        [Browsable(false)]
        Code = 'C',
        Date = 'D',
        UserDefined = 'U',
        [Browsable(false)]
        Unknown = 'X',
        [Browsable(false)]
        CompoundCode = 'Q'
    }
}
