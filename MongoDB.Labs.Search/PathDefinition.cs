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
using System.Collections.Generic;
using System.Linq;

namespace MongoDB.Labs.Search
{
    /// <summary>
    /// Base class for search paths.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public abstract class PathDefinition<TDocument>
    {
        /// <summary>
        /// Renders the path to a <see cref="BsonValue"/>.
        /// </summary>
        /// <param name="documentSerializer">The document serializer.</param>
        /// <param name="serializerRegistry">The serializer registry.</param>
        /// <returns>A <see cref="BsonValue"/>.</returns>
        public abstract BsonValue Render(IBsonSerializer<TDocument> documentSerializer, IBsonSerializerRegistry serializerRegistry);

        /// <summary>
        /// Performs an implicit conversion from <see cref="FieldDefinition{TDocument}"/> to
        /// <see cref="PathDefinition{TDocument}"/>.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator PathDefinition<TDocument>(FieldDefinition<TDocument> field)
        {
            return new SinglePathDefinition<TDocument>(field);
        }

        /// <summary>
        /// Performs an implicit conversion from a field name to <see cref="PathDefinition{TDocument}"/>.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator PathDefinition<TDocument>(string fieldName)
        {
            return new SinglePathDefinition<TDocument>(new StringFieldDefinition<TDocument>(fieldName));
        }

        /// <summary>
        /// Performs an implicit conversion from an array of <see cref="FieldDefinition{TDocument}"/> to
        /// <see cref="PathDefinition{TDocument}"/>.
        /// </summary>
        /// <param name="fields">The array of fields.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator PathDefinition<TDocument>(FieldDefinition<TDocument>[] fields)
        {
            return new MultiPathDefinition<TDocument>(fields);
        }

        /// <summary>
        /// Performs an implicit conversion from a list of <see cref="FieldDefinition{TDocument}"/> to
        /// <see cref="PathDefinition{TDocument}"/>.
        /// </summary>
        /// <param name="fields">The list of fields.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator PathDefinition<TDocument>(List<FieldDefinition<TDocument>> fields)
        {
            return new MultiPathDefinition<TDocument>(fields);
        }

        /// <summary>
        /// Performs an implicit conversion from an array of field names to 
        /// <see cref="PathDefinition{TDocument}"/>.
        /// </summary>
        /// <param name="fieldNames">The array of field names.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator PathDefinition<TDocument>(string[] fieldNames)
        {
            return new MultiPathDefinition<TDocument>(
                fieldNames.Select(fieldName => new StringFieldDefinition<TDocument>(fieldName)));
        }
    }
}
