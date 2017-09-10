#region

using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;

#endregion

namespace Advitex.ExceptionAnalizer.Models.XmlDocumentationModels
{
    /// <summary>
    /// Exception from xml documentation
    /// </summary>
    public class XmlDocException
    {
        internal XmlDocException([NotNull] string typeName, [NotNull] string description, DocumentRange range,
                                 [CanBeNull] ITypeElement exceptionType)
        {
            ExceptionTypeName = typeName;
            ExceptionDescription = description;
            ExceptionType = exceptionType;
            Range = range;
        }

        /// <summary>
        /// Exception type
        /// </summary>
        [CanBeNull]
        public ITypeElement ExceptionType { get; private set; }

        /// <summary>
        /// Name of exception type
        /// </summary>
        [NotNull]
        public string ExceptionTypeName { get; private set; }

        /// <summary>
        /// Exception description
        /// </summary>
        [NotNull]
        public string ExceptionDescription { get; private set; }

        /// <summary>
        /// Linked xml comment range
        /// </summary>
        public DocumentRange Range { get; private set; }
    }
}