#region

using System.Collections.Generic;
using System.Linq;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Utils;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

#endregion

namespace Advitex.ExceptionAnalizer.Models.InClassMemberModels
{
    /// <summary>
    /// Model of property call
    /// </summary>
    public class PropertyCallModel : BaseCallModel, IExceptionSource
    {
        /// <summary>
        /// Create model
        /// </summary>
        /// <param name = "method"> </param>
        public PropertyCallModel([NotNull] IReferenceExpression method)
            : base(method)
        {
        }

        /// <summary>
        /// Get exceptions that was thrown by the code model
        /// </summary>
        public IEnumerable<ThrownException> GetThrownExceptions()
        {
            return
                from exInfo in ReferencedMember.ExceptionInfos
                where ReferenceExpression.CheckIsCatched(exInfo.ExceptionType) == CatchStatusEnum.NotCatched
                select new ThrownException(this, exInfo.ExceptionType, exInfo.ExceptionDescription);
        }
    }
}