#region

using System;
using System.Linq;
using Advitex.ExceptionAnalizer.Models;
using Advitex.ExceptionAnalizer.Models.InClassMemberModels;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

#endregion

// ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException

namespace Advitex.ExceptionAnalizer.Utils
{
    public static class ThrowHelper
    {
        /// <summary>
        /// Get exception type of thrown exception
        /// </summary>
        /// <param name = "throwStatement"> Throw statement </param>
        /// <returns> Exception type </returns>
        /// <exception cref = "ArgumentNullException"> throwStatement is null </exception>
        [CanBeNull]
        public static ITypeElement GetExceptionType([NotNull] this IThrowStatement throwStatement)
        {
            if (throwStatement == null)
                throw new ArgumentNullException("throwStatement");

            IType psiType = null;

            if (throwStatement.Exception != null)
            {
                psiType = throwStatement.Exception.Type();
            }
            else
            {
                var catchClause = throwStatement.
                    Parents<ITryStatement>().
                    SelectMany(t => t.Catches).
                    FirstOrDefault(c => c.Contains(throwStatement));

                if (catchClause == null)
                    return null;

                psiType = catchClause.ExceptionType;
            }
                
            if (!psiType.IsResolved)
                return null;

            var declarationCache =
                throwStatement.GetPsiServices()
                              .CacheManager.GetDeclarationsCache(throwStatement.GetPsiModule(), true, true);
            return declarationCache.GetTypeElementByCLRName(psiType.GetLongPresentableName(throwStatement.Language));
        }

        /// <summary>
        /// Get exception type of thrown exception
        /// </summary>
        /// <param name = "catchClause"> Catch clause </param>
        /// <returns> Exception type </returns>
        /// <exception cref = "ArgumentNullException"> Catch clause is null </exception>
        [CanBeNull]
        public static ITypeElement GetExceptionType([NotNull] this ICatchClause catchClause)
        {
            if (catchClause == null)
                throw new ArgumentNullException("catchClause");

            var psiType = catchClause.ExceptionType;
            if (!psiType.IsResolved)
                return null;

            var declarationCache =
                catchClause.GetPsiServices().CacheManager.GetDeclarationsCache(catchClause.GetPsiModule(), true, true);
            return declarationCache.GetTypeElementByCLRName(psiType.GetLongPresentableName(catchClause.Language));
        }

        /// <summary>
        /// Check is exception catched
        /// </summary>
        /// <param name = "startNode"> start node of psi-tree </param>
        /// <param name = "exceptionType"> Exception type that is checked </param>
        /// <returns> Catch status </returns>
        /// <exception cref = "ArgumentNullException"> startNode is null </exception>
        /// <exception cref = "ArgumentNullException"> exceptionType is null </exception>
        public static CatchStatusEnum CheckIsCatched([NotNull] this ITreeNode startNode,
                                                     [NotNull] ITypeElement exceptionType)
        {
            if (startNode == null)
                throw new ArgumentNullException("startNode");
            if (exceptionType == null)
                throw new ArgumentNullException("exceptionType");

            var tryStatements = startNode.Parents<ITryStatement>();

            foreach (var tryStatement in tryStatements.Where(t => t.Try.Contains(startNode)))
            {
                if (tryStatement.CatchesAllExceptions)
                    return CatchStatusEnum.CatchedInAllClause;

                foreach (var catchClause in tryStatement.Catches)
                {
                    var catchedExceptionType = GetExceptionType(catchClause);

                    if (exceptionType.IsDescendantOf(catchedExceptionType))
                    {
                        var rethrow = catchClause.Body.Children<IThrowStatement>().FirstOrDefault();
                        if (rethrow == null)
                        {
                            return CatchStatusEnum.Catched;
                        }
                        else
                        {
                            if (rethrow.Exception == null)
                                return CatchStatusEnum.NotCatched;

                            var exRef = (rethrow.Exception as IReferenceExpression);

                            if (exRef != null && exRef.Reference.CurrentResolveResult != null &&
                                (exRef.Reference.CurrentResolveResult.DeclaredElement is ICatchVariableDeclaration))
                                return CatchStatusEnum.NotCatched;
                            else
                                return CatchStatusEnum.Catched;
                        }
                    }
                }
            }

            return CatchStatusEnum.NotCatched;
        }

