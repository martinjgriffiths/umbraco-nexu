﻿namespace Our.Umbraco.Nexu.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;

    using Our.Umbraco.Nexu.Common.Interfaces.Models;
    using Our.Umbraco.Nexu.Common.Interfaces.Services;
    using Our.Umbraco.Nexu.Core.Composing.Collections;

    /// <summary>
    /// Represents the nexu entity parsing service
    /// </summary>
    internal class NexuEntityParsingService : IEntityParsingService
    {
        /// <summary>
        /// The property value parser collection.
        /// </summary>
        private readonly PropertyValueParserCollection propertyValueParserCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="NexuEntityParsingService"/> class.
        /// </summary>
        /// <param name="propertyValueParserCollection">
        /// The property value parser collection.
        /// </param>
        public NexuEntityParsingService(PropertyValueParserCollection propertyValueParserCollection)
        {
            this.propertyValueParserCollection = propertyValueParserCollection;
        }        
        
        /// <inheritdoc />
        public void ParseContent(IContent content)
        {
            if (content.Blueprint)
            {
                return;
            }
            
            foreach (var prop in content.Properties)
            {
                var parser = this.propertyValueParserCollection.FirstOrDefault(x => x.IsParserFor(prop.PropertyType.PropertyEditorAlias));

                if (parser != null)
                {
                    foreach (var propValue in prop.Values)
                    {
                        parser.GetRelatedEntities(propValue.EditedValue.ToString());
                    }
                }
            }
        }

        /// <inheritdoc />
        public virtual IPropertyValueParser GetParserForPropertyEditor(string propertyEditorAlias)
        {
            return this.propertyValueParserCollection.FirstOrDefault(x => x.IsParserFor(propertyEditorAlias));
        }

        /// <inheritdoc />
        public virtual IEnumerable<IRelatedEntity> GetRelatedEntitiesFromPropertyEditorValue(string propertyEditorAlias, object propertyValue)
        {
            if (!string.IsNullOrWhiteSpace(propertyValue?.ToString()))
            {
                var parser = this.GetParserForPropertyEditor(propertyEditorAlias);

                if (parser != null)
                {
                    return parser.GetRelatedEntities(propertyValue.ToString()).DistinctBy(x => x.RelatedEntityUdi.ToString());
                }
            }
            
            return Enumerable.Empty<IRelatedEntity>();
        }
    }
}
