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
    /// Options for highlighting.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public class HighlightOptions<TDocument>
    {
        private PathDefinition<TDocument> _path;
        private int? _maxCharsToExamine;
        private int? _maxNumPassages;

        /// <summary>
        /// Gets or sets the document field to search.
        /// </summary>
        public PathDefinition<TDocument> Path
        {
            get { return _path; }
            set { _path = Ensure.IsNotNull(value, nameof(value)); }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters to examine on a document when performing
        /// highlighting for a field.
        /// </summary>
        public int? MaxCharsToExamine
        {
            get { return _maxCharsToExamine; }
            set { _maxCharsToExamine = Ensure.IsNullOrGreaterThanZero(value, nameof(value)); }
        }

        /// <summary>
        /// Gets or sets the number of high-scoring passages to return per document in the
        /// highlighting results for each field.
        /// </summary>
        public int? MaxNumPassages
        {
            get { return _maxNumPassages; }
            set { _maxNumPassages = Ensure.IsNullOrGreaterThanZero(value, nameof(value)); }
        }

        internal HighlightOptions() { }

        /// <summary>
        /// Renders the options to a <see cref="BsonDocument"/>.
        /// </summary>
        /// <param name="documentSerializer">The document serializer.</param>
        /// <param name="serializerRegistry">The serializer registry.</param>
        /// <returns>A <see cref="BsonDocument"/>.</returns>
        public BsonDocument Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry)
        {
            BsonDocument document = new BsonDocument();
            document.Add("path", _path.Render(documentSerializer, serializerRegistry));
            if (_maxCharsToExamine != null)
            {
                document.Add("maxCharsToExamine", _maxCharsToExamine);
            }
            if (_maxNumPassages != null)
            {
                document.Add("maxNumPassages", _maxNumPassages);
            }
            return document;
        }
    }
}
