// Copyright 2021-present MongoDB Inc.
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

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using Xunit;

namespace MongoDB.Labs.Search.Tests
{
    public class PipelineDefinitionBuilderTests
    {
        [Fact]
        public void Search_should_add_expected_stage()
        {
            var pipeline = new EmptyPipelineDefinition<BsonDocument>();
            var builder = new SearchDefinitionBuilder<BsonDocument>();
            var result = pipeline.Search(builder.Text("foo", "bar"));
            var stages = RenderStages(result, BsonDocumentSerializer.Instance);
            Assert.Equal(
                BsonDocument.Parse("{ $search: { text: { query: 'foo', path: 'bar' } } }"),
                stages[0]);
        }

        [Fact]
        public void Search_should_add_expected_stage_with_highlight()
        {
            var pipeline = new EmptyPipelineDefinition<BsonDocument>();
            var builder = new SearchDefinitionBuilder<BsonDocument>();
            var highlightBuilder = new HighlightOptionsBuilder<BsonDocument>();
            var result = pipeline.Search(builder.Text("foo", "bar"), highlightBuilder.Options("foo"));
            var stages = RenderStages(result, BsonDocumentSerializer.Instance);
            Assert.Equal(
                BsonDocument.Parse("{ $search: { text: { query: 'foo', path: 'bar' }, highlight: { path: 'foo' } } }"),
                stages[0]);
        }

        [Fact]
        public void Search_should_add_expected_stage_with_index()
        {
            var pipeline = new EmptyPipelineDefinition<BsonDocument>();
            var builder = new SearchDefinitionBuilder<BsonDocument>();
            var highlightBuilder = new HighlightOptionsBuilder<BsonDocument>();
            var result = pipeline.Search(builder.Text("foo", "bar"), indexName: "foo");
            var stages = RenderStages(result, BsonDocumentSerializer.Instance);
            Assert.Equal(
                BsonDocument.Parse("{ $search: { text: { query: 'foo', path: 'bar' }, index: 'foo' } }"),
                stages[0]);
        }

        [Fact]
        public void Search_should_throw_when_pipeline_is_null()
        {
            PipelineDefinition<BsonDocument, BsonDocument> pipeline = null;
            var builder = new SearchDefinitionBuilder<BsonDocument>();
            var exception = Record.Exception(() => pipeline.Search(builder.Text("foo", "bar")));
            var argumentNullException = Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("pipeline", argumentNullException.ParamName);
        }

        [Fact]
        public void Search_should_throw_when_query_is_null()
        {
            var pipeline = new EmptyPipelineDefinition<BsonDocument>();
            var exception = Record.Exception(() => pipeline.Search(null));
            var argumentNullException = Assert.IsType<ArgumentNullException>(exception);
            Assert.Equal("query", argumentNullException.ParamName);
        }

        private IList<BsonDocument> RenderStages<TInput, TOutput>(PipelineDefinition<TInput, TOutput> pipeline, IBsonSerializer<TInput> inputSerializer)
        {
            var renderedPipeline = pipeline.Render(inputSerializer, BsonSerializer.SerializerRegistry);
            return renderedPipeline.Documents;
        }
    }
}
