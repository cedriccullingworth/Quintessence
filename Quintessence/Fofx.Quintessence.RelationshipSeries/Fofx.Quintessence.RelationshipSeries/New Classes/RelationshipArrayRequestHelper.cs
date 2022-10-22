using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlClient;

using static Fofx.BaseRelationshipRevisableRequestHelper;

namespace Fofx
{
    internal sealed class RelationshipArrayRequestHelper //: BaseRelationshipRevisableRequestHelper, ITimeSeriesRequestHelper
    {
        /// <summary>
        /// Cedric's attempt at a generic verion of Read() for the RelationshipRevisable Request Helpers
        /// Usage: Read<RelationshipArrayDateValueRequestHelper>, Read<RelationshipArrayNumericValueRequestHelper>, Read<RelationshipArrayStringValueRequestHelper> or Read<RelationshipArrayDateValueRequestHelper>
        /// </summary>
        /// <typeparam name="TCallerType">The type of the ConstituentRevisableTimeSeries</typeparam>
        /// <param name="entities">An array of int - Unchanged</param>
        /// <param name="timeseries">An array of int - Unchanged</param>
        /// <param name="relationship">An array of int - Unchanged</param>
        /// <param name="args">A DatabaseRequestArgs</param>
        /// <param name="requester">A TimeSeriesDatabaseContext</param>
        /// <param name="keys">A dictionary ... Dictionary<QuickID, ITimeSeriesKey> OR Dictionary<int, ITimeSeriesKey></param>
        /// <param name="keys">    Handling the 'int' key (versus QuickID) is a TODO</param>
        /// <param name="dbTimeSeriesResult">The sorted list of results</param>
        internal void Read<TCallerType>(int[] entities, int[] timeseries, int[] relationship, DatabaseRequestArgs args, TimeSeriesDatabaseContext requester, Dictionary<QuickID, ITimeSeriesKey> keys, SortedList<ITimeSeriesKey, ITimeSeries> dbTimeSeriesResult)
        {
            object ncs;
            ITimeSeries referenceTimeSeries = null;
            ITimeSeriesKey referenceKey = null;

            Preference codeTypePreference = null;
            int previousEntityID = -1;
            int previousTimeSeriesValueID = -1;
            int previousRelationshipValueID = -1;
            DateTime previousValueDate = DateTime.MinValue;
            DateTime previousDeclarationDate = DateTime.MinValue;

            string typ = "";

            Type t = typeof(ConstituentDatePoint);
            object revisionCollection = null;
            object pointCollection = null;
            object constituentPoint = null;
            object dataReaderHome;

            // We unfortunately lose the ability to use a 'using ()' for the data reader, so will call the Dispose method after use
            INullableReader reader = null;

            try
            {
                // Determine the data type base on the type of TCallerType
                if (typeof(TCallerType) == typeof(RelationshipArrayDateValueRequestHelper))
                {
                    typ = "Date";
                    t = typeof(ConstituentDatePoint);
                    constituentPoint = new ConstituentDatePoint(DateTime.MinValue, DateTime.MaxValue, new NonKeyedAttributeSet());
                    dataReaderHome = new RelationshipArrayDateValueRequestHelper();
                    reader = ((RelationshipArrayDateValueRequestHelper)dataReaderHome).GetDataReader(entities, timeseries, relationship, args);
                }
                else if (typeof(TCallerType) == typeof(RelationshipArrayNumericValueRequestHelper))
                {
                    typ = "Numeric";
                    t = typeof(ConstituentNumericPoint);
                    constituentPoint = new ConstituentNumericPoint(DateTime.MinValue, DateTime.MaxValue, new NonKeyedAttributeSet());
                    dataReaderHome = new RelationshipArrayNumericValueRequestHelper();
                    reader = ((RelationshipArrayNumericValueRequestHelper)dataReaderHome).GetDataReader(entities, timeseries, relationship, args);
                }
                else if (typeof(TCallerType) == typeof(RelationshipArrayStringValueRequestHelper))
                {
                    typ = "Text";
                    t = typeof(ConstituentTextPoint);
                    constituentPoint = new ConstituentTextPoint(DateTime.MinValue, DateTime.MaxValue, new NonKeyedAttributeSet());
                    dataReaderHome = new RelationshipArrayEnumValueRequestHelper();
                    reader = ((RelationshipArrayStringValueRequestHelper)dataReaderHome).GetDataReader(entities, timeseries, relationship, args);
                }
                else if (typeof(TCallerType) == typeof(RelationshipArrayEnumValueRequestHelper))
                {
                    typ = "Enum";
                    t = typeof(ConstituentTextPoint);
                    constituentPoint = new ConstituentTextPoint(DateTime.MinValue, DateTime.MaxValue, new NonKeyedAttributeSet());
                    dataReaderHome = new RelationshipArrayEnumValueRequestHelper();
                    reader = ((RelationshipArrayEnumValueRequestHelper)dataReaderHome).GetDataReader(entities, timeseries, relationship, args);
                }
                else
                {
                    throw new Exception(ExceptionMsg("RelationshipArrayRequestHelper.Read", new Exception("The Caller Type provided to RelationshipArrayRequestHelper.Read was not recognised.")));
                }

                if (reader != null)
                {
                    try
                    {
                        while (reader.Read())
                        {
                            int entityID = reader.GetInt32(0);
                            int timeSeriesValueID = reader.GetInt32(1);
                            int relationshipValueID = reader.GetInt32(2);
                            int toEntityID = reader.GetInt32(3);
                            DateTime valueDate = reader.GetDateTime(4);
                            DateTime declarationDate = reader.GetDateTime(5);
                            // Default to string, change if necessary
                            object value = reader.GetNullableString(6);
                            if (typ == "Numeric")
                            {
                                value = value = reader.GetNullableDouble(6);
                            }

                            int? nonKeyedAttributeSetId = reader.GetNullableInt32(7);
                            NonKeyedAttributeSet nonKeyedAttributeSet = null;
                            if (nonKeyedAttributeSetId != null) // How can it not be null when it's just been set to null?
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
                                        // Use the identified data type to set the ConstituentRevisableTimeSeries, pointCollection, revisionCollection and constituentPoint to the right types
                                        switch (typ)
                                        {
                                            case "Date":
                                                {
                                                    pointCollection = pointCollection as SortedList<DateTime, Revision<ConstituentDatePoint>>;
                                                    ncs = referenceTimeSeries as DateConstituentRevisableTimeSeries;
                                                    if (ncs != null)
                                                    {
                                                        ((DateConstituentRevisableTimeSeries)ncs).Add(pointCollection);
                                                    }

                                                    revisionCollection = new Revision<ConstituentDatePoint>(valueDate);
                                                    ((SortedList<DateTime, Revision<ConstituentDatePoint>>)pointCollection).Add(valueDate, (Revision<ConstituentDatePoint>)revisionCollection);
                                                    constituentPoint = new ConstituentDatePoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                                    ((Revision<ConstituentDatePoint>)revisionCollection).Add(declarationDate, (ConstituentDatePoint)constituentPoint);
                                                    break;
                                                }
                                            case "Numeric":
                                                {
                                                    pointCollection = pointCollection as SortedList<DateTime, Revision<ConstituentNumericPoint>>;
                                                    ncs = referenceTimeSeries as NumericConstituentRevisableTimeSeries;
                                                    if (ncs != null)
                                                    {
                                                        ((NumericConstituentRevisableTimeSeries)ncs).Add(pointCollection);
                                                    }

                                                    revisionCollection = new Revision<ConstituentNumericPoint>(valueDate);
                                                    ((SortedList<DateTime, Revision<ConstituentNumericPoint>>)pointCollection).Add(valueDate, (Revision<ConstituentNumericPoint>)revisionCollection);
                                                    constituentPoint = new ConstituentNumericPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                                    ((Revision<ConstituentNumericPoint>)revisionCollection).Add(declarationDate, (ConstituentNumericPoint)constituentPoint);
                                                    break;
                                                }
                                            case "Text":
                                            case "Enum":
                                                {
                                                    pointCollection = pointCollection as SortedList<DateTime, Revision<ConstituentTextPoint>>;
                                                    ncs = referenceTimeSeries as TextConstituentRevisableTimeSeries;
                                                    if (ncs != null)
                                                    {
                                                        ((TextConstituentRevisableTimeSeries)ncs).Add(pointCollection);
                                                    }

                                                    revisionCollection = new Revision<ConstituentTextPoint>(valueDate);
                                                    ((SortedList<DateTime, Revision<ConstituentTextPoint>>)pointCollection).Add(valueDate, (Revision<ConstituentTextPoint>)revisionCollection);
                                                    constituentPoint = new ConstituentTextPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                                    ((Revision<ConstituentTextPoint>)revisionCollection).Add(declarationDate, (ConstituentTextPoint)constituentPoint);
                                                    break;
                                                }
                                            default:
                                                {
                                                    pointCollection = pointCollection as SortedList<DateTime, Revision<ConstituentTextPoint>>;
                                                    ncs = referenceTimeSeries as TextConstituentRevisableTimeSeries;
                                                    if (ncs != null)
                                                    {
                                                        ((TextConstituentRevisableTimeSeries)ncs).Add(pointCollection);
                                                    }

                                                    pointCollection = new SortedList<DateTime, Revision<ConstituentTextPoint>>();
                                                    revisionCollection = new Revision<ConstituentTextPoint>(valueDate);
                                                    ((SortedList<DateTime, Revision<ConstituentTextPoint>>)pointCollection).Add(valueDate, (Revision<ConstituentTextPoint>)revisionCollection);
                                                    constituentPoint = new ConstituentTextPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                                    ((Revision<ConstituentTextPoint>)revisionCollection).Add(declarationDate, (ConstituentTextPoint)constituentPoint);
                                                    break;
                                                }
                                        }
                                    }
                                }

                                previousEntityID = entityID;
                                previousTimeSeriesValueID = timeSeriesValueID;
                                previousRelationshipValueID = relationshipValueID;
                                previousValueDate = valueDate;
                                previousDeclarationDate = declarationDate;
                            }
                            else if (valueDate != previousValueDate)
                            {
                                switch (typ)
                                {
                                    case "Date":
                                        {
                                            pointCollection = pointCollection as SortedList<DateTime, Revision<ConstituentDatePoint>>;
                                            ncs = referenceTimeSeries as DateConstituentRevisableTimeSeries;
                                            if (ncs != null)
                                            {
                                                ((DateConstituentRevisableTimeSeries)ncs).Add(pointCollection);
                                            }

                                            revisionCollection = new Revision<ConstituentDatePoint>(valueDate);
                                            ((SortedList<DateTime, Revision<ConstituentDatePoint>>)pointCollection).Add(valueDate, (Revision<ConstituentDatePoint>)revisionCollection);
                                            constituentPoint = new ConstituentDatePoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                            ((Revision<ConstituentDatePoint>)revisionCollection).Add(declarationDate, (ConstituentDatePoint)constituentPoint);
                                            break;
                                        }
                                    case "Numeric":
                                        {
                                            pointCollection = pointCollection as SortedList<DateTime, Revision<ConstituentNumericPoint>>;
                                            ncs = referenceTimeSeries as NumericConstituentRevisableTimeSeries;
                                            if (ncs != null)
                                            {
                                                ((NumericConstituentRevisableTimeSeries)ncs).Add(pointCollection);
                                            }

                                            revisionCollection = new Revision<ConstituentNumericPoint>(valueDate);
                                            ((SortedList<DateTime, Revision<ConstituentNumericPoint>>)pointCollection).Add(valueDate, (Revision<ConstituentNumericPoint>)revisionCollection);
                                            constituentPoint = new ConstituentNumericPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                            ((Revision<ConstituentNumericPoint>)revisionCollection).Add(declarationDate, (ConstituentNumericPoint)constituentPoint);
                                            break;
                                        }
                                    case "Text":
                                    case "Enum":
                                        {
                                            pointCollection = pointCollection as SortedList<DateTime, Revision<ConstituentTextPoint>>;
                                            ncs = referenceTimeSeries as TextConstituentRevisableTimeSeries;
                                            if (ncs != null)
                                            {
                                                ((TextConstituentRevisableTimeSeries)ncs).Add(pointCollection);
                                            }

                                            revisionCollection = new Revision<ConstituentTextPoint>(valueDate);
                                            ((SortedList<DateTime, Revision<ConstituentTextPoint>>)pointCollection).Add(valueDate, (Revision<ConstituentTextPoint>)revisionCollection);
                                            constituentPoint = new ConstituentTextPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                            ((Revision<ConstituentTextPoint>)revisionCollection).Add(declarationDate, (ConstituentTextPoint)constituentPoint);
                                            break;
                                        }
                                    default:
                                        {
                                            pointCollection = pointCollection as SortedList<DateTime, Revision<ConstituentTextPoint>>;
                                            ncs = referenceTimeSeries as TextConstituentRevisableTimeSeries;
                                            if (ncs != null)
                                            {
                                                ((TextConstituentRevisableTimeSeries)ncs).Add(pointCollection);
                                            }

                                            revisionCollection = new Revision<ConstituentTextPoint>(valueDate);
                                            ((SortedList<DateTime, Revision<ConstituentTextPoint>>)pointCollection).Add(valueDate, (Revision<ConstituentTextPoint>)revisionCollection);
                                            constituentPoint = new ConstituentTextPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                            ((Revision<ConstituentTextPoint>)revisionCollection).Add(declarationDate, (ConstituentTextPoint)constituentPoint);
                                            break;
                                        }
                                }

                                previousValueDate = valueDate;
                                previousDeclarationDate = declarationDate;
                            }
                            else if (declarationDate != previousDeclarationDate)
                            {
                                switch (typ)
                                {
                                    case "Date":
                                        {
                                            revisionCollection = new Revision<ConstituentDatePoint>(valueDate);
                                            constituentPoint = new ConstituentDatePoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                            ((Revision<ConstituentDatePoint>)revisionCollection).Add(declarationDate, (ConstituentDatePoint)constituentPoint);
                                            break;
                                        }
                                    case "Numeric":
                                        {
                                            revisionCollection = new Revision<ConstituentNumericPoint>(valueDate);
                                            constituentPoint = new ConstituentNumericPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                            ((Revision<ConstituentNumericPoint>)revisionCollection).Add(declarationDate, (ConstituentNumericPoint)constituentPoint);
                                            break;
                                        }
                                    case "Text":
                                    case "Enum":
                                        {
                                            revisionCollection = new Revision<ConstituentTextPoint>(valueDate);
                                            constituentPoint = new ConstituentTextPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                            ((Revision<ConstituentTextPoint>)revisionCollection).Add(declarationDate, (ConstituentTextPoint)constituentPoint);
                                            break;
                                        }
                                    default:
                                        {
                                            revisionCollection = new Revision<ConstituentTextPoint>(valueDate);
                                            constituentPoint = new ConstituentTextPoint(declarationDate, valueDate, nonKeyedAttributeSet);
                                            ((Revision<ConstituentTextPoint>)revisionCollection).Add(declarationDate, (ConstituentTextPoint)constituentPoint);
                                            break;
                                        }
                                }

                                previousValueDate = valueDate;
                                previousDeclarationDate = declarationDate;
                            }

                            if (!requester.EntityLookup.TryGetValue(toEntityID, out entity))
                            {
                                if (!args.Translator.TryGetEntityDescriptorByID(toEntityID, codeTypePreference, out entity))
                                {
                                    entity = new EntityDescriptor(toEntityID);
                                }
                                else
                                {
                                    requester.EntityLookup.Add(toEntityID, entity);
                                }
                            }

                            if (typ == "Date")
                            {
                                ((ConstituentDatePoint)constituentPoint).Add(entity, value, nonKeyedAttributeSet);
                            }
                            else if (typ == "Numeric")
                            {
                                ((ConstituentNumericPoint)constituentPoint).Add(entity, value, nonKeyedAttributeSet);
                            }
                            else
                            {
                                ((ConstituentTextPoint)constituentPoint).Add(entity, value, nonKeyedAttributeSet);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ExceptionMsg("RelationshipArrayRequestHelper.Read", ex);
                        throw new Exception(message);
                    }
                }
                else
                {
                    string message = ExceptionMsg("RelationshipArrayRequestHelper.Read", new Exception($"Data reader for {typeof(TCallerType).Name}.Read has no value."));
                    throw new Exception(message);
                }

                // Disposing of the reader manually because 'using' couldn't work here
                reader.Dispose();

                QuickID qid2 = new QuickID(previousEntityID, previousTimeSeriesValueID, previousRelationshipValueID);
                if (keys.TryGetValue(qid2, out referenceKey))
                {
                    if (dbTimeSeriesResult.TryGetValue(referenceKey, out referenceTimeSeries))
                    {
                        ncs = referenceTimeSeries as DateConstituentRevisableTimeSeries;
                        if (ncs != null)
                        {
                            if (typ == "Date")
                            {
                                ((DateConstituentRevisableTimeSeries)ncs).Add(pointCollection);
                            }
                            else if (typ == "Numeric")
                            {
                                ((NumericConstituentRevisableTimeSeries)ncs).Add(pointCollection);
                            }
                            else
                            {
                                ((TextConstituentRevisableTimeSeries)ncs).Add(pointCollection);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // I would normally write something to display errors to the user in a friendly way. For now, I'm just throwing an exception with a
                // friendlier error message
                string message = ExceptionMsg("RelationshipArrayRequestHelper.Read", ex);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Extract meaningful messages for an exception
        /// </summary>
        /// <param name="caller">Which module experienced the exception</param>
        /// <param name="ex">The exception</param>
        /// <returns>The exception messages</returns>
        public static string ExceptionMsg(string caller, Exception ex)
        {
            return caller + ": " + ex.Message + (ex.InnerException != null ? "\r\n" + ex.InnerException.Message + (ex.InnerException.InnerException != null ? "\r\n" + ex.InnerException.InnerException.Message : string.Empty) : string.Empty);
        }
    }
}
