#region

using System;
using System.Collections.Generic;
using System.Linq;
using Advitex.ExceptionAnalizer.Models.Abstract;
using Advitex.ExceptionAnalizer.Utils;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Caches;

#endregion

// ReSharper disable NotDocumentedExceptionHighlighting.ArgumentNullException

namespace Advitex.ExceptionAnalizer.Models.InClassMemberModels
{
    /// <summary>
    /// Model of throw statement
    /// </summary>
    public class ThrowModel : AbstractInMemberModel, IExceptionSource
    {
        /// <summary>
        /// Create model
        /// </summary>
        public ThrowModel([NotNull] IThrowStatement throwStatement) : base(throwStatement)
        {
            InitializeExceptionData(throwStatement);

            _isCathedLazy = new Lazy<CatchStatusEnum>(
                () => IsExceptionTypeRecignized
                          ? PsiTreeElement.CheckIsCatched(ExceptionType)
                          : CatchStatusEnum.Unknown);

            _rethrowStatusLazy = new Lazy<RethrowStatusEnum>(throwStatement.GetRethrowState);

            _isInsideFinallyBlockLazy = new Lazy<bool>(PsiTreeElement.IsModelInsideFinallyBlock);
        }


        /// <summary>
        /// Throw statement
        /// </summary>
        [NotNull]
        public IThrowStatement ThrowStatement
        {
            // ReSharper disable AssignNullToNotNullAttribute
            get { return (IThrowStatement) PsiTreeElement; }
            // ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        /// Is exception type recognized
        /// </summary>
        public bool IsExceptionTypeRecignized { get; private set; }

        /// <summary>
        /// Exception type
        /// </summary>
        [NotNull]
        public ITypeElement ExceptionType { get; private set; }

        /// <summary>
        /// Exception description
        /// </summary>
        [NotNull]
        public string ExceptionDescription { get; private set; }

        /// <summary>
        /// Catch status of exception
        /// </summary>
        public CatchStatusEnum CatchStatus
        {
            // ReSharper disable NotDocumentedExceptionHighlighting
            get { return _isCathedLazy.Value; }
            // ReSharper restore NotDocumentedExceptionHighlighting
        }

        /// <summary>
        /// State of re-throw
        /// </summary>
        public RethrowStatusEnum RethrowStatus
        {
            // ReSharper disable NotDocumentedExceptionHighlighting
            get { return _rethrowStatusLazy.Value; }
            // ReSharper restore NotDocumentedExceptionHighlighting
        }

        /// <summary>
        /// Is exception is thrown in finally block
        /// </summary>
        public bool IsInsideFinallyBlock
        {
            // ReSharper disable NotDocumentedExceptionHighlighting
            get { return _isInsideFinallyBlockLazy.Value; }
            // ReSharper restore NotDocumentedExceptionHighlighting
        }

        /// <summary>
        /// Get exceptions that was thrown by the code model
        /// </summary>
        public IEnumerable<ThrownException> GetThrownExceptions()
        {
            return  IsExceptionTypeRecignized && CatchStatus == CatchStatusEnum.NotCatched
                       ? new[] {new ThrownException(this, ExceptionType, ExceptionDescription)}
                       : new ThrownException[0];
        }

        #region Private members

        private void InitializeExceptionData(IThrowStatement throwStatement)
        {
            var exType = ThrowStatement.GetExceptionType();
            ExceptionDescription = "N/A";

            IsExceptionTypeRecignized = exType != null;
            if (exType == null)
                return;

            ExceptionType = exType;

            var lang = throwStatement.Language;
            var declarationCache = throwStatement.GetPsiServices().CacheManager.
                                                  GetDeclarationsCache(DeclarationCacheLibraryScope.FULL, true);

            if (throwStatement.Exception is IObjectCreationExpression)
            {
                var creationModel = ModelFactory.CreateModel<ObjectCreationModel>(throwStatement.Exception);
                if (creationModel != null)
                {
                    var attribute =
                        creationModel.ConstructorReference.CompiledClassMember.
                                      GetAttributeInstances(true).
                                      FirstOrDefault(
                                          a => a.AttributeType.GetPresentableName(lang) == "ThrowDescriptionAttribute");

                    if (attribute != null)
                    {
                        var typeParam = attribute.NamedParameter("ExceptionType");
                        var descParam = attribute.NamedParameter("Description");

                        if (!typeParam.IsBadValue && typeParam.IsType &&
                            !descParam.IsBadValue && descParam.IsConstant)
                        {
                            var t = declarationCache.
                                GetTypeElementByCLRName(
                                    typeParam.TypeValue.
                                              GetLongPresentableName(lang));
                            var d = (string) descParam.ConstantValue.Value;
                            if (t != null)
                            {
                                var exInfo = new ExceptionInfo(t, d);
                                var thrownException = new ThrownException(this, exInfo, creationModel.Arguments);
                                ExceptionDescription = thrownException.ExceptionDescription;
                            }
                        }
                    }
                    else
                    {
                        var arg = creationModel.Arguments.FirstOrDefault(x => x.ParameterName == "message");
                        if (arg != null && arg.HasValue)
                            ExceptionDescription = (string) arg.Value;
                    }
                }
            }
            else
            {
                var classAttribute = exType.GetAttributeInstances(true).FirstOrDefault(
                    a => a.AttributeType.GetPresentableName(lang) == "ThrowDescriptionAttribute");

                if (classAttribute != null)
                {
                    var typeParam = classAttribute.NamedParameter("ExceptionType");
                    var descParam = classAttribute.NamedParameter("Description");

                    if (!typeParam.IsBadValue && typeParam.IsType &&
                        !descParam.IsBadValue && descParam.IsConstant)
                    {
                        var t = declarationCache.
                            GetTypeElementByCLRName(
                                typeParam.TypeValue.
                                          GetLongPresentableName(lang));
                        var d = (string) descParam.ConstantValue.Value;
                        if (t != null)
                        {
                            ExceptionType = t;
                            ExceptionDescription = d;
                        }
                    }
                }
            }
        }

        private Lazy<RethrowStatusEnum> _rethrowStatusLazy;
        private Lazy<CatchStatusEnum> _isCathedLazy;
        private Lazy<bool> _isInsideFinallyBlockLazy;

        #endregion
    }
}