        /// <summary>
        /// Get state of re-throw
        /// </summary>
        /// <param name = "throwStatement"> start node of psi-tree </param>
        /// <returns> Catch status </returns>
        /// <exception cref = "ArgumentNullException"> startNode is null </exception>
        /// <exception cref = "ArgumentNullException"> exceptionType is null </exception>
        public static RethrowStatusEnum GetRethrowState([NotNull] this IThrowStatement throwStatement)
        {
            if (throwStatement == null)
                throw new ArgumentNullException("throwStatement");

            var tryStatements = throwStatement.Parents<ITryStatement>();

            foreach (var tryStatement in tryStatements)
            {
                foreach (var catchClause in tryStatement.Catches)
                {
                    if (!catchClause.Contains(throwStatement))
                        continue;

                    if (throwStatement.Exception == null)
                        return RethrowStatusEnum.RethrowWithCallStackData;

                    var exRef = (throwStatement.Exception as IReferenceExpression);
                    if (exRef != null && exRef.Reference.CurrentResolveResult != null)
                    {
                        if (exRef.Reference.CurrentResolveResult.DeclaredElement is ICatchVariableDeclaration)
                            return RethrowStatusEnum.RethrowWithoutCallStackData;
                    }

                    if (throwStatement.Exception is IObjectCreationExpression)
                    {
                        var exCreate = ModelFactory.CreateModel<ObjectCreationModel>(throwStatement.Exception);
                        if (exCreate.Arguments.Any(IsArgumentIsSubtypeOfException))
                            return RethrowStatusEnum.RethrowNewExceptionWithInnerException;
                        else
                            return RethrowStatusEnum.RethrowNewExceptionWithoutInnerException;
                    }

                    return RethrowStatusEnum.RethrowNewExceptionWithoutInnerException;
                }
            }
            return RethrowStatusEnum.None;
        }

        /// <summary>
        /// Is statement declared in finally block
        /// </summary>
        /// <exception cref = "ArgumentNullException"> startNode is null </exception>
        public static bool IsModelInsideFinallyBlock([NotNull] this ITreeNode startNode)
        {
            if (startNode == null)
                throw new ArgumentNullException("startNode");

            var tryStatements = startNode.Parents<ITryStatement>();

            return
                tryStatements.Any(
                    tryStatement => tryStatement.FinallyBlock != null && tryStatement.FinallyBlock.Contains(startNode));
        }

        /// <summary>
        /// Get a catch clause that contains the startNode
        /// </summary>
        /// <exception cref = "ArgumentNullException"> startNode is null </exception>
        [CanBeNull]
        public static ICatchClause GetParentCatchClause([NotNull] this ITreeNode startNode)
        {
            if (startNode == null)
                throw new ArgumentNullException("startNode");

            return startNode.Parents<ICatchClause>().FirstOrDefault();
        }

        /// <summary>
        /// Is argument is subtype of exception
        /// </summary>
        /// <param name = "argument"> Argument </param>
        public static bool IsArgumentIsSubtypeOfException([NotNull] this Argument argument)
        {
            var arg = argument.PsiTreeElement as ICSharpArgument;
            if (arg == null)
                return false;

            var cache = arg.GetPsiServices()
                           .CacheManager.GetDeclarationsCache(arg.GetPsiModule(), true, true);

            if (arg.MatchingParameter == null)
                return false;

            var argTypeName = arg.MatchingParameter.Element.Type.GetLongPresentableName(arg.Language);

            var exType = cache.GetTypeElementByCLRName("System.Exception");
            var argType = cache.GetTypeElementByCLRName(argTypeName);

            return argType != null && argType.IsDescendantOf(exType);
        }
    }
}