using System;
using System.Collections;
using System.Collections.Generic;

namespace Fofx
{
    [Serializable]
    internal sealed class ArrayTypePoint : IDataPoint, IEnumerable<KeyValuePair<int, DataTypeValues>>
    {
        public DateTime DeclarationDate => throw new NotImplementedException();

        public DateTime ValueDate => throw new NotImplementedException();

        public object Value => throw new NotImplementedException();

        public NonKeyedAttributeSet NonKeyedAttribute => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<int, DataTypeValues>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IDataPoint Shift(DateTime valueDate, DateTime declarationDate)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
