﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Fofx
{
    [Serializable]
    internal sealed class DataTypePoint : IDataPoint, IEnumerable<KeyValuePair<string, object>>
    {
        public DateTime DeclarationDate => throw new NotImplementedException();

        public DateTime ValueDate => throw new NotImplementedException();

        public object Value => throw new NotImplementedException();

        public NonKeyedAttributeSet NonKeyedAttribute => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
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
