﻿using MongoDB.Driver.Core.Misc;

namespace MongoDB.Driver.Search
{
    public static class AggregateFluentBuilder
    {
        public static IAggregateFluent<TResult> Search<TResult>(
            this IAggregateFluent<TResult> fluent,
            SearchDefinition<TResult> query)
        {
            Ensure.IsNotNull(fluent, nameof(fluent));
            return fluent.AppendStage(PipelineStageDefinitionBuilder.Search(query));
        }
    }
}
