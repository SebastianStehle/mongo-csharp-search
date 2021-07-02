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
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Labs.Search
{
    /// <summary>
    /// Base class for search definitions.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public abstract class SearchDefinition<TDocument>
    {
        /// <summary>
        /// Renders the search definition to a <see cref="BsonDocument"/>.
        /// </summary>
        /// <param name="documentSerializer">The document serializer.</param>
        /// <param name="serializerRegistry">The serializer registry.</param>
        /// <returns>A <see cref="BsonDocument"/>.</returns>
        public abstract BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry);

        /// <summary>
        /// Performs an implicit conversion from a BSON document to a <see cref="SearchDefinition{TDocument}"/>.
        /// </summary>
        /// <param name="document">The BSON document specifying the search definition.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator SearchDefinition<TDocument>(BsonDocument document)
        {
            if (document == null)
            {
                return null;
            }

            return new BsonDocumentSearchDefinition<TDocument>(document);
        }

        /// <summary>
        /// Performs an implicit conversion from a string to a <see cref="SearchDefinition{TDocument}"/>.
        /// </summary>
        /// <param name="json">The string specifying the search definition in JSON.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator SearchDefinition<TDocument>(string json)
        {
            if (json == null)
            {
                return null;
            }

            return new JsonSearchDefinition<TDocument>(json);
        }
    }

    /// <summary>
    /// A search definition based on a BSON document.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public sealed class BsonDocumentSearchDefinition<TDocument> : SearchDefinition<TDocument>
    {
        private readonly BsonDocument _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonDocumentSearchDefinition{TDocument}"/> class.
        /// </summary>
        /// <param name="document">The BSON document specifying the search definition.</param>
        public BsonDocumentSearchDefinition(BsonDocument document)
        {
            _document = Ensure.IsNotNull(document, nameof(document));
        }

        /// <summary>
        /// Gets the BSON document.
        /// </summary>
        public BsonDocument Document
        {
            get { return _document; }
        }

        /// <inheritdoc />
        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            return _document;
        }
    }

    /// <summary>
    /// A search definition based on a JSON string.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public sealed class JsonSearchDefinition<TDocument> : SearchDefinition<TDocument>
    {
        private readonly string _json;

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonDocumentSearchDefinition{TDocument}"/> class.
        /// </summary>
        /// <param name="json">The JSON string specifying the search definition.</param>
        public JsonSearchDefinition(string json)
        {
            _json = Ensure.IsNotNullOrEmpty(json, nameof(json));
        }

        /// <summary>
        /// Gets the JSON string.
        /// </summary>
        public string Json
        {
            get { return _json; }
        }

        /// <inheritdoc />
        public override BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            return BsonDocument.Parse(_json);
        }
    }
}
