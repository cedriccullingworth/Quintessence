using System;
using System.Collections;
using System.Collections.Generic;

namespace Fofx
{
    [Serializable]
    internal sealed class DataPoint<T> : IDataPoint
    {
        public DateTime DeclarationDate => throw new NotImplementedException();

        public DateTime ValueDate => throw new NotImplementedException();

        public object Value => throw new NotImplementedException();

        public NonKeyedAttributeSet NonKeyedAttribute => throw new NotImplementedException();

        public IDataPoint Shift(DateTime valueDate, DateTime declarationDate)
        {
            throw new NotImplementedException();
        }
    }
}