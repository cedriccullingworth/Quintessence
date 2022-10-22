using System;
using System.Collections;
using System.Collections.Generic;

namespace Fofx
{
    [Serializable]
    internal sealed class ConstituentRevisableDataPointEnumerator<T> : IEnumerator<KeyValuePair<DateTime, IDataPoint>>
      where T : class, IConstituentDataPoint
    {
        public ConstituentRevisableDataPointEnumerator(SortedList<DateTime, Revision<T>> items)
        {
        }

        public KeyValuePair<DateTime, IDataPoint> Current => throw new NotImplementedException();

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            string message = RelationshipArrayRequestHelper.ExceptionMsg("ConstituentRevisableDataPointEnumerator.Dispose", new Exception("ConstituentRevisableDataPointEnumerator.Dispose has not been implemented."));
            throw new NotImplementedException(message);
        }

        public bool MoveNext()
        {
            string message = RelationshipArrayRequestHelper.ExceptionMsg("ConstituentRevisableDataPointEnumerator.MoveNext", new Exception("ConstituentRevisableDataPointEnumerator.MoveNext has not been implemented."));
            throw new NotImplementedException(message);
        }

        public void Reset()
        {
            string message = RelationshipArrayRequestHelper.ExceptionMsg("ConstituentRevisableDataPointEnumerator.Reset", new Exception("ConstituentRevisableDataPointEnumerator.Reset has not been implemented."));
            throw new NotImplementedException(message);
        }
    }
}
