using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Aurochses.Data.EntityFramework.Helpers
{
    /// <summary>
    /// Class PrimitivePropertyConfigurationHelpers
    /// </summary>
    public static class PrimitivePropertyConfigurationHelpers
    {
        /// <summary>
        /// Set Unique Index
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>PrimitivePropertyConfiguration.</returns>
        public static PrimitivePropertyConfiguration HasUniqueIndex(this PrimitivePropertyConfiguration property)
        {
            var indexAttribute = new IndexAttribute { IsUnique = true };
            var indexAnnotation = new IndexAnnotation(indexAttribute);

            return property.HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
        }

        /// <summary>
        /// Set Unique Index with more than one field
        /// </summary>
        /// <param name="property">Property</param>
        /// <param name="indexName">Index name <example>IX_User_FirstNameLastName</example></param>
        /// <param name="columnOrder">Property order starting from zero.</param>
        /// <returns>PrimitivePropertyConfiguration.</returns>
        public static PrimitivePropertyConfiguration HasUniqueIndex(this PrimitivePropertyConfiguration property, string indexName, int columnOrder)
        {
            var indexAttribute = new IndexAttribute(indexName, columnOrder) { IsUnique = true };
            var indexAnnotation = new IndexAnnotation(indexAttribute);

            return property.HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
        }
    }
}