﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Fofx
{
    public class RelationshipArrayDateValueRequestHelper : BaseRelationshipRevisableRequestHelper, ITimeSeriesRequestHelper
    {
        public override INullableReader GetDataReader(int[] entites, int[] factors, DatabaseRequestArgs args)
        {
            // Attempt to show the user a meaningful error
            string message = RelationshipArrayRequestHelper.ExceptionMsg("RelationshipArrayDateValueRequestHelper.GetDataReader", new Exception("RelationshipArrayDateValueRequestHelper.GetDataReader has not been implemented."));
            throw new NotImplementedException(message);
        }

        public override INullableReader GetDataReader(int[] entites, int[] factors, int[] relationships, DatabaseRequestArgs args)
        {
            SqlParameter[] parameters = GetParameters(entites, factors, relationships, args);
            return DataAccess.GetDataReader("[TimeSeries].[RelationshipRevisableStringTimeseriesLoad]", parameters);
        }

        public override ITimeSeries CreateNew(ITimeSeriesIterator iterator, TimeSeriesDescriptor factor)
        {
            return new DateConstituentArrayTimeSeries();
        }

        protected override void Read(int[] entities, int[] timeseries, int[] relationship, DatabaseRequestArgs args, TimeSeriesDatabaseContext requester, Dictionary<QuickID, ITimeSeriesKey> keys, SortedList<ITimeSeriesKey, ITimeSeries> dbTimeSeriesResult)
        {
            // Changed by Cedric to call a shared method instead of duplicating about 95% of the Read() code
            RelationshipArrayRequestHelper helper = new RelationshipArrayRequestHelper();
            helper.Read<RelationshipArrayDateValueRequestHelper>(entities, timeseries, relationship, args, requester, keys, dbTimeSeriesResult);

            #region Old code is commented out
            //DateConstituentArrayTimeSeries ncs = null;
            //ITimeSeries referenceTimeSeries = null;
            //ITimeSeriesKey referenceKey = null;

            //Preference codeTypePreference = null;
            //int previousEntityID = -1;
            //int previousTimeSeriesValueID = -1;
            //int previousRelationshipValueID = -1;
            //DateTime previousValueDate = DateTime.MinValue;
            //DateTime previousDeclarationDate = DateTime.MinValue;

            //SortedList<DateTime, Revision<ConstituentDateArrayPoint>> pointCollection = null;
            //Revision<ConstituentDateArrayPoint> revisionCollection = null;
            //ConstituentDateArrayPoint constituentPoint = null;

            //using (INullableReader reader = GetDataReader(entities, timeseries, relationship, args))
            //{
            //  while (reader.Read())
            //  {
            //    int entityID = reader.GetInt32(0);
            //    int timeSeriesValueID = reader.GetInt32(1);
            //    int relationshipValueID = reader.GetInt32(2);
            //    int toEntityID = reader.GetInt32(3);
            //    DateTime valueDate = reader.GetDateTime(4);
            //    DateTime declarationDate = reader.GetDateTime(5);
            //    string value = reader.GetNullableString(6);
            //    int? nonKeyedAttributeSetId = reader.GetNullableInt32(7);

            //    NonKeyedAttributeSet nonKeyedAttributeSet = null;
            //    if (nonKeyedAttributeSetId != null)
            //      nonKeyedAttributeSet = args.Translator.GetNonKeyedAttributeSet((int)nonKeyedAttributeSetId);

            //    IEntityDescriptor entity = null;

            //    if (entityID != previousEntityID || previousTimeSeriesValueID != timeSeriesValueID || previousRelationshipValueID != relationshipValueID)
            //    {
            //      QuickID qid = new QuickID(entityID, timeSeriesValueID, relationshipValueID);
            //      if (keys.TryGetValue(qid, out referenceKey))
            //      {
            //        RelationshipTimeSeriesKey tsk = referenceKey as RelationshipTimeSeriesKey;
            //        if (tsk == null || tsk.Relationship == null || tsk.Relationship.Source == null)
            //          codeTypePreference = null;
            //        else
            //        {
            //          if (codeTypePreference != null && tsk.Relationship.Source.CodePreference != null)
            //          {
            //            if (codeTypePreference.Id != tsk.Relationship.Source.CodePreference.Id)
            //              requester.EntityLookup.Clear();

            //            codeTypePreference = tsk.Relationship.Source.CodePreference;
            //          }
            //          else
            //          {
            //            requester.EntityLookup.Clear();
            //            codeTypePreference = tsk.Relationship.Source.CodePreference;
            //          }
            //        }
            //      }

            //      qid = new QuickID(previousEntityID, previousTimeSeriesValueID, previousRelationshipValueID);
            //      if (keys.TryGetValue(qid, out referenceKey))
            //      {
            //        if (dbTimeSeriesResult.TryGetValue(referenceKey, out referenceTimeSeries))
            //        {
            //          ncs = referenceTimeSeries as DateConstituentArrayTimeSeries;
            //          if (ncs != null)
            //          {
            //            ncs.Add(pointCollection);
            //          }
            //        }
            //      }

            //      pointCollection = new SortedList<DateTime, Revision<ConstituentDateArrayPoint>>();
            //      revisionCollection = new Revision<ConstituentDateArrayPoint>(valueDate);
            //      pointCollection.Add(valueDate, revisionCollection);
            //      constituentPoint = new ConstituentDateArrayPoint(declarationDate, valueDate, nonKeyedAttributeSet);
            //      revisionCollection.Add(declarationDate, constituentPoint);
            //      previousEntityID = entityID;
            //      previousTimeSeriesValueID = timeSeriesValueID;
            //      previousRelationshipValueID = relationshipValueID;
            //      previousValueDate = valueDate;
            //      previousDeclarationDate = declarationDate;
            //    }
            //    else if (valueDate != previousValueDate)
            //    {
            //      revisionCollection = new Revision<ConstituentDateArrayPoint>(valueDate);
            //      pointCollection.Add(valueDate, revisionCollection);
            //      constituentPoint = new ConstituentDateArrayPoint(declarationDate, valueDate, nonKeyedAttributeSet);
            //      revisionCollection.Add(declarationDate, constituentPoint);
            //      previousValueDate = valueDate;
            //      previousDeclarationDate = declarationDate;
            //    }
            //    else if (declarationDate != previousDeclarationDate)
            //    {
            //      constituentPoint = new ConstituentDateArrayPoint(declarationDate, valueDate, nonKeyedAttributeSet);
            //      revisionCollection.Add(declarationDate, constituentPoint);
            //      previousValueDate = valueDate;
            //      previousDeclarationDate = declarationDate;
            //    }

            //    if (!requester.EntityLookup.TryGetValue(toEntityID, out entity))
            //    {
            //      if (!args.Translator.TryGetEntityDescriptorByID(toEntityID, codeTypePreference, out entity))
            //        entity = new EntityDescriptor(toEntityID);
            //    }

            //    constituentPoint.Add(entity, value, nonKeyedAttributeSet);
            //  }
            //}

            //QuickID qid2 = new QuickID(previousEntityID, previousTimeSeriesValueID, previousRelationshipValueID);
            //if (keys.TryGetValue(qid2, out referenceKey))
            //{
            //  if (dbTimeSeriesResult.TryGetValue(referenceKey, out referenceTimeSeries))
            //  {
            //    ncs = referenceTimeSeries as DateConstituentArrayTimeSeries;
            //    if (ncs != null)
            //    {
            //      ncs.Add(pointCollection);
            //    }
            //  }
            //}
            #endregion
        }

        public override void Add(ITimeSeries iTimeSeries, INullableReader reader, DatabaseRequestArgs args, TimeSeriesDatabaseContext requester)
        {
            int toEntityID = reader.GetInt32(2);
            DateTime valueDate = reader.GetDateTime(3);
            DateTime declarationDate = reader.GetDateTime(4);
            string value = reader.GetString(5);
            int? nonKeyedAttributeSetId = reader.GetNullableInt32(7);

            NonKeyedAttributeSet nonKeyedAttributeSet = null;
            if (nonKeyedAttributeSetId != null)
            nonKeyedAttributeSet = args.Translator.GetNonKeyedAttributeSet((int)nonKeyedAttributeSetId);


            IEntityDescriptor entity = null;
            if (!requester.EntityLookup.TryGetValue(toEntityID, out entity))
            {
                if (!args.Translator.TryGetEntityDescriptorByID(toEntityID, out entity))
                {
                    entity = new EntityDescriptor(toEntityID);
                }
            }

            ((DateConstituentArrayTimeSeries)iTimeSeries).Add(entity, valueDate, declarationDate, value, nonKeyedAttributeSet);
        }
    }
}
