using System;
using System.Collections;
using System.Collections.Generic;

namespace Fofx
{
    [Serializable]
    internal sealed class ArrayTypeValues : IEnumerable<KeyValuePair<int, DataTypeValues>>
    {
        public IEnumerator<KeyValuePair<int, DataTypeValues>> GetEnumerator()
        {
            string message = RelationshipArrayRequestHelper.ExceptionMsg("ArrayTypeValues.GetEnumerator", new Exception("ArrayTypeValues.GetEnumerator has not been implemented."));
            throw new NotImplementedException(message);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            string message = RelationshipArrayRequestHelper.ExceptionMsg("ArrayTypeValues.GetEnumerator", new Exception("ArrayTypeValues.GetEnumerator has not been implemented."));
            throw new NotImplementedException(message);
        }
    }
}
