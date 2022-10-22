using System;
using System.Collections;
using System.Collections.Generic;

namespace Fofx
{
    [Serializable]
    internal sealed class DataTypeValues : IEnumerable<KeyValuePair<string, object>>
    {
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
