#region

using Advitex.ExceptionAnalizer.Models.XmlDocumentationModels;

#endregion

namespace Advitex.ExceptionAnalizer.Resharper.Specifications
{
    public class NotRecognizedExceptionTypeSpecification
    {
        public static bool IsSpecified(XmlDocException exception)
        {
            return (exception.ExceptionType == null);
        }
    }
}