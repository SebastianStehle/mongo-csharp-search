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

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.Labs.Search.Tests
{
    public class ProjectionDefinitionBuilderTests
    {
        [Fact]
        public void MetaSearchHighlights()
        {
            var subject = CreateSubject<BsonDocument>();

            AssertRendered(subject.MetaSearchHighlights("a"), "{ a: { $meta: 'searchHighlights' } }");
        }

        [Fact]
        public void MetaSearchScore()
        {
            var subject = CreateSubject<BsonDocument>();

            AssertRendered(subject.MetaSearchScore("a"), "{ a : { $meta: 'searchScore' } }");
        }

        private void AssertRendered<TDocument>(ProjectionDefinition<TDocument> projection, string expected)
        {
            AssertRendered(projection, BsonDocument.Parse(expected));
        }

        private void AssertRendered<TDocument>(ProjectionDefinition<TDocument> projection, BsonDocument expected)
        {
            var documentSerializer = BsonSerializer.SerializerRegistry.GetSerializer<TDocument>();
            var renderedProjection = projection.Render(documentSerializer, BsonSerializer.SerializerRegistry);

            Assert.Equal(expected, renderedProjection);
        }

        private ProjectionDefinitionBuilder<TDocument> CreateSubject<TDocument>()
        {
            return new ProjectionDefinitionBuilder<TDocument>();
        }
    }
}
