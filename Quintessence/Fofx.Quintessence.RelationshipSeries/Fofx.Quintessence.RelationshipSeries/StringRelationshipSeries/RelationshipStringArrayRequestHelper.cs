using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Fofx
{
    public class RelationshipArrayStringValueRequestHelper : BaseRelationshipRevisableRequestHelper, ITimeSeriesRequestHelper
    {
        public override INullableReader GetDataReader(int[] entites, int[] factors, DatabaseRequestArgs args)
        {
            string message = RelationshipArrayRequestHelper.ExceptionMsg("RelationshipArrayStringValueRequestHelper.GetDataReader", new Exception("RelationshipArrayStringValueRequestHelper.GetDataReader has not been implemented."));
            throw new NotImplementedException(message);
        }

        public override INullableReader GetDataReader(int[] entites, int[] factors, int[] relationships, DatabaseRequestArgs args)
        {
            /// <Cedric>
            /// The problem with data readers is the looping required to read them
            /// I'd prefer to call APIs or execute queries using EF and converting the result set to a List/IList
            /// </Cedric>
            SqlParameter[] parameters = GetParameters(entites, factors, relationships, args);
            return DataAccess.GetDataReader("[TimeSeries].[RelationshipRevisableStringTimeseriesLoad]", parameters);
        }

        public override ITimeSeries CreateNew(ITimeSeriesIterator iterator, TimeSeriesDescriptor factor)
        {
            return new TextConstituentArrayTimeSeries();
        }

        protected override void Read(int[] entities, int[] timeseries, int[] relationship, DatabaseRequestArgs args, TimeSeriesDatabaseContext requester, Dictionary<QuickID, ITimeSeriesKey> keys, SortedList<ITimeSeriesKey, ITimeSeries> dbTimeSeriesResult)
        {
            TextConstituentArrayTimeSeries ncs = null;
            ITimeSeries referenceTimeSeries = null;
            ITimeSeriesKey referenceKey = null;

            Preference codeTypePreference = null;
            int previousEntityID = -1;
            int previousTimeSeriesValueID = -1;
            int previousRelationshipValueID = -1;
            DateTime previousValueDate = DateTime.MinValue;
            DateTime previousDeclarationDate = DateTime.MinValue;

            SortedList<DateTime, Revision<ConstituentTextArrayPoint>> pointCollection = null;
            Revision<ConstituentTextArrayPoint> revisionCollection = null;
            ConstituentTextArrayPoint constituentPoint = null;

            using (INullableReader reader = GetDataReader(entities, timeseries, relationship, args))
            {
                while (reader.Read())
                {
                    int entityID = reader.GetInt32(0);
                    int timeSeriesValueID = reader.GetInt32(1);
                    int relationshipValueID = reader.GetInt32(2);
                    int toEntityID = reader.GetInt32(3);
                    DateTime valueDate = reader.GetDateTime(4);
                    DateTime declarationDate = reader.GetDateTime(5);
                    string value = reader.GetNullableString(6);
                    int? nonKeyedAttributeSetId = reader.GetNullableInt32(7);

                    NonKeyedAttributeSet nonKeyedAttributeSet = null;
                    if (nonKeyedAttributeSetId != null)
                    {
                        nonKeyedAttributeSet = args.Translator.GetNonKeyedAttributeSet((int)nonKeyedAttributeSetId);
                    }

                    IEntityDescriptor entity = null;

                    if (entityID != previousEntityID || previousTimeSeriesValueID != timeSeriesValueID || previousRelationshipValueID != relationshipValueID)
                    {
                        QuickID qid = new QuickID(entityID, timeSeriesValueID, relationshipValueID);
                        if (keys.TryGetValue(qid, out referenceKey))
                        {
                            RelationshipTimeSeriesKey tsk = referenceKey as RelationshipTimeSeriesKey;
                            if (tsk == null || tsk.Relationship == null || tsk.Relationship.Source == null)
                            {
                                codeTypePreference = null;
                            }
                            else
                            {
                                if (codeTypePreference != null && tsk.Relationship.Source.CodePreference != null)
                                {
                                    if (codeTypePreference.Id != tsk.Relationship.Source.CodePreference.Id)
                                    {
                                        requester.EntityLookup.Clear();
                                    }

                                    codeTypePreference = tsk.Relationship.Source.CodePreference;
                                }
                                else
                                {
                                    requester.EntityLookup.Clear();
                                    codeTypePreference = tsk.Relationship.Source.CodePreference;
                                }
                            }
                        }

                        qid = new QuickID(previousEntityID, previousTimeSeriesValueID, previousRelationshipValueID);
                        if (keys.TryGetValue(qid, out referenceKey))
                        {
                            if (dbTimeSeriesResult.TryGetValue(referenceKey, out referenceTimeSeries))
                            {
                                ncs = referenceTimeSeries as TextConstituentArrayTimeSeries;
                                if (ncs != null)
                                {
                                    ncs.Add(pointCollection);
                                }
                            }
                        }

                        pointCollection = new SortedList<DateTime, Revision<ConstituentTextArrayPoint>>();
                        revisionCollection = new Revision<ConstituentTextArrayPoint>(valueDate);
                        pointCollection.Add(valueDate, revisionCollection);
                        constituentPoint = new ConstituentTextArrayPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                        revisionCollection.Add(declarationDate, constituentPoint);
                        previousEntityID = entityID;
                        previousTimeSeriesValueID = timeSeriesValueID;
                        previousRelationshipValueID = relationshipValueID;
                        previousValueDate = valueDate;
                        previousDeclarationDate = declarationDate;
                    }
                    else if (valueDate != previousValueDate)
                    {
                        revisionCollection = new Revision<ConstituentTextArrayPoint>(valueDate);
                        pointCollection.Add(valueDate, revisionCollection);
                        constituentPoint = new ConstituentTextArrayPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                        revisionCollection.Add(declarationDate, constituentPoint);
                        previousValueDate = valueDate;
                        previousDeclarationDate = declarationDate;
                    }
                    else if (declarationDate != previousDeclarationDate)
                    {
                        constituentPoint = new ConstituentTextArrayPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                        revisionCollection.Add(declarationDate, constituentPoint);
                        previousValueDate = valueDate;
                        previousDeclarationDate = declarationDate;
                    }

                    if (!requester.EntityLookup.TryGetValue(toEntityID, out entity))
                    {
                        if (!args.Translator.TryGetEntityDescriptorByID(toEntityID, codeTypePreference, out entity))
                        {
                            entity = new EntityDescriptor(toEntityID);
                        }
                    }

                    constituentPoint.Add(entity, new string[] { value }, nonKeyedAttributeSet);
                }
            }

            QuickID qid2 = new QuickID(previousEntityID, previousTimeSeriesValueID, previousRelationshipValueID);
            if (keys.TryGetValue(qid2, out referenceKey))
            {
                if (dbTimeSeriesResult.TryGetValue(referenceKey, out referenceTimeSeries))
                {
                    ncs = referenceTimeSeries as TextConstituentArrayTimeSeries;
                    if (ncs != null)
                    {
                        ncs.Add(pointCollection);
                    }
                }
            }
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
            {
                nonKeyedAttributeSet = args.Translator.GetNonKeyedAttributeSet((int)nonKeyedAttributeSetId);
            }

            IEntityDescriptor entity = null;
            if (!requester.EntityLookup.TryGetValue(toEntityID, out entity))
            {
                if (!args.Translator.TryGetEntityDescriptorByID(toEntityID, out entity))
                {
                    entity = new EntityDescriptor(toEntityID);
                }
            }
          
            ((TextConstituentArrayTimeSeries)iTimeSeries).Add(entity, valueDate, declarationDate, value, nonKeyedAttributeSet);
        }
    }
}
