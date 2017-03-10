﻿namespace Our.Umbraco.Nexu.Parsers.Core
{
    using System.Collections.Generic;

    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;

    using Our.Umbraco.Nexu.Core.Interfaces;
    using Our.Umbraco.Nexu.Core.Models;

    /// <summary>
    /// The content picker parser.
    /// </summary>
    public class ContentPickerParser : IPropertyParser
    {
        /// <summary>
        /// Check if it's a parser for a data type definition
        /// </summary>
        /// <param name="dataTypeDefinition">
        /// The data type definition.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsParserFor(IDataTypeDefinition dataTypeDefinition)
        {
            return dataTypeDefinition.PropertyEditorAlias.Equals(
                 global::Umbraco.Core.Constants.PropertyEditors.ContentPickerAlias);
        }

        /// <summary>
        /// Get the linked entities from the property editor data
        /// </summary>
        /// <param name="property">The property to parse</param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<ILinkedEntity> GetLinkedEntities(Property property)
        {
            var entities = new List<LinkedDocumentEntity>();

            if (property.Value == null)
            {
                return entities;
            }

            var attemptInt = property.Value.TryConvertTo<int>();

            if (attemptInt.Success)
            {
                entities.Add(new LinkedDocumentEntity(attemptInt.Result));
            }

            return entities;
        }

        public IEnumerable<ILinkedEntity> GetLinkedEntities(object propertyValue)
        {
            throw new System.NotImplementedException();
        }
    }
}