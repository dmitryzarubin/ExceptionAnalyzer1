#region

using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.Abstract;

#endregion

namespace Advitex.ExceptionAnalizer.Resharper.Specifications
{
    public class IsCatchedExceptionSpecification
    {
        public static bool IsSpecified(ThrownException exception)
        {
//            if (exception.ExceptionSource.Owner == null)
//                return false;

            return exception.CatchStatus == CatchStatusEnum.Catched ||
                   exception.CatchStatus == CatchStatusEnum.CatchedInAllClause;
        }
    }
}