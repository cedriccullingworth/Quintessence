﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Fofx
{
    [Serializable]
    public abstract class ConstituentDataPoint<T> : IConstituentDataPoint
    {
        public IList<IEntityDescriptor> Entities => throw new NotImplementedException();

        public DateTime DeclarationDate => throw new NotImplementedException();

        public DateTime ValueDate => throw new NotImplementedException();

        public object Value => throw new NotImplementedException();

        public NonKeyedAttributeSet NonKeyedAttribute => throw new NotImplementedException();

        public void Add(IEntityDescriptor child, object obj, NonKeyedAttributeSet nonKey)
        {
            throw new NotImplementedException();
        }

        public void AddWithNull(IEntityDescriptor child, object obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<IEntityDescriptor, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public NonKeyedAttributeSet GetNonKey(IEntityDescriptor child)
        {
            throw new NotImplementedException();
        }

        public object GetValue(IEntityDescriptor child)
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
