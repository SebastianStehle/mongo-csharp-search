﻿// Copyright 2021-present MongoDB Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Labs.Search
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
