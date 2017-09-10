#region

using System.Collections.Generic;
using System.Linq;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Utils;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

#endregion

// ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException

namespace Advitex.ExceptionAnalizer.Models.InClassMemberModels
{
    /// <summary>
    /// Model of method call
    /// </summary>
    public class MethodCallModel : BaseCallModel, IExceptionSource
    {
        /// <summary>
        /// Create model
        /// </summary>
        public MethodCallModel([NotNull] IReferenceExpression method)
            : base(method)
        {
        }

        /// <summary>
        /// Method call arguments
        /// </summary>
        public IEnumerable<Argument> Arguments
        {
            get
            {
                var result = new List<Argument>();

                var invocation = ReferenceExpression.GetContainingExpression() as IInvocationExpression;
                if (invocation != null)
                    result.AddRange(invocation.Arguments.Select(arg => new Argument(arg)));

                if (ReferenceExpression.QualifierExpression is IReferenceExpression)
                {
                    var qualifier = ReferenceExpression.QualifierExpression as IReferenceExpression;
                    var methodParams = (ReferencedMember.CompiledClassMember as IParametersOwner);
                    if (methodParams != null)
                    {
                        var methodParam = methodParams.Parameters.FirstOrDefault(p => p.IsExtensionFirstParameter());
                        if (methodParam != null)
                            result.Add(new Argument(qualifier, methodParam.ShortName));
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Get exceptions that was thrown by the code model
        /// </summary>
        public IEnumerable<ThrownException> GetThrownExceptions()
        {
            return
                from exInfo in ReferencedMember.ExceptionInfos
                where ReferenceExpression.CheckIsCatched(exInfo.ExceptionType) == CatchStatusEnum.NotCatched
                select new ThrownException(this, exInfo, Arguments);
        }
    }
}