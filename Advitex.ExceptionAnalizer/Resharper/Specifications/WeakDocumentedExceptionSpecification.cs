#region

using System.Linq;
using Advitex.ExceptionAnalizer.Models;

#endregion

namespace Advitex.ExceptionAnalizer.Resharper.Specifications
{
    public class WeakDocumentedExceptionSpecification
    {
        public static bool IsSpecified(ThrownException exception)
        {
            var owner = exception.ExceptionSource.Owner;
            return owner.ExceptionInfos.Any(ei => ei.IsTypeEquals(exception));
        }
    }
}